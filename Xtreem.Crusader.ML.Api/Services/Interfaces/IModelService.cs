using System.Collections.Generic;
using Microsoft.ML;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Interfaces
{
    public interface IModelService
    {
        ITransformer Train<TOutput>(IEnumerable<OhlcvInput> ohlcvs) where TOutput : class;
    }
}
