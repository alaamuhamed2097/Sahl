# Template ?????? Controllers ?? API Versioning

## Pattern ??? ?????? ??? Controller

### ?????? 1: ???????

```csharp
// ? BEFORE (?? Controllers/{Category}/{Controller}.cs)
namespace Api.Controllers.{Category}
{
    [ApiController]
    [Route("api/[controller]")]
    public class {Controller}Controller : BaseController
    {
        // ... implementation
    }
}

// ? AFTER (?? Controllers/v1/{Category}/{Controller}.cs)
using Asp.Versioning;

namespace Api.Controllers.v1.{Category}
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class {Controller}Controller : BaseController
    {
        // ... implementation with updated XML comments
    }
}
```

### ?????? 2: ????? ???? Methods

```csharp
// ? BEFORE
/// <summary>
/// Gets all items.
/// </summary>
[HttpGet]
public async Task<IActionResult> Get()

// ? AFTER
/// <summary>
/// Gets all items.
/// </summary>
/// <remarks>
/// API Version: 1.0+
/// </remarks>
[HttpGet]
public async Task<IActionResult> Get()
```

## ????? ???? - ItemController

### ??? ?????: `Controllers/Catalog/ItemController.cs`

?????? ???: `Controllers/v1/Catalog/ItemController.cs`

```csharp
using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.Service.Item;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Item;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Catalog
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService, Serilog.ILogger logger)
            : base(logger)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Retrieves all items.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _itemService.GetAllAsync();
            // ... rest of implementation
        }

        // ... other methods with updated <remarks>
    }
}
```

## Scripts ?????? Controllers ?????

### PowerShell Script (??? ?????? ?????? - ??????)

```powershell
# ??? script ???? ???????? ?????? ???? Controllers
# ????? ?? ??????? ??????

# Create v1 folder structure
$categories = @("Catalog", "Order", "User", "Location", "Notification", "Authentication", "Warehouse", "Settings", "Sales", "Shipping", "Content", "Pricing", "Wallet", "Loyalty", "Campaign")

foreach ($category in $categories) {
    $path = "src/Presentation/Api/Controllers/v1/$category"
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Path $path -Force | Out-Null
    }
}
```

## ????? Controllers ????????

### Catalog (2)
- [ ] UnitController
- [ ] AttributeController
- [x] CategoryController ?
- [x] BrandController ?
- [ ] ItemController

### Order (5)
- [x] OrderController ?
- [ ] CartController
- [ ] CheckoutController
- [ ] DeliveryController
- [ ] PaymentController
- [ ] ShipmentController

### User (5)
- [x] CustomerController ?
- [ ] VendorController
- [ ] AdminController
- [ ] UserAuthenticationController
- [ ] UserProfileController
- [ ] UserActivationController

### Location (3)
- [x] CountryController ?
- [x] CityController ?
- [x] StateController ?

### Notification (2)
- [x] NotificationController ?
- [x] UserNotificationsController ?

### Authentication (2)
- [x] AuthController ?
- [ ] PasswordController

### Other Categories (8)
- [ ] ShippingCompanyController (Shipping)
- [ ] ContentAreaController (Content)
- [ ] SettingController (Settings)
- [ ] CurrencyController (Settings)
- [ ] PricingSystemController (Pricing)
- [ ] WalletController (Wallet)
- [ ] LoyaltyController (Loyalty)
- [ ] CampaignController (Campaign)
- [ ] CouponCodeController (Sales)

## ?????? Controllers

### ???????? cURL

```bash
# URL Path (Recommended)
curl -X GET "http://localhost:5000/api/v1/category"
curl -X GET "http://localhost:5000/api/v1/order"
curl -X GET "http://localhost:5000/api/v1/customer"

# Query String
curl -X GET "http://localhost:5000/api/category?api-version=1.0"

# Header
curl -X GET "http://localhost:5000/api/category" \
  -H "api-version: 1.0"

# With Authentication Token
curl -X GET "http://localhost:5000/api/v1/warehouse" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### ???????? Postman

1. ????? variable: `{{base_url}} = http://localhost:5000`
2. ??????? URL: `{{base_url}}/api/v1/category`
3. ?? ????? Header: `api-version: 1.0`
4. ????? ?????

## Swagger Testing

1. ???: `http://localhost:5000/swagger`
2. ?????? version ?? ??????? (v1)
3. ?????? endpoint ?? ??? Swagger UI
4. ??? ?? ???? ???? ?????

## ?????? ?? ????? ??????

### ? Namespace ????
```csharp
// Wrong
namespace Api.Controllers.Catalog

// Correct
namespace Api.Controllers.v1.Catalog
```

### ? Route ????
```csharp
// Wrong
[Route("api/[controller]")]

// Correct
[Route("api/v{version:apiVersion}/[controller]")]
```

### ? Missing ApiVersion
```csharp
// Wrong
public class ItemController

// Correct
[ApiVersion("1.0")]
public class ItemController
```

### ? Namespace import ?????
```csharp
// Wrong
// using Asp.Versioning; - missing

// Correct
using Asp.Versioning;
```

## Checklist ?????? ?? ?? Controller

- [ ] ??? ????? ?? ??????: `Controllers/v1/{Category}/{Controller}.cs`
- [ ] Namespace: `Api.Controllers.v1.{Category}`
- [ ] ????? ???: `using Asp.Versioning;`
- [ ] ????? ???: `[ApiVersion("1.0")]`
- [ ] Route ????? ???: `api/v{version:apiVersion}/[controller]`
- [ ] ???? methods ????? XML comments
- [ ] ???? XML comments ?????: `<remarks>API Version: 1.0+</remarks>`
- [ ] ?????? ??? ???? ?????
- [ ] Swagger ???? ??? endpoints ???? ????

## ???? ???? - ??? ?????

```csharp
using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.Service.Unit;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Unit;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Catalog
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService, Serilog.ILogger logger)
            : base(logger)
        {
            _unitService = unitService;
        }

        /// <summary>
        /// Retrieves all units.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var units = await _unitService.GetAllAsync();
            return Ok(new ResponseModel<IEnumerable<UnitDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = units
            });
        }

        /// <summary>
        /// Retrieves a unit by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                });

            var unit = await _unitService.GetByIdAsync(id);
            if (unit == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                });

            return Ok(new ResponseModel<UnitDto>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = unit
            });
        }

        /// <summary>
        /// Searches units with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _unitService.GetPageAsync(criteria);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PaginatedDataModel<UnitDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PaginatedDataModel<UnitDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Saves a unit.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] UnitDto unitDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInput
                });

            var success = await _unitService.Save(unitDto, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully,
                Data = true
            });
        }

        /// <summary>
        /// Deletes a unit by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var success = await _unitService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DeletedSuccessfully,
                Data = true
            });
        }
    }
}
```

## ??????? ????????

1. ??? ??? ??? template ??? controller ?????
2. ??????? `{Category}` ? `{Service}` ? `{Dto}` ?????? ???????
3. ?????? ?? ??????: `dotnet build`
4. ?????? Swagger: `http://localhost:5000/swagger`
5. ?????? endpoint ???? ??? ????? ??? controller

