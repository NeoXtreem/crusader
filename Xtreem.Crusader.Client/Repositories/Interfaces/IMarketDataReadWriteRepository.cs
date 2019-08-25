using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Repositories.Interfaces
{
    public interface IMarketDataReadWriteRepository : IMarketDataReadRepository
    {
        Task AddOhlcvsAsync(IEnumerable<Ohlcv> items, Resolution resolution);
    }
}
