using System;
using Microsoft.Extensions.DependencyInjection;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal class LazyService<T> : Lazy<T> where T : class
    {
        public LazyService(IServiceProvider serviceProvider)
            : base(serviceProvider.GetRequiredService<T>)
        {
        }
    }
}
