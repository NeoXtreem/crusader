using System;
using System.Collections.Generic;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Data.Repositories.Interfaces
{
    public interface IMarketDataReadRepository
    {
        IEnumerable<Ohlcv> GetOhlcvs(string baseCurrency, string quoteCurrency, Resolution resolution, long from, long to);

        IEnumerable<Ohlcv> GetOhlcvs(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to);
    }
}
