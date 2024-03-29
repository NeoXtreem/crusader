﻿using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Xtreem.Crusader.Server.Models;
using Xtreem.Crusader.Server.Services.Interfaces;
using Xtreem.Crusader.Shared.Models;
using Xtreem.Crusader.Utilities.Attributes;
using Xtreem.Crusader.Utilities.Exceptions;

namespace Xtreem.Crusader.Server.Services
{
    [Inject, UsedImplicitly]
    internal class MLService : IMLService
    {
        private readonly CrusaderApiOptions _options;

        public MLService(IOptionsFactory<CrusaderApiOptions> optionsFactory)
        {
            _options = optionsFactory.Create(Options.DefaultName);
        }

        public async Task<ReadOnlyCollection<Ohlcv>> PredictAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken)
        {
            using var client = new HttpClient {BaseAddress = new Uri(_options.MLApiBaseUrl)};
            var response = await client.PostAsJsonAsync("ml", currencyPairChartPeriod, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ReadOnlyCollection<Ohlcv>>(cancellationToken);
            }

            throw new HttpClientException($"ML request unsuccessful: {response.ReasonPhrase}") {Response = response};
        }
    }
}
