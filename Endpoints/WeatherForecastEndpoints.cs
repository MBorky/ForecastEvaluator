using ForecastEvaluator.Services;
namespace ForecastEvaluator.Endpoints
{
    public static class WeatherForecastEndpoints
    {
        public static void MapWeatherForecastEndpoints(this IEndpointRouteBuilder routes)
        {
            var forecasts = routes.MapGroup("/weather-forecasts");
            forecasts.MapGet("/update-icon-forecast", async (IWeatherService weatherService) =>
            {
                /*var rawDataIcon = await weatherService.FetchweatherDataAsync("ICON");
                await weatherService.SaveWeatherDataAsync(rawDataIcon);*/
                await weatherService.UpdateForecast("ICON");
                return Results.Ok("Icon zaktualizowany");
            });
            forecasts.MapGet("/update-france-meteo-forecast", async (IWeatherService weatherService) =>
            {
                /*var rawDataFrance = await weatherService.FetchweatherDataAsync("METEO-FRANCE");
                await weatherService.SaveWeatherDataAsync(rawDataFrance);*/
                await weatherService.UpdateForecast("METEO-FRANCE");
                return Results.Ok("france-meteo zaktualizowane");
            });
        }
    }
}
