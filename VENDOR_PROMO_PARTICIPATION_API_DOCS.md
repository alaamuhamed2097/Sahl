# Vendor Promo Code Participation API - Complete Documentation

## Overview

A RESTful API that enables vendors to request participation in public promo codes. This implementation follows clean architecture principles with direct repository injection (no UnitOfWork) and separated DTOs per file.

## ? Complete Compliance

### ? No Unit of Work
- Direct repository injection in the service
- Each repository is injected separately
- No UnitOfWork wrapper used

### ? One DTO Per File
- `CreateVendorPromoCodeParticipationRequestDto.cs` - Input model
- `UpdateVendorPromoCodeParticipationRequestDto.cs` - Update model  
- `VendorPromoCodeParticipationRequestDto.cs` - Response model
- `VendorPromoCodeParticipationRequestListDto.cs` - List model

## Architecture

### 1. DTOs Layer
**Location:** `src/Shared/Shared/DTOs/Merchandising/PromoCode/`

Four separate files, each with a single DTO:

```
CreateVendorPromoCodeParticipationRequestDto
??? PromoCodeId (Guid) - Required
??? DescriptionEn (string?) - Optional
??? DescriptionAr (string?) - Optional
??? Notes (string?) - Optional

VendorPromoCodeParticipationRequestDto
??? Id, SellerRequestId, VendorId
??? VendorName, PromoCodeId, PromoCodeValue
??? Status, StatusName
??? DescriptionEn, DescriptionAr, Notes
??? AdminNotes, SubmittedAt, ReviewedAt
??? ReviewedByUserName, CreatedDateUtc, UpdatedDateUtc

VendorPromoCodeParticipationRequestListDto
??? Id, PromoCodeValue, PromoCodeTitle
??? Status, StatusName
??? SubmittedAt, ReviewedAt

UpdateVendorPromoCodeParticipationRequestDto
??? Id (Guid)
??? DescriptionEn, DescriptionAr, Notes
```

### 2. Service Layer

**Interface:** `src/Core/BL/Contracts/Service/Merchandising/PromoCode/IVendorPromoCodeParticipationService.cs`

```csharp
Task<(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)> 
    SubmitParticipationRequestAsync(Guid vendorId, CreateVendorPromoCodeParticipationRequestDto request, Guid userId);

Task<(bool Success, AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>? Data)> 
    GetVendorParticipationRequestsAsync(Guid vendorId, BaseSearchCriteriaModel criteria);

Task<(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)> 
    GetParticipationRequestAsync(Guid requestId, Guid vendorId);

Task<(bool Success, string Message)> 
    CancelParticipationRequestAsync(Guid requestId, Guid vendorId, Guid userId);
```

**Implementation:** `src/Core/BL/Services/Merchandising/PromoCode/VendorPromoCodeParticipationService.cs`

Direct repository injection:
```csharp
private readonly ITableRepository<TbSellerRequest> _sellerRequestRepository;
private readonly ITableRepository<TbVendor> _vendorRepository;
private readonly ICouponCodeRepository _couponCodeRepository;
private readonly IMapper _mapper;
```

### 3. Controller Layer

**File:** `src/Presentation/Api/Controllers/v1/Merchandising/VendorPromoCodeParticipationController.cs`

```
POST   /api/v1/vendorpromocodeparticipation/submit   - Submit request
POST   /api/v1/vendorpromocodeparticipation/list     - List requests
GET    /api/v1/vendorpromocodeparticipation/{id}     - Get details
DELETE /api/v1/vendorpromocodeparticipation/{id}     - Cancel request
```

## API Endpoints

### 1. Submit Participation Request

**Endpoint:** `POST /api/v1/vendorpromocodeparticipation/submit`

**Auth:** Bearer token with Vendor role

**Request Body:**
```json
{
  "promoCodeId": "550e8400-e29b-41d4-a716-446655440000",
  "descriptionEn": "Looking forward to this promo",
  "descriptionAr": "????? ???? ?????",
  "notes": "Optional notes"
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "Participation request submitted successfully",
  "data": {
    "id": "660e8400-e29b-41d4-a716-446655440000",
    "sellerRequestId": "660e8400-e29b-41d4-a716-446655440000",
    "vendorId": "770e8400-e29b-41d4-a716-446655440000",
    "vendorName": "My Store",
    "promoCodeId": "550e8400-e29b-41d4-a716-446655440000",
    "promoCodeValue": "SUMMER2024",
    "promoCodeTitle": "Summer Sale",
    "status": 0,
    "statusName": "Pending",
    "submittedAt": "2024-01-15T10:30:00Z",
    "createdDateUtc": "2024-01-15T10:30:00Z"
  }
}
```

**Error Responses:**
- 400: Invalid input, vendor not found, promo code not found
- 401: Unauthorized
- 409: Duplicate pending/approved request

### 2. List Participation Requests

**Endpoint:** `POST /api/v1/vendorpromocodeparticipation/list`

