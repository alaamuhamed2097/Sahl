# DTOs and Configurations Updates - VendorId Refactoring

## ?? Overview
This document focuses specifically on the DTOs (Data Transfer Objects) and EF Core Configurations that were updated during the VendorId refactoring.

---

## ?? Configurations Updated

### 1. **OfferConfiguration.cs** ? (Updated)

**Location**: `src/Infrastructure/DAL/ApplicationContext/Configurations/OfferConfiguration.cs`

#### Changes Made:

**Before**:
```csharp
// Offer -> User (many-to-one)
builder.HasOne<ApplicationUser>(o => o.User)
       .WithMany()
       .HasForeignKey(o => o.UserId)
       .OnDelete(DeleteBehavior.Restrict);

// Index
builder.HasIndex(o => o.UserId)
       .HasDatabaseName("IX_TbOffers_UserId");
```

**After**:
```csharp
// Offer -> Vendor (many-to-one)
builder.HasOne<TbVendor>(o => o.Vendor)
       .WithMany()
       .HasForeignKey(o => o.VendorId)
       .OnDelete(DeleteBehavior.Restrict);

// Index
builder.HasIndex(o => o.VendorId)
       .HasDatabaseName("IX_TbOffers_VendorId");
```

#### Using Directives:
```csharp
// Added
using Domains.Entities.ECommerceSystem.Vendor;

// Removed
using Domains.Identity;
```

#### Impact:
- ? Direct vendor-offer relationship configured correctly
- ? Foreign key points to `TbVendors` table instead of `AspNetUsers`
- ? Referential integrity maintained
- ? Index optimized for vendor queries

---

### 2. **ShoppingCartItemConfiguration.cs** ? (No Changes Needed)

**Location**: `src/Infrastructure/DAL/Configurations/ShoppingCartItemConfiguration.cs`

#### Status:
- ? Already correctly configured
- ? Uses `OfferCombinationPricingId` (not affected by UserId ? VendorId)
- ? No updates required

**Relevant Configuration**:
```csharp
// Foreign key to OfferCombinationPricing
builder.HasOne(x => x.OfferCombinationPricing)
    .WithMany()
    .HasForeignKey(x => x.OfferCombinationPricingId)
    .OnDelete(DeleteBehavior.Restrict);
```

---

## ?? DTOs Updated/Reviewed

### 1. **CartItemDto** ? (No Changes Needed)

**Location**: `src/Shared/Shared/DTOs/ECommerce/Cart/CartDtos.cs`

#### Current Definition:
```csharp
public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public Guid OfferCombinationPricingId { get; set; }  // ? Uses offer pricing ID
    public string SellerName { get; set; } = null!;      // ? Mapped from vendor
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### Status:
- ? Already uses `OfferCombinationPricingId` (not UserId)
- ? `SellerName` property correctly populated from vendor info
- ? No schema changes required

---

### 2. **CartSummaryDto** ? (No Changes Needed)

**Location**: `src/Shared/Shared/DTOs/ECommerce/Cart/CartDtos.cs`

#### Current Definition:
```csharp
public class CartSummaryDto
{
    public Guid CartId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();  // ? Uses CartItemDto
    public decimal SubTotal { get; set; }
    public decimal ShippingEstimate { get; set; }
    public decimal TaxEstimate { get; set; }
    public decimal TotalEstimate { get; set; }
    public int ItemCount { get; set; }
}
```

#### Status:
- ? Already correctly structured
- ? No direct offer references
- ? No changes needed

---

### 3. **AddToCartRequest** ? (No Changes Needed)

**Location**: `src/Shared/Shared/DTOs/ECommerce/Cart/CartDtos.cs`

#### Current Definition:
```csharp
public class AddToCartRequest
{
    public Guid ItemId { get; set; }
    public Guid OfferCombinationPricingId { get; set; }  // ? Correct: uses pricing ID
    public int Quantity { get; set; }
}
```

#### Status:
- ? Already uses `OfferCombinationPricingId` correctly
- ? No changes needed

---

## ?? Mapper Configurations Updated

### 1. **CartMappingProfile.cs** ? (Updated)

**Location**: `src/Core/BL/Mapper/CartMappingProfile.cs`

#### Changes Made:

**Before**:
```csharp
CreateMap<TbShoppingCartItem, CartItemDto>()
    .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.TitleEn))
    .ForMember(dest => dest.SellerName, opt => opt.MapFrom(
        src => src.OfferCombinationPricing.Offer.User.FullName))  // ? Was using User
    .ForMember(dest => dest.SubTotal, opt => opt.Ignore());
```

**After**:
```csharp
CreateMap<TbShoppingCartItem, CartItemDto>()
    .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.TitleEn))
    .ForMember(dest => dest.SellerName, opt => opt.MapFrom(
        src => src.OfferCombinationPricing.Offer.Vendor.CompanyName))  // ? Now uses Vendor
    .ForMember(dest => dest.SubTotal, opt => opt.Ignore());
