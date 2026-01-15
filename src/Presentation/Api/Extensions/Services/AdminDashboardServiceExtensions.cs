using BL.Contracts.Service.AdminDashboard;
using BL.Service.AdminDashboard;

namespace Api.Extensions.Services
{
    public static class AdminDashboardServiceExtensions
    {
        public static IServiceCollection AddAdminDashboardServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            return services;
        }
    }
}
