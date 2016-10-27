using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Perro.Resources;
using Buddy;

namespace Perro
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        private void bcrea_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.Client.CreateUserAsync(HandleUser, boxuser.Text, PasswordBox.Password);
       

        }

        private void HandleUser(AuthenticatedUser p_user, BuddyCallbackParams p_params)
        {
            //check if everything went fine
            if (p_params.Exception != null || p_user == null)
            {
                //Display the buddy error if something went bad
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                //    MessageBox.Show("Something went wrong " + ((BuddyServiceException)p_params.Exception).Error);
                //});
            }
            else
            {
                //Store the user
                ServiceManager.User = p_user;
                //Debug.WriteLine("Success we got our user"); 

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Principal.xaml", UriKind.Relative));
                });
            }
        }
        

        private void blogin_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.Client.LoginAsync(HandleUser, boxuser.Text, PasswordBox.Password);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}