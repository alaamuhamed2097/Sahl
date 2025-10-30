using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Brand;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.ECommerce.Category;
using BL.Contracts.Service.ECommerce.Item;
using BL.Contracts.Service.ECommerce.Unit;
using BL.GeneralService.Location;
using BL.Service.Brand;
using BL.Service.Currency;
using BL.Service.ECommerce.Category;
using BL.Service.ECommerce.Unit;
using BL.Services.Items;

namespace Api.Extensions
{
    public static class ECommerceExtensions
    {
        public static IServiceCollection AddECommerceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services

            // Category Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAttributeService, AttributeService>();

            // Item  Services
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUnitService, UnitService>();

            // Brand Services
            services.AddScoped<IBrandService, BrandService>();

            // Currency Services
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyConversionFactory, CurrencyConversionFactory>();
            services.AddScoped<ILocationBasedCurrencyService, LocationBasedCurrencyService>();

            return services;
        }
    }
}
