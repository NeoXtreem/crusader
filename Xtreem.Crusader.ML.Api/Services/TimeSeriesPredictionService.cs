using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Xtreem.Crusader.Data.Services.Interfaces;
using Xtreem.Crusader.ML.Api.Profiles;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services
{
    [Inject, UsedImplicitly]
    internal class TimeSeriesPredictionService : PredictionService
    {
        private readonly TrainModelLoader _trainModelLoader;
        private readonly IMappingService _mappingService;

        public TimeSeriesPredictionService(
            IOptionsFactory<ModelOptions> optionsFactory,
            TrainModelLoader trainModelLoader,
            IMappingService mappingService)
            : base(optionsFactory)
        {
            _trainModelLoader = trainModelLoader;
            _mappingService = mappingService;
        }

        protected override ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs)
        {
            return _mappingService.GetMapper<OhlcvProfile>()
                .Map<IEnumerable<Ohlcv>>(_trainModelLoader.GetModel().CreateTimeSeriesEngine<OhlcvInput, OhlcvTimeSeriesPrediction>(new MLContext(0)).Predict())
                .AsReadOnly();
        }
    }
}
