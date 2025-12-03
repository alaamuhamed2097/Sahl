## Centralized Exception Handling - Implementation Guide

### Overview
Exception handling has been centralized in middleware instead of individual API controllers. This reduces code duplication, improves consistency, and makes error handling easier to maintain.

### Architecture

```
Request ? Middleware Pipeline ? Exception Handling Middleware ? Controllers ? Business Logic
                                        ?
                            If Exception Thrown:
                            - Catch exception
                            - Log error
                            - Return standardized ResponseModel with appropriate HTTP status code
```

### How It Works

1. **ExceptionHandlingMiddleware** catches all unhandled exceptions from controllers and business logic
2. **Custom exception types** (in `Api/Exceptions/ApiException.cs`) allow you to throw specific, typed exceptions
3. The middleware automatically:
   - Logs the exception
   - Maps exception types to appropriate HTTP status codes
   - Returns a standardized `ResponseModel<T>` response
   - Includes error codes and messages

### Supported Exception Types

#### 1. **ApiException** (Base Class)
Base exception for all custom API exceptions. Use when you have a custom error scenario.

```csharp
throw new ApiException("Something went wrong", 
    statusCode: 500, 
    errorCode: "CUSTOM_ERROR",
    errors: new List<string> { "Additional details" });
```

#### 2. **ValidationException**
Use for validation failures (HTTP 400).

```csharp
throw new ValidationException("Validation failed", 
    errors: new List<string> 
    { 
        "Email is required",
        "Password must be at least 8 characters"
    });
```

#### 3. **ResourceNotFoundException**
Use when a requested resource cannot be found (HTTP 404).

```csharp
// Option 1: With resource name and identifier
throw new ResourceNotFoundException("Campaign", "12345");

// Option 2: With custom message
throw new ResourceNotFoundException("No active campaigns found for this period");
```

#### 4. **UnauthorizedAccessException**
Use when user is not authenticated (HTTP 401).

```csharp
throw new UnauthorizedAccessException("User must be logged in to perform this action");
```

#### 5. **ForbiddenException**
Use when user is authenticated but not authorized (HTTP 403).

```csharp
throw new ForbiddenException("You do not have permission to edit this campaign");
```

#### 6. **BusinessLogicException**
Use for business logic violations (HTTP 400).

```csharp
throw new BusinessLogicException("Campaign end date must be after start date",
    errors: new List<string> { "Invalid date range" });
```

#### 7. **ConflictException**
Use for duplicate or conflict scenarios (HTTP 409).

```csharp
throw new ConflictException("A campaign with this name already exists");
```

#### 8. **ExternalServiceException**
Use when external service calls fail (HTTP 502).

```csharp
throw new ExternalServiceException("PaymentGateway", 
    "Failed to process payment: Connection timeout");
```

#### 9. **RateLimitException**
Use for rate limiting scenarios (HTTP 429).

```csharp
throw new RateLimitException("You have exceeded the maximum number of requests");
```

### Standard .NET Exceptions (Auto-Handled)

These standard exceptions are automatically handled and mapped to appropriate HTTP status codes:

| Exception | HTTP Status | Message |
|-----------|------------|---------|
| `ArgumentNullException` | 400 Bad Request | "Invalid input provided" |
| `ArgumentException` | 400 Bad Request | "Invalid argument provided" |
| `InvalidOperationException` | 400 Bad Request | "Invalid operation" |
| `KeyNotFoundException` | 404 Not Found | "Resource not found" |
| `UnauthorizedAccessException` | 401 Unauthorized | "Not authorized" |
| `OperationCanceledException` | 408 Request Timeout | "Request was cancelled" |
| `DbUpdateException` | 500 Internal Error | "Database operation failed" |
| `DbUpdateConcurrencyException` | 409 Conflict | "Record modified by another user" |
| `FluentValidation.ValidationException` | 400 Bad Request | "Validation failed" |

### Refactoring Examples

#### Before (With Try-Catch)
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
{
    try
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new ResponseModel<object>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            });
        }

        var campaign = await _campaignService.CreateCampaignAsync(dto);
        return CreatedAtAction(
            nameof(GetById),
            new { id = campaign.Id },
            new ResponseModel<CampaignDto>
            {
                Success = true,
                Data = campaign,
                Message = "Campaign created successfully"
            });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating campaign");
        return StatusCode(500, new ResponseModel<CampaignDto>
        {
            Success = false,
            Message = "An error occurred while creating the campaign",
            Errors = new List<string> { ex.Message }
        });
    }
}
```

#### After (Without Try-Catch)
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return BadRequest(new ResponseModel<object>
        {
            Success = false,
            Message = "Validation failed",
            Errors = errors
        });
    }

    var campaign = await _campaignService.CreateCampaignAsync(dto);
    return CreatedAtAction(
        nameof(GetById),
        new { id = campaign.Id },
        new ResponseModel<CampaignDto>
        {
            Success = true,
            Data = campaign,
            Message = "Campaign created successfully"
        });
}
```

