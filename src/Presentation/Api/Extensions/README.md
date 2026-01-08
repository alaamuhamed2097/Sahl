# API Extensions Documentation

This folder contains well-organized extension methods for configuring services and middleware in the API project.

## ?? Folder Structure

```
Extensions/
??? README.md                                     # This file
??? ServiceCollectionExtensions.cs               # Central registry (orchestrates all services)
??? HttpContextExtensions.cs                     # HTTP context utilities
??? OfferPriceHistoryConfiguration.cs           # Offer-specific configuration
?
??? Infrastructure/                              # Cross-cutting concerns
?   ??? README.md
?   ??? AuthenticationExtensions.cs
?   ??? CorsExtensions.cs
?   ??? DatabaseExtensions.cs
?   ??? LocalizationExtensions.cs
?   ??? LoggingExtensions.cs
?   ??? MvcExtensions.cs
?   ??? SwaggerExtensions.cs
?   ??? InfrastructureExtensions.cs
?
??? Services/                                    # Business/domain services
    ??? README.md
    ??? AdditionalServicesExtensions.cs
    ??? AutoMapperExtensions.cs
    ??? CatalogServiceExtensions.cs
    ??? CmsServicesExtensions.cs
    ??? CurrencyAndShippingExtensions.cs
    ??? GeneralServiceExtensions.cs
    ??? GeneralServicesExtensions.cs
    ??? HangfireExtensions.cs
    ??? LocationServicesExtensions.cs
    ??? MerchandisingServiceExtensions.cs
    ??? NotificationServicesExtensions.cs
    ??? OrderServiceExtensions.cs
    ??? PricingServiceExtensions.cs
    ??? RepositoryExtensions.cs
    ??? ReviewServiceExtensions.cs
    ??? SettingServiceExtensions.cs
    ??? UserManagementServicesExtensions.cs
    ??? VendorServiceExtensions.cs
    ??? WalletServiceExtensions.cs
```

## ?? Quick Start

### Option 1: Register Everything (Recommended)
```csharp
// In Program.cs
builder.Services.AddAllServices(builder.Configuration, builder.Environment);
```

### Option 2: Register Infrastructure Only
```csharp
builder.Services.AddInfrastructureOnly(builder.Configuration, builder.Environment);
// Then add business services separately
builder.Services.AddBusinessServices(builder.Configuration);
```

### Option 3: Manual Registration
```csharp
// Register infrastructure
builder.Services.AddSerilogConfiguration(builder.Configuration);
builder.Services.AddDatabaseConfiguration(builder.Configuration);
// ... etc

// Register business services
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddRepositoryServices();
// ... etc
```

## ?? Infrastructure Extensions (`/Infrastructure`)

Cross-cutting concerns for the entire application:

| Extension | Purpose |
|-----------|---------|
| **AuthenticationExtensions** | JWT Bearer token validation, expiration handling, security events |
| **DatabaseExtensions** | Entity Framework Core, SQL Server, Identity, password policies |
| **LoggingExtensions** | Serilog setup with console and SQL Server sinks |
| **LocalizationExtensions** | Multi-culture support (English, Arabic), request localization |
| **MvcExtensions** | API versioning, exception filtering, validation, compression |
| **SwaggerExtensions** | OpenAPI documentation, JWT security, XML comments |
| **CorsExtensions** | CORS policies for local dev, production, and payment gateways |
| **InfrastructureExtensions** | Memory caching, HTTP client, SignalR, response caching |

For detailed information, see [`Infrastructure/README.md`](Infrastructure/README.md).

## ?? Service Extensions (`/Services`)

Domain-specific business service registrations:

| Category | Extensions |
|----------|-----------|
| **Catalog** | CatalogServiceExtensions (items, categories, brands) |
| **CMS** | CmsServicesExtensions (auth, file upload, OAuth) |
| **Location** | LocationServicesExtensions, UserManagementServicesExtensions |
| **Orders** | OrderServiceExtensions (cart, checkout, payment) |
| **Merchandising** | MerchandisingServiceExtensions (campaigns, promotions) |
| **Pricing** | PricingServiceExtensions (multiple strategies) |
| **Reviews** | ReviewServiceExtensions (items, vendors, reports) |
| **Vendors** | VendorServiceExtensions, AdditionalServicesExtensions |
| **Notifications** | NotificationServicesExtensions (email, SMS, SignalR) |
| **Wallet** | WalletServiceExtensions (payments, transactions) |
| **Settings** | SettingServiceExtensions (system configuration) |
| **Data Access** | RepositoryExtensions, AutoMapperExtensions |

For detailed information, see [`Services/README.md`](Services/README.md).

## ?? Root Extensions

