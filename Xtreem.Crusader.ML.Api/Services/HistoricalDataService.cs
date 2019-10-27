using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Api.Repositories.Interfaces;
using Xtreem.Crusader.ML.Api.Services.Interfaces;

namespace Xtreem.Crusader.ML.Api.Services
{
    internal class HistoricalDataService : IHistoricalDataService
    {
        private readonly IMarketDataReadWriteRepository _marketDataReadWriteRepository;
        private readonly ICryptoCompareService _cryptoCompareService;

        public HistoricalDataService(IMarketDataReadWriteRepository marketDataReadWriteRepository, ICryptoCompareService cryptoCompareService)
        {
            _marketDataReadWriteRepository = marketDataReadWriteRepository;
            _cryptoCompareService = cryptoCompareService;
        }

        public async Task<IEnumerable<Ohlcv>> GetHistoricalDataAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken)
        {
            var newOhlcvs = new List<Ohlcv>();

            async Task LoadHistoricalDataForGap(DateTime gapFrom, DateTime gapTo)
            {
                newOhlcvs.AddRange(await _cryptoCompareService.LoadHistoricalDataAsync(new CurrencyPairChartPeriod {CurrencyPairChart = currencyPairChartPeriod.CurrencyPairChart, DateTimeInterval = new DateTimeInterval {From = gapFrom, To = gapTo}}, cancellationToken));
            }

            var ohlcvs = _marketDataReadWriteRepository.GetOhlcvs(currencyPairChartPeriod).ToArray();
            var currentFrom = currencyPairChartPeriod.DateTimeInterval.From;
            var interval = currencyPairChartPeriod.CurrencyPairChart.Resolution.Interval;

            // Find all gaps in the stored OHLCV data and fill these by loading them from CryptoCompare.
            foreach (var ohlcv in ohlcvs.OrderBy(o => o.Time))
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(ohlcv.Time).UtcDateTime;
                if (time > currentFrom + interval)
                {
                    await LoadHistoricalDataForGap(currentFrom, time - interval);
                }

                currentFrom = time;
            }

            // Ensure any trailing gap is covered.
            if (currentFrom != currencyPairChartPeriod.DateTimeInterval.To)
            {
                await LoadHistoricalDataForGap(currentFrom, currencyPairChartPeriod.DateTimeInterval.To);
            }

            return ohlcvs.Concat(newOhlcvs);
        }
    }
}
