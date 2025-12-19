using System.Text.Json.Serialization;

namespace weather.service.schema.Messages;

public class WttrApiResponse
{
    [JsonPropertyName("current_condition")]
    public List<CurrentCondition> CurrentCondition { get; set; } = new();

    [JsonPropertyName("nearest_area")]
    public List<NearestArea> NearestArea { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherForecast> Weather { get; set; } = new();
}

public class CurrentCondition
{
    [JsonPropertyName("temp_C")]
    public string TempC { get; set; } = string.Empty;

    [JsonPropertyName("temp_F")]
    public string TempF { get; set; } = string.Empty;

    [JsonPropertyName("FeelsLikeC")]
    public string FeelsLikeC { get; set; } = string.Empty;

    [JsonPropertyName("FeelsLikeF")]
    public string FeelsLikeF { get; set; } = string.Empty;

    [JsonPropertyName("weatherDesc")]
    public List<WeatherDescription> WeatherDesc { get; set; } = new();

    [JsonPropertyName("humidity")]
    public string Humidity { get; set; } = string.Empty;

    [JsonPropertyName("cloudcover")]
    public string CloudCover { get; set; } = string.Empty;

    [JsonPropertyName("windspeedKmph")]
    public string WindSpeedKmph { get; set; } = string.Empty;

    [JsonPropertyName("windspeedMiles")]
    public string WindSpeedMiles { get; set; } = string.Empty;

    [JsonPropertyName("winddir16Point")]
    public string WindDir16Point { get; set; } = string.Empty;

    [JsonPropertyName("pressure")]
    public string Pressure { get; set; } = string.Empty;

    [JsonPropertyName("uvIndex")]
    public string UvIndex { get; set; } = string.Empty;

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; } = string.Empty;
}

public class WeatherDescription
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

public class NearestArea
{
    [JsonPropertyName("areaName")]
    public List<AreaValue> AreaName { get; set; } = new();

    [JsonPropertyName("country")]
    public List<AreaValue> Country { get; set; } = new();

    [JsonPropertyName("region")]
    public List<AreaValue> Region { get; set; } = new();

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; } = string.Empty;

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; } = string.Empty;
}

public class AreaValue
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

public class WeatherForecast
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("maxtempC")]
    public string MaxTempC { get; set; } = string.Empty;

    [JsonPropertyName("mintempC")]
    public string MinTempC { get; set; } = string.Empty;

    [JsonPropertyName("avgtempC")]
    public string AvgTempC { get; set; } = string.Empty;
}
