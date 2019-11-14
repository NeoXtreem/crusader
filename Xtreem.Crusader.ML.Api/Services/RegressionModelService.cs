using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Attributes;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class RegressionModelService : ModelService
    {
        public RegressionModelService(IOptionsFactory<ModelOptions> optionsFactory)
            : base(optionsFactory)
        {
        }

        protected override ITransformer Train<TInput>(MLContext mlContext, IEnumerable<TInput> items)
        {
            var properties = typeof(TInput).GetProperties().Select(p => (property: p, attributes: p.GetCustomAttributes())).Where(p => !p.attributes.OfType<ColumnNameAttribute>().Any()).ToArray();
            var propertyNames = properties.Select(p => p.property.Name).ToArray();
            var encodedColumnPairs = GetPropertiesWithAttributes<EncodedColumnAttribute>().Select(p => new InputOutputColumnPair($"{p.property.Name}Encoded", p.property.Name)).ToArray();
            var labelColumnProperties = GetPropertiesWithAttributes<LabelColumnAttribute>().ToArray();

            IEnumerable<(PropertyInfo property, TAttribute attribute)> GetPropertiesWithAttributes<TAttribute>() where TAttribute : Attribute
            {
                return properties.Select(p => (p.property, attribute: p.attributes.OfType<TAttribute>().SingleOrDefault())).Where(p => p.attribute != null);
            }

            IEstimator<ITransformer> pipeline = new EstimatorChain<ITransformer>();

            if (labelColumnProperties.Any())
            {
                // Build a model for each specified named label column to achieve imputation.
                foreach (var (property, attribute) in labelColumnProperties)
                {
                    pipeline = pipeline.Append(CreateEstimators()
                        .Aggregate((IEstimator<ITransformer>)mlContext.Transforms.CopyColumns("Label", property.Name), (c, e) => c.Append(e))
                        .Append(mlContext.Transforms.CopyColumns(attribute.ScoreColumnName, "Score")));
                }
            }
            else
            {
                // Build a model on the assumption that columns named Label and Score are present.
                pipeline = CreateEstimators().Aggregate(pipeline, (c, e) => c.Append(e));
            }

            IEstimator<ITransformer>[] CreateEstimators(string labelPropertyName = null)
            {
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

            var trainTestData = mlContext.Data.TrainTestSplit(mlContext.Data.LoadFromEnumerable(items));
            var model = pipeline.Fit(trainTestData.TrainSet);

            mlContext.Model.Save(model, trainTestData.TrainSet.Schema, Options.FilePath);

            var metrics = mlContext.Regression.Evaluate(model.Transform(trainTestData.TestSet));

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine(new string('-', 25));
            Console.WriteLine($" RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($" Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}");

            return model;
        }
    }
}
