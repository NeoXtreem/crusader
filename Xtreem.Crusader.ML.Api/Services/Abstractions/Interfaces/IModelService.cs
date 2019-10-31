using System;
using System.Collections.Generic;
using Microsoft.ML;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces
{
    internal interface IModelService
    {
        bool CanTrain(Type type);

        ITransformer Train<TOutput>(IEnumerable<TOutput> items) where TOutput : class;
    }
}
