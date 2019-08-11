using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xtreem.CryptoPrediction.Client.Repositories;
using Xtreem.CryptoPrediction.Client.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Client.Services;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;
using Xtreem.CryptoPrediction.Client.Settings;
using Xtreem.CryptoPrediction.Data.Contexts;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Settings;

namespace Xtreem.CryptoPrediction.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHistoricalDataService, HistoricalDataService>();
            services.AddScoped<ICryptoCompareService, CryptoCompareService>();
            services.AddScoped<IBinanceService, BinanceService>();
            services.AddScoped<IPredictionService, PredictionService>();

            services.AddScoped<IMarketDataContext, MarketDataContext>();
            services.AddScoped<IMarketDataReadWriteRepository, MarketDataReadWriteRepository>();

            services.AddTransient<IKlineMappingService, KlineMappingService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<DataSettings>(Configuration.GetSection("Data"));
            services.Configure<CryptoCompareSettings>(Configuration.GetSection("CryptoCompare"));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
