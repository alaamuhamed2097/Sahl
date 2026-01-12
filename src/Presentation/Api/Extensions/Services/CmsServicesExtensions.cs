using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.GeneralService.CMS;
using BL.GeneralService.UserManagement;
using System;

namespace Api.Extensions.Services
{
    public static class CmsServicesExtensions
    {
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
