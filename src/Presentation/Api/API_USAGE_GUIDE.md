# Sahl API - Versioning Implementation Guide

## ?? ???? ????

?? ????? **API Versioning** ??? ???? ??? endpoints ???????? **Asp.Versioning** library.

???? ??? APIs ???? ????? ?? folder structure ???? ?? ??? ???? ??? versioning.

## ?? ????? ??????? ??? API

### ??????? 1: URL Path (?????? ??)
```bash
GET /api/v1/category
GET /api/v1/order
GET /api/v1/customer
GET /api/v1/warehouse
```

### ??????? 2: Query String
```bash
GET /api/category?api-version=1.0
GET /api/order?api-version=1.0
```

### ??????? 3: HTTP Header
```bash
GET /api/category
Header: api-version: 1.0
```

## ?? Folder Structure

```
Controllers/
??? v1/
?   ??? Authentication/
?   ?   ??? AuthController.cs
?   ?   ??? PasswordController.cs
?   ??? Catalog/
?   ?   ??? CategoryController.cs
?   ?   ??? BrandController.cs
?   ?   ??? UnitController.cs
?   ?   ??? ItemController.cs
?   ??? Location/
?   ?   ??? CountryController.cs
?   ?   ??? CityController.cs
?   ?   ??? StateController.cs
?   ??? Order/
?   ?   ??? OrderController.cs
?   ?   ??? CartController.cs
?   ?   ??? CheckoutController.cs
?   ?   ??? PaymentController.cs
?   ?   ??? ShipmentController.cs
?   ?   ??? DeliveryController.cs
?   ??? User/
?   ?   ??? CustomerController.cs
?   ?   ??? VendorController.cs
?   ?   ??? AdminController.cs
?   ?   ??? UserProfileController.cs
?   ?   ??? UserAuthenticationController.cs
?   ?   ??? UserActivationController.cs
?   ??? Warehouse/
?   ?   ??? WarehouseController.cs
?   ??? Notification/
?   ?   ??? NotificationController.cs
?   ?   ??? UserNotificationsController.cs
?   ??? Shipping/
?   ?   ??? ShippingCompanyController.cs
?   ??? Content/
?   ?   ??? ContentAreaController.cs
?   ??? Settings/
?   ?   ??? SettingController.cs
?   ?   ??? CurrencyController.cs
?   ??? Pricing/
?   ?   ??? PricingSystemController.cs
?   ??? Wallet/
?   ?   ??? WalletController.cs
?   ??? Loyalty/
?   ?   ??? LoyaltyController.cs
?   ??? Sales/
?   ?   ??? CouponCodeController.cs
?   ??? Campaign/
?   ?   ??? CampaignController.cs
?   ??? Base/
?       ??? BaseController.cs
??? v2/ (Future)
    ??? ... (new implementations for breaking changes)
```

## ?? Configuration

### MvcExtensions.cs
```csharp
// API Versioning configuration
services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("api-version"),
        new QueryStringApiVersionReader("api-version"),
        new UrlSegmentApiVersionReader()
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
```

## ?? Swagger Documentation

### Access Swagger UI
```
http://localhost:5000/swagger
```

### Export OpenAPI/Swagger JSON
```
GET /openapi/export
```

### Features
- ? ???? ????? ?????? ?? dropdown
- ? ????? ???? ??? endpoint
- ? ??????? ?????? ??? endpoints ?????? ?? ??? UI
- ? XML comments ???? ??? endpoints

## ?? Examples

### cURL Examples

#### Get All Categories (v1)
```bash
curl -X GET "http://localhost:5000/api/v1/category" \
  -H "Content-Type: application/json"
```

#### Get Category by ID (v1)
```bash
curl -X GET "http://localhost:5000/api/v1/category/12345" \
  -H "Content-Type: application/json"
```

#### Create Order (v1)
```bash
curl -X POST "http://localhost:5000/api/v1/order/create" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "deliveryAddressId": "12345",
    "items": []
  }'
```

#### Get Customer Orders (v1)
```bash
curl -X GET "http://localhost:5000/api/v1/order/my-orders?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Postman Examples

1. **Set Base URL Variable**
   - Variable name: `base_url`
   - Value: `http://localhost:5000`

2. **Get Request**
   - URL: `{{base_url}}/api/v1/category`
   - Method: GET

3. **Post Request**
   - URL: `{{base_url}}/api/v1/order/create`
   - Method: POST
   - Header: `Authorization: Bearer YOUR_TOKEN`
   - Body (JSON): `{"deliveryAddressId": "12345", "items": []}`

## ??? Adding New Controllers

### Step 1: Create Folder
```
Controllers/v1/{Category}/
```

### Step 2: Create Controller File
```csharp
using Asp.Versioning;
using Api.Controllers.Base;

namespace Api.Controllers.v1.{Category}
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class {Controller}Controller : BaseController
    {
        /// <summary>
        /// Method description.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Implementation
        }
    }
}
```

