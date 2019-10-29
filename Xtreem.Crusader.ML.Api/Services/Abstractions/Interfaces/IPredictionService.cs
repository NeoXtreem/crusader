using System.Collections.ObjectModel;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces
{
    public interface IPredictionService
    {
        bool CanPredict();

        ReadOnlyCollection<Ohlcv> Predict(CurrencyPairChartPeriod predictionPeriod);
    }
}
