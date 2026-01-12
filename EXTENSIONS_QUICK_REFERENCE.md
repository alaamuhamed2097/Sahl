# Extension System Quick Reference Guide

## ?? Quick Start (3 Options)

### Option 1: Register Everything (Recommended ?)
```csharp
// In Program.cs
using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// One line to register everything!
builder.Services.AddAllServices(builder.Configuration, builder.Environment);

var app = builder.Build();
// ... rest of Program.cs
```

### Option 2: Register Infrastructure Only
```csharp
builder.Services.AddInfrastructureOnly(builder.Configuration, builder.Environment);
// Later...
builder.Services.AddBusinessServices(builder.Configuration);
```

### Option 3: Manual Registration
```csharp
using Api.Extensions.Infrastructure;
using Api.Extensions.Services;

// Infrastructure
builder.Services.AddSerilogConfiguration(builder.Configuration);
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
// ... etc

// Services
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddRepositoryServices();
builder.Services.AddDomainServices(builder.Configuration);
// ... etc
```

---

## ?? Where Do Things Go?

### For Infrastructure/Cross-Cutting Concerns
**Put in:** `Extensions/Infrastructure/`
**Examples:** Authentication, Database, Logging, CORS, Swagger, Caching

### For Domain-Specific Business Services
**Put in:** `Extensions/Services/`
**Examples:** Product catalog, Orders, Reviews, Users, Notifications

### For Context Utilities
**Put in:** `Extensions/` (root)
**Examples:** HttpContextExtensions, configuration-specific stuff

---

## ??? Adding a New Infrastructure Extension

### Step 1: Create File
```
src/Presentation/Api/Extensions/Infrastructure/MyFeatureExtensions.cs
```

### Step 2: Use Template
```csharp
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring my feature.
    /// </summary>
    public static class MyFeatureExtensions
    {
        /// <summary>
        /// Adds my feature configuration.
        /// </summary>
        public static IServiceCollection AddMyFeatureConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // Your configuration here
            services.AddScoped<IMyService, MyService>();
            
            return services;
        }
    }
}
```

### Step 3: Register in ServiceCollectionExtensions.cs
```csharp
public static IServiceCollection AddInfrastructureOnly(...)
{
    // ... other infrastructure
    services.AddMyFeatureConfiguration(configuration);  // ? Add this line
    // ... rest of infrastructure
}
```

### Step 4: Update Infrastructure/README.md
Add entry documenting your new extension.

---

## ?? Adding a New Service Extension

### Step 1: Create File
```
src/Presentation/Api/Extensions/Services/{Domain}ServiceExtensions.cs
```

### Step 2: Use Template
```csharp
using BL.Contracts.Service.{Domain};
using BL.Services.{Domain};

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
            services.AddScoped<IMyService, MyService>();
            
            return services;
        }
    }
}
```

### Step 3: Register in ServiceCollectionExtensions.cs
```csharp
public static IServiceCollection AddBusinessServices(...)
{
    // ... other services
    services.Add{Domain}Services(configuration);  // ? Add this line
    // ... rest of services
}
```

### Step 4: Update Services/README.md
Add entry documenting your new extension.

---

## ?? Extension Checklist

When creating a new extension, ensure:

- [ ] File in correct folder (Infrastructure/ or Services/)
- [ ] Namespace is correct (Api.Extensions.Infrastructure or Api.Extensions.Services)
- [ ] Has XML documentation comments
- [ ] Extension method returns IServiceCollection for chaining
- [ ] Registered in ServiceCollectionExtensions.cs if it should be automatic
- [ ] README.md updated with new extension details
- [ ] Build succeeds without errors
- [ ] Extension is idempotent (can be called multiple times safely)

---

## ?? Finding Extensions

### By Responsibility
```
Infrastructure/
??? AuthenticationExtensions      ? JWT config
??? DatabaseExtensions           ? EF Core, SQL Server
??? LoggingExtensions           ? Serilog
??? LocalizationExtensions      ? i18n
??? MvcExtensions              ? Controllers, versioning
??? SwaggerExtensions          ? API documentation
??? CorsExtensions            ? CORS policies
??? InfrastructureExtensions  ? Caching, SignalR

Services/
??? RepositoryExtensions      ? Data access
??? AutoMapperExtensions      ? Mapping
??? CatalogServiceExtensions  ? Products
??? OrderServiceExtensions    ? Orders
??? UserManagementExtensions  ? Users
??? [Other domain services]
```

