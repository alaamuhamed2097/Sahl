# API v1 Controllers

This folder contains all API v1 (Version 1.0) controllers organized by feature/category.

## Structure

```
v1/
??? Authentication/     - Authentication & Authorization endpoints
??? Catalog/           - Product & Category management endpoints
??? Location/          - Geographic location endpoints (Countries, Cities, States)
??? Order/             - E-Commerce Order management endpoints
??? User/              - User management endpoints (Customers, Admins, Vendors)
??? Warehouse/         - Warehouse management endpoints
??? Notification/      - Notification & Alerts endpoints
??? Shipping/          - Shipping company management endpoints
??? Content/           - Content management endpoints
??? Settings/          - System settings endpoints
??? Pricing/           - Pricing system endpoints
??? Wallet/            - Digital wallet endpoints
??? Loyalty/           - Loyalty program endpoints
??? Campaign/          - Marketing campaign endpoints
??? Sales/             - Sales & Discount endpoints
??? Base/              - Shared BaseController (do not delete)
```

## Important Files

- **BaseController** (in `Base/` folder) - Contains shared functionality for all controllers
  - Don't modify or move this file
  - All controllers inherit from this class

## Route Pattern

All v1 endpoints follow this route pattern:
```
GET /api/v1/{controller-name}/{action}
```

Example:
- `GET /api/v1/category` - Get all categories
- `GET /api/v1/order/123` - Get order by ID
- `POST /api/v1/customer` - Create new customer

## Adding New Controllers

1. Create folder for the category if it doesn't exist: `v1/{Category}/`
2. Create controller file: `{Name}Controller.cs`
3. Ensure controller:
   - Uses namespace: `Api.Controllers.v1.{Category}`
   - Has attribute: `[ApiVersion("1.0")]`
   - Has route: `api/v{version:apiVersion}/[controller]`
   - Inherits from `BaseController`

Example:
```csharp
using Api.Controllers.Base;
using Asp.Versioning;

namespace Api.Controllers.v1.Sales
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CouponController : BaseController
    {
        // Implementation
    }
}
```

## Documentation

For detailed information, see:
- `../API_USAGE_GUIDE.md` - How to use the API
- `../VERSIONING_PROGRESS.md` - Implementation progress
- `../VERSIONING_TEMPLATE.md` - Template for new controllers
- `../IMPLEMENTATION_SUMMARY.md` - Implementation summary

## Build & Test

```bash
# Build the project
dotnet build

# Run the project
dotnet run

# Access Swagger documentation
# http://localhost:5000/swagger
```

## API Version Management

- Current version: 1.0
- Supported versions: 1.0
- Deprecated versions: None

For information about adding v2 or other versions, see versioning documentation.

---

**Status**: ? API v1 is production-ready
**Last Updated**: December 3, 2025
**Compatibility**: .NET 10
