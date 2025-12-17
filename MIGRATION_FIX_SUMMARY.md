# Migration Fix Summary

## Issue Overview
The migration `20251209162748_OptimizeItemSearchPerformance.cs` was failing due to references to non-existent database columns and incorrect entity property names. This prevented the migration from being applied to the database.

## Root Cause Analysis

### Problem 1: Incorrect Column Names in TbOffers Table
**Error**: Trying to create indexes on columns that don't exist
- **Original Code**: Referenced `UserId` column
- **Actual Column**: The entity uses `VendorId` (Guid) instead of `UserId`
- **Impact**: 
  - `IX_TbOffers_ItemId_UserId` ? Changed to `IX_TbOffers_ItemId_VendorId`
  - `IX_TbOffers_UserId` ? Changed to `IX_TbOffers_VendorId`
  - Stored procedure: Changed `o.UserId AS VendorId` to `o.VendorId`

**Migration Reference**: Line 44-58 in Up() method

### Problem 2: Non-existent Columns in TbOfferCombinationPricing Table
**Error**: Stored procedure and view referenced columns that don't exist in the entity
- **Missing Columns**:
  - `IsFreeShipping` - No such column exists
  - `EstimatedDeliveryDays` - No such column exists
  - `IsDefault` - No such column exists

**Solution**: 
1. Removed index creation for non-existent `IsDefault` column
2. Updated stored procedure to use only existing columns
3. Updated view to use `HandlingTimeInDays` from TbOffers instead of estimated delivery
4. Added proper IsDeleted flag checks to respect soft-delete pattern

**Migration References**: 
- Lines 75-80 (removed IsDefault index)
- Lines 89-286 (updated stored procedure)
- Lines 291-310 (updated view)

### Problem 3: Incorrect Soft-Delete Checks
**Issue**: The SQL queries weren't checking the `IsDeleted` flag
**Fix**: Added explicit checks for `IsDeleted = 0` in:
- Stored procedure WHERE clauses (lines 172-175)
- View WHERE clause (lines 305-307)

## Changes Made

### 1. Index Creation (Phase 2 - TbOffers)
```csharp
// BEFORE (Incorrect)
migrationBuilder.CreateIndex(name: "IX_TbOffers_ItemId_UserId", columns: "ItemId", "UserId");
migrationBuilder.CreateIndex(name: "IX_TbOffers_UserId", column: "UserId");

// AFTER (Correct)
migrationBuilder.CreateIndex(name: "IX_TbOffers_ItemId_VendorId", columns: "ItemId", "VendorId");
migrationBuilder.CreateIndex(name: "IX_TbOffers_VendorId", column: "VendorId");
```

### 2. Index Creation (Phase 3 - TbOfferCombinationPricing)
```csharp
// BEFORE (Incorrect)
migrationBuilder.CreateIndex(name: "IX_TbOfferCombinationPricing_IsDefault", column: "IsDefault");

// AFTER (Removed - column doesn't exist)
// Removed this index creation entirely
```

### 3. Stored Procedure Updates
**Key Changes**:
- Changed `o.UserId AS VendorId` to `o.VendorId`
- Removed `@FreeShippingOnly`, `@MaxDeliveryDays` parameters (no corresponding columns)
- Removed references to `p.IsFreeShipping` and `p.EstimatedDeliveryDays`
- Simplified BestOfferData CONCAT to only include existing columns
- Added soft-delete checks: `IsDeleted = 0` for all tables
- Updated vendor ID table to use UNIQUEIDENTIFIER instead of NVARCHAR(450)

### 4. View Updates
**Key Changes**:
- Removed `HasFreeShipping` - No such column exists
- Changed `FastestDelivery` to use `MIN(o.HandlingTimeInDays)` instead of non-existent field
- Added soft-delete checks for all tables
- Kept the important metrics: BestPrice, TotalStock, TotalOffers, FastestDelivery

### 5. Down Method Updates
- Updated index names to match the corrected Up() method
- Removed drop statement for non-existent IsDefault index

## Impact Analysis

### What Works Now
? All indexes are created on existing columns
? Stored procedure references only valid columns
? View references only valid columns
? Soft-delete pattern is properly applied
? Data type consistency (VendorId as GUID)

### Performance Implications
- **Phase 1 Indexes**: TitleAr, TitleEn, CategoryId+BrandId, IsActive+CreatedDateUtc - Good for text search and filtering
- **Phase 2 Indexes**: ItemId+VendorId, VisibilityScope, StorgeLocation, VendorId - Optimized for offer queries
- **Phase 3 Indexes**: OfferId+SalesPrice, SalesPrice, StockStatus+AvailableQuantity - Perfect for pricing and inventory queries

## Entity Column Mapping

### TbOffer Properties
| Property | Type | Database Column |
|----------|------|-----------------|
| ItemId | Guid | ItemId |
| VendorId | Guid | VendorId |
| VisibilityScope | Enum | VisibilityScope |
| StorgeLocation | Enum | StorgeLocation |
| HandlingTimeInDays | int | HandlingTimeInDays |
| IsBuyBoxWinner | bool | IsBuyBoxWinner |

### TbOfferCombinationPricing Properties
| Property | Type | Database Column |
|----------|------|-----------------|
| OfferId | Guid | OfferId |
| ItemCombinationId | Guid | ItemCombinationId |
| Price | decimal | Price |
| SalesPrice | decimal | SalesPrice |
| CostPrice | decimal? | CostPrice |
| AvailableQuantity | int | AvailableQuantity |
| StockStatus | Enum | StockStatus |
| LockedQuantity | int | LockedQuantity |
| ReservedQuantity | int | ReservedQuantity |

## Testing Recommendations

1. **Apply the Migration**: Run `dotnet ef database update`
2. **Verify Indexes**: Query SQL Server Management Studio
   ```sql
   SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('TbOffers')
   SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('TbOfferCombinationPricing')
   SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('TbItems')
   ```
3. **Test Stored Procedure**: Execute SpSearchItemsMultiVendor with various parameters
4. **Test View**: Query VwItemBestPrices and verify results

## Files Modified
- `src\Infrastructure\DAL\Migrations\20251209162748_OptimizeItemSearchPerformance.cs`

## Build Status
? Build successful - All compilation errors resolved
