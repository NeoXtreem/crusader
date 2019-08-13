using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Controllers
{
    [Route("api/[controller]")]
    public class CryptoPredictionController : Controller
    {
        private readonly ILogger<CryptoPredictionController> _logger;
        private readonly IHistoricalDataService _historicalDataService;
        private readonly IBinanceService _binanceService;
        private readonly IPredictionService _predictionService;

        public CryptoPredictionController(
            ILogger<CryptoPredictionController> logger,
            IHistoricalDataService historicalDataService,
            IBinanceService binanceService,
            IPredictionService predictionService)
        {
            _logger = logger;
            _historicalDataService = historicalDataService;
            _binanceService = binanceService;
            _predictionService = predictionService;
        }

        [HttpPost("[action]")]
        [Route("[action]/{baseCurrency}/{quoteCurrency}")]
        public async Task<ActionResult<object>> Predict(string baseCurrency, string quoteCurrency/*, DateTime to*/)
        {
            var ohlcvs = await _historicalDataService.GetHistoricalData(baseCurrency, quoteCurrency, Resolution.Minute, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1000)), DateTime.UtcNow);
            //_predictionService.Predict();

            return Ok();
        }

        //[HttpGet("[action]/{symbol}/{pageIndex}/{pageSize}")]
        //[Route("predict/{symbol}")]
        //public async Task<ActionResult<object>> Predict(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        //{
        //    //const string symbol = "BNBBTC";
        //    //const KlineInterval interval = KlineInterval.OneMinute;
        //    //var klines = _binanceService.GetKlines(symbol, interval, DateTime.UtcNow.AddMinutes(-60), DateTime.UtcNow, 60);

        //    var klines = _binanceService.GetKlines(symbol, interval, startTime, endTime, limit);

        //    _predictionService.Initialise(klines);
        //    _binanceService.SubscribeToKlines(symbol, interval, _predictionService.Predict);


        //    //return Ok(new HerdViewModel
        //    //{
        //    //    LabYaks = _labYakModelMappingService.Map(await _labYakPagerService.LoadPageAsync(day, pageIndex, pageSize))
        //    //});
        //}
    }
}
