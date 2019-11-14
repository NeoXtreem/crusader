using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Primitives;
using Microsoft.ML;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Shared.Types;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class TrainModelLoader : ModelLoader
    {
        private readonly IEnumerable<IModelService> _modelServices;
        private readonly IMapper _mapper;
        private readonly IHistoricalDataService _historicalDataService;
        private CancellationTokenSource _cts;

        public TrainModelLoader(IEnumerable<IModelService> modelServices, IMapper mapper, IHistoricalDataService historicalDataService)
        {
            _modelServices = modelServices;
            _mapper = mapper;
            _historicalDataService = historicalDataService;
        }

        public override ITransformer GetModel()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var toDate = DateTime.UtcNow;

            return _modelServices.Single(s => s.CanUse()).Train(_mapper.Map<IEnumerable<OhlcvInput>>(_historicalDataService.GetHistoricalDataAsync(new CurrencyPairChartPeriod
            {
                CurrencyPairChart = new CurrencyPairChart
                {
                    CurrencyPair = new CurrencyPair {BaseCurrency = "BTC", QuoteCurrency = "USD"},
                    Resolution = Resolution.Minute
                },
                DateTimeInterval = new DateTimeInterval {From = toDate.AddHours(-1), To = toDate}
            }, _cts.Token).Result));
        }

        public override IChangeToken GetReloadToken() => new CancellationChangeToken(CancellationToken.None);
    }
}
