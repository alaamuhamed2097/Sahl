using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Service.Location;
using BL.GeneralService.UserManagement;
using BL.Services.Location;

namespace Api.Extensions
{
    public static class UserManagementServicesExtensions
    {
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
