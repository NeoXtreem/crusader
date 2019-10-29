using System.Collections.Generic;
using Microsoft.ML;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces
{
    internal interface IModelService
    {
        bool CanTrain();

        ITransformer Train<TOutput>(IEnumerable<TOutput> items) where TOutput : class;
    }
}
