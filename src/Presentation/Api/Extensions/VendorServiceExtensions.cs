using BL.Contracts.Service.Vendor;
using BL.Contracts.Service.VendorDashboard;
using BL.Contracts.Service.VendorItem;
using BL.Services.Vendor;
using BL.Services.VendorDashboard;
using BL.Services.VendorItem;

namespace Api.Extensions
{
    public static class VendorServiceExtensions
    {
        public static IServiceCollection AddVendorServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Vendor Service
            services.AddScoped<IVendorManagementService, VendorManagementService>();

            // Vendor Items Services
            services.AddScoped<IVendorItemService, VendorItemService>();

            // Additional vendor-related services
            services.AddScoped<IVendorItemConditionService, VendorItemConditionService>();

            // Vendor Dashboard Services
            services.AddScoped<IVendorDashboardService, VendorDashboardService>();
            services.AddScoped<IVendorPerformanceIndicatorsService, VendorPerformanceIndicatorsService>();

            return services;
        }
    }
}
