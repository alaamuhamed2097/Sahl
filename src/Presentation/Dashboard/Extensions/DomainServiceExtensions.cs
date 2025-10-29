using Dashboard.Contracts.General;
using Dashboard.Services.General;

namespace Dashboard.Extensions
{
    public static class DomainServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ICountryPhoneCodeService, CountryPhoneCodeService>();


            return services;
        }
    }
}
