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

// W³¹czyæ code coverage - znaleŸæ narzêdzie do sprawdzenia pokrycia testów, przypadkowo nie podpi¹æ
// projektu testów, ciekawostka
// Wszystko co jest publiczne powinno byæ przetestowane
//  - Dostêp do aplikacji, w moim przypadku endpointy
//  * Na tym poziomie bardziej testy integracyjne
//  * W naszym za³o¿eniu jeœli coœ przetestowaliœmy jednostkowo to w integracyjnym nie 
// trzeba tego testowaæ tak szczegó³owo
//  * W takim razie trzeba przetestowaæ:
//  * Czy po podaniu poprawnych danych jest odpowiedŸ 200
//  * W rozszerzonym produkcie testujê te¿ walidacje, inne kody b³êdów

// Testy seriwsu, wszystkie publiczne metody musz¹ byæ przetestowane, tutaj testy jednostkowe
// Ka¿de rozga³êzienie w metodzie ma swój test (np. wszystkie if)






// xunit testy, plus moq, plus inmemory baza danych, flueant assertion

// Co dalej?
// 