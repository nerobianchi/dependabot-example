using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int days = 5);
    Task<WeatherForecast?> GetWeatherForecastForDateAsync(DateOnly date);
}

public class WeatherService : IWeatherService
{
    private readonly string[] _summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int days = 5)
    {
        var forecast = Enumerable.Range(1, days).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ));

        return Task.FromResult(forecast);
    }

    public Task<WeatherForecast?> GetWeatherForecastForDateAsync(DateOnly date)
    {
        var forecast = new WeatherForecast
        (
            date,
            Random.Shared.Next(-20, 55),
            _summaries[Random.Shared.Next(_summaries.Length)]
        );

        return Task.FromResult<WeatherForecast?>(forecast);
    }
}
