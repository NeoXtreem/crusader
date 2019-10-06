using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.Settings;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Client.Services
{
    public class MLService : IMLService
    {
        private readonly CrusaderApiSettings _settings;
        private readonly IHistoricalDataService _historicalDataService;

        public MLService(IOptions<CrusaderApiSettings> options, IHistoricalDataService historicalDataService)
        {
            _settings = options.Value;
            _historicalDataService = historicalDataService;
        }

        public async Task<float?> PredictAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken)
        {
            var ohlcvs = await _historicalDataService.GetHistoricalDataAsync(currencyPairChartPeriod, cancellationToken);

            using (var mlClient = new HttpClient {BaseAddress = new Uri(_settings.MLApiBaseUrl)})
            {
                if ((await mlClient.PostAsJsonAsync("ml", currencyPairChartPeriod, cancellationToken)).IsSuccessStatusCode)
                {
                    using (var capeClient = new HttpClient {BaseAddress = new Uri(_settings.CapeApiBaseUrl)})
                    {
                        var response = await capeClient.PostAsJsonAsync("cape", ohlcvs.Last(), cancellationToken);

                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsAsync<float>(cancellationToken);
                        }
                    }
                }
            }

            return null;
        }
    }
}
