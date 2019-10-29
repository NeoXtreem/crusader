using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Data.Settings;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal abstract class ModelService : IModelService
    {
        protected readonly ModelSettings ModelSettings;

        protected ModelService(IOptions<ModelSettings> modelOptions)
        {
            ModelSettings = modelOptions.Value;
        }

        public bool CanTrain() => GetType().Name.StartsWith(ModelSettings.Type);

        public ITransformer Train<TOutput>(IEnumerable<TOutput> items) where TOutput : class => Train(new MLContext(0), items);

        protected abstract ITransformer Train<TOutput>(MLContext mlContext, IEnumerable<TOutput> items) where TOutput : class;
    }
}
