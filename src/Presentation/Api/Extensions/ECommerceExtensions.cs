using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Brand;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.Customer;
using BL.Contracts.Service.ECommerce.Category;
using BL.Contracts.Service.ECommerce.Item;
using BL.Contracts.Service.ECommerce.Unit;
using BL.Contracts.Service.PromoCode;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.ShippingCompny;
using BL.Contracts.Service.Testimonial;
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
using BL.Service.Testimonial;
using BL.Service.Vendor;
using BL.Services.Items;

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
            services.AddScoped<IPromoCodeService, PromoCodeService>();

            // Testimonial Services
            services.AddScoped<ITestimonialService, TestimonialService>();

            // Setting Services
            services.AddScoped<ISettingService, SettingService>();

            return services;
        }
    }
}
