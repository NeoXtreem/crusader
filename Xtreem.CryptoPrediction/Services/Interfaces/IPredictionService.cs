using System.Collections.Generic;
using Xtreem.CryptoPrediction.Service.Models;

namespace Xtreem.CryptoPrediction.Service.Services.Interfaces
{
    public interface IPredictionService
    {
        void Initialise(IEnumerable<InputKline> klines);

        void Predict(InputKline kline);
    }
}
