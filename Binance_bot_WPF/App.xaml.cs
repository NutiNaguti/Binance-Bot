using Binance.Net;
using Binance.Net.Objects;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Windows;

namespace Binance_bot_WPF
{
   /*
    *  Класс с основной логикой
    */
    public partial class App : Application
    {
        public static string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\Settings.txt");
        public static string couple = ""; // торговая пара
        public static string API_KEY = ""; // API binance
        public static string SECRET_API_KEY = ""; // secret API binance
        public static string coinsString = "";
        public static string sec = "";
        public static int numberOfSigns = 0; // количество знаков после запятой
        public static decimal coins = 0.0m; // количество монет, которое будет выставляться в ордерах
        public static decimal[][] array = new decimal[10][];
        public static bool work = false;
        private static decimal priceForBuy = 0; // цена по которой выставится ордер на покупку
        private static decimal priceForSell = 0; // цена по которой выставится ордер на продажу
        private static decimal priceBuy = 0;
        private static decimal priceSell = 0;

        public static void Core()
        {
            try
            {
                SettingsWrite.Settings(); // запись существующих настроек в текстовый файл
                Autorization.AutorizationBinance(); // авторизация api 
                BuySell.Cycle(); // цикл while 
            }

            catch
            {
                SettingsWrite.SettingsRead();
                Core();
            }
        }

        public static void Core_2()
        {
            try
            {
                SettingsWrite.Settings(); // запись существующих настроек в текстовый файл
                Autorization.AutorizationBinance(); // авторизация api 
                BuySell.Cycle(Convert.ToInt32(sec)); // цикл while 
            }

            catch
            {
                SettingsWrite.SettingsRead();
                Core_2();
            }
        }

        public static void Buy()
        {
            coins = Convert.ToDecimal(coinsString);
            int index = 0;
            decimal[] percent = new decimal[3];
            decimal[] price = new decimal[10];
            decimal[] buy = new decimal[10];
            decimal value = decimal.MinValue;
            using (var client = new BinanceClient())
            {
                string Get(string uri)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }

                string binance = Get("https://api.binance.com/api/v1/depth?symbol=" + couple + "&limit=10");
                int element;
                for (element = 0; element < 10; element++)
                {
                    dynamic decoded = JsonConvert.DeserializeObject(binance);
                    var bids = decoded.bids[element][1];
                    buy[element] = bids;
                    if (buy[element] >= value)
                    {
                        value = buy[element];
                        index = element;
                    }

                    Console.WriteLine($"объем покупки: {buy[element]}");
                }

                Console.WriteLine($"индекс нужного ордера: {index}");

                for (element = 0; element < 10; element++)
                {
                    dynamic decoded = JsonConvert.DeserializeObject(binance);
                    var bids = decoded.bids[element][0];
                    price[element] = bids;
                    Console.WriteLine($"цена покупки: {price[element]}");
                }

                Console.WriteLine($"цена нужного ордера: {price[index]}");
                priceBuy = price[index];
                priceForBuy = priceBuy - Fee(priceBuy);
                priceForBuy = Math.Round(priceForBuy, numberOfSigns);
                var orderBuy = client.PlaceOrder(couple, OrderSide.Buy, OrderType.Limit, coins, price: priceForBuy,
                    timeInForce: TimeInForce.GoodTillCancel);
            }
        }

        public static void Sell()
        {
            coins = Convert.ToDecimal(coinsString);
            int index = 0, element;
            decimal[] price = new decimal[10];
            decimal[] sell = new decimal[10];
            decimal value = decimal.MinValue;
            using (var client = new BinanceClient())
            {
                string Get(string uri)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }

                var binance = Get("https://api.binance.com/api/v1/depth?symbol=" + couple + "&limit=10");

                for (element = 0; element < 10; element++)
                {
                    dynamic decoded = JsonConvert.DeserializeObject(binance);
                    var ask = decoded.asks[element][1];
                    sell[element] = ask;
                    if (sell[element] >= value)
                    {
                        value = sell[element];
                        index = element;
                    }

                    Console.WriteLine($"объем продажи: {sell[element]}");
                }

                Console.WriteLine($"индекс нужного ордера: {index}");

                for (element = 0; element < 10; element++)
                {
                    dynamic decoded = JsonConvert.DeserializeObject(binance);
                    var ask = decoded.asks[element][0];
                    price[element] = ask;
                    Console.WriteLine($"цена продажи: {price[element]}");
                }

                Console.WriteLine($"цена нужного ордера: {price[index]}");
                priceSell = price[index];
                priceForSell = priceSell + Fee(priceSell);
                priceForSell = Math.Round(priceForSell, numberOfSigns);
                if (priceForSell > priceBuy)
                {
                    var orderSell = client.PlaceOrder(couple, OrderSide.Sell, OrderType.Limit, coins,
                        price: priceForSell, timeInForce: TimeInForce.GoodTillCancel);
                }
                else
                    Sell();
            }
        }

        public static decimal Fee(decimal order)
        {
            return order / 100 * 0.075m;
        }
    }
}

