# Base Controller

This folder contains the `BaseController` class which is the base class for all API controllers.

## Purpose

The `BaseController` provides:
- Shared utility methods
- Common authentication properties
- Localization support
- Logging capabilities
- API version helpers

## Usage

All controllers should inherit from `BaseController`:

```csharp
namespace Api.Controllers.v1.YourCategory
{
    public class YourController : BaseController
    {
        // Your implementation
    }
}
```

## Available Properties & Methods

### Properties

```csharp
// Get current user's role
protected string? RoleName { get; }

// Get current user's ID
protected string? UserId { get; }

// Get current user's ID as Guid
protected Guid GuidUserId { get; }

// Get logger instance
protected readonly Serilog.ILogger _logger;
```

### Methods

```csharp
// Get localized resource string
protected string GetResource<T>(string resourceKey)
{
    // Supports:
    // - ActionsResources
    // - FormResources
    // - GeneralResources
    // - NotifiAndAlertsResources
    // - UserResources
}

// Get API version from route
protected string GetApiVersion()
{
    // Returns version like "1.0"
}
```

## Example Usage

```csharp
public class ProductController : BaseController
{
    public ProductController(IProductService service, Serilog.ILogger logger)
        : base(logger)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        _logger.Information($"User {UserId} getting product {id}");
        
        if (id == Guid.Empty)
        {
            var errorMsg = GetResource<NotifiAndAlertsResources>(
                nameof(NotifiAndAlertsResources.InvalidInputAlert)
            );
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = errorMsg
            });
        }

        var product = await _service.GetByIdAsync(id);
        if (product == null)
        {
            var notFoundMsg = GetResource<NotifiAndAlertsResources>(
                nameof(NotifiAndAlertsResources.NoDataFound)
            );
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = notFoundMsg
            });
        }

        return Ok(new ResponseModel<ProductDto>
        {
            Success = true,
            Message = GetResource<NotifiAndAlertsResources>(
                nameof(NotifiAndAlertsResources.DataRetrieved)
            ),
            Data = product
        });
    }
}
```

## Localization Support

The `GetResource<T>()` method automatically handles:

- Language detection from `x-language` header
- Fallback to Arabic (ar-EG) if not specified
- Support for both Arabic and English

Example:
```csharp
// Automatically detects language from header
var message = GetResource<NotifiAndAlertsResources>(
    nameof(NotifiAndAlertsResources.DataRetrieved)
);
// Returns: "?? ??????? ????????" (if Arabic)
// Or: "Data Retrieved" (if English)
```

## Important Notes

?? **Do Not Modify**
- BaseController should rarely be modified
- If you need to add functionality, discuss with the team first
- Any changes affect all controllers

? **Good Practices**
- Always call base(logger) in constructors
- Use GetResource<T>() for all user-facing messages
- Log important operations using _logger
- Return ResponseModel for consistency

## Testing BaseController Methods

```csharp
[Fact]
public void GetResource_WithValidKey_ReturnsLocalizedString()
{
    // Arrange
    var controller = new TestController(mockLogger);
    
    // Act
    var result = controller.GetResource<NotifiAndAlertsResources>(
        nameof(NotifiAndAlertsResources.DataRetrieved)
    );
    
    // Assert
    Assert.NotNull(result);
    Assert.NotEmpty(result);
}
```

## Version Information

- Created: December 3, 2025
- Status: Stable ?
- Used by: All API v1 controllers

---

**Remember**: BaseController is the foundation of all API controllers. Keep it clean and well-maintained!
