using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Text;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.Data.Exporter
{
    [UsedImplicitly]
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Configure options.
            var options = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("Data");

            // Set up DI.
            var serviceProvider = new ServiceCollection()
                .ScanAssembly()
                .Configure<DataOptions>(options)
                .BuildServiceProvider();

            using var writer = File.CreateText("ohlcv.csv");
            CsvSerializer.SerializeToWriter(serviceProvider.GetService<IMarketDataReadRepository>().GetOhlcvs(), writer);
        }
    }
}
