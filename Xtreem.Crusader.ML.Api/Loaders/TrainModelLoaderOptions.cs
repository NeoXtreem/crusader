using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;

namespace Xtreem.Crusader.ML.Api.Loaders
{
    internal class TrainModelLoaderOptions
    {
        public IModelService ModelService { get; set; }
    }
}
