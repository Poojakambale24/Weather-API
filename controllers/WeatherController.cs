using CLOOPS.NATS.Attributes;
using Microsoft.Extensions.Logging;
using weather.service.schema.Messages;
using weather.service.schema.SubjectTypes;
using weather.service.services;

namespace weather.service.controllers;

public class WeatherController
{
    private readonly WeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(WeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [NatsConsumer(WeatherSubjects.WeatherRequest)]
    public async Task<WeatherResponse> GetWeather(WeatherRequest request)
    {
        _logger.LogInformation("Received weather request for city: {City}", request.city);
        
        var response = await _weatherService.GetWeatherAsync(request);
        
        _logger.LogInformation("Returning weather response for city: {City}", request.city);
        
        return response;
    }
}
