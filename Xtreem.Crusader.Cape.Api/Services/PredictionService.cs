using System;
using Microsoft.Extensions.ML;
using Xtreem.Crusader.Cape.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.Cape.Api.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly PredictionEnginePool<OhlcvInput, OhlcvPrediction> _predictionEnginePool;

        public PredictionService(PredictionEnginePool<OhlcvInput, OhlcvPrediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public OhlcvPrediction Predict(OhlcvInput ohlcv)
        {
            var prediction = _predictionEnginePool.Predict(ohlcv);
            Console.WriteLine($"Predicted close: {prediction.ClosePrediction:0.####}, actual close: {prediction.Close:0.####}");
            return prediction;
        }
    }
}
