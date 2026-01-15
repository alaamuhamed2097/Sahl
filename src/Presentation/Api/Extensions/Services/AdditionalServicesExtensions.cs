using BL.Contracts.Service.Notification;
using BL.Contracts.Service.VendorWarehouse;
using BL.Contracts.Service.Warehouse;
using BL.Services.Notification;
using BL.Services.Warehouse;

namespace Api.Extensions.Services
{
    public static class AdditionalServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add all categorized domain services
            services.AddCmsServices();
            services.AddNotificationServices();
            services.AddUserManagementServices();
            services.AddLocationServices();
            services.AddGeneralServices(configuration);
            services.AddCatalogServices(configuration);
            services.AddVendorServices(configuration);
            services.AddCurrencyAndShippingServices(configuration);
            services.AddOrderServices(configuration);
            services.AddPricingServices(configuration);
            services.AddMerchandisingServices(configuration);
            services.AddReviewServices(configuration);
            services.AddWalletServices(configuration);
            services.AddWithdrawalMethodServices(configuration);
            services.AddSettingServices(configuration);

            return services;
        }

        public static IServiceCollection AddWarehouseAndInventoryServices(this IServiceCollection services)
        {
            // Warehouse Services
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IVendorWarehouseService, VendorWarehouseService>();
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
