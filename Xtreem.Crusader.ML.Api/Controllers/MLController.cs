using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Api.Services.Interfaces;

namespace Xtreem.Crusader.ML.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MLController : ControllerBase
    {
        private readonly ILogger<MLController> _logger;
        private readonly IPredictionService _predictionService;

        public MLController(ILogger<MLController> logger, IPredictionService predictionService)
        {
            _logger = logger;
            _predictionService = predictionService;
        }

        [HttpPost]
        public ActionResult<ReadOnlyCollection<Ohlcv>> Post(CurrencyPairChartPeriod currencyPairChartPeriod)
        {
            return Ok(_predictionService.Predict(currencyPairChartPeriod));
        }
    }
}
