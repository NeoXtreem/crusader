using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictController : ControllerBase
    {
        private readonly ILogger<PredictController> _logger;
        private readonly IPredictionService _predictionService;
        private readonly IOhlcvMappingService _ohlcvMappingService;
        private readonly IMarketDataReadRepository _marketDataReadRepository;

        public PredictController(ILogger<PredictController> logger, IPredictionService predictionService, IOhlcvMappingService ohlcvMappingService, IMarketDataReadRepository marketDataReadRepository)
        {
            _logger = logger;
            _predictionService = predictionService;
            _ohlcvMappingService = ohlcvMappingService;
            _marketDataReadRepository = marketDataReadRepository;
        }

        [HttpPost]
        [Route("{baseCurrency}/{quoteCurrency}/{resolution}/{from}/{to}")]
        public ActionResult<float> Post(string baseCurrency, string quoteCurrency, string resolution, DateTime from, DateTime to, [FromBody] Ohlcv ohlcv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _predictionService.Initialise(_ohlcvMappingService.Map(_marketDataReadRepository.GetOhlcvs(baseCurrency, quoteCurrency, Resolution.Parse(resolution), from, to)));
            _predictionService.Train();

            var ohlcvPrediction = _predictionService.Predict(_ohlcvMappingService.Map(ohlcv));

            return Ok(ohlcvPrediction.ClosePrediction);
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
