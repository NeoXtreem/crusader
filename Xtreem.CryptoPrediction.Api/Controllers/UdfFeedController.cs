using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Xtreem.CryptoPrediction.Api.Models;
using Xtreem.CryptoPrediction.Api.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Common.Helpers;
using Xtreem.CryptoPrediction.Data.Types;
using Type = Xtreem.CryptoPrediction.Api.Types.Type;

namespace Xtreem.CryptoPrediction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UdfFeedController : ControllerBase
    {
        private readonly IMarketDataRepository _marketDataRepository;

        public UdfFeedController(IMarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        [HttpGet]
        [Route("config")]
        public ActionResult<object> Config()
        {
            return new
            {
                supported_resolutions = new[] { "1", "5", "15", "30", "60", "1D", "1W", "1M" },
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
                SupportedResolutions = new[] { "1", "60", "1D", "W" },
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
        public async Task<ActionResult<StatusResponse>> History(string symbol, long from, long to, string resolution)
        {
            var ohlcvs = (await _marketDataRepository.GetOhlcvsAsync(symbol, "USD", EnumEx.GetValueFromDescription<Resolution>(resolution), from, to)).ToArray();

            if (ohlcvs.Any())
            {
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

            return new NoDataResponse
            {
                NextTime = await _marketDataRepository.GetNextTimeAsync(symbol, "USD", EnumEx.GetValueFromDescription<Resolution>(resolution), from)
            };
        }
    }
}
