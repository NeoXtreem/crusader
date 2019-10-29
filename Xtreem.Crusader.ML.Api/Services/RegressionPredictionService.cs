using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Api.Profiles;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Settings;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class RegressionPredictionService : PredictionService<OhlcvRegressionPrediction>
    {
        private readonly IMappingService _mappingService;

        public RegressionPredictionService(
            PredictionEnginePool<OhlcvInput, OhlcvRegressionPrediction> predictionEnginePool,
            IMappingService mappingService,
            IOptions<ModelSettings> modelOptions)
            : base(predictionEnginePool, modelOptions)
        {
            _mappingService = mappingService;
        }

        protected override ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs)
        {
            return _mappingService.GetMapper<OhlcvProfile>().Map<IEnumerable<Ohlcv>>(ohlcvs.Select(o => PredictionEnginePool.Predict(o))).AsReadOnly();
        }
    }
}
