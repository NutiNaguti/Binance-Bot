using System;
using System.Diagnostics;
using System.IO;

namespace Binance_bot_WPF
{
   /*
    *  Класс для записи и чтения api кеев
    */
    class Writer
    {
        public static string line { get; set; } = "";

        public static void Write()
        {
            using (var sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\API Key.txt")))
            {
                sw.WriteLine(App.API_KEY);
                sw.Close();
            }

            using (var sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\Secret API Key.txt")))
            {
                sw.WriteLine(App.SECRET_API_KEY);
                sw.Close();
            }
        }

        public static void Default()
        {
            using (var chek = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\API Key.txt")))
            {
                while ((line = chek.ReadLine()) != null)
                {
                    App.API_KEY = line;
                }
                chek.Close();
            }

            using (var chek = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\Secret API Key.txt")))
            {
                while ((line = chek.ReadLine()) != null)
                {
                    App.SECRET_API_KEY = line;
                }
                chek.Close();
            }
        }
    }
}