### Step 3: Test
- Build project: `dotnet build`
- Check Swagger: `http://localhost:5000/swagger`

## ?? API Versioning Status

### Completed Controllers (11)
- ? AuthController
- ? OrderController
- ? CategoryController
- ? CustomerController
- ? WarehouseController
- ? NotificationController
- ? UserNotificationsController
- ? CountryController
- ? CityController
- ? StateController
- ? BrandController

### In Progress / Pending Controllers
- ? UnitController
- ? AttributeController
- ? ItemController
- ? CartController
- ? CheckoutController
- ? PaymentController
- ? ShipmentController
- ? DeliveryController
- ? And 13 more...

## ?? Version Management

### Current Version
- **Latest**: 1.0
- **Supported**: 1.0
- **Deprecated**: None

### Response Headers
Each API response includes version information:
```
api-supported-versions: 1.0
api-deprecated-versions: (empty)
```

### Future Versions
When introducing breaking changes:
1. Create new folder: `Controllers/v2/`
2. Copy and modify controllers as needed
3. Keep v1 unchanged for backward compatibility
4. Swagger will automatically show both versions

## ?? Testing

### Unit Testing
```csharp
[Fact]
public async Task GetCategory_WithValidId_ReturnsOk()
{
    // Arrange
    var categoryId = Guid.NewGuid();
    
    // Act
    var response = await _httpClient.GetAsync($"/api/v1/category/{categoryId}");
    
    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

### Integration Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/Tests/Api.Tests/Api.Tests.csproj
```

## ?? Common Issues

### Issue: 404 Not Found
**Cause**: URL path is incorrect or controller not in v1 folder

**Solution**:
- Check: URL should be `/api/v1/controllerName`
- Verify controller file location: `Controllers/v1/{Category}/{Controller}.cs`

### Issue: Version Mismatch
**Cause**: ApiVersion attribute doesn't match requested version

**Solution**:
- Check: `[ApiVersion("1.0")]` is present
- Verify: Swagger shows correct version in dropdown

### Issue: Swagger Not Updated
**Cause**: Cache or build not complete

**Solution**:
```bash
# Clean and rebuild
dotnet clean
dotnet build

# Then refresh Swagger page (Ctrl+Shift+R in browser)
```

## ?? Documentation

### Files
- `VERSIONING_PROGRESS.md` - Current progress status
- `VERSIONING_TEMPLATE.md` - Template for new controllers
- `API_VERSIONING_GUIDE.md` - Detailed configuration guide

### External Resources
- [Asp.Versioning GitHub](https://github.com/dotnet/aspnet-api-versioning)
- [REST API Versioning Best Practices](https://restfulapi.net/versioning)
- [OpenAPI Specification](https://swagger.io/specification)

## ?? Getting Started

### Prerequisites
- .NET 10 SDK
- Visual Studio / VS Code
- REST client (cURL, Postman, or Insomnia)

### Run Application
```bash
# Navigate to API project
cd src/Presentation/Api

# Build
dotnet build

# Run
dotnet run

# Access Swagger
# http://localhost:5000/swagger
```

### Test Endpoints
```bash
# Get all categories
curl http://localhost:5000/api/v1/category

# Get with query string version
curl http://localhost:5000/api/category?api-version=1.0

# Get with header version
curl http://localhost:5000/api/category -H "api-version: 1.0"
```

## ?? Best Practices

1. **Always Use Versioned URLs**: `/api/v1/endpoint` instead of `/api/endpoint`
2. **Document XML Comments**: Add `<remarks>API Version: 1.0+</remarks>` to all methods
3. **Test All Versions**: Ensure backward compatibility
4. **Keep v1 Stable**: Don't modify v1 endpoints, create v2 for breaking changes
5. **Update Swagger**: After adding new endpoints, verify in Swagger UI

## ?? Contributing

When adding new endpoints:

1. Follow the folder structure: `Controllers/v1/{Category}/`
2. Add `[ApiVersion("1.0")]` attribute
3. Use route: `api/v{version:apiVersion}/[controller]`
4. Add XML comments with version info
5. Test in Swagger UI
6. Create pull request with versioned controllers

## ? FAQ

**Q: Can I use the old API endpoints?**
A: No, all endpoints must use versioned URLs: `/api/v1/endpoint`

**Q: How do I add a new API version (v2)?**
A: Create new folder `Controllers/v2/`, copy controllers, make changes, and keep v1 unchanged.

**Q: What if I need to deprecate an endpoint?**
A: Mark it with `[Obsolete]` attribute and create new version in v2.

**Q: How do clients specify the API version?**
A: Three ways: URL path (`/api/v1/`), query string (`?api-version=1.0`), or header (`api-version: 1.0`)

**Q: Is Swagger automatically updated?**
A: Yes, just rebuild the project and refresh Swagger page.

