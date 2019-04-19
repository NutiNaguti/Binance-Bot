using System;
using System.IO;

namespace Binance_bot_WPF
{
   /*
    *  Класс записи настроек бота для автоматического запуска после возникшего исключения
    */
    class SettingsWrite
    {
        private static object[] data = new object[6];

        public static void Settings()
        {
            data[0] = App.couple;
            data[1] = App.coins;
            //data[2] = App.numberOfSigns;
            data[3] = App.API_KEY;
            data[4] = App.SECRET_API_KEY;
            data[5] = App.sec;

            using (var sw = new StreamWriter(App.settingsPath))
            {
                foreach (var variable in data)
                {
                    sw.WriteLine(variable);
                }
                sw.Close();
            }
        }

        public static void SettingsRead()
        {
            using (var sr = new StreamReader(App.settingsPath))
            {
                for (var i = 0; i < 6; i++)
                {
                    data[i] = sr.ReadLine();
                }

                App.couple = Convert.ToString(data[0]);
                App.coins = Convert.ToDecimal(data[1]);
                //App.numberOfSigns = Convert.ToInt32(data[2]);
                App.API_KEY = Convert.ToString(data[3]);
                App.SECRET_API_KEY = Convert.ToString(data[4]);
                App.sec = Convert.ToString(data[5]);
                sr.Close();
            }
        }
    }
}
