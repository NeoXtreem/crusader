using System;
using Binance.Net.Objects;
using Xtreem.CryptoPrediction.Service.Services;

namespace Xtreem.CryptoPrediction.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var klineMappingService = new KlineMappingService();
            var predictionService = new PredictionService();

            using (var binanceService = new BinanceService(klineMappingService))
            {
                const string symbol = "BNBBTC";
                const KlineInterval interval = KlineInterval.OneMinute;

                var klines = binanceService.GetKlines(symbol, interval, DateTime.UtcNow.AddMinutes(-60), DateTime.UtcNow, 60);

                predictionService.Initialise(klines);
                binanceService.SubscribeToKlines(symbol, interval, predictionService.Predict);

                Console.ReadLine();
            }
        }
    }
}
