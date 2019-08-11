using System.Collections.Generic;
using Xtreem.CryptoPrediction.Client.Models;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface IPredictionService
    {
        void Initialise(IEnumerable<InputKline> klines);

        void Predict(InputKline kline);
    }
}
