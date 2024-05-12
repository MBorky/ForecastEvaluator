using ForecastEvaluator.DataModels;
using ForecastEvaluator.Endpoints;
using ForecastEvaluator.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using ForecastEvaluator.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.RegisterServices();
var app = builder.Build();
app.RegisterMiddlewares();
RecurringJob.AddOrUpdate("AnemometerCheck", (IAnemometerService anemometerService) =>
anemometerService.UpdateAnemometer("ANEMOMETER"), Cron.Hourly);
RecurringJob.AddOrUpdate("IconUpdate", (IWeatherService weatherService) =>
weatherService.UpdateForecast("ICON"), "0 10 * * *");
RecurringJob.AddOrUpdate("FranceUpdate", (IWeatherService weatherService) =>
weatherService.UpdateForecast("METEO-FRANCE"), "0 10 * * *");

// Aplication insights co do logow w app
// Wzorzec strukturalny circut braker co do zatrzymania odpytywan jak niedostepna baza
app.Run();
