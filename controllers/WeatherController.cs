using CLOOPS.NATS.Attributes;
using CLOOPS.NATS.Meta;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
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

    [NatsConsumer(_subject: WeatherSubjects.WeatherRequest)]
    public async Task<NatsAck> GetWeather(NatsMsg<WeatherRequest> msg, CancellationToken ct = default)
    {
        _logger.LogInformation("Received weather request for city: {City}", msg.Data?.city);
        
        var response = await _weatherService.GetWeatherAsync(msg.Data ?? new WeatherRequest());
        
        _logger.LogInformation("Returning weather response for city: {City}", msg.Data?.city);
        
        return new NatsAck(_isAck: true, _reply: response);
    }
}
