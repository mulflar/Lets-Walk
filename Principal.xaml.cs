using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Device.Location;
using Buddy;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Windows.Media;


namespace Perro
{
    public partial class Principal : PhoneApplicationPage
    {
        private GeoCoordinateWatcher _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        private DispatcherTimer _timer = new DispatcherTimer();
        private long _startTime;
        private MapPolyline _line;
        
        private double _kilometres;
        private long _previousPositionChangeTick;
        
        public Principal()
        {
            InitializeComponent();
                      
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            _line = new MapPolyline();
            ServiceManager.User.GetCheckInsAsync(HandleCheckins);
            _watcher.PositionChanged += Watcher_PositionChanged;

        }
        private void HandleCheckins(List<CheckInLocation> arg1, BuddyCallbackParams arg2)
        {
            // marker = new MapLayer();
            MapLayer layer = new MapLayer();
            foreach (CheckInLocation x in arg1)
            {
                MapOverlay overlay = new MapOverlay
                    {
                        GeoCoordinate = new GeoCoordinate(x.Latitude,x.Longitude),
                        Content = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.Red),
                            Width = 10,
                            Height = 10
                        }
                    };
                
                layer.Add(overlay);

            }
            map.Layers.Add(layer);


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
            timeLabel.Text = runTime.ToString(@"hh\:mm\:ss");
        }

        private void bcamina_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _watcher.Stop();
                _timer.Stop();
                bcamina.Content = "Start";
                textf1.Visibility = Visibility.Collapsed;
                textf2.Visibility = Visibility.Collapsed;
                textf3.Visibility = Visibility.Collapsed;
                textf4.Visibility = Visibility.Collapsed;
                timeLabel.Visibility = Visibility.Collapsed;
                distanceLabel.Visibility = Visibility.Collapsed;
                paceLabel.Visibility = Visibility.Collapsed;
                caloriesLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                _watcher.Start();
                _timer.Start();
                _startTime = System.Environment.TickCount;
                bcamina.Content = "Stop";
                textf1.Visibility = Visibility.Visible;
                textf2.Visibility = Visibility.Visible;
                textf3.Visibility = Visibility.Visible;
                textf4.Visibility = Visibility.Visible;
                timeLabel.Visibility = Visibility.Visible;
                distanceLabel.Visibility = Visibility.Visible;
                paceLabel.Visibility = Visibility.Visible;
                caloriesLabel.Visibility = Visibility.Visible;
            }
        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if (_line.Path.Count > 0)
            {
                // find the previos point and measure the distance travelled
                var previousPoint = _line.Path.Last();
                var distance = coord.GetDistanceTo(previousPoint);

                // compute pace
                var millisPerKilometer = (1000.0 / distance) * (System.Environment.TickCount - _previousPositionChangeTick);

                // compute total distance travelled
                _kilometres += distance / 1000.0;

                paceLabel.Text = TimeSpan.FromMilliseconds(millisPerKilometer).ToString(@"mm\:ss");
                distanceLabel.Text = string.Format("{0:f2} km", _kilometres);
                caloriesLabel.Text = string.Format("{0:f0}", _kilometres * 65);
            }

            _line.Path.Add(coord);
        }

        private async void bmarca_Click(object sender, RoutedEventArgs e)
        {
            Geocoordinate l_coordinate = await GetSinglePositionAsync();
            ServiceManager.User.CheckInAsync(HandleCheckin, l_coordinate.Latitude, l_coordinate.Longitude);
            

        }
        public async Task<Windows.Devices.Geolocation.Geocoordinate> GetSinglePositionAsync()
        {
            Windows.Devices.Geolocation.Geolocator geolocator = new Windows.Devices.Geolocation.Geolocator();
            Windows.Devices.Geolocation.Geoposition geoposition = await geolocator.GetGeopositionAsync();
            return geoposition.Coordinate;
        }

        private void HandleCheckin(Boolean p_result, BuddyCallbackParams p_params)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (p_result && p_params.Exception == null)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("An error occured " + (p_params.Exception as BuddyServiceException).Error);
                }
            });
        }

        private void map_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void map_CenterChanged(object sender, Microsoft.Phone.Maps.Controls.MapCenterChangedEventArgs e)
        {
            
            map.Center = coord; 
        }
    }
}