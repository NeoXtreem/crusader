using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Cape.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.Cape.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CapeController : ControllerBase
    {
        private readonly ILogger<CapeController> _logger;
        private readonly IPredictionService _predictionService;

        public CapeController(ILogger<CapeController> logger, IPredictionService predictionService)
        {
            _logger = logger;
            _predictionService = predictionService;
        }

        [HttpPost]
        public ActionResult<ReadOnlyCollection<OhlcvPrediction>> Post(CurrencyPairChartPeriod predictionPeriod)
        {
            return Ok(_predictionService.Predict(predictionPeriod));
        }
    }
}
