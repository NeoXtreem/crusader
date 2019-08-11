using System;
using System.Collections.Generic;
using System.IO;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Xtreem.CryptoPrediction.Client.Exceptions;
using Xtreem.CryptoPrediction.Client.Models;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;

namespace Xtreem.CryptoPrediction.Client.Services
{
    public class BinanceService : IBinanceService, IDisposable
    {
        private readonly IKlineMappingService _klineMappingService;
        private BinanceSocketClient _socketClient;

        public BinanceService(IKlineMappingService klineMappingService)
        {
            _klineMappingService = klineMappingService;
        }

        public IEnumerable<InputKline> GetKlines(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials("1D8oRfetJirxArg93ROEuOBUVvFEF4oepInxGUfrUdgOXqjE80lNtTetYnYj9DjP", "r0Cc1zYW2sLUnzo5gBUBd0E4mQq526YWPyamq1IvByY17znyB3hj0AfJgvqVrxcz"),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            using (var client = new BinanceClient())
            {
                var response = client.GetKlines(symbol, interval, startTime, endTime, limit);
                return response.Success ? _klineMappingService.Map(response.Data) : throw new BinanceServiceException();
            }
        }

        public void SubscribeToKlines(string symbol, KlineInterval interval, Action<InputKline> act)
        {
            _socketClient?.Dispose();
            _socketClient = new BinanceSocketClient();
            _socketClient.SubscribeToKlineStream(symbol, interval, d => { act(_klineMappingService.Map(d.Data.ToKline())); });
        }

        public void Dispose() => _socketClient?.Dispose();
    }
}
