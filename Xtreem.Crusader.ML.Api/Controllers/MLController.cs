using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.ML.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MLController : ControllerBase
    {
        private readonly ILogger<MLController> _logger;
        private readonly IEnumerable<IPredictionService> _predictionServices;

        public MLController(ILogger<MLController> logger, IEnumerable<IPredictionService> predictionServices)
        {
            _logger = logger;
            _predictionServices = predictionServices;
        }

        [HttpPost]
        public ActionResult<ReadOnlyCollection<Ohlcv>> Post(CurrencyPairChartPeriod currencyPairChartPeriod)
        {
            return Ok(_predictionServices.Single(s => s.CanPredict()).Predict(currencyPairChartPeriod));
        }
    }
}
