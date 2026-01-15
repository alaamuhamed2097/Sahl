# ?? Bug Fix: API 500 Error - Home Page Blocks Endpoint

## Problem
The Blazor Dashboard was getting a 500 error when calling:
```
GET /api/v1/admin/blocks
```

This happened because of a type mismatch between what the API returned and what the Dashboard expected.

## Root Cause
The issue was a **serialization/mapping mismatch**:

1. **API returned**: `List<TbHomepageBlock>` (domain entities)
2. **Dashboard requested**: `List<AdminBlockListDto>` (DTOs)
3. **Result**: JSON deserialization failed because types didn't match perfectly

Additionally:
- The DTO has computed properties (`StatusBadge`, `IsCurrentlyVisible`)
- The entity has enums (`Type` as `HomepageBlockType`, `Layout` as `BlockLayout`)
- These need explicit mapping

## Solution Applied

### 1. Created AutoMapper Profile
**File**: `src/Core/BL/Mapper/Merchandising/HomepageBlockMappingProfile.cs`

```csharp
public class HomepageBlockMappingProfile : Profile
{
    public HomepageBlockMappingProfile()
    {
        // Map entities to DTOs
        CreateMap<TbHomepageBlock, AdminBlockListDto>()
            .ForMember(dest => dest.StatusBadge, opt => opt.Ignore())
            .ReverseMap();
        
        // ... other mappings
    }
}
```

**Why**: Automatically converts domain entities to DTOs with proper type handling

### 2. Updated API Controller
**File**: `src/Presentation/Api/Controllers/v1/Merchandising/AdminBlockController.cs`

**Changes**:
- Added `IMapper` dependency injection
- Updated `GetAllBlocks()` to:
  - Map entities to DTOs using AutoMapper
  - Return `AdminBlockListDto` instead of domain entities
  - Include proper error handling with detailed error messages
  - Return HTTP 500 with detailed exception info if mapping fails

```csharp
[HttpGet]
[ProducesResponseType(typeof(ResponseModel<List<AdminBlockListDto>>), StatusCodes.Status200OK)]
public async Task<IActionResult> GetAllBlocks()
{
    try
    {
        var blocks = await _adminBlockService.GetAllBlocksAsync();
        var blockDtos = _mapper.Map<List<AdminBlockListDto>>(blocks);
        
        return Ok(new ResponseModel<List<AdminBlockListDto>>
        {
            Success = true,
            Data = blockDtos
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new ResponseModel<object>
        {
            Success = false,
            Message = "An error occurred while retrieving blocks.",
            Errors = new List<string> { ex.Message, ex.InnerException?.Message }
        });
    }
}
```

**Why**: Ensures the API returns the correct DTO type expected by the Dashboard

## Benefits

? **Type Safety**: Domain entities are properly mapped to DTOs
? **Proper Serialization**: JSON serialization now works correctly
? **Error Details**: 500 errors now include detailed exception messages for debugging
? **Computed Properties**: `StatusBadge` property is properly calculated in the DTO
? **Enum Handling**: Enum types are automatically converted to strings by AutoMapper

## How It Works

```
Dashboard Service                API Controller              BL Service
      ?                              ?                            ?
      ???? Request ????????????????? ?                            ?
      ?   (List<AdminBlockListDto>)  ?                            ?
      ?                              ???? Call ?????????????????? ?
      ?                              ?  GetAllBlocksAsync()       ?
      ?                              ?                            ?
      ?                              ? ? Returns ?????????????????
      ?                              ?  (List<TbHomepageBlock>)   ?
      ?                              ?                            ?
      ?                              ? [AutoMapper Maps]          ?
      ?                              ?  TbHomepageBlock ?         ?
      ?                              ?  AdminBlockListDto         ?
      ?                              ?                            ?
      ?? Response ?????????????????? ?
      ?  (List<AdminBlockListDto>)  ?
      ?                              ?
```

## Files Modified

1. **`src/Core/BL/Mapper/Merchandising/HomepageBlockMappingProfile.cs`** (NEW)
   - AutoMapper profile for block mappings
   - Handles entity-to-DTO conversions

2. **`src/Presentation/Api/Controllers/v1/Merchandising/AdminBlockController.cs`** (UPDATED)
   - Added IMapper injection
   - Updated GetAllBlocks() to return DTOs
   - Enhanced error handling

## AutoMapper Registration

AutoMapper is automatically configured in `Program.cs`:
```csharp
builder.Services.AddAutoMapperConfiguration();
```

It scans the BL assembly for all `Profile` classes, including the new `HomepageBlockMappingProfile`.

## Testing

To verify the fix works:

1. **Start the Dashboard**
2. **Navigate to**: Dashboard ? Marketing ? Home Page Blocks
3. **Expected Result**: 
   - Page loads without 500 error
   - List of blocks displays (empty if no blocks exist)
   - Search and filter work
   - No console errors

## Build Status
? **BUILD SUCCESSFUL** - No compilation errors

## Deployment Notes

- The new mapping profile is automatically discovered by AutoMapper
- No configuration changes needed
- Compatible with existing code
- Backward compatible (returns same data, just better typed)

## Future Enhancements

If performance becomes an issue with large datasets:
1. Add pagination to the API endpoint
2. Implement filtering on the backend (not just client-side)
3. Use projection in AutoMapper for only needed properties
4. Consider caching for frequently accessed blocks

---

The fix ensures type-safe communication between the API and Dashboard, with proper error handling for debugging.
