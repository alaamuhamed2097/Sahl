# Business Services Extensions Documentation

This folder contains extension methods for registering domain-specific business services organized by domain area.

## Folder Structure

All service extensions follow a consistent naming pattern and are organized by domain responsibility.

## Service Extensions by Domain

### Catalog Services
- **CatalogServiceExtensions**: Category, attribute, item, item details, and brand services for product management

### CMS & Content Services  
- **CmsServicesExtensions**: User authentication, activation, tokens, role management, file uploads, image processing, and OAuth

### Currency & Shipping Services
- **CurrencyAndShippingExtensions**: Currency conversion, location-based currency, and shipping calculation services

### General Services
- **GeneralServiceExtensions**: Current user context service
- **GeneralServicesExtensions**: Cache service and API service (general utilities)

### Location Services
- **LocationServicesExtensions**: IP geolocation service for detecting user location

### Merchandising Services
- **MerchandisingServiceExtensions**: Homepage, campaigns, coupon codes, homepage sliders, and admin block services

### Notification Services
- **NotificationServicesExtensions**: Email, SMS, SignalR notifications, templates, and verification codes

### Order Services
- **OrderServiceExtensions**: Cart, checkout, payment, order management, fulfillment, and wishlist services

### Pricing Services
- **PricingServiceExtensions**: Pricing strategies (simple, combination-based, quantity-based, hybrid) and settings

### Review Services
- **ReviewServiceExtensions**: Item reviews, vendor reviews, review reports, and voting services

### Setting Services
- **SettingServiceExtensions**: System settings and configuration management services

### User Management Services
- **UserManagementServicesExtensions**: User registration, profiles, and location (country, state, city) services

### Vendor Services
- **VendorServiceExtensions**: Vendor management and vendor item services

### Wallet Services
- **WalletServiceExtensions**: Customer wallet, wallet transactions, and wallet settings

### Additional Services
- **AdditionalServicesExtensions**: Warehouse, inventory, and enhanced notification channel services
- **HangfireExtensions**: Background job processing (currently commented out, enable when needed)

## Usage Pattern

Each extension follows this pattern:

```csharp
public static IServiceCollection Add{Domain}Services(
    this IServiceCollection services, 
    IConfiguration? configuration = null)
{
    // Register all related services for this domain
    services.AddScoped<IService, ServiceImplementation>();
    
    return services;
}
```

## Central Registry

All service extensions are orchestrated by the central registry in `../ServiceCollectionExtensions.cs`:

```csharp
// Option 1: Register all services (infrastructure + business)
builder.Services.AddAllServices(configuration, environment);

// Option 2: Register only business services
builder.Services.AddBusinessServices(configuration);
```

## Domain Dependencies

Service extensions may have dependencies on other extensions:

- **OrderServices** ? RepositoryExtensions (data access)
- **PricingServices** ? GeneralServicesExtensions (caching)
- **MerchandisingServices** ? RepositoryExtensions
- **ReviewServices** ? RepositoryExtensions, NotificationServices

The central registry handles all dependencies in the correct order.

## Adding New Service Extensions

When creating a new service extension:

1. **Create the file**: `{Domain}ServiceExtensions.cs`
2. **Follow the namespace**: `Api.Extensions.Services`
3. **Add XML documentation**: Include summary and parameter descriptions
4. **Register dependencies first**: If your services depend on repositories, call `AddRepositoryServices()` first
5. **Use consistent naming**: Method name should be `Add{Domain}Services`
6. **Update central registry**: Add your extension to `ServiceCollectionExtensions.cs`
7. **Update this README**: Document the new extension

Example template:

```csharp
namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering {domain} services.
    /// </summary>
    public static class {Domain}ServiceExtensions
    {
        /// <summary>
        /// Adds {domain}-related services.
        /// </summary>
        public static IServiceCollection Add{Domain}Services(
            this IServiceCollection services,
            IConfiguration? configuration = null)
        {
            services.AddScoped<IYourService, YourService>();
            return services;
        }
    }
}
```

## Best Practices

1. **Single Responsibility**: One extension per domain area
2. **Clear Naming**: Extension method names should clearly indicate what they register
3. **Configuration-Aware**: Accept `IConfiguration` if services need config values
4. **Documentation**: Include XML comments explaining registered services
5. **No Circular Dependencies**: Services registered in one extension shouldn't depend on extensions that depend on this one
6. **Idempotent**: Calling an extension multiple times should not cause issues

## Service Lifetime Scopes

All services are registered as **Scoped** (`AddScoped<>`):
- New instance per HTTP request
- Suitable for business logic and data access
- Allows dependency injection of context information

If you need different lifetimes:
- **Transient** (`AddTransient<>`): New instance every time (stateless utilities)
- **Singleton** (`AddSingleton<>`): Single instance for app lifetime (cache stores, factories)

## Related Folders

- `/Infrastructure` - Infrastructure/cross-cutting service extensions
- `../../Shared/` - Shared utilities and extensions
- `../../BL/` - Business logic layer implementations

## Common Patterns

### Service with Repository Dependency
```csharp
services.AddScoped<IMyService, MyService>(); // Uses injected IRepository
```

### Multiple Implementations of Same Interface
```csharp
// Multiple pricing strategies implementing IPricingStrategy
services.AddScoped<IPricingStrategy, SimplePricingStrategy>();
services.AddScoped<IPricingStrategy, CombinationBasedPricingStrategy>();
```

### Service Factory Pattern
```csharp
services.AddScoped<ICurrencyConversionFactory, CurrencyConversionFactory>();
```

### Configuration-Based Services
```csharp
public static IServiceCollection AddMyServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var settings = configuration.GetSection("MyFeature");
    // Use settings when needed
    return services;
}
```

