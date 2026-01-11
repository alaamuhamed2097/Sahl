using BL.Contracts.GeneralService;
using DAL.Services;

namespace Api.Extensions
{
    public static class GeneralServiceExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
