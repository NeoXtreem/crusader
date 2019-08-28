using System.Collections.Generic;
using Microsoft.ML;
using Xtreem.Crusader.Api.Models.ML;

namespace Xtreem.Crusader.Api.Services.Interfaces
{
    public interface IPredictionService
    {
        void Initialise(IEnumerable<OhlcvInput> ohlcvs);

        void Train();

        OhlcvPrediction Predict(OhlcvInput ohlcv);
    }
}
