﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime;
using Xtreem.Crusader.Server.Models;
using Xtreem.Crusader.Server.Repositories.Interfaces;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Shared.Types;
using Type = Xtreem.Crusader.Server.Types.Type;

namespace Xtreem.Crusader.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UdfFeedController : ControllerBase
    {
        private readonly ILogger<UdfFeedController> _logger;
        private readonly IMarketDataReadViewRepository _marketDataReadViewRepository;

        public UdfFeedController(ILogger<UdfFeedController> logger, IMarketDataReadViewRepository marketDataReadViewRepository)
        {
            _logger = logger;
            _marketDataReadViewRepository = marketDataReadViewRepository;
        }

        [HttpGet]
        [Route("config")]
        public ActionResult<object> Config()
        {
            return new
            {
                supported_resolutions = new[] {"1", "5", "15", "30", "60", "D", "W", "M"},
                supports_group_request = false,
                supports_marks = false,
                supports_search = true,
                supports_timescale_marks = false
            };
        }

        [HttpGet]
        [Route("symbols")]
        public ActionResult<SymbolInfoResponse> Symbols(string symbol)
        {
            return new SymbolInfoResponse
            {
                Name = symbol,
                ExchangeTraded = "Binance",
                ExchangeListed = "Binance",
                Timezone = DateTimeZoneProviders.Tzdb.GetSystemDefault().ToString(),
                MinMov = 1,
                Session = "24x7",
                HasIntraday = true,
                Description = "BTCUSDT",
                Type = Type.Crypto,
                SupportedResolutions = new[] {"1", "60", "D", "W"},
                PriceScale = 100
            };
        }

        [HttpGet]
        [Route("search")]
        public ActionResult<object> Search(string query, string type, string exchange, int limit)
        {
            return null;
        }

        [HttpGet]
        [Route("history")]
        public ActionResult<StatusResponse> History(string symbol, long from, long to, string resolution)
        {
            _logger.LogInformation($"Requesting history for {symbol} from {DateTimeOffset.FromUnixTimeSeconds(from)} to {DateTimeOffset.FromUnixTimeSeconds(to)} at {resolution} resolution.");

            var ohlcvs = _marketDataReadViewRepository.GetOhlcvs(new CurrencyPairChart
            {
                CurrencyPair = new CurrencyPair {BaseCurrency = symbol, QuoteCurrency = "USD"},
                Resolution = Resolution.Parse(resolution)
            }, from, to).OrderBy(o => o.Time).ToArray();

            if (ohlcvs.Any())
            {
                _logger.LogInformation($"Found {ohlcvs.Length} records from {DateTimeOffset.FromUnixTimeSeconds(ohlcvs.First().Time)} to {DateTimeOffset.FromUnixTimeSeconds(ohlcvs.Last().Time)}");
                return new HistoryResponse
                {
                    T = ohlcvs.Select(o => o.Time).ToArray(),
                    O = ohlcvs.Select(o => o.Open).ToArray(),
                    L = ohlcvs.Select(o => o.Low).ToArray(),
                    H = ohlcvs.Select(o => o.High).ToArray(),
                    C = ohlcvs.Select(o => o.Close).ToArray(),
                    V = ohlcvs.Select(o => o.VolumeFrom).ToArray()
                };
            }

            var nextTime = _marketDataReadViewRepository.GetNextTime(symbol, "USD", Resolution.Parse(resolution), from);
            _logger.LogInformation("No data available." + (nextTime != default ? $" Next time with data is {DateTimeOffset.FromUnixTimeSeconds(nextTime)}" : String.Empty));
            return new NoDataResponse {NextTime = nextTime};
        }
    }
}
