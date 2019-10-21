using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Services.Interfaces;

namespace Xtreem.Crusader.ML.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MLController : ControllerBase
    {
        private readonly ILogger<MLController> _logger;
        private readonly IModelService _modelService;
        private readonly IOhlcvMappingService _ohlcvMappingService;
        private readonly IMarketDataReadRepository _marketDataReadRepository;

        public MLController(ILogger<MLController> logger, IModelService modelService, IOhlcvMappingService ohlcvMappingService, IMarketDataReadRepository marketDataReadRepository)
        {
            _logger = logger;
            _modelService = modelService;
            _ohlcvMappingService = ohlcvMappingService;
            _marketDataReadRepository = marketDataReadRepository;
        }

        [HttpPost]
        public ActionResult Post(CurrencyPairChartPeriod currencyPairChartPeriod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _modelService.Initialise(_ohlcvMappingService.Map(_marketDataReadRepository.GetOhlcvs(currencyPairChartPeriod)));
            _modelService.Train<OhlcvInput>();

            return Ok();
        }

        [HttpGet("[action]")]
        public ActionResult Model()
        {
            return File(_modelService.GetModel(), MediaTypeNames.Application.Zip);
        }
    }
}
