using System.Threading;

namespace Binance_bot_WPF
{
    class BuySell
    {
        public static void Cycle(int x) // цикл покупки и продажи
        {
            while (App.work)
            {
                Thread.Sleep(x * 1000);
                App.Buy();
                Thread.Sleep(x * 1000);
                App.Sell();
            }
        }

        public static void Cycle() // цикл покупки и продажи
        {
            while (App.work)
            {
                App.Buy();
                App.Sell();
            }
        }
    }
}
