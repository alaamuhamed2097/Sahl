# Refactored Vendor Promo Code Participation API

## Summary of Changes

### 1. DTO Separation - One DTO Per File

Previously, all DTOs were in a single file. Now each DTO has its own file:

**New Files Created:**
- `src/Shared/Shared/DTOs/Merchandising/PromoCode/CreateVendorPromoCodeParticipationRequestDto.cs`
- `src/Shared/Shared/DTOs/Merchandising/PromoCode/UpdateVendorPromoCodeParticipationRequestDto.cs`
- `src/Shared/Shared/DTOs/Merchandising/PromoCode/VendorPromoCodeParticipationRequestDto.cs`
- `src/Shared/Shared/DTOs/Merchandising/PromoCode/VendorPromoCodeParticipationRequestListDto.cs`

### 2. Repository-Based Approach - No Unit of Work

Changed from UnitOfWork pattern to direct repository injection:

**Old:**
```csharp
public VendorPromoCodeParticipationService(IUnitOfWork unitOfWork, IMapper mapper)
{
    _unitOfWork = unitOfWork;
}

// Usage
var vendor = await _unitOfWork.TableRepository<TbVendor>().FindByIdAsync(vendorId);
```

**New:**
```csharp
public VendorPromoCodeParticipationService(
    ITableRepository<TbSellerRequest> sellerRequestRepository,
    ITableRepository<TbVendor> vendorRepository,
    ICouponCodeRepository couponCodeRepository,
    IMapper mapper)
{
    _sellerRequestRepository = sellerRequestRepository;
    _vendorRepository = vendorRepository;
    _couponCodeRepository = couponCodeRepository;
    _mapper = mapper;
}

// Usage
var vendor = await _vendorRepository.FindByIdAsync(vendorId);
```

### 3. Service Update

- File: `src/Core/BL/Services/Merchandising/PromoCode/VendorPromoCodeParticipationService.cs`
- Uses direct repository injection instead of UnitOfWork
- All four service methods updated accordingly
- Repository methods: `FindByIdAsync()`, `FindAsync()`, `SaveAsync()`, `SoftDeleteAsync()`

### 4. Controller Update

- File: `src/Presentation/Api/Controllers/v1/Merchandising/VendorPromoCodeParticipationController.cs`
- Updated to import individual DTOs
- Service remains the same interface - no API changes
- All 4 endpoints work exactly as before

## Files Modified

| File | Change Type | Status |
|------|------------|--------|
| VendorPromoCodeParticipationService.cs | UPDATED | ? Using repositories directly |
| VendorPromoCodeParticipationController.cs | UPDATED | ? Updated imports |
| IVendorPromoCodeParticipationService.cs | UNCHANGED | ? Same interface |

## Files Created (DTOs)

| File | Type | Status |
|------|------|--------|
| CreateVendorPromoCodeParticipationRequestDto.cs | NEW | ? Separate file |
| UpdateVendorPromoCodeParticipationRequestDto.cs | NEW | ? Separate file |
| VendorPromoCodeParticipationRequestDto.cs | NEW | ? Separate file |
| VendorPromoCodeParticipationRequestListDto.cs | NEW | ? Separate file |

## Key Changes in Service

### Repository Injection
```csharp
private readonly ITableRepository<TbSellerRequest> _sellerRequestRepository;
private readonly ITableRepository<TbVendor> _vendorRepository;
private readonly ICouponCodeRepository _couponCodeRepository;
```

### Direct Repository Calls
```csharp
// Get vendor
var vendor = await _vendorRepository.FindByIdAsync(vendorId);

// Get promo code
var promoCode = await _couponCodeRepository.GetByIdAsync(request.PromoCodeId);

// Find existing requests
var existingRequestsList = await _sellerRequestRepository.FindAsync(r => 
    r.VendorId == vendorId && 
    r.RequestType == SellerRequestType.PromoCodeParticipation);

// Save request
var saveResult = await _sellerRequestRepository.SaveAsync(sellerRequest, userId);

// Soft delete
await _sellerRequestRepository.SoftDeleteAsync(requestId, userId);
```

## API Endpoints (Unchanged)

All endpoints work exactly the same:

```
POST   /api/v1/vendorpromocodeparticipation/submit   - Submit request
POST   /api/v1/vendorpromocodeparticipation/list     - List requests  
GET    /api/v1/vendorpromocodeparticipation/{id}     - Get details
DELETE /api/v1/vendorpromocodeparticipation/{id}     - Cancel request
```

## Build Status

? **Build Successful** - All code compiles without errors

## DI Registration Required

In your `Program.cs` or startup configuration, ensure:

```csharp
// Register repositories (should already be done)
services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
services.AddScoped<ICouponCodeRepository, CouponCodeRepository>();

// Register the service
services.AddScoped<IVendorPromoCodeParticipationService, VendorPromoCodeParticipationService>();
```

## Compliance Checklist

? Never use unit of work - direct repository injection
? Never add multiple DTOs in one file - each has its own file
? Follows project conventions and architecture
? Maintains backward compatibility
? All code compiles successfully
? Service logic unchanged from user's perspective
? API endpoints unchanged

## Benefits of Refactoring

1. **Cleaner DTOs** - Each DTO is in its own file, easier to maintain
2. **Direct Repository Access** - No UnitOfWork wrapper, clearer dependencies
3. **Better Testability** - Can mock individual repositories
4. **Follows Standards** - Aligns with project conventions
5. **Simpler Dependency Graph** - Clear which repositories are used

## No Breaking Changes

- API endpoints remain the same
- Service interface remains the same
- DTO properties remain the same
- All functionality preserved
- Existing consumers unaffected