```

#### Impact:
- ? SellerName now correctly sourced from Vendor.CompanyName
- ? More semantically correct than User.FullName
- ? Better data presentation for e-commerce context
- ? Seller company name is more professional than user name

#### Navigation Path:
```
TbShoppingCartItem
  ? OfferCombinationPricing
    ? Offer
      ? Vendor  (NEW - was User)
        ? CompanyName
```

---

## ?? Configuration & Mapping Summary Table

| Component | File | Type | Status | Changes |
|-----------|------|------|--------|---------|
| `TbOffer` Relationship | `OfferConfiguration.cs` | Config | ? Updated | FK mapping changed |
| Cart Item Config | `ShoppingCartItemConfiguration.cs` | Config | ? OK | No changes |
| `CartItemDto` | `CartDtos.cs` | DTO | ? OK | No changes |
| `CartSummaryDto` | `CartDtos.cs` | DTO | ? OK | No changes |
| `AddToCartRequest` | `CartDtos.cs` | DTO | ? OK | No changes |
| Cart Mapping | `CartMappingProfile.cs` | Mapper | ? Updated | SellerName source updated |

---

## ?? Mapping Flow Diagram

### Before Refactoring:
```
TbShoppingCartItem
  ? OfferCombinationPricing
    ? Offer
      ? User (ApplicationUser)
        ? FullName

CartItemDto.SellerName = Offer.User.FullName
```

### After Refactoring:
```
TbShoppingCartItem
  ? OfferCombinationPricing
    ? Offer
      ? Vendor (TbVendor)
        ? CompanyName

CartItemDto.SellerName = Offer.Vendor.CompanyName
```

---

## ?? DTO/Configuration Testing Recommendations

### Configuration Tests:
1. ? Verify `OfferConfiguration` foreign key is correctly set up
2. ? Check that `IX_TbOffers_VendorId` index exists in database
3. ? Confirm `TbOffers` table has `VendorId` column (not `UserId`)
4. ? Test referential integrity: deleting vendor should fail if offers exist

### Mapper Tests:
1. ? CartItemDto mapping produces correct SellerName
2. ? SellerName populated from Vendor.CompanyName (not User.FullName)
3. ? Null handling: what happens if Vendor is null?
4. ? Integration test: full cart mapping with multiple vendors

### DTO Tests:
1. ? AddToCartRequest correctly specifies OfferCombinationPricingId
2. ? CartItemDto correctly populated via mapper
3. ? CartSummaryDto contains properly mapped CartItemDtos
4. ? Serialization/deserialization works correctly

---

## ?? Key Points for DTOs & Configurations

### For DTOs:
- **No schema changes** - DTOs already use correct IDs (OfferCombinationPricingId)
- **Mapping updated** - SellerName now comes from Vendor instead of User
- **Type safe** - All Guid properties used for IDs, not strings
- **Consistent** - Matches other order-related DTOs (OrderDetailDto, etc.)

### For Configurations:
- **Foreign key updated** - Points to TbVendor instead of AspNetUsers
- **Index optimized** - Changed to IX_TbOffers_VendorId
- **Referential integrity** - Stronger constraints with proper delete behavior
- **Consistent** - Follows EF Core best practices

---

## ? Benefits of These Changes

1. **Type Safety**: Using Guid instead of string for vendor IDs
2. **Semantic Clarity**: CompanyName is more appropriate than FullName for sellers
3. **Performance**: Direct foreign key relationships are more efficient
4. **Maintainability**: Configuration clearly shows vendor ownership of offers
5. **Data Integrity**: Proper referential constraints prevent orphaned data

---

## ?? Migration Path for DTOs

If you have other DTOs that expose offer data, ensure they:
- ? Map `Offer.Vendor` instead of `Offer.User`
- ? Use `Offer.VendorId` instead of `Offer.UserId`
- ? Display vendor company information appropriately
- ? Handle null vendors gracefully

### Example DTO Update Pattern:
```csharp
// Before
public class OfferDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }  // ? OLD
    public string SellerUserName { get; set; }
}

// After
public class OfferDto
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }  // ? NEW
    public string SellerCompanyName { get; set; }
}
```

---

## ?? Related Files Reference

### Entities:
- `TbOffer.cs` - Main entity
- `TbVendor.cs` - Vendor entity
- `TbShoppingCartItem.cs` - Cart item entity

### Repositories:
- `OfferRepository.cs` - Includes statements updated
- `CartRepository.cs` - ThenInclude statements updated
- `IOfferRepository.cs` - Interface signature updated

### Services:
- `CartService.cs` - Uses updated repository
- `OrderService.cs` - Uses vendor ID correctly

---

## ? Completion Checklist

- ? OfferConfiguration updated with correct FK and index
- ? CartMappingProfile updated to use Vendor instead of User
- ? All DTOs reviewed (no changes needed - already correct)
- ? All configurations reviewed (OfferConfiguration updated)
- ? Navigation properties corrected in repositories
- ? Build successful with all changes
- ? No breaking changes to DTO contracts
- ? All mappings semantic and correct

