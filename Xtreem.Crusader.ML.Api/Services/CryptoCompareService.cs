using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Api.Exceptions;
using Xtreem.Crusader.ML.Api.Models;
using Xtreem.Crusader.ML.Api.Repositories.Interfaces;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Api.Settings;
using Xtreem.Crusader.Utilities.Exceptions;

namespace Xtreem.Crusader.ML.Api.Services
{
    internal class CryptoCompareService : ICryptoCompareService
    {
        private readonly CryptoCompareSettings _settings;
        private readonly IMarketDataReadWriteRepository _marketDataReadWriteRepository;

        public CryptoCompareService(IOptions<CryptoCompareSettings> options, IMarketDataReadWriteRepository marketDataReadWriteRepository)
        {
            _settings = options.Value;
            _marketDataReadWriteRepository = marketDataReadWriteRepository;
        }

        public async Task<IEnumerable<Ohlcv>> LoadHistoricalDataAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken)
        {
            const int maxLimit = 2000;
            var ohlcvs = new List<Ohlcv>();

            var currencyPairChart = currencyPairChartPeriod.CurrencyPairChart;
            var currencyPair = currencyPairChart.CurrencyPair;
            var from = currencyPairChartPeriod.DateTimeInterval.From;
            var to = currencyPairChartPeriod.DateTimeInterval.To;
            var resolution = currencyPairChart.Resolution;
            var baseCurrency = currencyPair.BaseCurrency;
            var quoteCurrency = currencyPair.QuoteCurrency;

            // Handle the requested period in batches based on the limitation of the provider API.
            for (var batchTo = to; batchTo > from; batchTo = batchTo.Subtract(maxLimit * resolution.Interval))
            {
                if (cancellationToken.IsCancellationRequested) break;

                using var client = new HttpClient {BaseAddress = new Uri(_settings.BaseUrl)};

                // Calculate the limit to pass based on the size of the current batch.
                var limit = Math.Min(resolution.IntervalsInPeriod(batchTo - from), maxLimit);
                if (limit < 1) continue;

                using var response = await client.GetAsync(QueryHelpers.AddQueryString($"histo{resolution.ToString().ToLowerInvariant()}",
                    new (string key, string value)[]
                    {
                        ("fsym", baseCurrency),
                        ("tsym", quoteCurrency),
                        ("toTs", ((DateTimeOffset)batchTo).ToUniversalTime().ToUnixTimeSeconds().ToString()),
                        ("api_key", _settings.ApiKey),
                        ("limit", limit.ToString())
                    }.ToDictionary(p => p.key, p => p.value.ToString())), cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var historicalDataResponse = JsonConvert.DeserializeObject<HistoricalDataResponse>(await response.Content.ReadAsStringAsync());

                    if (historicalDataResponse.Response == "Success")
                    {
                        ohlcvs.AddRange(historicalDataResponse.Data.Select(o =>
                        {
                            o.Base = baseCurrency;
                            o.Quote = quoteCurrency;
                            o.Resolution = resolution.ToString();
                            return o;
                        }));
                    }
                    else
                    {
                        throw new HistoricalDataException(historicalDataResponse.Message) {Response = historicalDataResponse};
                    }
                }
                else
                {
                    throw new HttpClientException($"Historical data request unsuccessful: {response.ReasonPhrase}") {Response = response};
                }
            }

            await _marketDataReadWriteRepository.AddOhlcvsAsync(ohlcvs);
            return ohlcvs;
        }
    }
}
