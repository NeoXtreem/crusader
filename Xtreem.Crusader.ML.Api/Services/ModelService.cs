using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.ML;
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

        public void Train()
        {
            var properties = typeof(OhlcvInput).GetProperties().Select(p => (property: p, feature: p.GetCustomAttribute<FeatureColumnAttribute>())).Where(p => p.feature != null).ToArray();
            var encodedColumnPairs = properties.Where(p => p.feature.Encode).Select(p => new InputOutputColumnPair($"{p.property.Name}Encoded", p.property.Name)).ToArray();

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding(encodedColumnPairs)
                .Append(_mlContext.Transforms.Concatenate("Features", properties.Where(p => !p.feature.Encode).Select(p => p.property.Name).Concat(encodedColumnPairs.Select(p => p.OutputColumnName)).ToArray()))
                .Append(_mlContext.Regression.Trainers.FastTree());

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
