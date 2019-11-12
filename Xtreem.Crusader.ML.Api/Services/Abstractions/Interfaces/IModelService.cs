using System.Collections.Generic;
using Microsoft.ML;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces
{
    internal interface IModelService
    {
        bool CanUse();

        ITransformer Train<TInput>(IEnumerable<TInput> items) where TInput : class;
    }
}
