using BL.Contracts.GeneralService.Location;
using BL.GeneralService.Location;

namespace Api.Extensions
{
    public static class LocationServicesExtensions
    {
        public static IServiceCollection AddLocationServices(this IServiceCollection services)
        {
            // Location Services
            services.AddScoped<IIpGeolocationService, IpGeolocationService>();

            return services;
        }
    }
}
