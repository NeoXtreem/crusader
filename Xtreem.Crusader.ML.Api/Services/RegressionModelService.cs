using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Attributes;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class RegressionModelService : ImputationModelService
    {
        public RegressionModelService(IOptionsFactory<ModelOptions> optionsFactory)
            : base(optionsFactory)
        {
        }

        protected override IEstimator<ITransformer> AddColumnToPipeline<TInput>(IEstimator<ITransformer> pipeline, MLContext mlContext, PropertyInfo property, PredictColumnAttribute attribute, IEnumerable<TInput> items)
        {
            return pipeline.Append(CreateEstimators<TInput>(mlContext, property.Name)
                .Aggregate((IEstimator<ITransformer>)mlContext.Transforms.CopyColumns("Label", property.Name), (c, e) => c.Append(e))
                .Append(mlContext.Transforms.CopyColumns(attribute.PredictionColumnName, "Score")));
        }

        protected override IEstimator<ITransformer> CreateSinglePipeline<TInput>(IEstimator<ITransformer> pipeline, MLContext mlContext)
        {
            return CreateEstimators<TInput>(mlContext).Aggregate(pipeline, (c, e) => c.Append(e));
        }

        private IEnumerable<IEstimator<ITransformer>> CreateEstimators<TInput>(MLContext mlContext, string labelPropertyName = null) where TInput : class
        {
            var propertyNames = GetPropertiesWithAttributes<TInput>().Select(p => p.property.Name).ToArray();
            var encodedColumnPairs = GetPropertiesWithAttributes<TInput, EncodedColumnAttribute>().Select(p => new InputOutputColumnPair($"{p.property.Name}Encoded", p.property.Name)).ToArray();
            
            // Determine the feature columns taking account of the naming of the encoded columns.
            var featureColumns = propertyNames
                .Except(encodedColumnPairs.Select(p => p.InputColumnName))
                .Concat(encodedColumnPairs.Select(p => p.OutputColumnName))
                .Where(n => n != labelPropertyName)
                .ToArray();

            return new IEstimator<ITransformer>[]
            {
                mlContext.Transforms.Categorical.OneHotEncoding(encodedColumnPairs),
                mlContext.Transforms.Concatenate("Features", featureColumns),
                mlContext.Regression.Trainers.FastTree()
            };
        }

        protected override ITransformer Evaluate<TInput>(MLContext mlContext, IEstimator<ITransformer> pipeline, IEnumerable<TInput> items)
        {
            var trainTestData = mlContext.Data.TrainTestSplit(mlContext.Data.LoadFromEnumerable(items));
            var transformer = pipeline.Fit(trainTestData.TrainSet);

            mlContext.Model.Save(transformer, trainTestData.TrainSet.Schema, Options.FilePath);

            var metrics = mlContext.Regression.Evaluate(transformer.Transform(trainTestData.TestSet));

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine(new string('-', 25));
            Console.WriteLine($" RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($" Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}");

            return transformer;
        }
    }
}
