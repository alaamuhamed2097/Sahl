using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Service.Customer;
using BL.GeneralService.UserManagement;
using BL.Services.Customer;

namespace Api.Extensions.Services
{
    public static class UserManagementServicesExtensions
    {
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services)
        {
            // User Management Services
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            // Customer Management Services
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
