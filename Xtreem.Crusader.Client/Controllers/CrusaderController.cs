using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.ViewModels;
using Xtreem.Crusader.Data.Types;

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
        public async Task<ActionResult<float?>> Predict(CurrencyPairChartPeriodViewModel currencyPairChartPeriod)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            currencyPairChartPeriod.Resolution = Resolution.Minute.ToString();
            currencyPairChartPeriod.From = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1000));
            currencyPairChartPeriod.To = DateTime.UtcNow;

            var close = await _mlService.PredictAsync(_currencyPairChartPeriodMappingService.Map(currencyPairChartPeriod), _cts.Token);

            return Ok(close);
        }
    }
}
