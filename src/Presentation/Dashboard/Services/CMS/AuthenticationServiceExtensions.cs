using Dashboard.Contracts.CMS;
using Dashboard.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Services.CMS
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            // ✅ UPDATED: Use cookie-based authentication providers
            services.AddAuthorizationCore();

            services.AddScoped<CookieAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<CookieAuthenticationStateProvider>());

            services.AddScoped<IAuthenticationService, CookieAuthenticationService>();

            return services;
        }
    }
}
