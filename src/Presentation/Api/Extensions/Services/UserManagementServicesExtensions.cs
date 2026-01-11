using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Service.Location;
using BL.GeneralService.UserManagement;
using BL.Services.Location;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering user management and location services.
    /// </summary>
    public static class UserManagementServicesExtensions
    {
        /// <summary>
        /// Adds user registration, profile, and location (country/state/city) services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services)
        {
            // User Management Services
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICityService, CityService>();

            return services;
        }
    }
}
