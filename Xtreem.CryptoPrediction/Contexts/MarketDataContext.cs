using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Settings;

namespace Xtreem.CryptoPrediction.Data.Contexts
{
    public class MarketDataContext : IMarketDataContext
    {
        private readonly DataSettings _settings;
        private readonly IMongoDatabase _mongoDb;

        static MarketDataContext()
        {
            ConventionRegistry.Register("EnumStringConvention", new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            }, t => true);
        }

        public MarketDataContext(IOptions<DataSettings> options)
        {
            _settings = options.Value;
            _mongoDb = new MongoClient(_settings.ConnectionString).GetDatabase(_settings.MarketDataDatabase);
        }

        public IMongoCollection<Ohlcv> HistoricalOhlcvCollection => _mongoDb.GetCollection<Ohlcv>(_settings.HistoricalOhlcvCollection);
    }
}
