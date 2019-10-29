using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Primitives;
using Microsoft.ML;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Services.Abstractions.Interfaces;
using Xtreem.Crusader.Data.Types;
using Xtreem.Crusader.ML.Api.Profiles;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.ML.Api.Loaders
{
    [Inject, UsedImplicitly]
    internal class TrainModelLoader : ModelLoader
    {
        private readonly IModelService _modelService;
        private readonly IMappingService _mappingService;
        private readonly IHistoricalDataService _historicalDataService;
        private CancellationTokenSource _cts;

        public TrainModelLoader(IModelService modelService, IMappingService mappingService, IHistoricalDataService historicalDataService)
        {
            _modelService = modelService;
            _mappingService = mappingService;
            _historicalDataService = historicalDataService;
        }

        public override ITransformer GetModel()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            return _modelService.Train(_mappingService.GetMapper<OhlcvProfile>().Map<IEnumerable<OhlcvInput>>(_historicalDataService.GetHistoricalDataAsync(new CurrencyPairChartPeriod
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
