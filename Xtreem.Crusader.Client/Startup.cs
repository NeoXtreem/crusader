using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xtreem.Crusader.Client.Repositories;
using Xtreem.Crusader.Client.Repositories.Interfaces;
using Xtreem.Crusader.Client.Services;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.Settings;
using Xtreem.Crusader.Data.Contexts;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Settings;

namespace Xtreem.Crusader.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ReSharper disable once CommentTypo
            //TODO: Added AddNewtonsoftJson to resolve undesired behaviour on TradingView as per https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#jsonnet-support. Periodically check if resolved.
            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            services.AddScoped<IMLService, MLService>();
            services.AddScoped<IHistoricalDataService, HistoricalDataService>();
            services.AddScoped<ICryptoCompareService, CryptoCompareService>();
            services.AddScoped<IMarketDataContext, MarketDataContext>();
            services.AddScoped<IMarketDataReadWriteRepository, MarketDataReadWriteRepository>();
            services.AddScoped<IMarketDataReadViewRepository, MarketDataReadViewRepository>();
            services.AddTransient<ICurrencyPairChartPeriodMappingService, CurrencyPairChartPeriodMappingService>();

            services.Configure<DataSettings>(Configuration.GetSection("Data"));
            services.Configure<CrusaderApiSettings>(Configuration.GetSection("CrusaderApi"));
            services.Configure<CryptoCompareSettings>(Configuration.GetSection("CryptoCompare"));

            // In production, the React files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
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
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer("start");
                }
            });
        }
    }
}
