using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Xtreem.Crusader.Client.Repositories.Interfaces;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.Settings;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Services
{
    public class CryptoCompareService : ICryptoCompareService
    {
        private readonly CryptoCompareSettings _settings;
        private readonly IMarketDataReadWriteRepository _marketDataReadWriteRepository;

        public CryptoCompareService(IOptions<CryptoCompareSettings> options, IMarketDataReadWriteRepository marketDataReadWriteRepository)
        {
            _settings = options.Value;
            _marketDataReadWriteRepository = marketDataReadWriteRepository;
        }

        public async Task<IEnumerable<Ohlcv>> LoadHistoricalDataAsync(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            const int maxLimit = 2000;
            var ohlcvs = new List<Ohlcv>();

            // Handle the requested period in batches based on the limitation of the provider API.
            for (var batchTo = to; batchTo > from; batchTo = batchTo.Subtract(maxLimit * resolution.Interval))
            {
                if (cancellationToken.IsCancellationRequested) break;

                using (var client = new HttpClient {BaseAddress = new Uri(_settings.BaseUrl)})
                {
                    // Calculate the limit to pass based on the size of the current batch.
                    var limit = (batchTo - from > maxLimit * resolution.Interval ? maxLimit : resolution.IntervalsInPeriod(batchTo - from)) - 1;
                    if (limit < 0) continue;

                    using (var response = await client.GetAsync(QueryHelpers.AddQueryString($"histo{resolution.ToString().ToLowerInvariant()}",
                        new (string key, string value)[]
                        {
                            ("fsym", baseCurrency),
                            ("tsym", quoteCurrency),
                            ("toTs", ((DateTimeOffset)batchTo).ToUniversalTime().ToUnixTimeSeconds().ToString()),
                            ("api_key", _settings.ApiKey),
                            ("limit", limit.ToString())
                        }.ToDictionary(p => p.key, p => p.value.ToString())), cancellationToken))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ohlcvs.AddRange(JObject.Parse(await response.Content.ReadAsStringAsync()).SelectTokens("$.Data[*]").Select(t =>
                            {
                                var ohlcv = t.ToObject<Ohlcv>();
                                ohlcv.Base = baseCurrency;
                                ohlcv.Quote = quoteCurrency;
                                ohlcv.Resolution = resolution.ToString();
                                return ohlcv;
                            }));
                        }
                    }
                }
            }

            await _marketDataReadWriteRepository.AddOhlcvsAsync(ohlcvs, resolution);
            return ohlcvs;
        }
    }
}
