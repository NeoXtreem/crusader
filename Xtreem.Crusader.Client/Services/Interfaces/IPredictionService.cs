using System;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<float?> PredictAsync(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}