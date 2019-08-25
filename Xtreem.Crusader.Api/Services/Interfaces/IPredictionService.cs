using System.Collections.Generic;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Api.Services.Interfaces
{
    public interface IPredictionService
    {
        void Initialise(IEnumerable<Ohlcv> klines);

        void Predict(Ohlcv kline);
    }
}
