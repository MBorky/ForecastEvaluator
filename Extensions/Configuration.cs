using ForecastEvaluator.DataModels;
using ForecastEvaluator.Services;
using Microsoft.EntityFrameworkCore;

namespace ForecastEvaluator.Extensions
{
    public static class Configuration
    {
        public static void RegisterServices(this WebApplicationBuilder builder) 
        {
            builder.Services
                .AddDbContext<ForecastContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDatabase")))
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddHttpClient()
                .AddHttpClient<AnemometerService>();
        }
        public static void RegisterMiddlewares(this WebApplication app)
        {
            if(app.Environment.IsDevelopment()) 
            {
                app.UseSwagger().UseSwaggerUI();
            }
        }
    }
}
