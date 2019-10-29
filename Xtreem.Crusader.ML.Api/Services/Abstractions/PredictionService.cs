using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Settings;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal abstract class PredictionService<TPrediction> : IPredictionService where TPrediction : class, new()
    {
        protected readonly PredictionEnginePool<OhlcvInput, TPrediction> PredictionEnginePool;
        private readonly ModelSettings _modelSettings;

        protected PredictionService(PredictionEnginePool<OhlcvInput, TPrediction> predictionEnginePool, IOptions<ModelSettings> modelOptions)
        {
            PredictionEnginePool = predictionEnginePool;
            _modelSettings = modelOptions.Value;
        }

        public bool CanPredict() => GetType().Name.StartsWith(_modelSettings.Type);

        public ReadOnlyCollection<Ohlcv> Predict(CurrencyPairChartPeriod predictionPeriod)
        {
            var ohlcvs = Enumerable.Range(0, Math.Max(0, predictionPeriod.CurrencyPairChart.Resolution.IntervalsInPeriod(predictionPeriod.DateTimeInterval.To - predictionPeriod.DateTimeInterval.From)))
                .Select(i => new OhlcvInput
                {
                    Base = predictionPeriod.CurrencyPairChart.CurrencyPair.BaseCurrency,
                    Quote = predictionPeriod.CurrencyPairChart.CurrencyPair.QuoteCurrency,
                    Resolution = predictionPeriod.CurrencyPairChart.Resolution.ToString(),
                    Time = ((DateTimeOffset)(predictionPeriod.DateTimeInterval.From + predictionPeriod.CurrencyPairChart.Resolution.Interval * i)).ToUnixTimeSeconds()
                });

            return Predict(ohlcvs);
        }

        protected abstract ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs);
    }
}
