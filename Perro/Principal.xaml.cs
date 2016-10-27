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
using System.IO.IsolatedStorage;


namespace Perro
{
    public partial class Principal : PhoneApplicationPage
    {
        private GeoCoordinateWatcher _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        private DispatcherTimer _timer = new DispatcherTimer();
        private long _startTime;
        private MapPolyline _line;
        private BuddyClient BuddyClient;
        
        
        private double _kilometres;
        private long _previousPositionChangeTick;
        
        public Principal()
        {
            InitializeComponent();
            BuddyClient = new BuddyClient(ServiceManager.Client.AppName, ServiceManager.Client.AppPassword);
            
                      
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            _line = new MapPolyline();
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] == true)
            {
                //ServiceManager.User.GetCheckInsAsync(HandleCheckins);
                PintarMapa();
                _watcher.PositionChanged += Watcher_PositionChanged;
            }

        }

        static void GeoLocationLocationAddHandler(object sender, Buddy.BuddyService.GeoLocation_Location_AddCompletedEventArgs e)
        {
            e.Result.ToString();
        }

        private void PintarMapa()
        {
            ServiceManager.Client.Metadata.GetAllAsync((Dictionary<string, Buddy.MetadataItem> arg1, BuddyCallbackParams arg2) =>
            {
                MapLayer layer = new MapLayer();
                for (var i = 0; i < arg1.Keys.Count; i++)
                {
                    string userid = arg1.ToArray()[i].Value.Value;
                    if (userid == ServiceManager.User.Token)
                    {
                        double x = arg1.ToArray()[i].Value.Latitude;
                        double y = arg1.ToArray()[i].Value.Longitude;
                        ServiceManager.Client.Metadata.FindAsync((Dictionary<string, Buddy.MetadataItem> arg3, BuddyCallbackParams arg4) =>
                            {
                                for (var a = 0; a < arg3.Keys.Count; a++)
                                {
                                    string userid2 = arg3.ToArray()[a].Value.Value;
                                    if (userid2 != ServiceManager.User.Token)
                                    {  
                                        double x2 = arg3.ToArray()[a].Value.Latitude;
                                        double y2 = arg3.ToArray()[a].Value.Longitude;

                                        MapOverlay overlay2 = new MapOverlay
                                        {
                                            GeoCoordinate = new GeoCoordinate(x2, y2),
                                            Content = new Ellipse
                                            {
                                                Fill = new SolidColorBrush(Colors.Blue),
                                                Width = 10,
                                                Height = 10
                                            }
                                        };

                                        layer.Add(overlay2);
                                    }
                                }
                            },
                            searchDistanceMeters: 10, latitude: x, longitude: y, numberOfResults: 20, withKey: null, withValue: null, updatedMinutesAgo: -1, valueMin: 0, valueMax: 100, searchAsFloat: false, sortAscending: false, disableCache: false, state: null);
                        
                        MapOverlay overlay = new MapOverlay
                            {
                                GeoCoordinate = new GeoCoordinate(x, y),
                                Content = new Ellipse
                                {
                                    Fill = new SolidColorBrush(Colors.Red),
                                    Width = 10,
                                    Height = 10
                                }
                            };

                        layer.Add(overlay);
                    }


                }
                map.Layers.Add(layer);
            });
        }
                
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
            timeLabel.Text = runTime.ToString(@"hh\:mm\:ss");
        }

        private void bcamina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (_timer.IsEnabled)
                {
                    //string ttiempo = "tiempo";
                    //jugador.añadirpuntuacion(int.Parse(timeLabel.Text), ttiempo);
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
                    //if (IsolatedStorageSettings.ApplicationSettings.Contains("coordIniX") || (IsolatedStorageSettings.ApplicationSettings.Contains("coordIniY")))
                    //{
                    //    MessageBox.Show("Coordeada X: " + IsolatedStorageSettings.ApplicationSettings.Values("coordIniX") + " Coordenada Y :" + IsolatedStorageSettings.ApplicationSettings.Contains("coordIniX"));
                    //    if (IsolatedStorageSettings.ApplicationSettings.Contains("coordIniX"))
                    //    {
                    //        int x = IsolatedStorageSettings.ApplicationSettings.Keys[coordIniX];   
                    //    }
                        
                    //    return;
                    //}
                    //else
                    //{
                    //    Geocoordinate l_coordinate = await GetSinglePositionAsync();
                    //    //ServiceManager.User.CheckInAsync(HandleCheckin, l_coordinate.Latitude, l_coordinate.Longitude);
                    //    double x = l_coordinate.Latitude;
                    //    double y = l_coordinate.Longitude;
                    //    IsolatedStorageSettings.ApplicationSettings["coordIniX"] = l_coordinate.Latitude;
                    //    IsolatedStorageSettings.ApplicationSettings["coordIniY"] = l_coordinate.Longitude;

                    //    IsolatedStorageSettings.ApplicationSettings.Save();

                    //    MessageBox.Show("La primera coordenada se ha creado");
                    //}
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
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("location  is disabled in phone settings.");
                }
                //else
                {
                    // something else happened acquring the location
                }
            }
        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] == true)
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
        }

        private async void bmarca_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] == true)
            {
                Geocoordinate l_coordinate = await GetSinglePositionAsync();
                //ServiceManager.User.CheckInAsync(HandleCheckin, l_coordinate.Latitude, l_coordinate.Longitude);
                double x = l_coordinate.Latitude;
                double y = l_coordinate.Longitude;
                addlocation(x, y);
            }
        }
        public void addlocation(double x, double y)
        {

            string keyMarca = ServiceManager.User.Name + DateTime.Now.ToString();
                        ServiceManager.Client.Metadata.SetAsync((result, state) =>
                                {
                                    if (state.Exception != null) MessageBox.Show("ha ocurrido un error guardando el punto");
                                    else
                                    {
                                        MessageBox.Show("Punto Guardado");
                                    }
                                    //User result
                                }, key: keyMarca, value: ServiceManager.User.Token , latitude: x, longitude: y, appTag: "Marca", state: null
                            );
	                


        }
        public async Task<Windows.Devices.Geolocation.Geocoordinate> GetSinglePositionAsync()
        {

            Windows.Devices.Geolocation.Geolocator geolocator = new Windows.Devices.Geolocation.Geolocator();
            Windows.Devices.Geolocation.Geoposition geoposition = await geolocator.GetGeopositionAsync();
            return geoposition.Coordinate;
        }

        //private void HandleCheckin(Boolean p_result, BuddyCallbackParams p_params)
        //{
        //    if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] == true)
        //    {
        //        Deployment.Current.Dispatcher.BeginInvoke(() =>
        //        {
        //            if (p_result && p_params.Exception == null)
        //            {
        //                MessageBox.Show("Success");
        //            }
        //            else
        //            {
        //                MessageBox.Show("An error occured " + (p_params.Exception as BuddyServiceException).Error);
        //            }
        //        });
        //    }
        //}

        private void map_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void map_CenterChanged(object sender, Microsoft.Phone.Maps.Controls.MapCenterChangedEventArgs e)
        {
            
            //map.Center = coord; 
        }
    }
}