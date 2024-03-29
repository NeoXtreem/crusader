﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.Data.Repositories
{
    [Inject, UsedImplicitly]
    public class MarketDataReadRepository : IMarketDataReadRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadRepository(IMarketDataContext context) => _context = context;

        public IEnumerable<Ohlcv> GetOhlcvs() => _context.GetHistoricalOhlcvsQuery().AsEnumerable();

        public IEnumerable<Ohlcv> GetOhlcvs(CurrencyPairChart currencyPairChart, long from, long to)
        {
            var currencyPair = currencyPairChart.CurrencyPair;
            return _context.GetHistoricalOhlcvsQuery().Where(o =>
                o.Base == currencyPair.BaseCurrency &&
                o.Quote == currencyPair.QuoteCurrency &&
                o.Resolution == currencyPairChart.Resolution.ToString() &&
                o.Time >= from &&
                o.Time <= to).AsEnumerable();
        }

        public IEnumerable<Ohlcv> GetOhlcvs(CurrencyPairChartPeriod currencyPairChartPeriod)
        {
            return GetOhlcvs(
                currencyPairChartPeriod.CurrencyPairChart,
                ((DateTimeOffset)currencyPairChartPeriod.DateTimeInterval.From).ToUnixTimeSeconds(),
                ((DateTimeOffset)currencyPairChartPeriod.DateTimeInterval.To).ToUnixTimeSeconds());
        }
    }
}
