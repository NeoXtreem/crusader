using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Xtreem.Crusader.Cape.Api.Services;
using Xtreem.Crusader.Cape.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Contexts;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Settings;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Services;
using Xtreem.Crusader.ML.Data.Services.Interfaces;
using Xtreem.Crusader.ML.Data.Settings;

namespace Xtreem.Crusader.Cape.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IMarketDataContext, MarketDataContext>();
            services.AddScoped<IPredictionService, PredictionService>();
            services.AddScoped<IMarketDataReadRepository, MarketDataReadRepository>();
            services.AddTransient<IOhlcvMappingService, OhlcvMappingService>();

            var modelConfiguration = Configuration.GetSection("Model");
            var modelSettings = modelConfiguration.Get<ModelSettings>();
            services.Configure<ModelSettings>(modelConfiguration);
            services.Configure<DataSettings>(Configuration.GetSection("Data"));

            services.AddPredictionEnginePool<OhlcvInput, OhlcvPrediction>().FromUri(modelSettings.FilePath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
