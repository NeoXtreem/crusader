using System.Collections.Generic;
using System.IO;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Interfaces
{
    public interface IModelService
    {
        void Initialise(IEnumerable<OhlcvInput> ohlcvs);

        void Train<TOutput>() where TOutput : class;

        FileStream GetModel();
    }
}
