using Dashboard.Contracts;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.Currency;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Location;
using Dashboard.Contracts.Order;
using Dashboard.Contracts.User;
using Dashboard.Services;
using Dashboard.Services.Brand;
using Dashboard.Services.Currency;
using Dashboard.Services.ECommerce.Category;
using Dashboard.Services.ECommerce.Item;
using Dashboard.Services.General;
using Dashboard.Services.Location;
using Dashboard.Services.Order;
using Dashboard.Services.User;

namespace Dashboard.Extensions
{
    public static class DomainServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IPromoCodeService, PromoCodeService>();
            services.AddScoped<IShippingCompanyService, ShippingCompanyService>();

            services.AddScoped<ICountryPhoneCodeService, CountryPhoneCodeService>();

            services.AddScoped<ICurrencyService, CurrencyService>();

            return services;
        }
    }
}
