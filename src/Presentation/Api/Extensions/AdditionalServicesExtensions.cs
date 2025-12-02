using BL.Contracts.Service.Content;
using BL.Contracts.Service.Notification;
using BL.Contracts.Service.Warehouse;
using BL.Service.Content;
using BL.Service.Notification;
using BL.Service.Warehouse;

namespace Api.Extensions
{
    public static class AdditionalServicesExtensions
    {
        public static IServiceCollection AddWarehouseAndInventoryServices(this IServiceCollection services)
        {
            // Warehouse Services
            services.AddScoped<IWarehouseService, WarehouseService>();

            return services;
        }

        public static IServiceCollection AddContentManagementServices(this IServiceCollection services)
        {
            // Content Management Services
            services.AddScoped<IContentAreaService, ContentAreaService>();
            services.AddScoped<IMediaContentService, MediaContentService>();

            return services;
        }

        public static IServiceCollection AddEnhancedNotificationServices(this IServiceCollection services)
        {
            // Enhanced Notification Services
            services.AddScoped<INotificationChannelService, NotificationChannelService>();
            services.AddScoped<INotificationsService, NotificationsService>();
            services.AddScoped<INotificationPreferencesService, NotificationPreferencesService>();

            return services;
        }
    }
}
