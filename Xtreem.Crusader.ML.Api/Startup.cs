using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;
using Xtreem.Crusader.Data.Contexts;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Settings;
using Xtreem.Crusader.ML.Api.Loaders;
using Xtreem.Crusader.ML.Api.Repositories;
using Xtreem.Crusader.ML.Api.Repositories.Interfaces;
using Xtreem.Crusader.ML.Api.Services;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
using Xtreem.Crusader.ML.Api.Settings;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Services;
using Xtreem.Crusader.ML.Data.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Settings;

namespace Xtreem.Crusader.ML.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ReSharper disable once CommentTypo
            //TODO: Call AddNewtonsoftJson for Resolution type to deserialize as it has a readonly property as per https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#jsonnet-support. Revert if https://github.com/dotnet/corefx/issues/40602 is resolved.
            services
                .AddControllers()
                .AddNewtonsoftJson();

            services
                .AddScoped<IPredictionService, PredictionService>()
                .AddScoped<IMarketDataContext, MarketDataContext>()
                .AddScoped<IMarketDataReadRepository, MarketDataReadRepository>()
                .AddScoped<IMarketDataReadWriteRepository, MarketDataReadWriteRepository>()
                .AddScoped<IHistoricalDataService, HistoricalDataService>()
                .AddScoped<ICryptoCompareService, CryptoCompareService>()
                .AddScoped<IRegressionModelService, RegressionModelService>()
                .AddTransient<IOhlcvMappingService, OhlcvMappingService>()
                .Configure<ModelSettings>(Configuration.GetSection("Model"))
                .Configure<DataSettings>(Configuration.GetSection("Data"))
                .Configure<CryptoCompareSettings>(Configuration.GetSection("CryptoCompare"));

            //TODO: Uncomment the delegate in the following statement, and remove the next statement once this PR is done: https://github.com/dotnet/machinelearning/pull/4393
            services.AddPredictionEnginePool<OhlcvInput, OhlcvPrediction>( /*serviceProvider =>
            {
                services.AddOptions<PredictionEnginePoolOptions<OhlcvInput, OhlcvPrediction>>().Configure(options =>
                {
                    options.ModelLoader = new TrainModelLoader(serviceProvider.GetService<IModelService>(), serviceProvider.GetService<IOhlcvMappingService>(), serviceProvider.GetService<IHistoricalDataService>());
                });
                return new PredictionEnginePool<OhlcvInput, OhlcvPrediction>();
            }*/);

            services.AddOptions<PredictionEnginePoolOptions<OhlcvInput, OhlcvPrediction>>().Configure(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                options.ModelLoader = new TrainModelLoader(serviceProvider.GetService<IModelService>(), serviceProvider.GetService<IOhlcvMappingService>(), serviceProvider.GetService<IHistoricalDataService>());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
