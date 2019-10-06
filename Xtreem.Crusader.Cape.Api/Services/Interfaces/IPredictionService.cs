using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.Cape.Api.Services.Interfaces
{
    public interface IPredictionService
    {
        OhlcvPrediction Predict(OhlcvInput ohlcv);
    }
}
