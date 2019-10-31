using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Types;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class TimeSeriesModelService : ModelService
    {
        public TimeSeriesModelService(IOptionsFactory<ModelOptions> optionsFactory)
            : base(optionsFactory)
        {
        }

        protected override PredictionModel PredictionModel => PredictionModel.TimeSeries;

        protected override ITransformer Train<TOutput>(MLContext mlContext, IEnumerable<TOutput> items)
        {
            var trainTestData = mlContext.Data.TrainTestSplit(mlContext.Data.LoadFromEnumerable(items));
            var seriesLength = (int)trainTestData.TrainSet.GetRowCount().GetValueOrDefault();

            IEstimator<ITransformer> estimator = mlContext.Forecasting.ForecastBySsa(
                nameof(OhlcvRegressionPrediction.ClosePrediction),
                nameof(OhlcvInput.Close),
                12,
                seriesLength,
                seriesLength,
                2,
                confidenceLowerBoundColumn: nameof(OhlcvTimeSeriesPrediction.ConfidenceLowerBound),
                confidenceUpperBoundColumn: nameof(OhlcvTimeSeriesPrediction.ConfidenceUpperBound));

            var transformer = estimator.Fit(trainTestData.TrainSet);
            using var engine = transformer.CreateTimeSeriesEngine<OhlcvInput, OhlcvTimeSeriesPrediction>(mlContext);
            engine.CheckPoint(mlContext, Options.FilePath);

            return transformer;
        }
    }
}
