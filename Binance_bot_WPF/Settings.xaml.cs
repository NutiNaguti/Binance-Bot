using System;
using System.Windows;
using System.IO;
using Path = System.IO.Path;

namespace Binance_bot_WPF
{
    public partial class Settings : Window
    {
        public static string line = "";
        public static string path = "";

        public Settings()
        {
            InitializeComponent();
        }

        private void API_Key_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // api key
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\API Key.txt");

            App.API_KEY = API_Key.Text;

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(App.API_KEY);
                sw.Close();
            }
        }

        private void Secret_API_Key_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) //secret api key
        {
            App.SECRET_API_KEY = Secret_API_Key.Text;

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(App.SECRET_API_KEY);
                sw.Close();
            }
        }

        private void Seconds_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // Wait
        {
            App.sec = Seconds.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Default(object sender, RoutedEventArgs e)
        {
            using (var chek = new StreamReader(@"C:\Users\User\source\repos\Binance_bot_WPF\Binance_bot_WPF\Resources\API Key.txt"))
            {
                while ((line = chek.ReadLine()) != null)
                {
                    App.API_KEY = line;
                }
                chek.Close();
            }

            using (var chek = new StreamReader(@"C:\Users\User\source\repos\Binance_bot_WPF\Binance_bot_WPF\Resources\Secret API Key.txt"))
            {
                while ((line = chek.ReadLine()) != null)
                {
                    App.SECRET_API_KEY = line;
                }
                chek.Close();
            }

            Close();
        }
    }
}
