using BL.Contracts.Service.Notification;
using BL.Contracts.Service.Warehouse;
using BL.Services.Notification;
using BL.Services.Warehouse;

namespace Api.Extensions
{
    public static class AdditionalServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add all categorized domain services
            services.AddGeneralServices(configuration);
            services.AddCatalogServices(configuration);
            services.AddVendorServices(configuration);
            services.AddCurrencyAndShippingServices(configuration);
            services.AddOrderServices(configuration);
            services.AddPricingServices(configuration);
            services.AddMerchandisingServices(configuration);
            services.AddReviewServices(configuration);
            services.AddWalletServices(configuration);
            services.AddSettingServices(configuration);

            return services;
        }

        public static IServiceCollection AddWarehouseAndInventoryServices(this IServiceCollection services)
        {
            // Warehouse Services
            services.AddScoped<IWarehouseService, WarehouseService>();

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
