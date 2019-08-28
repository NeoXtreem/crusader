using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Services.Interfaces
{
    public interface IHistoricalDataService
    {
        Task<IEnumerable<Ohlcv>> GetHistoricalDataAsync(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}
