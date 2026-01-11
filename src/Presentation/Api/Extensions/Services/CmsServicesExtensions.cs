using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.GeneralService.CMS;
using BL.GeneralService.UserManagement;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering CMS and user authentication services.
    /// </summary>
    public static class CmsServicesExtensions
    {
        /// <summary>
        /// Adds CMS-related services for user authentication and file management.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddCmsServices(this IServiceCollection services)
        {
            // CMS Services
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserActivationService, UserActivationService>();
            services.AddScoped<IUserTokenService, UserTokenService>();
            services.AddScoped<IRoleManagementService, RoleManagementService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<IOAuthService, OAuthService>();

            return services;
        }
    }
}
