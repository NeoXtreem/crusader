using System.Collections.Generic;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.Data.Repositories.Interfaces
{
    public interface IMarketDataReadRepository
    {
        IEnumerable<Ohlcv> GetOhlcvs();

        IEnumerable<Ohlcv> GetOhlcvs(CurrencyPairChart currencyPairChart, long from, long to);

        IEnumerable<Ohlcv> GetOhlcvs(CurrencyPairChartPeriod currencyPairChartPeriod);
    }
}
