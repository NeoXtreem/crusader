using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.Settings;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly CrusaderApiSettings _settings;
        private readonly IHistoricalDataService _historicalDataService;

        public PredictionService(IOptions<CrusaderApiSettings> options, IHistoricalDataService historicalDataService)
        {
            _settings = options.Value;
            _historicalDataService = historicalDataService;
        }

        public async Task<float?> PredictAsync(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var ohlcvs = await _historicalDataService.GetHistoricalDataAsync(baseCurrency, quoteCurrency, resolution, from, to, cancellationToken);

            using (var client = new HttpClient {BaseAddress = new Uri(_settings.BaseUrl)})
            {
                var path = new PathString("/predict")
                    .Add($"/{baseCurrency}")
                    .Add($"/{quoteCurrency}")
                    .Add($"/{resolution}")
                    .Add($"/{from:o}")
                    .Add($"/{to:o}");

                var response = await client.PostAsJsonAsync(path.ToString().TrimStart('/'), ohlcvs.Last(), cancellationToken);
                return response.IsSuccessStatusCode ? (float?)await response.Content.ReadAsAsync<float>(cancellationToken) : null;
            }
        }
    }
}
