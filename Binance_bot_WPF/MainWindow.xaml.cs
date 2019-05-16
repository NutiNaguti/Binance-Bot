using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Binance_bot_WPF
{
   /*
    *  Класс основного окна wpf
    */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files"));
                using (var file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/API Key.txt")))
                {
                    file.Close();
                }
                using (var file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/Secret API Key.txt")))
                {
                    file.Close();
                }
                using (var file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/Settings.txt")))
                {
                    file.Close();
                }
                using (var file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/Readme.txt")))
                {
                    file.Close();
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Stop
        {
            App.work = false;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\котя.jpg");
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
            Background = myBrush;
            Background.Opacity = 0.8;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) // Help
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\Read me.txt");
            Process.Start(path);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) // Exit
        {
            Thread mainThread = new Thread(() => Environment.Exit(0));
            mainThread.Start();
        }

        private void Button_Click4(object sender, RoutedEventArgs e) // Settings
        {
            Settings stg = new Settings();
            stg.Show();
        }

        private void Couple_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // couple
        {
            App.couple = Couple.Text;
            Sample.Sort();
        }

        private void Coins_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // coins
        {
            App.coinsString = Coins.Text;
        }

        //private void Number_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // number
        //{
        //    App.numberOfSignsString = Number.Text;
        //}

        public void Button_Click(object sender, RoutedEventArgs e) // start
        {
            App.work = true;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\crypto.jpg");
            try
            {
                if (Convert.ToInt32(App.sec) < int.MaxValue && Convert.ToInt32(App.sec) > 0)
                {
                    var mainThread = new Thread(App.Core_2);
                    mainThread.Start();
                    var myBrush = new ImageBrush {ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute))};
                    Background = myBrush;
                    Background.Opacity = 0.8;

                }

                else
                {
                    var mainThread2 = new Thread(App.Core);
                    mainThread2.Start();
                    var myBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute)) };
                    Background = myBrush;
                    Background.Opacity = 0.8;
                }
            }
            catch (FormatException)
            {
                App.sec = "0";
                var mainThread2 = new Thread(App.Core_2);
                mainThread2.Start();
                var myBrush = new ImageBrush {ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute))};
                Background = myBrush;
                Background.Opacity = 0.8;
            }
        }
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        //private void Value_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    App.j = Value.Text;
        //}
    }
}

