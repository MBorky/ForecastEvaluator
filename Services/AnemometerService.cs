using ForecastEvaluator.DataModels;
using System.Globalization;

namespace ForecastEvaluator.Services
{
    public class AnemometerService
    {
        private readonly HttpClient _httpClient;
        private readonly ForecastContext _dbContext;
        public AnemometerService(HttpClient httpClient, ForecastContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }
        public async Task<string>FetchDataAsync(string url)
        {
            var response = await _httpClient.GetAsync(url); 
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public string[] StringToList (string rawData)
        {
            string[] strings = rawData.Split(' ');
            return strings;
        }
        public async Task SaveDataAsync(string[] data)
        {
            var anemometerReading = new AnemometerReading
            {
                ReadingDate = DateOnly.Parse(data[0]),
                Hour = TimeOnly.Parse(data[1]),
                WindSpeed = decimal.Parse(data[5]),
                WindGusts = decimal.Parse(data[40]),
                WindDirection = int.Parse(data[7])
            };
        
            
            _dbContext.AnemometerReadings.Add(anemometerReading);
            await _dbContext.SaveChangesAsync();
        }

    }
}
