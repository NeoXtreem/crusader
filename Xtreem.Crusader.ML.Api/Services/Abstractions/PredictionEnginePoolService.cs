using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal abstract class PredictionEnginePoolService<TPrediction> : PredictionService where TPrediction : class, new()
    {
        protected readonly LazyService<PredictionEnginePool<OhlcvInput, TPrediction>> LazyPredictionEnginePool;

        protected PredictionEnginePoolService(IOptionsFactory<ModelOptions> optionsFactory, LazyService<PredictionEnginePool<OhlcvInput, TPrediction>> lazyPredictionEnginePool)
            : base(optionsFactory)
        {
            LazyPredictionEnginePool = lazyPredictionEnginePool;
        }
    }
}
