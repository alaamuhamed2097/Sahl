using BL.Contracts.Service.Warehouse;
using BL.Contracts.Service.Inventory;
using BL.Contracts.Service.Content;
using BL.Contracts.Service.Notification;
using BL.Service.Warehouse;
using BL.Service.Inventory;
using BL.Service.Content;
using BL.Service.Notification;

namespace Api.Extensions
{
    public static class AdditionalServicesExtensions
    {
        public static IServiceCollection AddWarehouseAndInventoryServices(this IServiceCollection services)
        {
            // Warehouse Services
            services.AddScoped<IWarehouseService, WarehouseService>();

            // Inventory Services
            services.AddScoped<IMoitemService, MoitemService>();
            services.AddScoped<IMortemService, MortemService>();
            services.AddScoped<IMovitemsdetailService, MovitemsdetailService>();

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
