# API Versioning Implementation Guide

## Overview

This guide documents the complete API versioning implementation for the Sahl API using ASP.NET Core's `Asp.Versioning` package.

## What is API Versioning?

API versioning allows you to release new versions of your API while maintaining backward compatibility with existing clients. This is crucial for:

- **Backward Compatibility**: Existing clients continue to work with older versions
- **Gradual Migration**: Give clients time to update to newer versions
- **Parallel Development**: Support multiple versions simultaneously during transition periods
- **Documentation**: Clear documentation for each API version

## Versioning Strategy

### Default Behavior

- **Default API Version**: 1.0
- **Supported Versioning Methods**:
  1. **URL Path**: `/api/v1/warehouse` (recommended)
  2. **Query String**: `/api/warehouse?api-version=1.0`
  3. **HTTP Header**: `api-version: 1.0`

### Version Format

- **Format**: Semantic Versioning (Major.Minor)
- **Examples**: 1.0, 1.1, 2.0, 2.1

## Implementation Details

### 1. Service Configuration (MvcExtensions.cs)

```csharp
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

**Key Options**:

- `DefaultApiVersion`: Falls back to v1.0 if not specified
- `AssumeDefaultVersionWhenUnspecified`: Use default version for unversioned endpoints
- `ReportApiVersions`: Include API version info in response headers
- `ApiVersionReader.Combine()`: Support multiple versioning methods

### 2. Controller Routing

```csharp
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class WarehouseController : BaseController
{
    // Controller methods...
}
```

**Route Components**:

- `api/v{version:apiVersion}`: Enables URL-based versioning
- `[controller]`: Replaced with lowercase controller name (e.g., "warehouse")
- `[ApiVersion("1.0")]`: Declares which version(s) this controller supports

### 3. Swagger Configuration

The versioning system automatically:

- Creates separate Swagger documentation for each API version
- Generates OpenAPI specs at `/openapi/v1.json`, `/openapi/v2.json`, etc.
- Displays tabs in Swagger UI for each version
- Includes deprecation warnings for deprecated versions

## Supported API Endpoints

### Current Endpoints (v1.0)

All endpoints are currently at version 1.0:

**Warehouse Management**
- `GET /api/v1/warehouse`
- `GET /api/v1/warehouse/active`
- `GET /api/v1/warehouse/{id}`
- `GET /api/v1/warehouse/search`
- `POST /api/v1/warehouse/save`
- `POST /api/v1/warehouse/delete`
- `POST /api/v1/warehouse/toggle-status`

*Additional endpoints for inventory, returns, content areas, and media content follow the same pattern*

## Using the API

### Example Requests

#### 1. URL Path Versioning (Recommended)

```bash
curl -H "Authorization: Bearer <token>" \
  https://api.sahl.com/api/v1/warehouse
```

#### 2. Query String Versioning

```bash
curl -H "Authorization: Bearer <token>" \
  https://api.sahl.com/api/warehouse?api-version=1.0
```

#### 3. Header-Based Versioning

```bash
curl -H "Authorization: Bearer <token>" \
  -H "api-version: 1.0" \
  https://api.sahl.com/api/warehouse
```

### Response Headers

The API includes versioning information in response headers:

```
api-supported-versions: 1.0
api-deprecated-versions: (empty)
```

These headers indicate:

- Which versions are currently supported
- Which versions are deprecated (if applicable)

## Migration Guide

### Creating a New API Version

When you need to create a breaking change, follow these steps:

#### Step 1: Update the Controller

Add support for the new version:

```csharp
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")] // Add new version
public class WarehouseController : BaseController
{
    // Existing methods for v1.0...

    /// <summary>
    /// Get warehouse with new fields (v2.0).
    /// </summary>
    [HttpGet("v2")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetV2()
    {
        // New implementation...
    }
}
```

#### Step 2: Update the Swagger Documentation

The `ConfigureSwaggerOptions` class automatically handles this, but ensure:

- Documentation is clear about changes in each version
- Breaking changes are documented with migration guides

#### Step 3: Test Both Versions

```bash
# Test v1.0
curl https://api.sahl.com/api/v1/warehouse

# Test v2.0
curl https://api.sahl.com/api/v2/warehouse
```

#### Step 4: Announce Deprecation

Update the OpenApiInfo to mark old versions as deprecated:

```csharp
if (description.IsDeprecated)
{
    info.Description += " (This API version is deprecated. Please upgrade to v2.0)";
}
```

### Deprecation Timeline

Recommended timeline for API deprecation:

1. **Release New Version**: Announce v2.0
2. **Support Period**: Keep v1.0 active for 6+ months
3. **Deprecation Notice**: Mark v1.0 as deprecated 3 months before removal
4. **Final Deadline**: Remove v1.0 after 6 months
5. **Removal**: Stop supporting v1.0

## Client Implementation

### C# Client (Dashboard)

Update `ApiEndpoints.cs` to support multiple versions:

```csharp
public static class Warehouse
{
    // v1.0 endpoints
    public const string Get = "api/v1/warehouse";
    public const string GetActive = "api/v1/warehouse/active";
    public const string Search = "api/v1/warehouse/search";
    public const string Save = "api/v1/warehouse/save";
    public const string Delete = "api/v1/warehouse/delete";
    public const string ToggleStatus = "api/v1/warehouse/toggle-status";
    
