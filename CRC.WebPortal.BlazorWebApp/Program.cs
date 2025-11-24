using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Blazored.LocalStorage;
using CRC.WebPortal.BlazorWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Information);

// Set root components for pure WebAssembly
builder.RootComponents.Add<CRC.WebPortal.BlazorWebApp.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register the custom authorization message handler
builder.Services.AddScoped<AuthorizationMessageHandler>();

using var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
try
{
    using var appsettings = await httpClient.GetStreamAsync("appsettings.json");
    builder.Configuration.AddJsonStream(appsettings);
}
catch
{
}

if (builder.HostEnvironment.Environment == "Development")
{
    try
    {
        using var appsettingsDev = await httpClient.GetStreamAsync("appsettings.Development.json");
        builder.Configuration.AddJsonStream(appsettingsDev);
    }
    catch
    {
    }
}

builder.Services.AddHttpClient("API", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var apiBaseUrl = configuration["ApiBaseUrl"] ?? "https://crc-system-webapis.azurewebsites.net";
    client.BaseAddress = new Uri(apiBaseUrl);
})
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// Add Blazored LocalStorage for token storage
builder.Services.AddBlazoredLocalStorage();

// Add AuthorizationCore to enable AuthorizeView and policies
builder.Services.AddAuthorizationCore();

// Register Blazor-specific services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

// Register Toast Service following CQRS pattern
builder.Services.AddScoped<IToastService, ToastService>();

// Register UnitType Service following CQRS pattern
builder.Services.AddScoped<IUnitTypeService, UnitTypeService>();

await builder.Build().RunAsync();
