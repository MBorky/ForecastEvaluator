using Microsoft.EntityFrameworkCore;

namespace ForecastEvaluator.DataModels
{
    public class ForecastContext : DbContext
    {
        public ForecastContext(DbContextOptions<ForecastContext> options) : base(options)
        { 
        }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<ForecastDetail> ForecastsDetails { get; set; }
        public DbSet<AnemometerReading> AnemometerReadings { get; set; }
    }
}
