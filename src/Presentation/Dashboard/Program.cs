using Dashboard;
using Dashboard.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Resources;
using Resources.Enumerations;
using Resources.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Infrastructure + Core Services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Add Authentication Services
builder.Services.AddAuthenticationServices();

// Fallback HttpClient (used for static file requests usually)
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Localization (if you're not using built-in .NET localization)
ResourceManager.CurrentLanguage = Language.Arabic;

var supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en-US"),
    new CultureInfo("ar-EG")
};


var host = builder.Build();
var languageService = host.Services.GetRequiredService<LanguageService>();
ResourceManager.SetLanguageService(languageService);

await host.RunAsync();