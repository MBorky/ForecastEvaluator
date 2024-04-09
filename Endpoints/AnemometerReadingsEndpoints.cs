using System.Runtime.CompilerServices;
using ForecastEvaluator.DataModels;
using ForecastEvaluator.Services;
namespace ForecastEvaluator.Endpoints
{
    public static class AnemometerReadingsEndpoints
    {
        public static void MapAnemometerReadingsEndpoints(this IEndpointRouteBuilder routes)
        {
            var anemometer = routes.MapGroup("/anemometer-readings");
            anemometer.MapGet("/update-anemometer", async (AnemometerService aneomometerService) =>
            {
                var rawData = await aneomometerService.FetchDataAsync("https://www.wiatrkadyny.pl/draga/realtime.txt");
                string[] data = aneomometerService.StringToList(rawData);
                await aneomometerService.SaveDataAsync(data);
                return Results.Ok("Dane anemometru zostały zaktualizowane.");
            });
            
        }
    }
}
