﻿using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;
using Xtreem.Crusader.ML.Api.Models;
using Xtreem.Crusader.ML.Api.Profiles;
using Xtreem.Crusader.ML.Api.Services;
using Xtreem.Crusader.ML.Api.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Shared.Models;
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
            services.AddControllers();

            services
                .ScanAssembly()
                .Configure<ModelOptions>(Configuration.GetSection("Model"))
                .Configure<DataOptions>(Configuration.GetSection("Data"))
                .Configure<CryptoCompareOptions>(Configuration.GetSection("CryptoCompare"))
                .AddAutoMapper(typeof(OhlcvProfile))
                /*.AddPredictionEnginePoolConfigOnly()*/; //TODO: Uncomment once this PR is done: https://github.com/dotnet/machinelearning/pull/4393

            AddPredictionEnginePool<OhlcvPrediction>();

            void AddPredictionEnginePool<TPrediction>() where TPrediction : class, new()
            {
                services.AddPredictionEnginePool<OhlcvInput, TPrediction>(); //TODO: Remove once this PR is done: https://github.com/dotnet/machinelearning/pull/4393
                services.AddSingleton(sp => new LazyService<PredictionEnginePool<OhlcvInput, TPrediction>>(sp));

                services.AddOptions<PredictionEnginePoolOptions<OhlcvInput, TPrediction>>().Configure(options =>
                {
                    options.ModelLoader = services.BuildServiceProvider().GetService<TrainModelLoader>();
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
