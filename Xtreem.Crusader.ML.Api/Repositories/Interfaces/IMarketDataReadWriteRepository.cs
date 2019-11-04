using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.ML.Api.Repositories.Interfaces
{
    internal interface IMarketDataReadWriteRepository : IMarketDataReadRepository
    {
        Task AddOhlcvsAsync(IEnumerable<Ohlcv> items);
    }
}
