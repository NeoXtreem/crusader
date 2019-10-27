using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Text;
using Xtreem.Crusader.Data.Contexts;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Settings;

namespace Xtreem.Crusader.Data.Exporter
{
    [UsedImplicitly]
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Configure settings.
            var dataSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("Data");

            // Set up DI.
            var serviceProvider = new ServiceCollection()
                .AddScoped<IMarketDataContext, MarketDataContext>()
                .AddScoped<IMarketDataReadRepository, MarketDataReadRepository>()
                .Configure<DataSettings>(dataSettings)
                .BuildServiceProvider();

            using var writer = File.CreateText("ohlcv.csv");
            CsvSerializer.SerializeToWriter(serviceProvider.GetService<IMarketDataReadRepository>().GetOhlcvs(), writer);
        }
    }
}
