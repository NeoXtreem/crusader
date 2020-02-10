using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Attributes;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class TimeSeriesModelService : ImputationModelService
    {
        public TimeSeriesModelService(IOptionsFactory<ModelOptions> optionsFactory)
            : base(optionsFactory)
        {
        }

        protected override IEstimator<ITransformer> AddColumnToPipeline<TInput>(IEstimator<ITransformer> pipeline, MLContext mlContext, PropertyInfo property, PredictColumnAttribute attribute, IEnumerable<TInput> items)
        {
            var dataLength = items.ToArrayOrCast().Length;

            return pipeline.Append(mlContext.Forecasting.ForecastBySsa(
                attribute.PredictionColumnName,
                property.Name,
                12,
                dataLength,
                dataLength,
                2,
                confidenceLowerBoundColumn: property.Name + "ConfidenceLowerBound",
                confidenceUpperBoundColumn: property.Name + "ConfidenceUpperBound"));
        }

        protected override ITransformer Evaluate<TInput>(MLContext mlContext, IEstimator<ITransformer> pipeline, IEnumerable<TInput> items)
        {
            var transformer = pipeline.Fit(mlContext.Data.LoadFromEnumerable(items));
            using var engine = transformer.CreateTimeSeriesEngine<OhlcvInput, OhlcvPredictionSeries>(mlContext);
            engine.CheckPoint(mlContext, Options.FilePath);

            return transformer;
        }
    }
}