### All Extensions at a Glance

**Infrastructure (9):**
Authentication • CORS • Database • Localization • Logging • MVC • Swagger • Infrastructure • (Root: HttpContext)

**Services (19):**
Additional • AutoMapper • Catalog • CMS • Currency • General(2) • Hangfire • Location • Merchandising • Notification • Order • Pricing • Repository • Review • Setting • UserManagement • Vendor • Wallet

---

## ?? Common Patterns

### Pattern 1: Service with Dependencies
```csharp
public static IServiceCollection AddMyServices(
    this IServiceCollection services)
{
    services.AddScoped<IMyService, MyService>();  // Uses injected IRepository
    return services;
}
```

### Pattern 2: Multiple Implementations
```csharp
services.AddScoped<IPricingStrategy, SimplePricingStrategy>();
services.AddScoped<IPricingStrategy, AdvancedPricingStrategy>();
// Factory selects at runtime
```

### Pattern 3: Configuration-Dependent
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

### Pattern 4: Factory Registration
```csharp
services.AddScoped<ICurrencyFactory>(sp =>
    new CurrencyFactory(sp.GetRequiredService<IConfiguration>())
);
```

---

## ?? Testing

### Register Minimal Services for Tests
```csharp
[TestFixture]
public class MyServiceTests
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        
        // Register only what's needed
        services.AddRepositoryServices();
        services.AddScoped<IMyService, MyService>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
}
```

### Create Test Extension
```csharp
namespace Api.Extensions.Tests
{
    public static class TestServiceExtensions
    {
        public static IServiceCollection AddTestServices(
            this IServiceCollection services)
        {
            // Register test implementations
            return services;
        }
    }
}
```

---

## ?? Common Mistakes

? **Don't:**
- Put business logic extensions in Infrastructure/
- Forget to return IServiceCollection (breaks chaining)
- Skip XML documentation comments
- Create circular dependencies between extensions
- Modify global state in extensions

? **Do:**
- Keep concerns separated
- Always return services for chaining
- Document all public methods
- Ensure idempotency
- Keep extensions stateless

---

## ?? Documentation Files

| File | Purpose | Size |
|------|---------|------|
| `Extensions/README.md` | Overview & quick start | 470 lines |
| `Infrastructure/README.md` | Infrastructure guide | 200 lines |
| `Services/README.md` | Services guide | 350 lines |
| `ServiceCollectionExtensions.cs` | Central registry | 80 lines |
| `EXTENSIONS_REFACTORING_SUMMARY.md` | Complete refactoring details | 400 lines |

---

## ?? Useful Links

- **Overview:** See `Extensions/README.md`
- **Infrastructure Details:** See `Infrastructure/README.md`
- **Services Details:** See `Services/README.md`
- **Central Registry:** See `Extensions/ServiceCollectionExtensions.cs`
- **Full Summary:** See `EXTENSIONS_REFACTORING_SUMMARY.md`

---

## ?? Quick Q&A

**Q: Where should I put a new logging service?**
A: Infrastructure/ (LoggingExtensions already exists, add to it or create new extension)

**Q: Where should I put a new order-related service?**
A: Services/OrderServiceExtensions.cs

**Q: Can I register services multiple times?**
A: Yes, but last registration wins. Extensions are idempotent.

**Q: Do I have to use the central registry?**
A: No, it's optional. You can still use individual extensions. The central registry just makes it convenient.

**Q: What's the difference between Infrastructure and Services?**
A: Infrastructure = cross-cutting (auth, logging, db). Services = domain-specific business logic.

**Q: Where do I put utilities like HttpContextExtensions?**
A: Root Extensions/ folder.

---

## ?? Need Help?

1. **Check the relevant README** (Infrastructure/ or Services/)
2. **Look at similar extensions** for patterns
3. **Review ServiceCollectionExtensions.cs** for registration order
4. **Check EXTENSIONS_REFACTORING_SUMMARY.md** for detailed information

---

**Happy extending! ??**
