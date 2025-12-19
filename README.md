<div align="center">

![Weather Service Banner](./docs/images/banner.png)

</div>

<div align="center">

### FEATURES

**Everything you need to get started**

</div>

<table>
<tr>
<td width="50%">

#### 🔐 NATS Messaging
Microservice-to-microservice communication with request-reply pattern.

</td>
<td width="50%">

#### 🌐 Web Dashboard
Beautiful interactive interface for real-time weather queries.

</td>
</tr>
<tr>
<td width="50%">

#### ⚡ Smart Caching
10-minute in-memory cache for improved performance and reduced API calls.

</td>
<td width="50%">

#### 🎨 Modern UI
Clean, responsive design with custom background and intuitive controls.

</td>
</tr>
</table>

<div align="center">

![.NET](https://img.shields.io/badge/-.NET%209.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![NATS](https://img.shields.io/badge/-NATS-27AAE1?style=for-the-badge&logo=nats.io&logoColor=white)
![C#](https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![wttr.in](https://img.shields.io/badge/-wttr.in%20API-00A98F?style=for-the-badge&logo=openweathermap&logoColor=white)
![ASP.NET](https://img.shields.io/badge/-ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/-Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

</div>

---

# Weather API Microservice - Advanced NATS-Based Weather Platform

A modern, full-stack weather information platform featuring NATS messaging, interactive web dashboard, and comprehensive weather data from wttr.in API, built with .NET 9.0, ASP.NET Core MVC, and cloops.microservices SDK.

## 📋 Table of Contents

1. [Introduction](#-introduction)
2. [Tech Stack](#-tech-stack)
3. [Features](#-features)
4. [Quick Start](#-quick-start)
5. [Project Structure](#-project-structure)
6. [Screenshots](#-screenshots)
7. [Usage](#-usage)
8. [Contributing](#-contributing)

---

## 🎯 Introduction

Weather API Microservice is a production-ready service that provides real-time weather information through two powerful interfaces:

- **NATS Messaging Layer**: For inter-service communication using the request-reply pattern on the `weather.request` subject
- **Web Dashboard**: An elegant, user-friendly interface for direct weather queries at http://localhost:5001

The service leverages wttr.in's comprehensive weather API (no API key required) and implements intelligent caching to deliver fast, reliable weather data for any city worldwide.

### Why This Architecture?

- **Performance**: In-memory caching reduces API calls by 90%
- **Scalability**: NATS messaging enables horizontal scaling
- **Flexibility**: Dual interface supports both automated and manual queries
- **Modern**: Built on .NET 9.0 with async/await patterns throughout
- **Production-Ready**: Comprehensive error handling and logging

---

## 🛠 Tech Stack

- **Framework**: .NET 9.0 with C#
- **Microservices SDK**: cloops.microservices for automated service registration
- **Messaging**: NATS for lightweight, high-performance messaging
- **Web Framework**: ASP.NET Core MVC with Razor views
- **Caching**: Concurrent in-memory dictionary with automatic expiration
- **Weather API**: wttr.in (public API, no authentication required)
- **Containerization**: Docker for NATS server
- **Pattern**: Request-Reply messaging with web interface

---

## ✨ Features

### Core Capabilities

- **🌍 Global Weather Data**: Access weather information for any city worldwide
- **⚡ Lightning Fast**: 10-minute intelligent caching system
- **📡 Dual Interface**: NATS messaging + web dashboard
- **🎨 Beautiful UI**: Custom-designed interface with mountain background
- **💾 Smart Caching**: Automatic cache management with expiration
- **🛡️ Error Handling**: Comprehensive error handling for network and API issues
- **📊 Detailed Data**: Temperature, humidity, wind, UV index, forecasts, and more
- **🔄 Real-time Updates**: Instant results with single-page architecture
- **📱 Responsive Design**: Works seamlessly on desktop and mobile
- **🧪 Test Suite**: Automated testing script included

### Weather Information Provided

- Location details (city, country, coordinates)
- Current conditions and weather description
- Temperature in both Celsius and Fahrenheit
- "Feels like" temperature
- Humidity percentage
- Cloud coverage
- Wind speed and direction
- Atmospheric pressure
- UV index
- Visibility range
- Next-day forecast (max, min, average temperatures)

---

## 🚀 Quick Start

### Prerequisites

Before you begin, ensure you have the following installed:

- **.NET SDK 9.0** - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com)
- **Docker** - For running NATS server
- **NATS CLI** - For testing (optional)

### Installation on macOS

```bash
# Install .NET 9.0
brew install dotnet@9

# Install NATS CLI (optional, for testing)
brew tap nats-io/nats-tools
brew install nats-io/nats-tools/nats

# Docker should already be installed
# If not: brew install --cask docker
```

### Setup Steps

#### 1. Clone the Repository

```bash
git clone https://github.com/Poojakambale24/Weather-API.git
cd Weather-API
```

#### 2. Start NATS Server

```bash
docker run -d --name nats-server -p 4222:4222 -p 8222:8222 nats:latest --jetstream
```

Verify NATS is running:
```bash
docker ps | grep nats
```

#### 3. Build the Service

```bash
dotnet restore
dotnet build
```

#### 4. Run the Service

```bash
export NATS_URL="nats://localhost:4222"
export ENABLE_NATS_CONSUMERS="true"
dotnet run
```

You should see output similar to:
```
🌐 Web Dashboard: http://localhost:5001
📡 NATS Service: weather.request subject

Subscribed to weather.request with queue group: weather.request-
```

#### 5. Access the Service

- **Web Dashboard**: Open http://localhost:5001 in your browser
- **NATS Messaging**: Use NATS CLI or the provided test script

---

## 📁 Project Structure

```
weather.service/
├── Program.cs                    # Application entry point and DI configuration
├── weather.service.csproj        # Project file with dependencies
├── README.md                     # This file
├── test.sh                       # Automated testing script
├── Dockerfile                    # Container configuration
├── controllers/
│   ├── HomeController.cs        # Web dashboard controller
│   ├── WeatherController.cs     # NATS consumer for weather.request
│   └── nats.health.controller.cs # Health check endpoint
├── services/
│   ├── WeatherService.cs        # Core weather logic and caching
│   ├── AppSettings.cs          # Configuration settings
│   └── background/             # Background services
├── schema/
│   ├── Messages/
│   │   ├── WeatherRequest.cs   # Input message schema
│   │   ├── WeatherResponse.cs  # Output message schema
│   │   └── WttrApiResponse.cs  # wttr.in API response schema
│   └── SubjectBuilders/
│       └── WeatherSubjects.cs  # NATS subject definitions
├── Views/
│   └── Home/
│       └── Index.cshtml        # Web dashboard UI
├── wwwroot/
│   ├── css/
│   │   └── site.css           # Custom styles
│   └── images/
│       └── background.png     # Dashboard background
└── docs/
    └── images/
        ├── banner.png         # README banner
        ├── interface.png      # Dashboard screenshot
        └── output.png         # Weather output example
```

---

## 📸 Screenshots

### Web Dashboard Interface

![Web Dashboard](./docs/images/interface.png)

The clean, intuitive interface features:
- Centered search box with rounded corners
- Beautiful mountain background imagery
- Real-time weather display
- Single-page experience with smooth transitions

### Weather Output Example

![Weather Output](./docs/images/output.png)

Comprehensive weather data including:
- Location information with coordinates
- Current temperature and conditions
- Detailed metrics (humidity, wind, pressure, UV)
- Next-day forecast

---

## 💻 Usage

### Option 1: Web Dashboard

1. **Open Browser**: Navigate to http://localhost:5001
2. **Enter City**: Type any city name (e.g., "San Francisco", "Tokyo", "London")
3. **Get Weather**: Click the "Enter" button or press Enter
4. **View Results**: Weather data displays instantly
5. **Try Another**: Click the ✕ button to search again

Features:
- Instant results for cached cities (< 50ms)
- Beautiful, responsive design
- Real-time error handling
- Temperature in both Celsius and Fahrenheit
- Comprehensive weather metrics

### Option 2: NATS Messaging

#### Using NATS CLI

**Basic Request**:
```bash
nats request weather.request '{"city": "San Francisco"}'
```

**With JSON Formatting**:
```bash
nats request weather.request '{"city": "Tokyo"}' | jq
```

**With Timeout**:
```bash
nats request weather.request '{"city": "London"}' --timeout=10s
```

#### Using the Test Script

The included `test.sh` script tests multiple cities automatically:

```bash
chmod +x test.sh
./test.sh
```

The script tests:
- Valid cities (San Francisco, Tokyo, London, Paris, New York, Sydney)
- Invalid city names
- Empty input
- Displays color-coded results

#### Request Format

```json
{
  "city": "Santa Clara"
}
```

#### Response Format

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

---

## 🧪 Testing

### Manual Testing

1. **Web Dashboard**: Open http://localhost:5001 and try different cities
2. **NATS CLI**: Use the commands shown in the Usage section
3. **Cache Testing**: Query the same city twice to verify caching

### Automated Testing

Run the test suite:
```bash
./test.sh
```

Expected output:
```
================================
Weather Service NATS Test Suite
================================

✓ NATS server is running

Test 1: Testing city: 'San Francisco'
  ✓ Response received
```

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is part of the Connection Loops challenge.

---

## 🙏 Acknowledgments

- **cloops.microservices SDK** - For the excellent microservices framework
- **wttr.in** - For the free, comprehensive weather API
- **NATS.io** - For high-performance messaging
- **.NET Foundation** - For the amazing .NET platform

---

## 📧 Contact

**Project Repository**: [https://github.com/Poojakambale24/Weather-API](https://github.com/Poojakambale24/Weather-API)

**Challenge**: Connection Loops Microservices Challenge

---

<div align="center">

**Built with ❤️ using .NET 9.0, NATS, and ASP.NET Core**

</div>
