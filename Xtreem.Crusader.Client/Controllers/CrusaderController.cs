using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtreem.Crusader.Client.Profiles;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.ViewModels;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Services.Abstractions.Interfaces;

namespace Xtreem.Crusader.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrusaderController : Controller
    {
        private readonly ILogger<CrusaderController> _logger;
        private readonly IMLService _mlService;
        private readonly IMapper _mapper;
        private CancellationTokenSource _cts;

        public CrusaderController(ILogger<CrusaderController> logger, IMLService mlService, IMappingService mappingService)
        {
            _logger = logger;
            _mlService = mlService;
            _mapper = mappingService.GetMapper<CurrencyPairChartPeriodProfile>();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ReadOnlyCollection<Ohlcv>>> Predict(CurrencyPairChartPeriodViewModel currencyPairChartPeriod)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            return Ok(await _mlService.PredictAsync(_mapper.Map<CurrencyPairChartPeriodViewModel, CurrencyPairChartPeriod>(currencyPairChartPeriod), _cts.Token));
        }
    }
}
