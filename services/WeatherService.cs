using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using weather.service.schema.Messages;

namespace weather.service.services;

public class WeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly HttpClient _httpClient;
    private const string WttrBaseUrl = "http://wttr.in";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
    
    // Simple in-memory cache
    private static readonly ConcurrentDictionary<string, CacheEntry> _cache = new();

    private class CacheEntry
    {
        public string Data { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public WeatherService(
        ILogger<WeatherService> logger, 
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<WeatherResponse> GetWeatherAsync(WeatherRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.city))
            {
                _logger.LogWarning("Empty city name provided");
                return new WeatherResponse 
                { 
                    weather = JsonSerializer.Serialize(new { error = "City name is required" }) 
                };
            }

            // Check cache first
            var cacheKey = $"weather_{request.city.ToLowerInvariant()}";
            if (_cache.TryGetValue(cacheKey, out var entry) && entry.ExpiresAt > DateTime.UtcNow)
            {
                _logger.LogInformation("Returning cached weather for city: {City}", request.city);
                return new WeatherResponse { weather = entry.Data };
            }

            // Clean up expired entries periodically
            CleanExpiredCache();

            var encodedCity = Uri.EscapeDataString(request.city);
            var url = $"{WttrBaseUrl}/{encodedCity}?format=j2";

            _logger.LogInformation("Fetching weather from API for city: {City}", request.city);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("wttr.in API returned error status: {StatusCode}", response.StatusCode);
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new WeatherResponse 
                    { 
                        weather = JsonSerializer.Serialize(new { error = $"City '{request.city}' not found" }) 
                    };
                }

                return new WeatherResponse 
                { 
                    weather = JsonSerializer.Serialize(new { error = "Weather service unavailable" }) 
                };
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            
            var wttrResponse = JsonSerializer.Deserialize<WttrApiResponse>(jsonContent);
            
            if (wttrResponse == null || 
                wttrResponse.CurrentCondition == null || 
                wttrResponse.CurrentCondition.Count == 0)
            {
                _logger.LogWarning("Invalid weather data received for city: {City}", request.city);
                return new WeatherResponse 
                { 
                    weather = JsonSerializer.Serialize(new { error = "Invalid weather data received" }) 
                };
            }

            var current = wttrResponse.CurrentCondition[0];
            var area = wttrResponse.NearestArea.FirstOrDefault();
            var forecast = wttrResponse.Weather.FirstOrDefault();

            var weatherData = new
            {
                location = new
                {
                    city = area?.AreaName?.FirstOrDefault()?.Value ?? request.city,
                    country = area?.Country?.FirstOrDefault()?.Value ?? "Unknown",
                    region = area?.Region?.FirstOrDefault()?.Value ?? "Unknown",
                    latitude = area?.Latitude ?? "Unknown",
                    longitude = area?.Longitude ?? "Unknown"
                },
                current = new
                {
                    temperature_celsius = current.TempC,
                    temperature_fahrenheit = current.TempF,
                    feels_like_celsius = current.FeelsLikeC,
                    feels_like_fahrenheit = current.FeelsLikeF,
                    condition = current.WeatherDesc?.FirstOrDefault()?.Value ?? "Unknown",
                    humidity = current.Humidity + "%",
                    cloud_cover = current.CloudCover + "%",
                    wind_speed_kmph = current.WindSpeedKmph,
                    wind_speed_mph = current.WindSpeedMiles,
                    wind_direction = current.WindDir16Point,
                    pressure_mb = current.Pressure,
                    uv_index = current.UvIndex,
                    visibility_km = current.Visibility
                },
                forecast = forecast != null ? new
                {
                    date = forecast.Date,
                    max_temp_celsius = forecast.MaxTempC,
                    min_temp_celsius = forecast.MinTempC,
                    avg_temp_celsius = forecast.AvgTempC
                } : null
            };

            var weatherJson = JsonSerializer.Serialize(weatherData, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            // Cache the result
            _cache[cacheKey] = new CacheEntry 
            { 
                Data = weatherJson, 
                ExpiresAt = DateTime.UtcNow.Add(CacheDuration) 
            };

            _logger.LogInformation("Successfully fetched weather for city: {City}", request.city);

            return new WeatherResponse { weather = weatherJson };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching weather for city: {City}", request.city);
            return new WeatherResponse 
            { 
                weather = JsonSerializer.Serialize(new { error = "Failed to connect to weather service" }) 
            };
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request timeout while fetching weather for city: {City}", request.city);
            return new WeatherResponse 
            { 
                weather = JsonSerializer.Serialize(new { error = "Weather service request timeout" }) 
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for city: {City}", request.city);
            return new WeatherResponse 
            { 
                weather = JsonSerializer.Serialize(new { error = "Invalid weather data format" }) 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching weather for city: {City}", request.city);
            return new WeatherResponse 
            { 
                weather = JsonSerializer.Serialize(new { error = "Internal server error" }) 
            };
        }
    }

    private static void CleanExpiredCache()
    {
        var now = DateTime.UtcNow;
        var expiredKeys = _cache.Where(kvp => kvp.Value.ExpiresAt <= now)
                                .Select(kvp => kvp.Key)
                                .ToList();
        
        foreach (var key in expiredKeys)
        {
            _cache.TryRemove(key, out _);
        }
    }
}
