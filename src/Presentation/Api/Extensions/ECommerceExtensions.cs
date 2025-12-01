using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Brand;
using BL.Contracts.Service.CouponCode;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.Customer;
using BL.Contracts.Service.ECommerce.Category;
using BL.Contracts.Service.ECommerce.Item;
using BL.Contracts.Service.ECommerce.Unit;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.ShippingCompny;
using BL.Contracts.Service.Vendor;
using BL.GeneralService.Location;
using BL.Service.Brand;
using BL.Service.Currency;
using BL.Service.Customer;
using BL.Service.ECommerce.Category;
using BL.Service.ECommerce.Unit;
using BL.Service.PromoCode;
using BL.Service.Setting;
using BL.Service.ShippingCompany;
using BL.Service.Vendor;
using BL.Contracts.Service.Pricing;
using BL.Service.Pricing;

namespace Api.Extensions
{
    public static class ECommerceExtensions
    {
        public static IServiceCollection AddECommerceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services

            // Vendor Service
            services.AddScoped<IVendorService, VendorService>();
            // Customer Service
            services.AddScoped<ICustomerService, CustomerService>();

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

            // shipping company
            services.AddScoped<IShippingCompanyService, ShippingCompanyService>();

            // Promo code
            services.AddScoped<ICouponCodeService, CouponCodeService>();

            // Setting Services
            services.AddScoped<ISettingService, SettingService>();

            // Pricing Services
            services.AddScoped<IPricingService, PricingService>();

            return services;
        }
    }
}
