﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.ML;
using Xtreem.Crusader.Cape.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities;

namespace Xtreem.Crusader.Cape.Api.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly PredictionEnginePool<OhlcvInput, OhlcvPrediction> _predictionEnginePool;

        public PredictionService(PredictionEnginePool<OhlcvInput, OhlcvPrediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public ReadOnlyCollection<OhlcvPrediction> Predict(CurrencyPairChartPeriod predictionPeriod)
        {
            var ohlcvs = Enumerable.Range(0, Math.Max(0, predictionPeriod.CurrencyPairChart.Resolution.IntervalsInPeriod(predictionPeriod.DateTimeInterval.To - predictionPeriod.DateTimeInterval.From)))
                .Select(i => new OhlcvInput
                {
                    Base = predictionPeriod.CurrencyPairChart.CurrencyPair.BaseCurrency,
                    Quote = predictionPeriod.CurrencyPairChart.CurrencyPair.QuoteCurrency,
                    Time = ((DateTimeOffset)(predictionPeriod.DateTimeInterval.From + predictionPeriod.CurrencyPairChart.Resolution.Interval * i)).ToUnixTimeSeconds()
                });

            return ohlcvs.Select(o => _predictionEnginePool.Predict(o)).AsReadOnly();
        }
    }
}
