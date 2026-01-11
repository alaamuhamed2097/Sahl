# Infrastructure Extensions Documentation

This folder contains extension methods for configuring cross-cutting infrastructure concerns such as logging, database, authentication, and middleware setup.

## Files Overview

### Authentication & Security
- **AuthenticationExtensions**: JWT Bearer token authentication with token validation, expiration handling, and security event handlers

### Database & ORM
- **DatabaseExtensions**: Entity Framework Core configuration with SQL Server, Identity setup, password policies, and lockout settings

### Logging
- **LoggingExtensions**: Serilog configuration with console and SQL Server sinks, enrichment, and self-logging for diagnostics

### Localization (i18n)
- **LocalizationExtensions**: Request localization with multi-culture support (English, Arabic) and culture provider configuration

### API & MVC Configuration
- **MvcExtensions**: Controller configuration, API versioning (v1, v2, etc.), response compression (Gzip/Brotli), and global exception filtering

### API Documentation
- **SwaggerExtensions**: OpenAPI/Swagger UI setup with JWT bearer schema, XML documentation, and versioned endpoint documentation

### CORS (Cross-Origin Resource Sharing)
- **CorsExtensions**: CORS policy configuration for allowed origins (local, production, and payment gateways)

### General Infrastructure
- **InfrastructureExtensions**: Memory caching, response caching, HTTP client factory, and SignalR real-time communication

## Usage in Program.cs

```csharp
// Option 1: Register all infrastructure services
builder.Services.AddInfrastructureOnly(builder.Configuration, builder.Environment);

// Option 2: Register everything (infrastructure + business services)
builder.Services.AddAllServices(builder.Configuration, builder.Environment);
```

## Service Registration Order

The extensions are designed to be order-independent but follow this logical sequence:

1. **Logging** - Configured first for visibility into subsequent operations
2. **Database & Identity** - Core persistence layer
3. **Authentication** - JWT and security
4. **CORS** - Network policy
5. **API Configuration** - Controllers, versioning, exception handling
6. **Caching & HTTP** - Supporting services
7. **Localization** - Multi-language support
8. **Documentation** - Swagger/OpenAPI

## Best Practices

1. **Configuration-driven**: Extensions read from `IConfiguration` for environment-specific settings
2. **Idempotent**: Can be called multiple times without side effects
3. **Well-documented**: Each method has XML comments explaining its purpose
4. **Composable**: Each extension is independent and can be used standalone
5. **Testable**: Clear separation of concerns makes unit testing easier

## Adding New Infrastructure Extensions

When adding new infrastructure configuration:

1. Create a new file following the naming pattern: `{Feature}Extensions.cs`
2. Add XML documentation comments to the extension method
3. Include both the IServiceCollection and any configuration parameters (IConfiguration, IWebHostEnvironment, etc.)
4. Update the central `ServiceCollectionExtensions` if it should be part of the main pipeline
5. Update this README

## Related Folders

- `/Services` - Business/domain service extensions
- `../` - Root extensions (utility extensions like HttpContextExtensions)

## Common Configuration Scenarios

### Local Development
- JWT key from `appsettings.json`
- SQL Server connection to local database
- Console and SQL logging
- CORS allows localhost origins
- Swagger enabled for API testing

### Production
- JWT key from environment variables
- Production database connection string
- Limited SQL logging (warnings and errors only)
- CORS restricted to production domains
- Swagger disabled or restricted

### Testing
- In-memory database or test database
- Minimal logging (errors only)
- Mock HTTP clients
- Swagger may be enabled for API testing

