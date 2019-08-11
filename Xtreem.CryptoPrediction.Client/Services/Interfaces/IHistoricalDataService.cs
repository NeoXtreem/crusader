using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface IHistoricalDataService
    {
        Task<IEnumerable<Ohlcv>> GetHistoricalData(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to);
    }
}
