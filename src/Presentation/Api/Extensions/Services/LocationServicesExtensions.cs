using BL.Contracts.GeneralService.Location;
using BL.GeneralService.Location;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering location-based services.
    /// </summary>
    public static class LocationServicesExtensions
    {
        /// <summary>
        /// Adds location and IP geolocation services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddLocationServices(this IServiceCollection services)
        {
            // Location Services
            services.AddScoped<IIpGeolocationService, IpGeolocationService>();

            return services;
        }
    }
}
