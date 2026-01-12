using BL.Contracts.GeneralService;
using DAL.Services;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering general application services.
    /// </summary>
    public static class GeneralServiceExtensions
    {
        /// <summary>
        /// Adds general/core application services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddGeneralServices(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
