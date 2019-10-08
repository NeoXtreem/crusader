using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xtreem.Crusader.Data.Contexts;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Settings;
using Xtreem.Crusader.ML.Api.Services;
using Xtreem.Crusader.ML.Api.Services.Interfaces;
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ReSharper disable once CommentTypo
            //TODO: Added AddNewtonsoftJson for Resolution type to deserialize as it has a readonly property as per https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#jsonnet-support. Revert if https://github.com/dotnet/corefx/issues/40602 is resolved.
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddScoped<IMarketDataContext, MarketDataContext>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IMarketDataReadRepository, MarketDataReadRepository>();
            services.AddTransient<IOhlcvMappingService, OhlcvMappingService>();

            services.Configure<ModelSettings>(Configuration.GetSection("Model"));
            services.Configure<DataSettings>(Configuration.GetSection("Data"));
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
