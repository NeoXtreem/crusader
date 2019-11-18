using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class TimeSeriesModelService : ModelService
    {
        public TimeSeriesModelService(IOptionsFactory<ModelOptions> optionsFactory)
            : base(optionsFactory)
        {
        }

        protected override ITransformer Train<TInput>(MLContext mlContext, IEnumerable<TInput> items)
        {
            var data = items.ToArrayOrCast();
            var trainData = mlContext.Data.LoadFromEnumerable(data);

            var estimator = mlContext.Forecasting.ForecastBySsa(
                nameof(OhlcvSeriesPrediction.ClosePrediction),
                nameof(OhlcvInput.Close),
                12,
                data.Length,
                data.Length,
                2,
                confidenceLowerBoundColumn: nameof(OhlcvSeriesPrediction.ConfidenceLowerBound),
                confidenceUpperBoundColumn: nameof(OhlcvSeriesPrediction.ConfidenceUpperBound));

            var transformer = estimator.Fit(trainData);
            using var engine = transformer.CreateTimeSeriesEngine<OhlcvInput, OhlcvSeriesPrediction>(mlContext);
            engine.CheckPoint(mlContext, Options.FilePath);

            return transformer;
        }
    }
}
