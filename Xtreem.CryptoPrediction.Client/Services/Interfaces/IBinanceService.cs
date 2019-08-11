using System;
using System.Collections.Generic;
using Binance.Net.Objects;
using Xtreem.CryptoPrediction.Client.Models;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface IBinanceService
    {
        IEnumerable<InputKline> GetKlines(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null);

        void SubscribeToKlines(string symbol, KlineInterval interval, Action<InputKline> act);
    }
}
