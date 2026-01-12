using BL.Contracts.GeneralService.UserManagement;
using BL.GeneralService.UserManagement;

namespace Api.Extensions.Services
{
    public static class UserManagementServicesExtensions
    {
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services)
        {
            // User Management Services
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            return services;
        }
    }
}
