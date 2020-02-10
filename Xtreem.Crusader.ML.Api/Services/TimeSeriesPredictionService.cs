using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
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
        private readonly IMapper _mapper;

        public TimeSeriesPredictionService(IOptionsFactory<ModelOptions> optionsFactory, TrainModelLoader trainModelLoader, IMapper mapper)
            : base(optionsFactory)
        {
            _trainModelLoader = trainModelLoader;
            _mapper = mapper;
        }

        protected override ReadOnlyCollection<Ohlcv> Predict(IEnumerable<OhlcvInput> ohlcvs)
        {
            return _mapper.Map<IEnumerable<Ohlcv>>(_trainModelLoader.GetModel().CreateTimeSeriesEngine<OhlcvInput, OhlcvPredictionSeries>(new MLContext(0)).Predict()).AsReadOnly();
        }
    }
}
