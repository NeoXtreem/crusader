using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Client.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Services
{
    public class HistoricalDataService : IHistoricalDataService
    {
        private readonly IMarketDataReadWriteRepository _marketDataReadWriteRepository;
        private readonly ICryptoCompareService _cryptoCompareService;

        public HistoricalDataService(IMarketDataReadWriteRepository marketDataReadWriteRepository, ICryptoCompareService cryptoCompareService)
        {
            _marketDataReadWriteRepository = marketDataReadWriteRepository;
            _cryptoCompareService = cryptoCompareService;
        }

        public async Task<IEnumerable<Ohlcv>> GetHistoricalData(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to)
        {
            var newOhlcvs = new List<Ohlcv>();

            async Task LoadHistoricalDataForGap(DateTime gapFrom, DateTime gapTo)
            {
                newOhlcvs.AddRange(await _cryptoCompareService.LoadHistoricalData(baseCurrency, quoteCurrency, resolution, gapFrom, gapTo));
            }

            var ohlcvs = _marketDataReadWriteRepository.GetOhlcvs(baseCurrency, quoteCurrency, resolution, from, to).ToArray();
            var currentFrom = from;

            // Find all gaps in the stored OHLCV data and fill these by loading them from CryptoCompare.
            foreach (var ohlcv in ohlcvs.OrderBy(o => o.Time))
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(ohlcv.Time).UtcDateTime;
                if (time > currentFrom + resolution.Interval)
                {
                    await LoadHistoricalDataForGap(currentFrom, time - resolution.Interval);
                }

                currentFrom = time;
            }

            // Ensure any trailing gap is covered.
            if (currentFrom != to)
            {
                await LoadHistoricalDataForGap(currentFrom, to);
            }

            return ohlcvs.Concat(newOhlcvs);
        }
    }
}
