# Controller Versioning Template

This template shows how to update your existing controllers to support API versioning.

## Step-by-Step Update Process

### 1. Add Using Statement

Add the Asp.Versioning namespace:

```csharp
using Asp.Versioning;
```

### 2. Update Route and Add Version Attribute

**Before**:
```csharp
[Route("api/[controller]")]
[ApiController]
public class MyController : BaseController
{
    // ...
}
```

**After**:
```csharp
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class MyController : BaseController
{
    // ...
}
```

### 3. Update XML Documentation

Add API version information to method summaries:

```csharp
/// <summary>
/// Description of what the endpoint does.
/// </summary>
/// <remarks>
/// Requires [appropriate role].
/// 
/// API Version: 1.0+
/// </remarks>
[HttpGet]
public async Task<IActionResult> Get()
{
    // ...
}
```

## Complete Example

Here's a complete example showing a fully versioned controller:

```csharp
using Api.Controllers.Base;
using BL.Contracts.Service.Example;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Example;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Asp.Versioning;

namespace Api.Controllers.Example
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ExampleController : BaseController
    {
        private readonly IExampleService _exampleService;

        public ExampleController(IExampleService exampleService, Serilog.ILogger logger)
            : base(logger)
        {
            _exampleService = exampleService;
        }

        /// <summary>
        /// Gets all examples.
        /// </summary>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _exampleService.GetAllAsync();

                return Ok(new ResponseModel<IEnumerable<ExampleDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(
                        nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = items
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets an example by ID.
        /// </summary>
        /// <param name="id">The ID of the example.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(
                            nameof(NotifiAndAlertsResources.InvalidInputAlert))
                    });

                var item = await _exampleService.GetByIdAsync(id);
                if (item == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(
                            nameof(NotifiAndAlertsResources.NoDataFound))
                    });

                return Ok(new ResponseModel<ExampleDto>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(
                        nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = item
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches examples with pagination and filtering.
        /// </summary>
        /// <param name="criteriaModel">Search criteria including pagination and filters.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search(
            [FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            try
            {
                criteriaModel.PageNumber = criteriaModel.PageNumber < 1 
                    ? 1 
                    : criteriaModel.PageNumber;
                criteriaModel.PageSize = criteriaModel.PageSize < 1 
                    || criteriaModel.PageSize > 100 
                    ? 10 
                    : criteriaModel.PageSize;

                var result = await _exampleService.SearchAsync(criteriaModel);

                return Ok(new ResponseModel<PaginatedDataModel<ExampleDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(
                        nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Creates a new example or updates an existing one.
        /// </summary>
        /// <param name="dto">The example data.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] ExampleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid example data."
                    });

                var success = await _exampleService.SaveAsync(dto, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(
                            nameof(NotifiAndAlertsResources.SaveFailed))
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(
                        nameof(NotifiAndAlertsResources.SavedSuccessfully))
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes an example by ID (soft delete).
        /// </summary>
        /// <param name="id">The ID of the example to delete.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid example ID."
                    });

                var success = await _exampleService.DeleteAsync(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(
                            nameof(NotifiAndAlertsResources.DeleteFailed))
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(
                        nameof(NotifiAndAlertsResources.DeletedSuccessfully))
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
```

## Quick Checklist

Use this checklist when updating each controller:

- [ ] Add `using Asp.Versioning;` statement
- [ ] Update `[Route]` to include `v{version:apiVersion}`
- [ ] Add `[ApiVersion("1.0")]` attribute to class
- [ ] Add "API Version: 1.0+" to XML comments
- [ ] Update all endpoint XML documentation
- [ ] Test with URL path: `/api/v1/controllername`
- [ ] Test with query string: `/api/controllername?api-version=1.0`
- [ ] Verify Swagger shows endpoint in v1.0 section
- [ ] Verify response headers include `api-supported-versions: 1.0`

## Controllers to Update

Based on your file structure, update these controllers:

### Authentication
- [x] AuthController
- [ ] PasswordController
- [ ] UserAuthenticationController
- [ ] UserActivationController

### User Management
- [ ] AdminController
- [ ] VendorController
- [ ] CustomerController
- [ ] UserProfileController

### Catalog
- [ ] ItemController
- [ ] AttributeController
- [ ] CategoryController
- [ ] BrandController
- [ ] UnitController

