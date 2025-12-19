<div align="center">

<img src="./docs/images/banner.png" alt="Weather Service Banner" width="100%" />

</div>

<div align="center">

![.NET](https://img.shields.io/badge/-.NET%209.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![NATS](https://img.shields.io/badge/-NATS-27AAE1?style=for-the-badge&logo=nats.io&logoColor=white)
![C#](https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/-ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/-Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

</div>

---

<div align="center">

# Weather API Microservice

</div>

## How to Run

### Prerequisites

- **.NET SDK 9.0** - [Download here](https://dotnet.microsoft.com)
- **Docker** - For NATS server
- **NATS CLI** - For testing (optional)

### Setup

**1. Clone the Repository**
```bash
git clone https://github.com/Poojakambale24/Weather-API.git
cd Weather-API
```

**2. Start NATS Server**
```bash
docker run -d --name nats-server -p 4222:4222 -p 8222:8222 nats:latest --jetstream
```

**3. Build and Run**
```bash
dotnet restore
dotnet build
export NATS_URL="nats://localhost:4222"
export ENABLE_NATS_CONSUMERS="true"
dotnet run
```

**4. Access the Service**
- **Web Dashboard**: http://localhost:5001
- **NATS Messaging**: `weather.request` subject

---

## How to Test

### Web Dashboard
Open http://localhost:5001 in your browser and enter any city name.

### NATS CLI
```bash
nats request weather.request '{"city": "San Francisco"}'
```

### Test Script
```bash
chmod +x test.sh
./test.sh
```

---

## Interface

<div align="center">

<img src="./docs/images/interface.png" alt="Web Dashboard Interface" width="80%" />

</div>

---

## Output

<div align="center">

<img src="./docs/images/output.png" alt="Weather Output Example" width="80%" />

</div>

---

## Test Results

<div align="center">

<img src="./docs/images/test-result.png" alt="Test Script Results" width="80%" />

</div>

---

<div align="center">

**Built with ❤️ using .NET 9.0, NATS, and ASP.NET Core**

</div>
