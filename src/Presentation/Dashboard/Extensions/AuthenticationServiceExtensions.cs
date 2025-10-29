using Dashboard.Contracts.General;
using Dashboard.Contracts.Handlers;
using Dashboard.Handlers;
using Dashboard.Providers;
using Dashboard.Services.General;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Extensions
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            // ✅ UPDATED: Use cookie-based authentication providers
            services.AddAuthorizationCore();

            services.AddScoped<ApiAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IApiStatusHandler, ApiStatusHandler>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
