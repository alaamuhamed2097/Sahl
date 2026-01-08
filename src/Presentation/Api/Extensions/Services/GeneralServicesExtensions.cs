using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using BL.GeneralService;
using BL.GeneralService.CMS;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering general cross-cutting services.
    /// </summary>
    public static class GeneralServicesExtensions
    {
        /// <summary>
        /// Adds general application and caching services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            // General Application Services
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IApiService, ApiService>();

            return services;
        }
    }
}
