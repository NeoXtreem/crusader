using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Attributes;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Settings;

namespace Xtreem.Crusader.ML.Api.Services
{
    public class ModelService : IModelService
    {
        private readonly ModelSettings _modelSettings;

        private MLContext _mlContext;
        private DataOperationsCatalog.TrainTestData _trainTestData;

        public ModelService(IOptions<ModelSettings> modelOptions)
        {
            _modelSettings = modelOptions.Value;
        }

        public void Initialise(IEnumerable<OhlcvInput> ohlcvs)
        {
            _mlContext = new MLContext(0);
            _trainTestData = _mlContext.Data.TrainTestSplit(_mlContext.Data.LoadFromEnumerable(ohlcvs));
        }

        public void Train<TOutput>() where TOutput : class
        {
            var properties = typeof(TOutput).GetProperties().Select(p => (property: p, attributes: p.GetCustomAttributes())).Where(p => !p.attributes.OfType<ColumnNameAttribute>().Any()).ToArray();
            var propertyNames = properties.Select(p => p.property.Name).ToArray();
            var encodedColumnPairs = GetPropertiesWithAttributes<EncodedColumnAttribute>().Select(p => new InputOutputColumnPair($"{p.property.Name}Encoded", p.property.Name)).ToArray();
            var labelColumnProperties = GetPropertiesWithAttributes<LabelColumnAttribute>().ToArray();

            IEnumerable<(PropertyInfo property, TAttribute attribute)> GetPropertiesWithAttributes<TAttribute>() where TAttribute : Attribute
            {
                return properties.Select(p => (p.property, attribute: p.attributes.OfType<TAttribute>().SingleOrDefault())).Where(p => p.attribute != null);
            }

            IEstimator<ITransformer> pipeline = null;

            // If there are label columns specified, build a model for each one.
            foreach (var (property, attribute) in labelColumnProperties)
            {
                IEstimator<ITransformer> estimator = CreateEstimators(property.Name)
                    .Aggregate((IEstimator<ITransformer>)_mlContext.Transforms.CopyColumns("Label", property.Name), (c, e) => c.Append(e))
                    .Append(_mlContext.Transforms.CopyColumns(attribute.ScoreColumnName, "Score"));

                // Appends to the pipeline only after the first iteration.
                pipeline = pipeline?.Append(estimator) ?? estimator;
            }

            // Otherwise, build a model on the assumption that columns named Label and Score are specified.
            if (pipeline is null)
            {
                var estimators = CreateEstimators();
                pipeline = estimators.Skip(1).Aggregate(estimators.First(), (c, e) => c.Append(e));
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
                    _mlContext.Transforms.Categorical.OneHotEncoding(encodedColumnPairs),
                    _mlContext.Transforms.Concatenate("Features", featureColumns),
                    _mlContext.Regression.Trainers.FastTree()
                };
            }

            var model = pipeline.Fit(_trainTestData.TrainSet);
            _mlContext.Model.Save(model, _trainTestData.TrainSet.Schema, _modelSettings.FilePath);

            var metrics = _mlContext.Regression.Evaluate(model.Transform(_trainTestData.TestSet));

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine(new string('-', 25));
            Console.WriteLine($" RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($" Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}");
        }

        public FileStream GetModel() => File.Open(_modelSettings.FilePath, FileMode.Open);
    }
}
