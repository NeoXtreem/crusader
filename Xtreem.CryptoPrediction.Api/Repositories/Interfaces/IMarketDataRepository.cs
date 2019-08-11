using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Api.Repositories.Interfaces
{
    public interface IMarketDataRepository
    {
        Task<IEnumerable<Ohlcv>> GetOhlcvsAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from, long to);

        Task<long> GetNextTimeAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from);
    }
}
