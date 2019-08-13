using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface ICryptoCompareService
    {
        Task<IEnumerable<Ohlcv>> LoadHistoricalData(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to);
    }
}