using System.Collections.ObjectModel;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces
{
    public interface IPredictionService
    {
        bool CanUse();

        ReadOnlyCollection<Ohlcv> Predict(CurrencyPairChartPeriod predictionPeriod);
    }
}
