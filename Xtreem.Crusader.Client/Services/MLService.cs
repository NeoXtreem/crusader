using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.Settings;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Utilities.Exceptions;

namespace Xtreem.Crusader.Client.Services
{
    internal class MLService : IMLService
    {
        private readonly CrusaderApiSettings _settings;

        public MLService(IOptions<CrusaderApiSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<ReadOnlyCollection<Ohlcv>> PredictAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken)
        {
            var response = await new HttpClient {BaseAddress = new Uri(_settings.MLApiBaseUrl)}.PostAsJsonAsync("ml", currencyPairChartPeriod, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ReadOnlyCollection<Ohlcv>>(cancellationToken);
            }

            throw new HttpClientException($"ML request unsuccessful: {response.ReasonPhrase}") {Response = response};
        }
    }
}
