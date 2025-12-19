
using CLOOPS.microservices;
using Microsoft.Extensions.DependencyInjection;

var app = new App();

// Register HttpClient factory for external API calls
app.builder.Services.AddHttpClient();

await app.RunAsync();