### E-Commerce
- [ ] CouponCodeController
- [ ] Campaign
Controller
- [ ] WalletController

### Content
- [ ] ContentAreaController
- [ ] NotificationController
- [ ] UserNotificationsController

### Location
- [ ] CountryController
- [ ] StateController
- [ ] CityController

### Settings & Other
- [ ] SettingController
- [ ] CurrencyController
- [ ] ShippingCompanyController
- [ ] PricingSystemController

## Common Patterns

### Pattern 1: Simple GET

```csharp
/// <summary>
/// Gets all items.
/// </summary>
/// <remarks>
/// API Version: 1.0+
/// </remarks>
[HttpGet]
public async Task<IActionResult> Get()
{
    try
    {
        var items = await _service.GetAllAsync();
        return Ok(new ResponseModel<IEnumerable<ItemDto>> { /* ... */ });
    }
    catch (Exception ex)
    {
        return HandleException(ex);
    }
}
```

### Pattern 2: GET by ID

```csharp
/// <summary>
/// Gets an item by ID.
/// </summary>
/// <param name="id">The item ID.</param>
/// <remarks>
/// API Version: 1.0+
/// </remarks>
[HttpGet("{id}")]
public async Task<IActionResult> Get(Guid id)
{
    // Validation, service call, return...
}
```

### Pattern 3: POST Save

```csharp
/// <summary>
/// Creates or updates an item.
/// </summary>
/// <param name="dto">The item data.</param>
/// <remarks>
/// API Version: 1.0+
/// </remarks>
[HttpPost("save")]
public async Task<IActionResult> Save([FromBody] ItemDto dto)
{
    // Validation, service call, return...
}
```

### Pattern 4: POST Delete

```csharp
/// <summary>
/// Deletes an item.
/// </summary>
/// <param name="id">The item ID to delete.</param>
/// <remarks>
/// API Version: 1.0+
/// </remarks>
[HttpPost("delete")]
public async Task<IActionResult> Delete([FromBody] Guid id)
{
    // Validation, service call, return...
}
```

## Version Mapping for Specific Endpoints

When you have multiple versions supporting different implementations:

```csharp
/// <summary>
/// Get items (v1.0 - Returns basic fields).
/// </summary>
[HttpGet]
[ApiVersion("1.0")]
public async Task<IActionResult> GetV1()
{
    // Old implementation
}

/// <summary>
/// Get items (v2.0 - Returns additional metadata).
/// </summary>
[HttpGet("v2")]
[MapToApiVersion("2.0")]
public async Task<IActionResult> GetV2()
{
    // New implementation with more data
}
```

## Testing the Implementation

### Via cURL

```bash
# Test with URL path (recommended)
curl -H "Authorization: Bearer <token>" \
  https://localhost:7282/api/v1/warehouse

# Test with query string
curl -H "Authorization: Bearer <token>" \
  https://localhost:7282/api/warehouse?api-version=1.0

# Test with header
curl -H "Authorization: Bearer <token>" \
  -H "api-version: 1.0" \
  https://localhost:7282/api/warehouse
```

### Via Postman

1. Create a new request
2. Set URL to: `https://localhost:7282/api/v1/warehouse`
3. Add Authorization header
4. Send request
5. Check response headers for `api-supported-versions`

### Via Swagger UI

1. Navigate to `https://localhost:7282/swagger`
2. Select version from dropdown
3. Look for your controller in the list
4. Try it out with the "Try it out" button

## Troubleshooting

### Error: "No matching API version found"

**Cause**: Route doesn't match the versioning pattern

**Fix**: Verify:
1. Route includes `v{version:apiVersion}`
2. Class has `[ApiVersion("1.0")]` attribute
3. Client is requesting `/api/v1/...` format

### Error: "The request had route values"

**Cause**: Mixed versioning strategies

**Fix**: Use one strategy consistently:
- Prefer URL path: `/api/v1/...`
- Use headers for API clients: `-H "api-version: 1.0"`

### Swagger not showing endpoint

**Cause**: Missing `[ApiVersion]` attribute

**Fix**: Add to controller class:
```csharp
[ApiVersion("1.0")]
public class MyController : BaseController { }
```

---

**Template Version**: 1.0  
**Last Updated**: January 2025