    // v2.0 endpoints (when available)
    // public const string GetV2 = "api/v2/warehouse";
}
```

### JavaScript/TypeScript Client

```javascript
// Set API version in fetch headers
const headers = {
    'Authorization': `Bearer ${token}`,
    'api-version': '1.0'
};

const response = await fetch('https://api.sahl.com/api/warehouse', {
    method: 'GET',
    headers: headers
});
```

Or use URL path:

```javascript
const response = await fetch('https://api.sahl.com/api/v1/warehouse', {
    method: 'GET',
    headers: {
        'Authorization': `Bearer ${token}`
    }
});
```

## Swagger UI Access

### View All Versions

1. Navigate to: `https://localhost:7282/swagger`
2. Use the version dropdown to switch between available API versions
3. Each version has its own complete documentation

### Export OpenAPI Specification

Export specs for all versions:

```bash
# The endpoint is available at
https://localhost:7282/openapi/export

# It exports to: <solution>/api-specs/swagger.json
```

## Best Practices

### 1. Keep Versions Compatible

- Maintain backward compatibility within a version
- Use separate routes for breaking changes in new versions
- Don't remove fields from old versions

### 2. Document Changes

Add detailed comments for each version:

```csharp
/// <summary>
/// Get warehouse list.
/// </summary>
/// <remarks>
/// **Version 1.0**: Initial release
/// **Version 2.0**: Added metadata field
/// 
/// **Breaking Changes**: None
/// **New Fields**: metadata
/// </remarks>
[HttpGet]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public async Task<IActionResult> Get() { }
```

### 3. Version Numbering

- **Patch increment (.X)**: Not used in URL (handle with API updates)
- **Minor increment (1.X)**: Add new optional fields or endpoints
- **Major increment (X.0)**: Breaking changes, removed fields

### 4. Support Window

- Support current version indefinitely
- Support previous version for 6-12 months
- Mark deprecated versions clearly in documentation

### 5. Client Communication

- Announce version changes 3 months in advance
- Provide clear migration guides
- Offer technical support during transition

## Troubleshooting

### Issue: Endpoints return 404

**Cause**: Client is using wrong version format

**Solution**:
```bash
# Check version is correct
curl -H "api-version: 1.0" https://api.sahl.com/api/warehouse

# Or use URL path (recommended)
curl https://api.sahl.com/api/v1/warehouse
```

### Issue: Swagger shows no endpoints

**Cause**: `ApiVersion` attribute missing from controller

**Solution**:
```csharp
[ApiVersion("1.0")] // Add this
public class WarehouseController : BaseController { }
```

### Issue: Response headers missing api-supported-versions

**Cause**: `ReportApiVersions` not enabled

**Solution**: Verify in `MvcExtensions.cs`:
```csharp
options.ReportApiVersions = true;
```

## Configuration Reference

### MvcExtensions.cs

Located in: `src/Presentation/Api/Extensions/MvcExtensions.cs`

Key settings:

- `DefaultApiVersion`: Default to 1.0
- `AssumeDefaultVersionWhenUnspecified`: Assume v1.0 for unversioned requests
- `ReportApiVersions`: Include version info in headers

### SwaggerExtensions.cs

Located in: `src/Presentation/Api/Extensions/SwaggerExtensions.cs`

Key classes:

- `ConfigureSwaggerOptions`: Configures Swagger for all versions
- `SwaggerDefaultValues`: Filter to clean up version parameters

### Program.cs

Located in: `src/Presentation/Api/Program.cs`

Key changes:

- Gets `IApiVersionDescriptionProvider` from DI
- Uses it to dynamically create Swagger UI tabs

## Future Enhancements

### Planned Features

- [ ] Rate limiting per API version
- [ ] Sunset policies for deprecated versions
- [ ] API version analytics dashboard
- [ ] Automatic API version upgrade wizard for clients

### Recommended Additions

1. **API Version Policy**: Define sunset dates for each version
2. **Monitoring**: Track API usage by version
3. **Webhooks**: Notify clients when version support ends

## Additional Resources

### ASP.NET Core Versioning Documentation

- [API Versioning Package](https://github.com/dotnet/aspnet-api-versioning)
- [Official Documentation](https://github.com/dotnet/aspnet-api-versioning/wiki)

### Related Files

- `src/Presentation/Api/Extensions/MvcExtensions.cs`: Versioning configuration
- `src/Presentation/Api/Extensions/SwaggerExtensions.cs`: Swagger versioning
- `src/Presentation/Api/Controllers/Base/BaseController.cs`: Base controller with version helper
- `src/Presentation/Api/Program.cs`: Versioning middleware setup

### OpenAPI Specification

- `https://localhost:7282/openapi/v1.json`: OpenAPI spec for v1.0
- `https://localhost:7282/swagger`: Interactive Swagger UI

## Summary

The API versioning system is now fully implemented with:

? Multiple versioning strategies (URL, query, header)  
? Automatic Swagger documentation per version  
? Version information in response headers  
? Support for multiple simultaneous versions  
? Clear deprecation support  
? Backward compatibility maintained  

**All controllers must be updated to use versioned routing. See `WarehouseController` as the reference implementation.**

---

**Last Updated**: January 2025  
**Versioning Package**: Asp.Versioning v8.x  
**Status**: Complete
