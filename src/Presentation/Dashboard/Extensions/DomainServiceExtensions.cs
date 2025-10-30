using Dashboard.Contracts.Brand;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Dashboard.Services.Brand;
using Dashboard.Services.ECommerce.Category;
using Dashboard.Services.ECommerce.Item;
using Dashboard.Services.General;

namespace Dashboard.Extensions
{
    public static class DomainServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ICountryPhoneCodeService, CountryPhoneCodeService>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAttributeService, AttributeService>();

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUnitService, UnitService>();

            services.AddScoped<IBrandService, BrandService>();

            return services;
        }
    }
}
