using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Client.Exceptions;
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

        public async Task<ReadOnlyCollection<Ohlcv>> PredictAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken)
        {
            var ohlcvs = await _historicalDataService.GetHistoricalDataAsync(currencyPairChartPeriod, cancellationToken);

            // Build the ML model.
            var mlClientResponse = await new HttpClient {BaseAddress = new Uri(_settings.MLApiBaseUrl)}.PostAsJsonAsync("ml", currencyPairChartPeriod, cancellationToken);

            if (mlClientResponse.IsSuccessStatusCode)
            {
                var predictionPeriod = new CurrencyPairChartPeriod
                {
                    CurrencyPairChart = currencyPairChartPeriod.CurrencyPairChart,
                    DateTimeInterval = new DateTimeInterval {From = DateTime.UtcNow, To = currencyPairChartPeriod.DateTimeInterval.To}
                };

                // Predict future prices.
                var capeClientResponse = await new HttpClient {BaseAddress = new Uri(_settings.CapeApiBaseUrl)}.PostAsJsonAsync("cape", predictionPeriod, cancellationToken);

                if (capeClientResponse.IsSuccessStatusCode)
                {
                    return await capeClientResponse.Content.ReadAsAsync<ReadOnlyCollection<Ohlcv>>(cancellationToken);
                }

                throw new HttpClientException($"CAPE request unsuccessful: {capeClientResponse.ReasonPhrase}") {Response = capeClientResponse};
            }

            throw new HttpClientException($"ML request unsuccessful: {mlClientResponse.ReasonPhrase}") {Response = mlClientResponse};
        }
    }
}
