using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Controllers
{
    [Route("api/[controller]")]
    public class CrusaderController : Controller
    {
        private readonly ILogger<CrusaderController> _logger;
        private readonly IPredictionService _predictionService;
        private CancellationTokenSource _cancellationTokenSource;

        public CrusaderController(ILogger<CrusaderController> logger, IPredictionService predictionService)
        {
            _logger = logger;
            _predictionService = predictionService;
        }

        [HttpPost("[action]")]
        [Route("[action]/{baseCurrency}/{quoteCurrency}")]
        public async Task<ActionResult<object>> Predict(string baseCurrency, string quoteCurrency/*, DateTime to*/)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            var close = await _predictionService.PredictAsync(baseCurrency, quoteCurrency, Resolution.Minute, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1000)), DateTime.UtcNow, _cancellationTokenSource.Token);

            return Ok();
        }
    }
}
