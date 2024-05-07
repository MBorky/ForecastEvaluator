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
            anemometer.MapGet("/update-anemometer", async (IAnemometerService anemometerService) =>
            {
                /*var rawData = await aneomometerService.FetchAnemometerDataAsync("ANEMOMETER");
                string[] data = aneomometerService.StringToList(rawData);
                await aneomometerService.SaveAnemometerDataAsync(data);*/
                await anemometerService.UpdateAnemometer("ANEMOMETER");
                return Results.Ok("Dane anemometru zostały zaktualizowane.");
            });
        }
    }
}
