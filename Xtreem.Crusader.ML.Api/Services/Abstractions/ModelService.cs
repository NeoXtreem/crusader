using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Data.Settings;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal abstract class ModelService : IModelService
    {
        private readonly ModelSettings _modelSettings;

        protected ModelService(IOptions<ModelSettings> modelOptions)
        {
            _modelSettings = modelOptions.Value;
        }

        public ITransformer Train<TOutput>(IEnumerable<TOutput> items) where TOutput : class
        {
            var mlContext = new MLContext(0);
            var trainTestData = mlContext.Data.TrainTestSplit(mlContext.Data.LoadFromEnumerable(items));
            var model = BuildPipeline<TOutput>(mlContext).Fit(trainTestData.TrainSet);
            mlContext.Model.Save(model, trainTestData.TrainSet.Schema, _modelSettings.FilePath);

            var metrics = mlContext.Regression.Evaluate(model.Transform(trainTestData.TestSet));

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine(new string('-', 25));
            Console.WriteLine($" RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($" Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}");

            return model;
        }

        protected abstract IEstimator<ITransformer> BuildPipeline<TOutput>(MLContext mlContext) where TOutput : class;
    }
}
