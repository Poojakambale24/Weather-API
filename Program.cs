
using CLOOPS.microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// Set port for web dashboard via environment variable
Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "http://localhost:5001");

// Create a separate web application for the dashboard FIRST
var webAppBuilder = WebApplication.CreateBuilder();

// Add services to web app
webAppBuilder.Services.AddMemoryCache();
webAppBuilder.Services.AddHttpClient();
webAppBuilder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Register WeatherService manually for the web app
webAppBuilder.Services.AddSingleton<weather.service.services.WeatherService>();

var webApp = webAppBuilder.Build();

// Configure middleware
webApp.UseStaticFiles();
webApp.UseRouting();
webApp.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create the NATS microservice app (this builds the host in constructor)
var natsApp = new App();

Console.WriteLine("🌐 Web Dashboard: http://localhost:5001");
Console.WriteLine("📡 NATS Service: weather.request subject");
Console.WriteLine();

// Start both applications concurrently
var natsTask = natsApp.RunAsync();
var webTask = webApp.RunAsync();

await Task.WhenAll(natsTask, webTask);
