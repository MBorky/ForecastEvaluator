using ForecastEvaluator.DataModels;
using ForecastEvaluator.Endpoints;
using ForecastEvaluator.Extensions;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.RegisterServices();
var app = builder.Build();
app.RegisterMiddlewares();
//Register Endpoints
app.MapAnemometerReadingsEndpoints();

app.Run();

