using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace Binance_bot_WPF
{
    /*
     *  Класс для визуализации графиков
     */
    public partial class Graphics : Window
    {
        public Graphics()
        {
            InitializeComponent();
        }

        public static SeriesCollection SeriesCollection { get; private set; }

        public static void smth()
        {
            SeriesCollection = new SeriesCollection
            {
                
                new LineSeries
                {
                    Values = new ChartValues<double> {1, 2, 3, 4, 5}
                }

            };
        }
    }
}
