# API Versioning Quick Reference

## ?? Quick Start

### Current Status
? **API Versioning Fully Implemented**
- Default version: 1.0
- All current endpoints at version 1.0
- Reference controller: `WarehouseController`

### Test the API

**Swagger UI**
```
https://localhost:7282/swagger
```

**cURL Examples**
```bash
# URL path (recommended)
curl https://localhost:7282/api/v1/warehouse

# Query string
curl https://localhost:7282/api/warehouse?api-version=1.0

# Header
curl -H "api-version: 1.0" https://localhost:7282/api/warehouse
```

## ?? Update a Controller

### 1-Minute Update Guide

**Before:**
```csharp
[Route("api/[controller]")]
[ApiController]
public class MyController : BaseController { }
```

**After:**
```csharp
using Asp.Versioning;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class MyController : BaseController { }
```

**Update XML Comments:**
```csharp
/// <summary>
/// Description here.
/// </summary>
/// <remarks>
/// API Version: 1.0+
/// </remarks>
[HttpGet]
public async Task<IActionResult> Get() { }
```

### Full Checklist
- [ ] Add `using Asp.Versioning;`
- [ ] Change route to `[Route("api/v{version:apiVersion}/[controller]")]`
- [ ] Add `[ApiVersion("1.0")]` attribute
- [ ] Add "API Version: 1.0+" to method comments
- [ ] Test in Swagger
- [ ] Test with `/api/v1/...` URL

## ?? Update Dashboard

Update `src/Presentation/Dashboard/Constants/ApiEndpoints.cs`:

```csharp
// Change all endpoints from:
public const string Get = "api/Warehouse";

// To:
public const string Get = "api/v1/warehouse";
```

## ?? Documentation

- **Full Guide**: See `API_VERSIONING_GUIDE.md`
- **Update Template**: See `CONTROLLER_VERSIONING_TEMPLATE.md`
- **Status Report**: See `API_VERSIONING_IMPLEMENTATION_COMPLETE.md`

## ?? Creating a New Version

When you need to make breaking changes:

### Option 1: New Endpoint Method
```csharp
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class WarehouseController : BaseController
{
    // v1.0
    [HttpGet]
    public async Task<IActionResult> Get() { }

    // v2.0
    [HttpGet("v2")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetV2() { }
}
```

### Option 2: Deprecate Old Version
```csharp
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("2.0")]
public class WarehouseController : BaseController { }
```

## ?? API Response Headers

Every response includes:
```
api-supported-versions: 1.0
```

This tells clients which versions are currently available.

## ? Controllers Updated
- [x] WarehouseController (v1.0)

## ? Controllers Pending Update
- [ ] 23 other controllers (see detailed lists in template)

## ?? Key Points

1. **Always use versioned URLs**: `/api/v1/endpoint` not `/api/endpoint`
2. **Swagger shows all versions**: Multiple tabs in Swagger UI
3. **Backward compatible**: Old clients still work with v1.0
4. **Easy to extend**: Add new versions without breaking existing ones
5. **Well documented**: Each version documented separately

## ?? Configuration Files

### MvcExtensions.cs
Configures versioning strategies (URL, query, header)

### SwaggerExtensions.cs
Generates separate Swagger docs for each version

### Program.cs
Integrates versioning into middleware pipeline

### BaseController.cs
Includes `GetApiVersion()` helper method

## ?? Quick Commands

```bash
# Build project
dotnet build

# Run API
dotnet run

# Navigate to Swagger
https://localhost:7282/swagger

# Export OpenAPI spec
https://localhost:7282/openapi/export
```

## ?? Example Request/Response

**Request:**
```
GET /api/v1/warehouse HTTP/1.1
Host: localhost:7282
Authorization: Bearer <token>
```

**Response:**
```
HTTP/1.1 200 OK
api-supported-versions: 1.0
Content-Type: application/json

{
  "success": true,
  "message": "Data Retrieved",
  "data": [ { ... } ]
}
```

## ? Common Tasks

### Test All Three Versioning Methods
```bash
# Method 1: URL Path
curl https://localhost:7282/api/v1/warehouse

# Method 2: Query String
curl https://localhost:7282/api/warehouse?api-version=1.0

# Method 3: Header
curl -H "api-version: 1.0" https://localhost:7282/api/warehouse
```

### Add Support for Both v1 and v2

```csharp
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ExampleController : BaseController
{
    // Both v1 and v2 use this
    [HttpGet]
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] ExampleDto dto) { }

    // Only v2 has this
    [HttpGet("v2")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetV2() { }
}
```

### Check Which Version is Supported
```bash
# Response headers will show:
# api-supported-versions: 1.0, 2.0
```

## ?? Important: Update All Controllers

Before deploying, ensure all controllers follow the versioning pattern:

```
? GET /api/warehouse          # OLD - Will not work
? GET /api/v1/warehouse       # NEW - Use this
```

## ?? Related Files

| File | Purpose |
|------|---------|
| MvcExtensions.cs | Versioning configuration |
| SwaggerExtensions.cs | Swagger documentation per version |
| Program.cs | Middleware integration |
| BaseController.cs | Helper methods |
| WarehouseController.cs | Reference implementation |
| API_VERSIONING_GUIDE.md | Detailed guide |
| CONTROLLER_VERSIONING_TEMPLATE.md | Update template |

---

**Version**: 1.0  
**Last Updated**: January 2025  
**Status**: Ready to use  
**Next Step**: Update remaining controllers using the template
