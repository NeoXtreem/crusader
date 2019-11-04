﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Data.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Api.Profiles;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class RegressionPredictionService : PredictionService<OhlcvRegressionPrediction>
    {
        private readonly IMappingService _mappingService;

        public RegressionPredictionService(
            IOptionsFactory<ModelOptions> optionsFactory,
            LazyService<PredictionEnginePool<OhlcvInput, OhlcvRegressionPrediction>> lazyPredictionEnginePool,
            IMappingService mappingService)
            : base(optionsFactory, lazyPredictionEnginePool)
        {
            _mappingService = mappingService;
        }

        protected override ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs)
        {
            return _mappingService.GetMapper<OhlcvProfile>().Map<IEnumerable<Ohlcv>>(ohlcvs.Select(o => LazyPredictionEnginePool.Value.Predict(o))).AsReadOnly();
        }
    }
}