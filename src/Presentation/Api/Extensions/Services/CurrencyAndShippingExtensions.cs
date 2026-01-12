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
    public static class CurrencyAndShippingExtensions
    {
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
