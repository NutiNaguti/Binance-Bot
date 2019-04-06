using System;
using System.Collections.Generic;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Windows;

namespace Binance_bot_WPF
{
    public partial class App : Application
    {
        private static string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Settings.txt");
        public static string couple = ""; // торговая пара
        public static string API_KEY = ""; // API binance
        public static string SECRET_API_KEY = ""; // secret API binance
        public static string coinsString = "";
        public static string numberOfSignsString = "";
        public static string sec = "";
        private static int numberOfSigns = 0; // количество знаков после запятой
        private static decimal priceForBuy = 0; // цена по которой выставится ордер на покупку
        private static decimal priceForSell = 0; // цена по которой выставится ордер на продажу
        private static decimal priceBuy = 0;
        private static decimal priceSell = 0;
        private static decimal coins = 0.0m; // количество монет, которое будет выставляться в ордерах
        public static decimal[][] array = new decimal[10][];
        public static bool work = false;
        private static object[] data = new object[6];

        public static void Core()
        {
            try
            {
                SettingsWrite(); // запись существующих настроек в текстовый файл
                AutrizationBinance(); // авторизация api 
                BuySell.Cycle(); // цикл while 
            }

            catch
            {
                SettingsRead();
                Core();
            }
        }

        public static void Core_2()
        {
            try
            {
                SettingsWrite(); // запись существующих настроек в текстовый файл
                AutrizationBinance(); // авторизация api 
                BuySell.Cycle(Convert.ToInt32(sec)); // цикл while 
            }

            catch
            {
                SettingsRead();
                Core_2();
            }
        }

        private static void AutrizationBinance()
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(API_KEY, SECRET_API_KEY),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials(API_KEY, SECRET_API_KEY),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
        }

        public static void Buy()
        {
            numberOfSigns = Convert.ToInt32(numberOfSignsString);
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
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
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
            numberOfSigns = Convert.ToInt32(numberOfSignsString);
            coins = Convert.ToDecimal(coinsString);
            int index = 0, element;
            decimal[] price = new decimal[10];
            decimal[] sell = new decimal[10];
            decimal value = decimal.MinValue;
            using (var client = new BinanceClient())
            {
                string Get(string uri)
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (var response = (HttpWebResponse) request.GetResponse())
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

        public static void SettingsWrite()
        {
            data[0] = couple;
            data[1] = coins;
            data[2] = numberOfSigns;
            data[3] = API_KEY;
            data[4] = SECRET_API_KEY;
            data[5] = sec;

            using (var sw = new StreamWriter(settingsPath))
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
            using (var sr = new StreamReader(settingsPath))
            {
                for (var i = 0; i < 6; i++)
                {
                    data[i] = sr.ReadLine();
                }

                couple = Convert.ToString(data[0]);
                coins = Convert.ToDecimal(data[1]);
                numberOfSigns = Convert.ToInt32(data[2]);
                API_KEY = Convert.ToString(data[3]);
                SECRET_API_KEY = Convert.ToString(data[4]);
                sec = Convert.ToString(data[5]);
                sr.Close();
            }
        }
    }
}

