using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.Devices.Sensors;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Crazy_Squirrel
{
    public partial class Play : PhoneApplicationPage
    {
        private static int best;
        private static int nutAmount;
        private Image[] imge;
        private DispatcherTimer backTimer;
        private DispatcherTimer rotateTimer;
        private DispatcherTimer speedTimer;
        private int countSquirrelMoving;
        private double dirRotation;
        private bool bRotation;
        private double angle;
        private int speed;
        private int speedCheck;
        private Accelerometer acc;
        private Stream stream;
        private SoundEffect effect;


        public Play()
        {
           
            InitializeComponent();
      

        }

        public static void setBest(int n)
        {
            best = n;
        }

        public static void setNutAmount(int n)
        {
            nutAmount = n;
        }
        private void startAcc()
        {
            //throw new NotImplementedException();
            acc = new Accelerometer();
            acc.ReadingChanged += acc_ReadingChanged;
            acc.Start();
        }

        void acc_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            // throw new NotImplementedException();
            Dispatcher.BeginInvoke(() =>
            {
                if (this.Orientation == PageOrientation.LandscapeLeft)
                {
                    dirRotation -= e.Y;
                    angle -= e.Y;
                }
                else
                {
                    dirRotation += e.Y;
                    angle += e.Y;
                }
            });
        }

        public void StartBackGround()
        {
            reset();
            backTimer = new DispatcherTimer();
            rotateTimer = new DispatcherTimer();
            speedTimer = new DispatcherTimer();
            speedTimer.Interval = TimeSpan.FromSeconds(5);
            rotateTimer.Interval = TimeSpan.FromMilliseconds(100);
            backTimer.Interval = TimeSpan.FromMilliseconds(100);

            // Sub-routine OnTimerTick will be called at every 1 second
            backTimer.Tick += backTimer_Tick;
            rotateTimer.Tick += rotateTimer_Tick;
            speedTimer.Tick += speedTimer_Tick;
            // starting the timer
            backTimer.Start();
            rotateTimer.Start();
            speedTimer.Start();
            // Compass comps = new Compass();
            //comps.TimeBetweenUpdates = TimeSpan.FromMilliseconds(10);

        }

        void speedTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            bRotation = false;
        }

        void rotateTimer_Tick(object sender, EventArgs e)
        {

            if (speed != 0)
            {
                speedCheck++;
                speedCheck = speed == speedCheck ? 0 : speedCheck;
            }

            if (speedCheck == 0)
            {

                if (!bRotation)
                {
                    double decrease = new Random().NextDouble();
                    dirRotation = new Random().Next(-2, 2);
                    angle += dirRotation * decrease;
                    bRotation = true;
                }
                else
                {
                    //dirRotation=dirRotation==0 ? 1:dirRotation;
                    speed = speed > 0 ? --speed : 0;
                    if (speed == 0 && dirRotation < 2.0 && dirRotation > -2.0 && dirRotation != 0)
                    {
                        dirRotation = dirRotation > 0 ? (dirRotation + 0.2) : (dirRotation - 0.2);
                    }
                    angle += dirRotation;
                }
                RotateTransform rt = new RotateTransform();
                rt.Angle = angle;
                imgSquirrel.RenderTransform = rt;
            }
            if (angle < -60 || angle > 60)
            {
                //exit
                this.speedTimer.Stop();
                this.backTimer.Stop();
                this.rotateTimer.Stop();
                newRecord();
                hideUI();
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            //reset();
            //backTimer = new DispatcherTimer();
            //rotateTimer = new DispatcherTimer();
            //speedTimer = new DispatcherTimer();
            //speedTimer.Interval = TimeSpan.FromSeconds(5);
            //rotateTimer.Interval = TimeSpan.FromMilliseconds(100);
            //backTimer.Interval = TimeSpan.FromMilliseconds(100);
            //countSquirrelMoving = 0;
            //angle = 0;

            //rotateTimer.Tick += rotateTimer_Tick;
            //backTimer.Tick += backTimer_Tick;
            //// starting the timer
            if (nutAmount >= 5)
            {
                nutAmount -= 5;
                txtNuts.Text = nutAmount.ToString();
                angle = 0;
                dirRotation = 0;
                speed = 10;
                speedCheck = 0;
                bRotation = true;
                RotateTransform rt = new RotateTransform();
                rt.Angle = angle;
                imgSquirrel.RenderTransform = rt;
                speedTimer.Start();
                backTimer.Start();
                rotateTimer.Start();
                showUI();
            }
            else
            {
                MessageBox.Show("Need more nuts!", "Warning", MessageBoxButton.OK);
            }
            
        }

        private void hideUI()
        {
            // throw new NotImplementedException();
            acc.Stop();
            img0.Visibility = System.Windows.Visibility.Collapsed;
            img1.Visibility = System.Windows.Visibility.Collapsed;
            img2.Visibility = System.Windows.Visibility.Collapsed;
            img3.Visibility = System.Windows.Visibility.Collapsed;
            img4.Visibility = System.Windows.Visibility.Collapsed;
            img5.Visibility = System.Windows.Visibility.Collapsed;
            imgSquirrel.Visibility = System.Windows.Visibility.Collapsed;
            txtDistance.Visibility = System.Windows.Visibility.Collapsed;
            btnExit.Visibility = System.Windows.Visibility.Visible;
            btnContinue.Visibility = System.Windows.Visibility.Visible;
            txtBest.Visibility = System.Windows.Visibility.Visible;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("/Img/SquirrelFall.png", UriKind.Relative));
            this.ContentPannel.Background = ib;
        }

        private void showUI()
        {
            // throw new NotImplementedException();
            acc.Start();
            img0.Visibility = System.Windows.Visibility.Visible;
            img1.Visibility = System.Windows.Visibility.Visible;
            img2.Visibility = System.Windows.Visibility.Visible;
            img3.Visibility = System.Windows.Visibility.Visible;
            img4.Visibility = System.Windows.Visibility.Visible;
            img5.Visibility = System.Windows.Visibility.Visible;
            imgSquirrel.Visibility = System.Windows.Visibility.Visible;
            txtDistance.Visibility = System.Windows.Visibility.Visible;
            btnExit.Visibility = System.Windows.Visibility.Collapsed;
            btnContinue.Visibility = System.Windows.Visibility.Collapsed;
            txtBest.Visibility = System.Windows.Visibility.Collapsed;
            this.ContentPannel.Background = null;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) 
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        void backTimer_Tick(object sender, EventArgs e)
        {
            //if (++countSquirrelMoving % 2 == 0)
            //{
                //changePicture Here

                txtDistance.Text = (++countSquirrelMoving / 10).ToString() + "m";
                if (countSquirrelMoving % 100 == 0)
                {
                    txtNotice.Text = (countSquirrelMoving / 10).ToString() + "m";
                    txtNotice.Visibility = System.Windows.Visibility.Visible;
                    DispatcherTimer tempTimer = new DispatcherTimer();
                    tempTimer.Interval = TimeSpan.FromSeconds(2);
                    tempTimer.Tick += (sender1, event1) => {
                        txtNotice.Visibility = System.Windows.Visibility.Collapsed;
                        tempTimer.Stop();
                        
                    };
                    tempTimer.Start();
                }
                if (countSquirrelMoving / 10 > 10 && countSquirrelMoving % 10 == 0 &&countSquirrelMoving/10%5==0)
                {
                    nutAmount++;
                    txtNuts.Text = nutAmount.ToString();
                }

                if (countSquirrelMoving / 10 == 10)
                {
                    speedTimer.Stop();
                    speedTimer.Interval = TimeSpan.FromSeconds(4);
                    speedTimer.Start();
                    rotateTimer.Stop();
                    rotateTimer.Interval = TimeSpan.FromMilliseconds(100);
                    rotateTimer.Start();
                    backTimer.Stop();
                    backTimer.Interval = TimeSpan.FromMilliseconds(100);
                    backTimer.Start();
                    Stream stream00 = TitleContainer.OpenStream("Sound/LevelUp.wav");
                   SoundEffect effect00 = SoundEffect.FromStream(stream00);
                    FrameworkDispatcher.Update();
                    
                    effect00.Play();
                    level.Text = "at 30m";

                 img0.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L2/img01.png");
                 img1.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L2/img11.png");
                 img2.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L2/img21.png");
                 img3.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L2/img31.png");
                 img4.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L2/img41.png");
                 img5.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L2/img51.png");
                }
                if (countSquirrelMoving / 10 == 30)
                {
                    speedTimer.Stop();
                    speedTimer.Interval = TimeSpan.FromSeconds(4);
                    speedTimer.Start();
                    rotateTimer.Stop();
                    rotateTimer.Interval = TimeSpan.FromMilliseconds(50);
                    rotateTimer.Start();
                    backTimer.Stop();
                    backTimer.Interval = TimeSpan.FromMilliseconds(80);
                    backTimer.Start();
                    Stream stream11 = TitleContainer.OpenStream("Sound/LevelUp.wav");
                    SoundEffect effect11 = SoundEffect.FromStream(stream11);
                    FrameworkDispatcher.Update();
                    
                    effect11.Play();
                    level.Text = "at 60m";
                    img0.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L3/img02.png");
                    img1.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L3/img12.png");
                    img2.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L3/img22.png");
                    img3.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L3/img32.png");
                    img4.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L3/img42.png");
                    img5.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L3/img52.png");
                    //change red background
                }
                if (countSquirrelMoving / 10 == 60)
                {
                    speedTimer.Stop();
                    speedTimer.Interval = TimeSpan.FromSeconds(2);
                    speedTimer.Start();
                    rotateTimer.Stop();
                    rotateTimer.Interval = TimeSpan.FromMilliseconds(25);
                    rotateTimer.Start();
                    backTimer.Stop();
                    backTimer.Interval = TimeSpan.FromMilliseconds(80);
                    backTimer.Start();
                    Stream stream22 = TitleContainer.OpenStream("Sound/LevelUp.wav");
                    SoundEffect effect22 = SoundEffect.FromStream(stream22);
                    FrameworkDispatcher.Update();

                    effect22.Play();
                    level.Text = "at 100m";
                    img0.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L4/img03.png");
                    img1.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L4/img13.png");
                    img2.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L4/img23.png");
                    img3.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L4/img33.png");
                    img4.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L4/img43.png");
                    img5.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L4/img53.png");
                    //change red background
                }
                if (countSquirrelMoving / 10 == 100)
                {
                    speedTimer.Stop();
                    speedTimer.Interval = TimeSpan.FromSeconds(2);
                    speedTimer.Start();
                    rotateTimer.Stop();
                    rotateTimer.Interval = TimeSpan.FromMilliseconds(10);
                    rotateTimer.Start();
                    backTimer.Stop();
                    backTimer.Interval = TimeSpan.FromMilliseconds(40);
                    backTimer.Start();
                    Stream stream33 = TitleContainer.OpenStream("Sound/LevelUp.wav");
                    SoundEffect effect33 = SoundEffect.FromStream(stream33);
                    FrameworkDispatcher.Update();

                    effect33.Play();
                    level.Text = "at 150m";
                    img0.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L5/img04.png");
                    img1.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L5/img14.png");
                    img2.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L5/img24.png");
                    img3.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L5/img34.png");
                    img4.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L5/img44.png");
                    img5.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L5/img54.png");
                    //change red background
                }
                if (countSquirrelMoving / 10 == 150)
                {
                    speedTimer.Stop();
                    speedTimer.Interval = TimeSpan.FromSeconds(1);
                    speedTimer.Start();
                    rotateTimer.Stop();
                    rotateTimer.Interval = TimeSpan.FromMilliseconds(10);
                    rotateTimer.Start();
                    backTimer.Stop();
                    backTimer.Interval = TimeSpan.FromMilliseconds(20);
                    backTimer.Start();
                    Stream stream44 = TitleContainer.OpenStream("Sound/LevelUp.wav");
                    SoundEffect effect44 = SoundEffect.FromStream(stream44);
                    FrameworkDispatcher.Update();

                    effect44.Play();
                    level.Text = "at 200m";
                    img0.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L6/img05.png");
                    img1.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L6/img15.png");
                    img2.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L6/img25.png");
                    img3.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L6/img35.png");
                    img4.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L6/img45.png");
                    img5.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L6/img55.png");
                    //change red background
                }
                if (countSquirrelMoving / 10 == 200)
                {
                    speedTimer.Stop();
                    speedTimer.Interval = TimeSpan.FromSeconds(1);
                    speedTimer.Start();
                    rotateTimer.Stop();
                    rotateTimer.Interval = TimeSpan.FromMilliseconds(1);
                    rotateTimer.Start();
                    backTimer.Stop();
                    backTimer.Interval = TimeSpan.FromMilliseconds(10);
                    backTimer.Start();
                    Stream stream55 = TitleContainer.OpenStream("Sound/LevelUp.wav");
                    SoundEffect effect55 = SoundEffect.FromStream(stream55);
                    FrameworkDispatcher.Update();

                    effect55.Play();
                    txtLevel.Text = "";
                    level.Text = "The Last Level!";
                    img0.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L7/img06.png");
                    img1.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L7/img16.png");
                    img2.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L7/img26.png");
                    img3.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L7/img36.png");
                    img4.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L7/img46.png");
                    img5.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/L7/img56.png");
                    //change red background
                }
                imgSquirrel.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/CrazySquirrel" + (countSquirrelMoving / 2) % 4 + ".png");
                if (countSquirrelMoving % 4 == 0)
                {
                    effect.Play();
                }
            //}
            for (int i = 0; i < imge.Length; i++)
            {
                Thickness temp = imge[i].Margin;
                temp.Top -= 16;
                if (temp.Top <= -96)
                {
                    temp.Top = 480;
                    imge[i].Margin = temp;
                }
                else
                {
                    imge[i].Margin = temp;
                }
                //imge[i]. = imge[i].Margin.Top - 1;
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            reset();
            backTimer.Stop();
            rotateTimer.Stop();
            speedTimer.Stop();
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;
            using (fs = savegameStorage.CreateFile("NutAmount"))
            {
                if (fs != null)
                {
                    // just overwrite the existing info for this example.
                    byte[] bytes = System.BitConverter.GetBytes(nutAmount);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            //   newRecord();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
         startAcc();
            StartBackGround();
            stream = TitleContainer.OpenStream("Sound/footstep.wav");
            effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            txtNuts.Text = nutAmount.ToString();
            //.base.OnNavigatedTo(e);
        }

        private void newRecord()
        {
            if (countSquirrelMoving / 10 > best)
            {
                IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
                best = countSquirrelMoving / 10;

                // open isolated storage, and write the savefile.
                IsolatedStorageFileStream fs = null;
                using (fs = savegameStorage.CreateFile("Best"))
                {
                    if (fs != null)
                    {
                        // just overwrite the existing info for this example.
                        byte[] bytes = System.BitConverter.GetBytes(countSquirrelMoving / 10);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                txtBest.Text = "New Record!\n" + best.ToString() + "m\nSpend 5 nuts to continue!";
            }
            else
            {
                txtBest.Text = "This time you get: " + (countSquirrelMoving / 10).ToString() + "m\nYour Best: " + best.ToString() + "m\nSpend 5 nuts to continue!";
            }
        }

        private void reset()
        {
            //throw new NotImplementedException();
            countSquirrelMoving = 0;
            angle = 0;
            dirRotation = 0;
            speed = 10;
            speedCheck = 0;
            bRotation = false;
            imge = new Image[6];
            for (int i = 0; i < imge.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        imge[i] = img0;
                        break;
                    case 1:
                        imge[i] = img1;
                        break;
                    case 2:
                        imge[i] = img2;
                        break;
                    case 3:
                        imge[i] = img3;
                        break;
                    case 4:
                        imge[i] = img4;
                        break;
                    case 5:
                        imge[i] = img5;
                        break;
                    default:
                        break;
                }
            }
            imgSquirrel.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Img/CrazySquirrel0.png");
            RotateTransform rt = new RotateTransform();
            // rt = (RotateTransform)imgSquirrel.RenderTransform.GetValue(new DependencyProperty());
            rt.Angle = 0;

            imgSquirrel.RenderTransform = rt;

        }
    }
}