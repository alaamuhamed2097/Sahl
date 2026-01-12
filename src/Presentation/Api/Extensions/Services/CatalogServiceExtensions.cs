using BL.Contracts.Service.Brand;
using BL.Contracts.Service.Catalog.Category;
using BL.Contracts.Service.Catalog.Item;
using BL.Contracts.Service.Catalog.Unit;
using BL.Contracts.Service.Customer;
using BL.Services.Brand;
using BL.Services.Catalog.Category;
using BL.Services.Catalog.Item;
using BL.Services.Catalog.Unit;
using BL.Services.Customer;

namespace Api.Extensions.Services
{
    public static class CatalogServiceExtensions
    {
        public static IServiceCollection AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Category Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAttributeService, AttributeService>();

            // Item Services
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemSearchService, ItemSearchService>();
            services.AddScoped<IUnitService, UnitService>();

            // Custom Items Services
            services.AddScoped<ICustomerItemViewService, CustomerItemViewService>();

            // Register services
            services.AddScoped<IItemDetailsService, ItemDetailsService>();

            // Brand Services
            services.AddScoped<IBrandService, BrandService>();

            return services;
        }
    }
}
