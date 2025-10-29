using Dashboard.Contracts.General;
using Dashboard.Contracts.Page;
using Dashboard.Contracts.Setting;
using Dashboard.Services.General;
using Dashboard.Services.Page;
using Dashboard.Services.Setting;

namespace Dashboard.Extensions
{
    public static class GeneralServiceExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped(typeof(ISearchService<>), typeof(SearchService<>));
            services.AddScoped<IResourceLoaderService, ResourceLoaderService>();
            return services;
        }
    }
}
