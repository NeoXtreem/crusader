using System.Collections.Generic;
using Microsoft.ML;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces
{
    public interface IModelService
    {
        ITransformer Train<TOutput>(IEnumerable<TOutput> ohlcvs) where TOutput : class;
    }
}
