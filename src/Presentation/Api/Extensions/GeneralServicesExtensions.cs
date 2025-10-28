using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using BL.GeneralService;
using BL.GeneralService.CMS;

namespace Api.Extensions
{
    public static class GeneralServicesExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            // General Application Services
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IApiService, ApiService>();

            return services;
        }
    }
}
