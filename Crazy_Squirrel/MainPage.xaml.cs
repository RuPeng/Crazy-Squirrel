using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;

namespace Crazy_Squirrel
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            checkBest();
            if (!Accelerometer.IsSupported)
            {
                txtWarning.Visibility = System.Windows.Visibility.Visible;
                btnPlay.IsEnabled = false;
                txtBest.Visibility = System.Windows.Visibility.Collapsed;
                txtShow.Visibility = System.Windows.Visibility.Collapsed;
            }
         
        }
        private void checkBest()
        {
            // open isolated storage, and load data from the savefile if it exists.

            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication())

            {
                if (savegameStorage.FileExists("Best"))
                {
                    using (IsolatedStorageFileStream fs = savegameStorage.OpenFile("Best", System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            // Reload the saved high-score data.
                            byte[] saveBytes = new byte[4];
                            int count = fs.Read(saveBytes, 0, 4);
                            if (count > 0)
                            {
                                Play.setBest(System.BitConverter.ToInt32(saveBytes, 0));
                                txtBest.Text = System.BitConverter.ToInt32(saveBytes, 0).ToString() + "m";
                            }
                        }
                    }
                }
                else
                {

                }
                if (savegameStorage.FileExists("NutAmount"))
                {
                    using (IsolatedStorageFileStream fs = savegameStorage.OpenFile("NutAmount", System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            // Reload the saved high-score data.
                            byte[] saveBytes = new byte[4];
                            int count = fs.Read(saveBytes, 0, 4);
                            if (count > 0)
                            {
                                Play.setNutAmount(System.BitConverter.ToInt32(saveBytes, 0));
                                txtNuts.Text = System.BitConverter.ToInt32(saveBytes, 0).ToString();
                            }
                        }
                    }
                }
            }
            
        }


        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Play.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            checkBest();
        }
    }
}