using System;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface ICryptoCompareService
    {
        Task<object> GetHistoricalData(string baseCurrency, string quoteCurrency, DateTimeOffset to, Resolution resolution);
    }
}