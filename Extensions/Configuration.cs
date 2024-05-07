using ForecastEvaluator.DataModels;
using ForecastEvaluator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using ForecastEvaluator.Endpoints;

namespace ForecastEvaluator.Extensions
{
    public static class Configuration
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddDbContext<ForecastContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDatabase")));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient("ICON", clinet =>
            {
                clinet.BaseAddress = new Uri
                ("https://api.open-meteo.com/v1/dwd-icon?latitude=54.6983&longitude=18.6773&hourly=wind_speed_10m,wind_direction_10m,wind_gusts_10m&wind_speed_unit=kn&timezone=Europe%2FBerlin&forecast_days=3&models=icon_d2");
                clinet.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddHttpClient("METEO-FRANCE", clinet =>
            {
                clinet.BaseAddress = new Uri
                ("https://api.open-meteo.com/v1/meteofrance?latitude=54.6983&longitude=18.6773&hourly=wind_speed_10m,wind_direction_10m,wind_gusts_10m&wind_speed_unit=kn&timezone=Europe%2FBerlin&forecast_days=3&models=arpege_europe");
                clinet.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddHttpClient("ANEMOMETER", clinet =>
            {
                clinet.BaseAddress = new Uri
                ("https://www.wiatrkadyny.pl/draga/realtime.txt");
                clinet.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddScoped<IAnemometerService, AnemometerService>();
            builder.Services.AddScoped<IWeatherService, WeatherService>();

            // Add Hangfire services.
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();

            // Add framework services.
            builder.Services.AddMvc();
        }
        public static void RegisterMiddlewares(this WebApplication app)
        {
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger().UseSwaggerUI();
            //}

            app.MapWeatherForecastEndpoints();
            app.MapAnemometerReadingsEndpoints();
            app.MapHangfireDashboard();
            app.UseHangfireDashboard();
        }
    }
}
