using Dashboard.Contracts.General;
using Dashboard.Contracts.Notification;
using Dashboard.Contracts.Page;
using Dashboard.Contracts.Setting;
using Dashboard.Services.General;
using Dashboard.Services.Notification;
using Dashboard.Services.Page;
using Dashboard.Services.Setting;

namespace Dashboard.Extensions
{
    public static class GeneralServiceExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            // services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationStateService, NotificationStateService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<ISettingService, SettingService>();
            // services.AddScoped<ITestimonialService, TestimonialService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped(typeof(ISearchService<>), typeof(SearchService<>));
            services.AddScoped<IResourceLoaderService, ResourceLoaderService>();
            return services;
        }
    }
}
