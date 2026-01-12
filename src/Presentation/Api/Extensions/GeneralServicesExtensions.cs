using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using BL.GeneralService;
using BL.GeneralService.CMS;
using DAL.Services;

namespace Api.Extensions
{
    public static class GeneralServicesExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}