- **ServiceCollectionExtensions.cs** - Central registry with three options:
  - `AddAllServices()` - Everything
  - `AddInfrastructureOnly()` - Infrastructure only
  - `AddBusinessServices()` - Business services only

- **HttpContextExtensions.cs** - HTTP context utilities:
  - `GetClientIpAddress()` - Extract client IP with proxy support

- **OfferPriceHistoryConfiguration.cs** - Offer-specific configuration

## ? Best Practices

### 1. Organization by Concern
- **Infrastructure**: Cross-cutting concerns (logging, auth, db)
- **Services**: Domain-specific business logic organized by domain

### 2. Naming Conventions
- Files: `{Feature}Extensions.cs`
- Methods: `Add{Feature}Services()` or `Add{Feature}Configuration()`
- Namespaces: `Api.Extensions.Infrastructure` or `Api.Extensions.Services`

### 3. Documentation
Every public extension method includes XML documentation:
```csharp
/// <summary>
/// Brief description of what this configures
/// </summary>
/// <param name="services">The IServiceCollection instance.</param>
/// <returns>The IServiceCollection for chaining.</returns>
public static IServiceCollection AddMyServices(this IServiceCollection services)
{
    // ...
}
```

### 4. Service Lifetimes
All business services use **Scoped** lifetime (new instance per HTTP request):
```csharp
services.AddScoped<IMyService, MyService>();
```

Use other lifetimes when appropriate:
- **Transient**: Stateless utilities
- **Singleton**: Caches, factories, static resources

### 5. Configuration-Aware
Extensions that need configuration accept `IConfiguration`:
```csharp
public static IServiceCollection AddMyServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var settings = configuration.GetSection("MyFeature");
    // Use settings
}
```

### 6. Idempotency
Extensions can be called multiple times without side effects. No global state modifications.

## ?? Adding New Extensions

### New Infrastructure Extension
1. Create `src/Presentation/Api/Extensions/Infrastructure/{Feature}Extensions.cs`
2. Add public extension method with XML documentation
3. Update `ServiceCollectionExtensions.cs` to include it
4. Update [`Infrastructure/README.md`](Infrastructure/README.md)

### New Service Extension
1. Create `src/Presentation/Api/Extensions/Services/{Domain}ServiceExtensions.cs`
2. Add public extension method with XML documentation
3. Update `ServiceCollectionExtensions.cs` to include it in `AddBusinessServices()`
4. Update [`Services/README.md`](Services/README.md)

### Example Template
```csharp
namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering {domain} services.
    /// </summary>
    public static class {Domain}ServiceExtensions
    {
        /// <summary>
        /// Adds {domain}-related services to the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration (optional).</param>
        /// <returns>The IServiceCollection for chaining.</returns>
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

## ?? Extension Dependencies

Extensions are designed to be composable with clear dependency chains:

```
HttpContextExtensions (no dependencies)
        ?
Infrastructure Extensions (base layer)
        ?
Data Access (Repositories, Mappers)
        ?
Business Services (domain services)
```

The `ServiceCollectionExtensions` central registry ensures all dependencies are resolved in the correct order.

## ?? Testing

For unit tests, you can register only what you need:
```csharp
var services = new ServiceCollection();
services.AddAutoMapperConfiguration();
services.AddRepositoryServices();  // For repository-based services
```

Or create a test configuration extension:
```csharp
public static IServiceCollection AddTestServices(this IServiceCollection services)
{
    // Register only test implementations
}
```

## ?? Documentation Files

- **README.md** (this file) - Overview and quick start
- **Infrastructure/README.md** - Infrastructure extensions guide
- **Services/README.md** - Business services guide
- **Program.cs** - Shows how to use the extensions

## ?? Related Layers

- **`src/Core/BL/`** - Business logic implementations
- **`src/Infrastructure/DAL/`** - Data access layer implementations
- **`src/Shared/Common/`** - Shared extensions (strings, enums, collections)
- **`src/Shared/Shared/`** - Shared models and utilities

## ?? Common Patterns

### Pattern 1: Feature Toggle
```csharp
public static IServiceCollection AddMyFeature(
    this IServiceCollection services,
    IConfiguration config)
{
    if (config.GetValue<bool>("Features:MyFeature:Enabled"))
    {
        services.AddScoped<IMyService, MyService>();
    }
    return services;
}
```

### Pattern 2: Multiple Implementations
```csharp
services.AddScoped<IPricingStrategy, SimplePricingStrategy>();
services.AddScoped<IPricingStrategy, AdvancedPricingStrategy>();
// Factory or decorator can select implementation at runtime
```

### Pattern 3: Chain Dependencies
```csharp
services.AddRepositoryServices();  // Must come first
services.AddMyDomainServices();    // Uses repositories
```
