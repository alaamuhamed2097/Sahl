using BL.Contracts.Service.Vendor;
using BL.Contracts.Service.VendorDashboard;
using BL.Contracts.Service.VendorItem;
using BL.Services.Vendor;
using BL.Services.VendorDashboard;
using BL.Services.VendorItem;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering vendor-related services.
    /// </summary>
    public static class VendorServiceExtensions
    {
        /// <summary>
        /// Adds vendor and vendor item services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddVendorServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Vendor Service
            services.AddScoped<IVendorManagementService, VendorManagementService>();

            // Vendor Items Services
            services.AddScoped<IVendorItemService, VendorItemService>();

            // Additional vendor-related services can be registered here
            services.AddScoped<IVendorItemConditionService, VendorItemConditionService>();
            // Vendor Dashboard Services
            services.AddScoped<IVendorDashboardService, VendorDashboardService>();
            services.AddScoped<IVendorPerformanceIndicatorsService, VendorPerformanceIndicatorsService>();

            return services;
        }
    }
}
