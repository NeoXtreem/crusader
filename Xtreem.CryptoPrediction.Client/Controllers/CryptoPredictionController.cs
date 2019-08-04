using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net.Objects;
using Microsoft.AspNetCore.Mvc;
using Xtreem.CryptoPrediction.Service.Services.Interfaces;

namespace Xtreem.CryptoPrediction.Client.Controllers
{
    [Route("api/[controller]")]
    public class CryptoPredictionController : Controller
    {
        private readonly IBinanceService _binanceService;
        private readonly IPredictionService _predictionService;

        public CryptoPredictionController(
            IBinanceService binanceService,
            IPredictionService predictionService)
        {
            _binanceService = binanceService;
            _predictionService = predictionService;
        }

        [HttpGet("[action]/{symbol}/{pageIndex}/{pageSize}")]
        [Route("predict/{symbol}")]
        public async Task<ActionResult<HerdViewModel>> Predict(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            //const string symbol = "BNBBTC";
            //const KlineInterval interval = KlineInterval.OneMinute;
            //var klines = _binanceService.GetKlines(symbol, interval, DateTime.UtcNow.AddMinutes(-60), DateTime.UtcNow, 60);

            var klines = _binanceService.GetKlines(symbol, interval, startTime, endTime, limit);

            _predictionService.Initialise(klines);
            _binanceService.SubscribeToKlines(symbol, interval, _predictionService.Predict);


            return Ok(new HerdViewModel
            {
                LabYaks = _labYakModelMappingService.Map(await _labYakPagerService.LoadPageAsync(day, pageIndex, pageSize))
            });
        }

    }
}