### Best Practices

#### 1. **Keep Controllers Clean**
- Remove try-catch blocks from controllers
- Let middleware handle unhandled exceptions
- Only validate ModelState in controllers

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    // ? GOOD: Let middleware handle this
    var item = await _service.GetByIdAsync(id);
    
    // ? GOOD: Specific check with proper exception
    if (item == null)
        throw new ResourceNotFoundException("Item", id.ToString());
    
    return Ok(item);
}
```

#### 2. **Use Custom Exceptions in Business Logic**
- Throw custom exceptions from services
- Let them bubble up to middleware

```csharp
public class CampaignService : ICampaignService
{
    public async Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto dto)
    {
        // Validation in service
        if (dto.EndDate <= dto.StartDate)
            throw new ValidationException("Campaign end date must be after start date");

        // Business rule check
        if (await _campaignRepository.ExistsByNameAsync(dto.Name))
            throw new ConflictException($"Campaign with name '{dto.Name}' already exists");

        // ... create campaign
        return campaign;
    }
}
```

#### 3. **Don't Log in Controllers**
- The middleware logs all exceptions automatically
- Only log business-specific information in services

```csharp
// ? DON'T: Don't log here
[HttpPost]
public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
{
    try
    {
        var campaign = await _campaignService.CreateCampaignAsync(dto);
        _logger.LogInformation("Campaign created"); // Middleware already logs
    }
    catch { }
}

// ? DO: Log in services when needed for business reasons
public class CampaignService
{
    public async Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto dto)
    {
        var campaign = new Campaign { /* ... */ };
        await _campaignRepository.AddAsync(campaign);
        
        // Log business event
        _logger.LogInformation("Campaign {CampaignId} created by {UserId}", 
            campaign.Id, _currentUser.Id);
        
        return _mapper.Map<CampaignDto>(campaign);
    }
}
```

#### 4. **Handle Validation Errors Specifically**
Keep validation error handling in controllers, but use custom exceptions for deeper validation

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
{
    // ModelState validation (ASP.NET level)
    if (!ModelState.IsValid)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
        return BadRequest(new ResponseModel<object> { /* ... */ });
    }

    // Service will throw ValidationException for business validation
    var campaign = await _campaignService.CreateCampaignAsync(dto);
    
    return CreatedAtAction(nameof(GetById), new { id = campaign.Id }, 
        new ResponseModel<CampaignDto> { /* ... */ });
}
```

### Response Format

All exceptions are mapped to this standard response format:

```json
{
  "success": false,
  "statusCode": 400,
  "data": null,
  "message": "Validation failed",
  "errorCode": "VALIDATION_ERROR",
  "errors": [
    "Email is required",
    "Password must be at least 8 characters"
  ]
}
```

### Testing Exception Handling

```csharp
[TestFixture]
public class ExceptionHandlingTests
{
    [Test]
    public async Task Create_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var invalidDto = new CampaignCreateDto 
        { 
            StartDate = DateTime.Now.AddDays(10),
            EndDate = DateTime.Now.AddDays(5) // Invalid: end before start
        };

        // Act & Assert
        Assert.ThrowsAsync<ValidationException>(
            async () => await _service.CreateCampaignAsync(invalidDto));
    }

    [Test]
    public async Task GetById_WhenNotFound_ShouldThrowResourceNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ResourceNotFoundException>(
            async () => await _service.GetByIdAsync(Guid.NewGuid()));
    }
}
```

### Migration Checklist

- [x] ExceptionHandlingMiddleware created
- [x] Custom exception classes created
- [x] Program.cs updated to use middleware
- [x] BaseController cleaned up
- [x] AuthController refactored
- [x] CampaignController refactored (as template)
- [ ] Refactor remaining controllers (use CampaignController as template)
- [ ] Update all services to use custom exceptions
- [ ] Add unit tests for exception handling
- [ ] Test all API endpoints for consistent error responses

### References

- **Exception Classes**: `src/Presentation/Api/Exceptions/ApiException.cs`
- **Middleware**: `src/Presentation/Api/Middleware/ExceptionHandlingMiddleware.cs`
- **Base Controller**: `src/Presentation/Api/Controllers/Base/BaseController.cs`
- **Reference Implementation**: `src/Presentation/Api/Controllers/Campaign/CampaignController.cs`
