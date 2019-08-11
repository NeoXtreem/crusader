using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;
using Xtreem.CryptoPrediction.Client.Settings;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Services
{
    public class CryptoCompareService : ICryptoCompareService
    {
        private readonly CryptoCompareSettings _settings;
        private readonly IMarketDataRepository _marketDataRepository;

        public CryptoCompareService(IOptions<CryptoCompareSettings> options, IMarketDataRepository marketDataRepository)
        {
            _settings = options.Value;
            _marketDataRepository = marketDataRepository;
        }

        public async Task<object> GetHistoricalData(string baseCurrency, string quoteCurrency, DateTimeOffset to, Resolution resolution)
        {
            using (var client = new HttpClient {BaseAddress = new Uri(_settings.BaseUrl)})
            {
                var response = await client.GetAsync(QueryHelpers.AddQueryString($"histo{resolution.ToString().ToLowerInvariant()}",
                    new (string key, string value)[]
                    {
                        ("fsym", baseCurrency),
                        ("tsym", quoteCurrency),
                        ("toTs", to.ToUniversalTime().ToUnixTimeSeconds().ToString()),
                        ("api_key", _settings.ApiKey)
                    }.ToDictionary(p => p.key, p => p.value.ToString())));

                if (response.IsSuccessStatusCode)
                {
                    var content = JObject.Parse(await response.Content.ReadAsStringAsync());

                    var data = content.SelectTokens("$.Data[*]");

                    await _marketDataRepository.AddOhlcvsAsync(data.Select(t =>
                    {
                        var ohlcv = t.ToObject<Ohlcv>();
                        ohlcv.Base = baseCurrency;
                        ohlcv.Quote = quoteCurrency;
                        ohlcv.Resolution = resolution;
                        return ohlcv;
                    }), resolution);





                    return null;
                    //return Ok(_rssItemMappingService.Map(JsonConvert.DeserializeObject<IEnumerable<object>>(await response.Content.ReadAsStringAsync())));
                }

                return null;

            }

        }
    }
}
