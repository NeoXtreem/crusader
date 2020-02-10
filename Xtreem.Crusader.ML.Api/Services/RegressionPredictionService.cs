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
    internal class RegressionPredictionService : PredictionEnginePoolService<OhlcvPrediction>
    {
        private readonly TrainModelLoader _trainModelLoader;
        private readonly IMapper _mapper;

        public RegressionPredictionService(IOptionsFactory<ModelOptions> optionsFactory, TrainModelLoader trainModelLoader, LazyService<PredictionEnginePool<OhlcvInput, OhlcvPrediction>> lazyPredictionEnginePool, IMapper mapper)
            : base(optionsFactory, lazyPredictionEnginePool)
        {
            _trainModelLoader = trainModelLoader;
            _mapper = mapper;
        }

        protected override ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs)
        {
            _trainModelLoader.GetModel();
            return _mapper.Map<IEnumerable<Ohlcv>>(ohlcvs.Select(o => LazyPredictionEnginePool.Value.Predict(o))).AsReadOnly();
        }
    }
}
