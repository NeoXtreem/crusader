using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Repositories.Interfaces
{
    public interface IMarketDataReadWriteRepository : IMarketDataReadRepository
    {
        Task AddOhlcvsAsync(IEnumerable<Ohlcv> items, Resolution resolution);
    }
}
