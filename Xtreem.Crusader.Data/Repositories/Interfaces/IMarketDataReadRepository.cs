using System.Collections.Generic;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Data.Repositories.Interfaces
{
    public interface IMarketDataReadRepository
    {
        IEnumerable<Ohlcv> GetOhlcvs(CurrencyPairChart currencyPairChart, long from, long to);

        IEnumerable<Ohlcv> GetOhlcvs(CurrencyPairChartPeriod currencyPairChartPeriod);
    }
}
