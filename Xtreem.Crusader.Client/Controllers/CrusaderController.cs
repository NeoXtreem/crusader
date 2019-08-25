using System;
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
        private readonly IHistoricalDataService _historicalDataService;

        public CrusaderController(ILogger<CrusaderController> logger, IHistoricalDataService historicalDataService)
        {
            _logger = logger;
            _historicalDataService = historicalDataService;
        }

        [HttpPost("[action]")]
        [Route("[action]/{baseCurrency}/{quoteCurrency}")]
        public async Task<ActionResult<object>> Predict(string baseCurrency, string quoteCurrency/*, DateTime to*/)
        {
            var ohlcvs = await _historicalDataService.GetHistoricalData(baseCurrency, quoteCurrency, Resolution.Minute, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1000)), DateTime.UtcNow);
            //_predictionService.Predict();

            return Ok();
        }
    }
}
