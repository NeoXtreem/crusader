using System;
using System.Threading;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Primitives;
using Microsoft.ML;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Types;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Services.Interfaces;

namespace Xtreem.Crusader.ML.Api.Loaders
{
    internal class TrainModelLoader : ModelLoader
    {
        private readonly IModelService _modelService;
        private readonly IOhlcvMappingService _ohlcvMappingService;
        private readonly IHistoricalDataService _historicalDataService;
        private CancellationTokenSource _cts;

        public TrainModelLoader(IModelService modelService, IOhlcvMappingService ohlcvMappingService, IHistoricalDataService historicalDataService)
        {
            _modelService = modelService;
            _ohlcvMappingService = ohlcvMappingService;
            _historicalDataService = historicalDataService;
        }

        public override ITransformer GetModel()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            return _modelService.Train<OhlcvInput>(_ohlcvMappingService.Map(_historicalDataService.GetHistoricalDataAsync(new CurrencyPairChartPeriod
            {
                CurrencyPairChart = new CurrencyPairChart
                {
                    CurrencyPair = new CurrencyPair {BaseCurrency = "BTC", QuoteCurrency = "USD"},
                    Resolution = Resolution.Minute
                },
                DateTimeInterval = new DateTimeInterval {From = DateTime.UtcNow.AddHours(-1), To = DateTime.UtcNow}
            }, _cts.Token).Result));
        }

        public override IChangeToken GetReloadToken() => new CancellationChangeToken(CancellationToken.None);
    }
}
