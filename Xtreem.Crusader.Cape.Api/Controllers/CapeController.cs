﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Cape.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Data.Services.Interfaces;

namespace Xtreem.Crusader.Cape.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapeController : ControllerBase
    {
        private readonly ILogger<CapeController> _logger;
        private readonly IPredictionService _predictionService;
        private readonly IOhlcvMappingService _ohlcvMappingService;

        public CapeController(ILogger<CapeController> logger, IPredictionService predictionService, IOhlcvMappingService ohlcvMappingService)
        {
            _logger = logger;
            _predictionService = predictionService;
            _ohlcvMappingService = ohlcvMappingService;
        }

        [HttpPost]
        public ActionResult<float> Post(Ohlcv ohlcv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var ohlcvPrediction = _predictionService.Predict(_ohlcvMappingService.Map(ohlcv));

            return Ok(ohlcvPrediction.ClosePrediction);
        }
    }
}
