using Dashboard.Constants.ECommerce.Category;
using Dashboard.Contracts.General;
using Dashboard.Services.ECommerce.Category;
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

            return services;
        }
    }
}
