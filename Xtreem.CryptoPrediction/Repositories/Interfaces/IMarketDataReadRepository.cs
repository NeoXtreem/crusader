using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Data.Repositories.Interfaces
{
    public interface IMarketDataReadRepository
    {
        Task<IEnumerable<Ohlcv>> GetOhlcvsAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from, long to);

        Task<IEnumerable<Ohlcv>> GetOhlcvsAsync(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to);
    }
}
