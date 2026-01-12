using Api.Extensions.Infrastructure;
using Api.Extensions.Services;

namespace Api.Extensions
{
    /// <summary>
    /// Central registry for all service extension methods.
    /// This extension serves as the main entry point for configuring all API services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all infrastructure and application services to the dependency injection container.
        /// This method orchestrates the registration of services across multiple layers.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="environment">The web host environment.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddAllServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            // ============================================
            // Infrastructure Services
            // ============================================
            services.AddSerilogConfiguration(configuration);
            services.AddDatabaseConfiguration(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddCorsConfiguration(environment);
            services.AddLocalizationConfiguration();
            services.AddMvcConfiguration();
            services.AddCompressionConfiguration();
            services.AddSwaggerConfiguration();
            services.AddInfrastructureServices();

            // ============================================
            // Mapping & Repository Services
            // ============================================
            services.AddAutoMapperConfiguration();
            services.AddRepositoryServices();

            // ============================================
            // Domain/Business Services (organized by domain)
            // ============================================
            // AddDomainServices includes all business services:
            // CMS, Notification, UserManagement, Location, General, Catalog, Vendor, Currency, Order, Pricing, Merchandising, Review, Wallet, WithdrawalMethod, Settings
            services.AddDomainServices(configuration);

            // ============================================
            // Additional Services
            // ============================================
            services.AddWarehouseAndInventoryServices();
            services.AddEnhancedNotificationServices();

            return services;
        }

        /// <summary>
        /// Adds only infrastructure services (logging, database, authentication, etc.).
        /// Use this if you need fine-grained control over service registration.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="environment">The web host environment.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddInfrastructureOnly(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddSerilogConfiguration(configuration);
            services.AddDatabaseConfiguration(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddCorsConfiguration(environment);
            services.AddLocalizationConfiguration();
            services.AddMvcConfiguration();
            services.AddCompressionConfiguration();
            services.AddSwaggerConfiguration();
            services.AddInfrastructureServices();

            return services;
        }

        /// <summary>
        /// Adds only business/domain services.
        /// Use this after infrastructure services are already configured.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddBusinessServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapperConfiguration();
            services.AddRepositoryServices();
            // AddDomainServices includes all business services
            services.AddDomainServices(configuration);
            services.AddWarehouseAndInventoryServices();
            services.AddEnhancedNotificationServices();

            return services;
        }
    }
}
