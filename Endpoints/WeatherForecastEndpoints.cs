namespace ForecastEvaluator.Endpoints
{
    public static class WeatherForecastEndpoints
    {
        public static void MapWeatherForecastEndpoints(this IEndpointRouteBuilder routes)
        {
            var forecasts = routes.MapGroup("/weather-forecasts");
        }
    }
}
