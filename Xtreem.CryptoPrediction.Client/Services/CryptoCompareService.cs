using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Xtreem.CryptoPrediction.Client.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;
using Xtreem.CryptoPrediction.Client.Settings;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Services
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

        public async Task<IEnumerable<Ohlcv>> LoadHistoricalData(string baseCurrency, string quoteCurrency, DateTime from, DateTime to, Resolution resolution)
        {
            const int maxLimit = 2000;
            var ohlcvs = new List<Ohlcv>();

            for (var batchTo = to; batchTo > from; batchTo = batchTo.Subtract(maxLimit * resolution.Interval))
            {
                using (var client = new HttpClient {BaseAddress = new Uri(_settings.BaseUrl)})
                {
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
                        }.ToDictionary(p => p.key, p => p.value.ToString()))))
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
