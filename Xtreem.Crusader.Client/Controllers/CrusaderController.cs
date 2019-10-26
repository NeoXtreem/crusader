using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.ViewModels;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrusaderController : Controller
    {
        private readonly ILogger<CrusaderController> _logger;
        private readonly IMLService _mlService;
        private readonly ICurrencyPairChartPeriodMappingService _currencyPairChartPeriodMappingService;
        private CancellationTokenSource _cts;

        public CrusaderController(ILogger<CrusaderController> logger, IMLService mlService, ICurrencyPairChartPeriodMappingService currencyPairChartPeriodMappingService)
        {
            _logger = logger;
            _mlService = mlService;
            _currencyPairChartPeriodMappingService = currencyPairChartPeriodMappingService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ReadOnlyCollection<Ohlcv>>> Predict(CurrencyPairChartPeriodViewModel currencyPairChartPeriod)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            return Ok(await _mlService.PredictAsync(_currencyPairChartPeriodMappingService.Map(currencyPairChartPeriod), _cts.Token));
        }
    }
}
