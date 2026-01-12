using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Location;
using BL.GeneralService.Location;
using BL.Services.Location;

namespace Api.Extensions
{
    public static class LocationServicesExtensions
    {
        public static IServiceCollection AddLocationServices(this IServiceCollection services)
        {
            // Location Geolocation Services
            services.AddScoped<IIpGeolocationService, IpGeolocationService>();

            // Location Country/State/City Services
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICityService, CityService>();

            return services;
        }
    }
}
