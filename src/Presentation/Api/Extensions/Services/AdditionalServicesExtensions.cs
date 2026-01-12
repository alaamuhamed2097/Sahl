using BL.Contracts.Service.Notification;
using BL.Contracts.Service.Warehouse;
using BL.Services.Notification;
using BL.Services.Warehouse;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering additional domain services (warehouse, inventory, notifications).
    /// </summary>
    public static class AdditionalServicesExtensions
    {
        /// <summary>
        /// Adds all categorized domain services for the application.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
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
            services.AddWithdrawalMethodServices(configuration);
            services.AddSettingServices(configuration);

            return services;
        }

        /// <summary>
        /// Adds warehouse and inventory services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddWarehouseAndInventoryServices(this IServiceCollection services)
        {
            // Warehouse Services
            services.AddScoped<IWarehouseService, WarehouseService>();

            return services;
        }

        /// <summary>
        /// Adds enhanced notification channel services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
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
