using CLOOPS.NATS.Attributes;
using CLOOPS.NATS.Meta;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using weather.service.services;
using weather.service.services.http;

namespace weather.service.controllers;

public class HealthController
{
    private readonly ILogger<HealthController> _logger;
    private readonly AppSettings _appSettings;
    private readonly EchoService _echoService;
    private readonly DefaultHttpService _defaultHttpService;
    public HealthController(ILogger<HealthController> logger, AppSettings appSettings, EchoService echoService, DefaultHttpService defaultHttpService)
    {
        _logger = logger;
        _appSettings = appSettings;
        _echoService = echoService;
        _defaultHttpService = defaultHttpService;
    }

    [NatsConsumer(_subject: "health.weather.service")]
    public async Task<NatsAck> GetHealth(NatsMsg<string> msg, CancellationToken ct = default)
    {
        _logger.LogDebug("Health check requested");
        var reply = new HealthReply
        {
            Status = new()
            {
                ["appName"] = _appSettings.AssemblyName,
                ["appStatus"] = "ok",
                ["appMessage"] = $"what you said: {_echoService.Echo(msg.Data ?? "no message")}",
                ["responder"] = $"{_appSettings.AssemblyName}:{Environment.MachineName}"
            }
        };
        return new NatsAck(_isAck: true, _reply: reply);
    }
}