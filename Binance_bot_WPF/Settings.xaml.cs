using System.Windows;

namespace Binance_bot_WPF
{
   /*
    *  Класс окна wpf с настройками
    */
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void API_Key_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // api key
        {
            App.API_KEY = API_Key.Text;
        }

        private void Secret_API_Key_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) //secret api key
        {
            App.SECRET_API_KEY = Secret_API_Key.Text;
        }

        private void Seconds_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) // Wait
        {
            App.sec = Seconds.Text;
        }

        private void Button_Click_Accept(object sender, RoutedEventArgs e)
        {
            if (API_Key.Text.Length > 10 && Secret_API_Key.Text.Length > 10)
            {
                Writer.Write();
            }
            Close();
        }

        private void Button_Click_Default(object sender, RoutedEventArgs e)
        {
            Writer.Default();
            Close();
        }
    }
}
