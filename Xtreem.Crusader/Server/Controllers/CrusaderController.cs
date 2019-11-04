using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Server.Services.Interfaces;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrusaderController : Controller
    {
        private readonly ILogger<CrusaderController> _logger;
        private readonly IMLService _mlService;
        private CancellationTokenSource _cts;

        public CrusaderController(ILogger<CrusaderController> logger, IMLService mlService)
        {
            _logger = logger;
            _mlService = mlService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ReadOnlyCollection<Ohlcv>>> Predict(CurrencyPairChartPeriod currencyPairChartPeriod)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            return Ok(await _mlService.PredictAsync(currencyPairChartPeriod, _cts.Token));
        }
    }
}
