using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Services.General;
using Resources.Services;

namespace Dashboard.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Bind ApiSettings
            services.Configure<ApiSettings>(apiSettings =>
        config.GetSection("ApiSettings").Bind(apiSettings));

            // ✅ CRITICAL: In Blazor WebAssembly, HttpClient doesn't automatically send cookies
            // We use JavaScript fetch with credentials: 'include' instead
            // CookieApiService uses IJSRuntime to call JavaScript fetch, NOT HttpClient

            // Register CookieApiService with IJSRuntime (NOT HttpClient)
            services.AddScoped<IApiService, CookieApiService>();

            // Service For Toast
            //services.AddBlazoredToast();

            services.AddAuthorizationCore();

            services.AddSingleton<LanguageService>();

            return services;
        }
    }
}
