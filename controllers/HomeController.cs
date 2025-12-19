using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using weather.service.services;
using weather.service.schema.Messages;

namespace weather.service.Controllers;

public class HomeController : Controller
{
    private readonly WeatherService _weatherService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(WeatherService weatherService, ILogger<HomeController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetWeather([FromForm] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            ViewBag.Error = "Please enter a city name";
            return View("Index");
        }

        var request = new WeatherRequest { city = city };
        var response = await _weatherService.GetWeatherAsync(request);

        ViewBag.City = city;
        ViewBag.WeatherData = response.weather;

        return View("Index");
    }
}
