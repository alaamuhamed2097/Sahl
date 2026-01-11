using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.Order.Fulfillment;
using BL.Contracts.Service.ShippingCompny;
using BL.GeneralService.Location;
using BL.Services.Currency;
using BL.Services.Order.Fulfillment;
using BL.Services.ShippingCompany;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering currency and shipping-related services.
    /// </summary>
    public static class CurrencyAndShippingExtensions
    {
        /// <summary>
        /// Adds currency conversion and shipping calculation services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddCurrencyAndShippingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Currency Services
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyConversionFactory, CurrencyConversionFactory>();
            services.AddScoped<ILocationBasedCurrencyService, LocationBasedCurrencyService>();

            // Shipping company
            services.AddScoped<IShippingCalculationService, ShippingCalculationService>();
            services.AddScoped<IShippingCompanyService, ShippingCompanyService>();

            return services;
        }
    }
}
