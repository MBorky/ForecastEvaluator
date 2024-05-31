using ForecastEvaluator.DataModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ForecastEvaluator.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ForecastContext _dbcontext;
        public WeatherService(IHttpClientFactory httpClient, ForecastContext dbcontext)
        {
            _httpClient = httpClient;
            _dbcontext = dbcontext;
        }
        // Jeden test, który sprawdza czy poprawnie zostały metody wywołane
        public async Task UpdateForecast(string apiUrlKey)
        {
            var rawData = await FetchweatherDataAsync(apiUrlKey);
            await SaveWeatherDataAsync(rawData);
        }
        // Linijka 1. Sprrawdzam czy api URL key jest poprawny format, czy nie jest NULL, czy jest konkretnym z 2 stringów
        // Linijka 2. W teście mokuje NULL i jak mój kod się zachowa, jak się zachowa jak przyjdzie zła lub dobra wartość
        // Linijka 5. Co się wydarzy co przyjdzie ok? Czyli np. podanie określonego pola i czy zapisze się to w zamierzonym miejscu i wratością
        // Linijka 6. 
        // Rozważyć wyciągnięcie klientów na zewnątrz do osobnej klasy z interfejsem by użyć DI, wtedy wywalić pierwsze 4 linijki do innej klasy
        // Ostatnia linijka, czy zwrca odpowiednio wartości
        public async Task<(Hourly,string)>FetchweatherDataAsync(string apiUrlKey)
        {
            HttpClient client = _httpClient.CreateClient(apiUrlKey);
            var response = await client.GetAsync("");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            var data = System.Text.Json.JsonSerializer.Deserialize<Root>(jsonString);
            return (data.hourly, apiUrlKey);
        }
        // saving data forecast for next day
        // Walidujemy parametry czyli sprawdzamy co się stanie jak przyjdzie NULL
        // Sprawdzić czy apiUrlKey nie jest nullem, co tam się dzieje, czy dobrze dane przypisuje np. czy jak przychodzi 15knt to zapisuje 15knt, a nie np kierunek
        // Co zrobię jak przyszły złe parametry? Czyli potrzebuję w logice validacji
        // Po zapisie tesetem odczytać bazę i czy jest poprawny record.
        public async Task SaveWeatherDataAsync((Hourly weatherData, string apiUrlKey) data)
        {
            var (weatherData, apiUrlKey) = data;


            var weatherForecast = new WeatherForecast
            {
                RetrievalDate = DateOnly.ParseExact(weatherData.time[0], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                ForecastForDate = DateOnly.ParseExact(weatherData.time[24], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                ModelType = apiUrlKey,
                ForecastDetails = new List<ForecastDetail>()

            };
            for (int i = 34; i <= 42; i++)
            {
                weatherForecast.ForecastDetails.Add(new ForecastDetail
                {
                    // 34 is 10:00 i table, 42 is 18;00

                    Hour = TimeOnly.ParseExact(weatherData.time[i], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                    WindSpeed = weatherData.wind_speed_10m[i],
                    WindDirection = weatherData.wind_direction_10m[i],
                    WindGusts = weatherData.wind_gusts_10m[i]
                });
            }
             _dbcontext.WeatherForecasts.Add(weatherForecast);
            await _dbcontext.SaveChangesAsync();
        }
    }

    public interface IWeatherService
    {
        Task UpdateForecast(string apiUrlKey);
        Task<(Hourly,string)> FetchweatherDataAsync(string apiUrlKey);
        Task SaveWeatherDataAsync((Hourly weatherData, string apiUrlKey) data);
    }
}
public class Hourly
{
    public List<string> time { get; set; }
    public List<decimal?> wind_speed_10m { get; set; }
    public List<int?> wind_direction_10m { get; set; }
    public List<decimal?> wind_gusts_10m { get; set; }
}

public class HourlyUnits
{
    public string time { get; set; }
    public string wind_speed_10m { get; set; }
    public string wind_direction_10m { get; set; }
    public string wind_gusts_10m { get; set; }
}

public class Root
{
    public decimal latitude { get; set; }
    public decimal longitude { get; set; }
    public decimal generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public decimal elevation { get; set; }
    public HourlyUnits hourly_units { get; set; }
    public Hourly hourly { get; set; }
}
