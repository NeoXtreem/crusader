using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Api.Loaders;
using Xtreem.Crusader.ML.Api.Models;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Extensions;

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
                .ScanAssembly()
                .Configure<ModelOptions>(Configuration.GetSection("Model"))
                .Configure<DataOptions>(Configuration.GetSection("Data"))
                .Configure<CryptoCompareOptions>(Configuration.GetSection("CryptoCompare"));

            AddPredictionEnginePool<OhlcvRegressionPrediction>();
            AddPredictionEnginePool<OhlcvTimeSeriesPrediction>();

            void AddPredictionEnginePool<TPrediction>() where TPrediction : class, new()
            {
                //TODO: Uncomment the delegate in the following statement, and remove the next statement once this PR is done: https://github.com/dotnet/machinelearning/pull/4393
                services.AddPredictionEnginePool<OhlcvInput, TPrediction>( /*sp =>
                {
                    services.AddOptions<PredictionEnginePoolOptions<OhlcvInput, TPrediction>>().Configure(options =>
                    {
                        options.ModelLoader = sp.GetService<TrainModelLoader>();
                    });
                    return new PredictionEnginePool<OhlcvInput, TPrediction>();
                }*/);

                services.AddSingleton<LazyService<PredictionEnginePool<OhlcvInput, TPrediction>>>();

                services.AddOptions<PredictionEnginePoolOptions<OhlcvInput, TPrediction>>().Configure(pepOptions =>
                {
                    services.AddOptions<TrainModelLoaderOptions>().Configure(tmlOptions =>
                    {
                        tmlOptions.ModelService = services.BuildServiceProvider().GetServices<IModelService>().Single(s => s.CanTrain(typeof(TPrediction)));
                    });

                    pepOptions.ModelLoader = services.BuildServiceProvider().GetService<TrainModelLoader>();
                });
            }
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
