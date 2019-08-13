using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Settings;

namespace Xtreem.CryptoPrediction.Data.Contexts
{
    public class MarketDataContext : IMarketDataContext, IDisposable
    {
        private readonly Uri _collectionUri;
        private readonly DocumentClient _client;

        public MarketDataContext(IOptions<DataSettings> options)
        {
            var settings = options.Value;
            _client = new DocumentClient(new Uri(settings.Endpoint), settings.PrimaryKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            });

            _client.CreateDatabaseIfNotExistsAsync(new Database {Id = settings.MarketDataDb}).Wait();

            var partitionKey = new PartitionKeyDefinition();
            partitionKey.Paths.Add($"/{nameof(Ohlcv.Base)}");
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(settings.MarketDataDb), new DocumentCollection {Id = settings.HistoricalOhlcvContainer, PartitionKey = partitionKey}).Wait();
            _collectionUri = UriFactory.CreateDocumentCollectionUri(settings.MarketDataDb, settings.HistoricalOhlcvContainer);
        }

        public async Task<BulkExecutor> GetHistoricalOhlcvBulkExecutorAsync()
        {
            var bulkExecutor = new BulkExecutor(_client, await _client.ReadDocumentCollectionAsync(_collectionUri));
            await bulkExecutor.InitializeAsync();
            return bulkExecutor;
        }

        public IOrderedQueryable<Ohlcv> GetHistoricalOhlcvsQuery() => _client.CreateDocumentQuery<Ohlcv>(_collectionUri);

        public void Dispose() => _client.Dispose();
    }
}
