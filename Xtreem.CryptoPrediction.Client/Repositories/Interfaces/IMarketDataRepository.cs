using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Data.Repositories.Interfaces
{
    public interface IMarketDataRepository
    {
        Task AddOhlcvsAsync(IEnumerable<Ohlcv> items, Resolution resolution);
    }
}
