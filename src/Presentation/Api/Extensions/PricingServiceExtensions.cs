using BL.Contracts.Service.Catalog.Pricing;
using BL.Contracts.Service.Pricing;
using BL.Services.Catalog.Pricing;
using BL.Services.Setting.Pricing;

namespace Api.Extensions
{
    public static class PricingServiceExtensions
    {
        public static IServiceCollection AddPricingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Pricing Services
            services.AddScoped<IPricingSettingsService, PricingSettingsService>();

            services.AddScoped<IPricingStrategy, SimplePricingStrategy>();
            services.AddScoped<IPricingStrategy, CombinationBasedPricingStrategy>();
            services.AddScoped<IPricingStrategy, QuantityBasedPricingStrategy>();
            services.AddScoped<IPricingStrategy, HybridPricingStrategy>();

            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<SimplePricingStrategy>();
            services.AddScoped<CombinationBasedPricingStrategy>();
            services.AddScoped<QuantityBasedPricingStrategy>();
            services.AddScoped<HybridPricingStrategy>();

            return services;
        }
    }
}
