using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Xtreem.Crusader.Api.Attributes;
using Xtreem.Crusader.Api.Models;
using Xtreem.Crusader.Api.Services.Interfaces;
using Xtreem.Crusader.Api.Settings;

namespace Xtreem.Crusader.Api.Services
{
    public class PredictionService : IPredictionService
    {
        private const string Label = "Label";
        private const string Features = "Features";

        private readonly ModelSettings _modelSettings;
        private readonly PredictionEnginePool<OhlcvInput, OhlcvPrediction> _predictionEnginePool;
        private readonly IApplicationLifetime _applicationLifetime;

        private MLContext _mlContext;
        private DataOperationsCatalog.TrainTestData _trainTestData;

        public PredictionService(IOptions<ModelSettings> modelOptions, PredictionEnginePool<OhlcvInput, OhlcvPrediction> predictionEnginePool, IApplicationLifetime applicationLifetime)
        {
            _modelSettings = modelOptions.Value;
            _predictionEnginePool = predictionEnginePool;
            _applicationLifetime = applicationLifetime;
        }

        public void Initialise(IEnumerable<OhlcvInput> ohlcvs)
        {
            _mlContext = new MLContext(0);
            _trainTestData = _mlContext.Data.TrainTestSplit(_mlContext.Data.LoadFromEnumerable(ohlcvs));
        }

        public void Train()
        {
            //var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(PredictionKline.Close))
            //    //.Append(mlContext.Transforms.Conversion.ConvertType(new[] { new InputOutputColumnPair(nameof(InputKline.OpenTime)), new InputOutputColumnPair(nameof(InputKline.CloseTime)) }))
            //    .Append(_mlContext.Transforms.Concatenate("Features", features.ToArray()))
            //    .Append(_mlContext.Regression.Trainers.FastTree());

            var properties = typeof(OhlcvInput).GetProperties();
            var labelColumnName = properties.Single(p => p.GetCustomAttribute<LabelColumnAttribute>() != null).Name;

            var pipeline = _mlContext.Transforms.CopyColumns(Label, labelColumnName)
                .Append(_mlContext.Transforms.Concatenate(Features, properties.Select(p => p.Name).Where(n => n != labelColumnName).ToArray()))
                .Append(_mlContext.Regression.Trainers.FastTree());

            var model = pipeline.Fit(_trainTestData.TrainSet);
            _mlContext.Model.Save(model, _trainTestData.TrainSet.Schema, _modelSettings.FileName);

            if (_modelSettings.FileExists)
            {
                _applicationLifetime.StopApplication();
            }

            // Evaluate model.
            var metrics = _mlContext.Regression.Evaluate(model.Transform(_trainTestData.TestSet));

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine(new string('-', 25));
            Console.WriteLine($" RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($" Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}");
        }

        public OhlcvPrediction Predict(OhlcvInput ohlcv)
        {
            var close = ohlcv.Close;
            ohlcv.Close = 0;

            var prediction = _predictionEnginePool.Predict(ohlcv);
            Console.WriteLine($"Predicted close: {prediction.Close:0.####}, actual close: {close:0.####}");
            return prediction;
        }
    }
}
