using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class RegressionPredictionService : PredictionEnginePoolService<OhlcvRegressionPrediction>
    {
        private readonly IMapper _mapper;

        public RegressionPredictionService(IOptionsFactory<ModelOptions> optionsFactory, LazyService<PredictionEnginePool<OhlcvInput, OhlcvRegressionPrediction>> lazyPredictionEnginePool, IMapper mapper)
            : base(optionsFactory, lazyPredictionEnginePool)
        {
            _mapper = mapper;
        }

        protected override ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs)
        {
            return _mapper.Map<IEnumerable<Ohlcv>>(ohlcvs.Select(o => LazyPredictionEnginePool.Value.Predict(o))).AsReadOnly();
        }
    }
}
