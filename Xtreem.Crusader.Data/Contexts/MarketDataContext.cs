using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.Data.Contexts
{
    [Inject, UsedImplicitly]
    public class MarketDataContext : IMarketDataContext, IDisposable
    {
        private readonly Uri _collectionUri;
        private readonly DocumentClient _client;

        public MarketDataContext(IOptionsFactory<DataOptions> optionsFactory)
        {
            var options = optionsFactory.Create(Options.DefaultName);
            _client = new DocumentClient(new Uri(options.Endpoint), options.PrimaryKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            });

            _client.CreateDatabaseIfNotExistsAsync(new Database {Id = options.MarketDataDb}).Wait();

            var partitionKey = new PartitionKeyDefinition();
            partitionKey.Paths.Add($"/{nameof(Ohlcv.Base)}");
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(options.MarketDataDb), new DocumentCollection {Id = options.HistoricalOhlcvContainer, PartitionKey = partitionKey}).Wait();
            _collectionUri = UriFactory.CreateDocumentCollectionUri(options.MarketDataDb, options.HistoricalOhlcvContainer);
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
