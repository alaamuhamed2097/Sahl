using BL.Contracts.Service.Setting;
using BL.Services.Setting;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering settings and configuration services.
    /// </summary>
    public static class SettingServiceExtensions
    {
        /// <summary>
        /// Adds system settings and configuration services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddSettingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Setting Services
            services.AddScoped<ISettingService, SettingService>();

            services.AddScoped<ISystemSettingsService, SystemSettingsService>();

            return services;
        }
    }
}
