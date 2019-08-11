using System;
using System.Collections.Generic;
using Microsoft.ML;
using Xtreem.CryptoPrediction.Client.Models;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;

namespace Xtreem.CryptoPrediction.Client.Services
{
    public class PredictionService : IPredictionService
    {
        private MLContext _mlContext;
        private ITransformer _model;

        public void Initialise(IEnumerable<InputKline> klines)
        {
            _mlContext = new MLContext(0);
            var dataView = _mlContext.Data.LoadFromEnumerable(klines);
            _model = Train(_mlContext, dataView);
            Evaluate(_mlContext, dataView, _model);
        }

        private static ITransformer Train(MLContext mlContext, IDataView dataView)
        {
            var pipeline = mlContext.Transforms.CopyColumns("Label", nameof(PredictionKline.Close))
                .Append(mlContext.Transforms.Conversion.ConvertType(new[] { new InputOutputColumnPair(nameof(InputKline.OpenTime)), new InputOutputColumnPair(nameof(InputKline.CloseTime)) }))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(InputKline.Open), nameof(InputKline.Close), nameof(InputKline.Low), nameof(InputKline.High), nameof(InputKline.OpenTime), nameof(InputKline.CloseTime), nameof(InputKline.Volume), nameof(InputKline.QuoteAssetVolume), nameof(InputKline.TradeCount), nameof(InputKline.TakerBuyBaseAssetVolume), nameof(InputKline.TakerBuyQuoteAssetVolume)))
                .Append(mlContext.Regression.Trainers.FastTree());

            return pipeline.Fit(dataView);
        }

        private static void Evaluate(MLContext mlContext, IDataView dataView, ITransformer model)
        {
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions);

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine(new string('-', 25));
            Console.WriteLine($" RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($" Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}");
        }

        public void Predict(InputKline kline)
        {
            var close = kline.Close;
            kline.Close = 0;

            var predictionFunction = _mlContext.Model.CreatePredictionEngine<InputKline, PredictionKline>(_model);
            var prediction = predictionFunction.Predict(kline);
            Console.WriteLine($"Predicted close: {prediction.Close:0.####}, actual close: {close:0.####}");
        }
    }
}