**Request Body:**
```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "searchTerm": "summer"
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Data retrieved successfully",
  "data": {
    "items": [
      {
        "id": "660e8400-e29b-41d4-a716-446655440000",
        "promoCodeValue": "SUMMER2024",
        "promoCodeTitle": "Summer Sale",
        "status": 0,
        "statusName": "Pending",
        "submittedAt": "2024-01-15T10:30:00Z",
        "reviewedAt": null
      }
    ],
    "totalRecords": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

### 3. Get Request Details

**Endpoint:** `GET /api/v1/vendorpromocodeparticipation/{id}`

**Response (200):** Returns full `VendorPromoCodeParticipationRequestDto`

### 4. Cancel Request

**Endpoint:** `DELETE /api/v1/vendorpromocodeparticipation/{id}`

**Response (200):**
```json
{
  "success": true,
  "message": "Request cancelled successfully"
}
```

## Data Model

Uses existing `TbSellerRequest` entity:

```csharp
public class TbSellerRequest : BaseEntity
{
    public Guid VendorId { get; set; }
    public SellerRequestType RequestType { get; set; }        // PromoCodeParticipation
    public SellerRequestStatus Status { get; set; }           // Pending initially
    public string TitleEn { get; set; }                       // Auto-generated
    public string TitleAr { get; set; }                       // Auto-generated
    public string DescriptionEn { get; set; }                 // From vendor
    public string DescriptionAr { get; set; }                 // From vendor
    public string RequestData { get; set; }                   // Stores PromoCodeId
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewedByUserId { get; set; }
    public string? ReviewNotes { get; set; }
    public virtual TbVendor Vendor { get; set; }
    public virtual ApplicationUser? ReviewedByUser { get; set; }
}
```

## Service Methods

### SubmitParticipationRequestAsync

**Purpose:** Create a new participation request

**Validations:**
- Vendor must exist
- Promo code must exist
- Prevent duplicate pending/approved requests

**Process:**
1. Validate inputs
2. Create `TbSellerRequest` with Status=Pending
3. Store PromoCodeId in RequestData
4. Save to database
5. Return request details

### GetVendorParticipationRequestsAsync

**Purpose:** List vendor's requests with pagination

**Features:**
- Pagination support
- Full-text search (Title/Description)
- Status filtering
- Sorted by date descending

### GetParticipationRequestAsync

**Purpose:** Get details of a specific request

**Validations:**
- Request must exist
- Request must belong to vendor
- Request must be PromoCodeParticipation type

### CancelParticipationRequestAsync

**Purpose:** Cancel a pending request

**Validations:**
- Request must exist
- Request must be pending
- Uses soft delete

## Status Enum

```csharp
public enum SellerRequestStatus
{
    Pending = 0,      // Awaiting review
    Approved = 1,     // Admin approved
    Rejected = 2,     // Admin rejected
    Withdrawn = 3,    // Vendor withdrew
    Archived = 4      // Archived
}
```

## Dependencies

**DTOs:** `Shared` project
**Service Interface:** `BL.Contracts` 
**Service Implementation:** `BL`
**Controller:** `Api`
**Repositories:** `DAL`

## Dependency Injection Setup

```csharp
// In Program.cs or Startup.cs
services.AddScoped<ITableRepository<TbSellerRequest>, TableRepository<TbSellerRequest>>();
services.AddScoped<ITableRepository<TbVendor>, TableRepository<TbVendor>>();
services.AddScoped<ICouponCodeRepository, CouponCodeRepository>();
services.AddScoped<IVendorPromoCodeParticipationService, VendorPromoCodeParticipationService>();
```

## Error Handling

All operations return tuples with Success flag and Message:

```csharp
(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)
(bool Success, AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>? Data)
(bool Success, string Message)
```

Controller maps to appropriate HTTP status codes:
- 200: Success
- 400: Bad request / validation error
- 401: Unauthorized
- 404: Not found
- 409: Conflict (duplicate request)

## Security

- **Authentication:** JWT Bearer tokens required
- **Authorization:** Vendor role required
- **Vendor Verification:** Vendor ID extracted from authenticated user claims
- **Data Access:** Vendors can only view/manage their own requests

## Best Practices Implemented

? Single responsibility per DTO file
? Direct repository injection (no UnitOfWork)
? Comprehensive validation
? Soft delete for data integrity
? Consistent error responses
? Proper HTTP status codes
? Role-based access control
? Async/await pattern
? Mapper for DTO conversion
? Clear naming conventions

## Testing Examples

### cURL Examples

```bash
# Submit request
curl -X POST "https://api.example.com/api/v1/vendorpromocodeparticipation/submit" \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"promoCodeId": "550e8400-e29b-41d4-a716-446655440000"}'

# List requests
curl -X POST "https://api.example.com/api/v1/vendorpromocodeparticipation/list" \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"pageNumber": 1, "pageSize": 10}'

# Get details
curl -X GET "https://api.example.com/api/v1/vendorpromocodeparticipation/660e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer TOKEN"

# Cancel request
curl -X DELETE "https://api.example.com/api/v1/vendorpromocodeparticipation/660e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer TOKEN"
```

## Build Status

? **Build Successful** - All code compiles without errors

## Summary

The Vendor Promo Code Participation API provides a complete solution for vendors to:
1. Request participation in public promo codes
2. Track their requests with pagination and search
3. View detailed status and admin feedback
4. Cancel pending requests

All implemented with clean architecture, direct repository injection, separated DTOs, and comprehensive error handling.
