using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Xtreem.Crusader.Client
{
    [UsedImplicitly]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            //var klineMappingService = new KlineMappingService();
            //var predictionService = new PredictionService();

            //using (var binanceService = new BinanceService(klineMappingService))
            //{
            //    const string symbol = "BNBBTC";
            //    const KlineInterval interval = KlineInterval.OneMinute;

            //    var klines = binanceService.GetKlines(symbol, interval, DateTime.UtcNow.AddMinutes(-60), DateTime.UtcNow, 60);

            //    predictionService.Initialise(klines);
            //    binanceService.SubscribeToKlines(symbol, interval, predictionService.Predict);

            //    Console.ReadLine();
            //}
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
