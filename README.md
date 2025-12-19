# Weather API Microservice

A NATS-based microservice that provides weather information for cities using the wttr.in public API.

## Overview

This microservice is built using the `cloops.microservices` SDK and communicates via NATS messaging rather than traditional REST endpoints. It listens on the `weather.request` subject and returns weather data in JSON format.

## Architecture

- **Framework**: .NET 9.0 with cloops.microservices SDK
- **Messaging**: NATS for inter-service communication
- **Weather Data Source**: wttr.in public API (no API key required)
- **Pattern**: Request-Reply messaging pattern

## Prerequisites

- .NET SDK 9.0
- Docker (for running NATS server)
- NATS CLI (for testing)

### Installation

**macOS**:
```bash
# Install .NET 9.0
brew install dotnet@9

# Install NATS CLI
brew tap nats-io/nats-tools
brew install nats-io/nats-tools/nats

# Docker should already be installed
```

## Setup

### 1. Start NATS Server

```bash
docker run -d --name nats-server -p 4222:4222 -p 8222:8222 nats:latest --jetstream
```

### 2. Build the Service

```bash
dotnet restore
dotnet build
```

### 3. Run the Service

```bash
export NATS_URL="nats://127.0.0.1:4222"
export ENABLE_NATS_CONSUMERS="True"
dotnet run
```

The service will start and subscribe to the `weather.request` subject.

## Usage

### Request Format

Send a NATS request to the `weather.request` subject with the following JSON payload:

```json
{
  "city": "Santa Clara"
}
```

### Response Format

The service returns a JSON response with the following structure:

```json
{
  "weather": "{
    \"location\": {
      \"city\": \"Santa Clara\",
      \"country\": \"United States of America\",
      \"region\": \"California\",
      \"latitude\": \"37.354\",
      \"longitude\": \"-121.954\"
    },
    \"current\": {
      \"temperature_celsius\": \"12\",
      \"temperature_fahrenheit\": \"53\",
      \"feels_like_celsius\": \"12\",
      \"feels_like_fahrenheit\": \"54\",
      \"condition\": \"Overcast\",
      \"humidity\": \"86%\",
      \"cloud_cover\": \"100%\",
      \"wind_speed_kmph\": \"4\",
      \"wind_speed_mph\": \"2\",
      \"wind_direction\": \"E\",
      \"pressure_mb\": \"1019\",
      \"uv_index\": \"0\",
      \"visibility_km\": \"13\"
    },
    \"forecast\": {
      \"date\": \"2025-12-19\",
      \"max_temp_celsius\": \"17\",
      \"min_temp_celsius\": \"9\",
      \"avg_temp_celsius\": \"12\"
    }
  }"
}
```

### Testing with NATS CLI

```bash
# Request weather for a city
nats req weather.request '{"city":"London"}' --timeout=15s

# Request weather for Santa Clara
nats req weather.request '{"city":"Santa Clara"}' --timeout=15s

# Request weather for New York
nats req weather.request '{"city":"New York"}' --timeout=15s
```

## Error Handling

The service handles various error scenarios gracefully:

### Empty City Name

**Request**:
```bash
nats req weather.request '{"city":""}'
```

**Response**:
```json
{
  "weather": "{\"error\":\"City name is required\"}"
}
```

### Invalid City Name

**Request**:
```bash
nats req weather.request '{"city":"InvalidCityXYZ12345"}'
```

**Response**:
```json
{
  "weather": "{\"error\":\"City 'InvalidCityXYZ12345' not found\"}"
}
```

### API Timeout

**Response**:
```json
{
  "weather": "{\"error\":\"Weather service request timeout\"}"
}
```

### Network Failure

**Response**:
```json
{
  "weather": "{\"error\":\"Failed to connect to weather service\"}"
}
```

## Project Structure

```
weather.service/
├── Program.cs                          # Application entry point
├── weather.service.csproj              # Project configuration
├── controllers/
│   ├── WeatherController.cs           # NATS consumer for weather.request
│   └── nats.health.controller.cs      # Health check endpoint
├── services/
│   ├── WeatherService.cs              # Business logic for weather data
│   ├── AppSettings.cs                 # Application configuration
│   └── ...                            # Other services
├── schema/
│   ├── Messages/
│   │   ├── WeatherRequest.cs          # Request message schema
│   │   ├── WeatherResponse.cs         # Response message schema
│   │   └── WttrApiResponse.cs         # wttr.in API response mapping
│   └── SubjectTypes/
│       └── WeatherSubjects.cs         # NATS subject constants
└── util/
    └── Util.cs                        # Utility functions
```

## Development

### Running in Development Mode

```bash
export NATS_URL="nats://127.0.0.1:4222"
export ENABLE_NATS_CONSUMERS="True"
dotnet watch run
```

### Running Tests

```bash
cd weather.service.Tests
dotnet test
```

### Building Docker Image

```bash
docker build -t weather-service:latest .
```

### Running in Docker

```bash
docker run -d \
  --name weather-service \
  --network host \
  -e NATS_URL="nats://127.0.0.1:4222" \
  -e ENABLE_NATS_CONSUMERS="True" \
  weather-service:latest
```

## Environment Variables

| Variable | Description | Default | Required |
|----------|-------------|---------|----------|
| `NATS_URL` | NATS server connection URL | `tls://nats.ccnp.cloops.in:4222` | Yes (for local dev) |
| `ENABLE_NATS_CONSUMERS` | Enable NATS message consumers | `False` | Yes |
| `OTEL_ENDPOINT` | OpenTelemetry endpoint for metrics | - | No |

## Technology Stack

- **.NET 9.0**: Modern, high-performance runtime
- **cloops.microservices SDK**: Opinionated microservices framework
- **NATS**: Lightweight, high-performance messaging system
- **wttr.in API**: Free weather data API

## Features

- **Real-time weather data**: Current conditions and forecasts
- **Error handling**: Comprehensive error messages for various failure scenarios
- **Structured logging**: JSON-formatted logs for easy parsing
- **Health checks**: Built-in health check endpoint
- **Metrics**: OpenTelemetry integration for monitoring
- **Type-safe**: Strong typing for all messages and responses

## License

This project is part of the Connection Loops microservices ecosystem.

## Support

For issues or questions, please refer to the [cloops.microservices documentation](https://github.com/connectionloops/cloops.microservices).
