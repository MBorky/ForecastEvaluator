using ForecastEvaluator.DataModels;
using System.Globalization;

namespace ForecastEvaluator.Services
{
    public class AnemometerService : IAnemometerService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ForecastContext _dbContext;
        public AnemometerService(IHttpClientFactory httpClient, ForecastContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }
        public async Task UpdateAnemometer(string apiUrlKey)
        {
            var rawData = await FetchAnemometerDataAsync("ANEMOMETER");
            string[] data = StringToList(rawData);
            await SaveAnemometerDataAsync(data);
        }
        public async Task<string>FetchAnemometerDataAsync(string apiUrlKey)
        {
            HttpClient client = _httpClient.CreateClient(apiUrlKey);
            var response = await client.GetAsync(""); 
            response.EnsureSuccessStatusCode();
            var respone = await response.Content.ReadAsStringAsync();
            return respone;
        }
        public string[] StringToList (string rawData)
        {
            string[] strings = rawData.Split(' ');
            return strings;
        }
        public async Task SaveAnemometerDataAsync(string[] data)
        {
            var date = DateOnly.ParseExact(data[0], "dd/MM/yy", CultureInfo.InvariantCulture);
            //var time = TimeOnly.Parse(data[1]); // Ensure your time format is correctly handled here
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowUtc = DateTime.UtcNow;
            var nowInPoland = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, timeZone);
            var timeOnly = TimeOnly.FromDateTime(nowInPoland);


            var anemometerReading = new AnemometerReading
            {
                ReadingDate = date,
                Hour = timeOnly,
                //Hour = time,
                //ReadingDate = DateOnly.Parse(data[0]),
                //Hour = TimeOnly.Parse(data[1]),
                WindSpeed = decimal.Parse(data[5]),
                WindGusts = decimal.Parse(data[40]),
                WindDirection = int.Parse(data[7])
            };
        
            _dbContext.AnemometerReadings.Add(anemometerReading);
            await _dbContext.SaveChangesAsync();
        }
    }

    public interface IAnemometerService
    {
        Task UpdateAnemometer(string apiUrlKey);
        Task<string> FetchAnemometerDataAsync(string url);
        string[] StringToList(string rawData);
        Task SaveAnemometerDataAsync(string[] data);
    }
}
