using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance_bot_WPF
{
    /*
     *  Это класс для анализа торговой пары и назначения кол-ва знаков после запятой
     */
    class Sample
    {
        public static void Sort()
        {
            if (App.couple.Contains("BNBUSDT") || App.couple.Contains("ADAUSDT") ||
            App.couple.Contains("IOTAUSDT") || App.couple.Contains("ZRXUSDT") ||
            App.couple.Contains("BATUSDT") || App.couple.Contains("LINKUSDT"))
            {
                App.numberOfSigns = 4;
            }

            if (App.couple.Contains("BTTUSDT") || App.couple.Contains("HOTUSDT"))
            {
                App.numberOfSigns = 7;
            }

            if (App.couple.Contains("TRXUSDT") || App.couple.Contains("XLMUSDT") ||
                App.couple.Contains("THETAUSDT") || App.couple.Contains("XRPUSDT"))
            {
                App.numberOfSigns = 5;
            }
        }
    }
}
