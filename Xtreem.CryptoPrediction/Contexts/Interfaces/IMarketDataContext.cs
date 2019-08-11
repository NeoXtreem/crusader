using MongoDB.Driver;
using Xtreem.CryptoPrediction.Data.Models;

namespace Xtreem.CryptoPrediction.Data.Contexts.Interfaces
{
    public interface IMarketDataContext
    {
        IMongoCollection<Ohlcv> HistoricalOhlcvCollection { get; }
    }
}