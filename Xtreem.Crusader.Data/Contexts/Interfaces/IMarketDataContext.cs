﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.Data.Contexts.Interfaces
{
    public interface IMarketDataContext
    {
        IOrderedQueryable<Ohlcv> GetHistoricalOhlcvsQuery();

        Task<BulkExecutor> GetHistoricalOhlcvBulkExecutorAsync();
    }
}
