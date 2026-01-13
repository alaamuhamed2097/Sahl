using BL.Contracts.Service.Setting;
using BL.Services.Setting;

namespace Api.Extensions.Services
{
    public static class SettingServiceExtensions
    {
        public static IServiceCollection AddSettingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Setting Services
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();
            services.AddScoped<IDevelopmentSettingsService, DevelopmentSettingsService>();

            return services;
        }
    }
}

