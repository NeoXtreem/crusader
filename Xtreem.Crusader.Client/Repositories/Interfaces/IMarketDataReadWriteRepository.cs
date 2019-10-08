using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories.Interfaces;

namespace Xtreem.Crusader.Client.Repositories.Interfaces
{
    public interface IMarketDataReadWriteRepository : IMarketDataReadRepository
    {
        Task AddOhlcvsAsync(IEnumerable<Ohlcv> items);
    }
}
