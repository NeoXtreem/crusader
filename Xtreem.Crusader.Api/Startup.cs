﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xtreem.Crusader.Api.Repositories;
using Xtreem.Crusader.Api.Repositories.Interfaces;
using Xtreem.Crusader.Api.Services;
using Xtreem.Crusader.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Contexts;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Settings;

namespace Xtreem.Crusader.Api
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
            services.AddScoped<IMarketDataContext, MarketDataContext>();
            services.AddScoped<IPredictionService, PredictionService>();

            services.AddScoped<IMarketDataReadViewRepository, MarketDataReadViewRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<DataSettings>(Configuration.GetSection("Data"));
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
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}