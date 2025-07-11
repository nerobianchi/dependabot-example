using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    /// <summary>
    /// Gets weather forecast for the next specified number of days
    /// </summary>
    /// <param name="days">Number of days to forecast (default: 5)</param>
    /// <returns>Weather forecast data</returns>
    [HttpGet("forecast")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WeatherForecast>>>> GetWeatherForecast([FromQuery] int days = 5)
    {
        try
        {
            if (days <= 0 || days > 10)
            {
                return BadRequest(ApiResponse<IEnumerable<WeatherForecast>>.ErrorResponse("Days must be between 1 and 10"));
            }

            var forecast = await _weatherService.GetWeatherForecastAsync(days);
            return Ok(ApiResponse<IEnumerable<WeatherForecast>>.SuccessResponse(forecast, $"Weather forecast for {days} days"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<WeatherForecast>>.ErrorResponse($"An error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Gets weather forecast for a specific date
    /// </summary>
    /// <param name="date">Date in YYYY-MM-DD format</param>
    /// <returns>Weather forecast for the specified date</returns>
    [HttpGet("forecast/{date:datetime}")]
    public async Task<ActionResult<ApiResponse<WeatherForecast>>> GetWeatherForecastForDate(DateTime date)
    {
        try
        {
            var dateOnly = DateOnly.FromDateTime(date);
            var forecast = await _weatherService.GetWeatherForecastForDateAsync(dateOnly);
            
            if (forecast == null)
            {
                return NotFound(ApiResponse<WeatherForecast>.ErrorResponse("Weather forecast not found for the specified date"));
            }

            return Ok(ApiResponse<WeatherForecast>.SuccessResponse(forecast, "Weather forecast retrieved successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<WeatherForecast>.ErrorResponse($"An error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <returns>API health status</returns>
    [HttpGet("health")]
    public ActionResult<ApiResponse<string>> HealthCheck()
    {
        return Ok(ApiResponse<string>.SuccessResponse("Weather API is running", "Health check successful"));
    }
}
