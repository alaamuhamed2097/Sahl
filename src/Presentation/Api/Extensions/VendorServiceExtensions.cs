using BL.Contracts.Service.Vendor;
using BL.Contracts.Service.VendorItem;
using BL.Services.Vendor;
using BL.Services.VendorItem;

namespace Api.Extensions
{
    public static class VendorServiceExtensions
    {
        public static IServiceCollection AddVendorServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Vendor Service
            services.AddScoped<IVendorService, VendorService>();

            // Vendor Items Services
            services.AddScoped<IVendorItemService, VendorItemService>();

            return services;
        }
    }
}
