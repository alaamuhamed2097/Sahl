# Migration Validation & Implementation Guide

## Pre-Migration Checklist

### 1. Backup Database
```sql
-- Backup current database before migration
BACKUP DATABASE [YourDatabaseName] 
TO DISK = 'C:\Backups\YourDatabaseName_PreMigration.bak'
```

### 2. Verify Current State
```sql
-- Check if indexes already exist (should be false)
SELECT * FROM sys.indexes 
WHERE name LIKE 'IX_TbItems_%' 
   OR name LIKE 'IX_TbOffers_%'
   OR name LIKE 'IX_TbOfferCombinationPricing_%'

-- Check if stored procedure exists (should be false)
SELECT * FROM sys.procedures 
WHERE name = 'SpSearchItemsMultiVendor'

-- Check if view exists (should be false)
SELECT * FROM sys.views 
WHERE name = 'VwItemBestPrices'
```

### 3. Entity Verification
Ensure these tables exist with correct column names:
- **TbItems**: Id, TitleAr, TitleEn, IsActive, IsDeleted, CreatedDateUtc, CategoryId, BrandId, ShortDescriptionAr, ShortDescriptionEn, ThumbnailImage
- **TbOffers**: Id, ItemId, VendorId, IsBuyBoxWinner, VisibilityScope, StorgeLocation, HandlingTimeInDays, IsDeleted
- **TbOfferCombinationPricing**: Id, OfferId, SalesPrice, Price, AvailableQuantity, StockStatus, IsDeleted, ItemCombinationId

## Migration Execution

### Command to Apply Migration
```bash
# Using Entity Framework CLI
dotnet ef database update 20251209162748_OptimizeItemSearchPerformance

# Or for specific project
dotnet ef database update 20251209162748_OptimizeItemSearchPerformance --project src/Infrastructure/DAL
```

### Expected Migration Steps
1. Create 4 indexes on TbItems
2. Create 4 indexes on TbOffers
3. Create 3 indexes on TbOfferCombinationPricing
4. Create SpSearchItemsMultiVendor stored procedure
5. Create VwItemBestPrices view

## Post-Migration Validation

### 1. Index Verification
```sql
-- Verify Phase 1: TbItems Indexes
SELECT name, type_desc, is_disabled 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('TbItems')
AND name IN ('IX_TbItems_TitleAr', 'IX_TbItems_TitleEn', 
             'IX_TbItems_CategoryId_BrandId', 'IX_TbItems_IsActive_CreatedDate')

-- Verify Phase 2: TbOffers Indexes
SELECT name, type_desc, is_disabled 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('TbOffers')
AND name IN ('IX_TbOffers_ItemId_VendorId', 'IX_TbOffers_VisibilityScope',
             'IX_TbOffers_StorgeLocation', 'IX_TbOffers_VendorId')

-- Verify Phase 3: TbOfferCombinationPricing Indexes
SELECT name, type_desc, is_disabled 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('TbOfferCombinationPricing')
AND name IN ('IX_TbOfferCombinationPricing_OfferId_SalesPrice',
             'IX_TbOfferCombinationPricing_SalesPrice',
             'IX_TbOfferCombinationPricing_StockStatus_AvailableQuantity')
```

### 2. Stored Procedure Verification
```sql
-- Check if procedure exists
SELECT * FROM sys.procedures 
WHERE name = 'SpSearchItemsMultiVendor'

-- Check procedure definition
EXEC sp_helptext 'SpSearchItemsMultiVendor'

-- Test basic execution
EXEC SpSearchItemsMultiVendor
    @SearchTerm = NULL,
    @CategoryIds = NULL,
    @BrandIds = NULL,
    @MinPrice = NULL,
    @MaxPrice = NULL,
    @VendorIds = NULL,
    @InStockOnly = 0,
    @OnSaleOnly = 0,
    @BuyBoxWinnersOnly = 0,
    @SortBy = 'newest',
    @PageNumber = 1,
    @PageSize = 20
```

### 3. View Verification
```sql
-- Check if view exists
SELECT * FROM sys.views 
WHERE name = 'VwItemBestPrices'

-- Check view definition
EXEC sp_helptext 'VwItemBestPrices'

-- Test basic query
SELECT TOP 10 * FROM VwItemBestPrices
```

## Comprehensive Test Cases

### Test 1: Search by Text
```sql
EXEC SpSearchItemsMultiVendor
    @SearchTerm = 'laptop',
    @PageNumber = 1,
    @PageSize = 20
```

### Test 2: Filter by Category
```sql
EXEC SpSearchItemsMultiVendor
    @CategoryIds = 'category-guid-1,category-guid-2',
    @PageNumber = 1,
    @PageSize = 20
```

### Test 3: Price Range Filter
```sql
EXEC SpSearchItemsMultiVendor
    @MinPrice = 100.00,
    @MaxPrice = 500.00,
    @PageNumber = 1,
    @PageSize = 20
```

### Test 4: In-Stock Items Only
```sql
EXEC SpSearchItemsMultiVendor
    @InStockOnly = 1,
    @PageNumber = 1,
    @PageSize = 20
```

### Test 5: On-Sale Items
```sql
EXEC SpSearchItemsMultiVendor
    @OnSaleOnly = 1,
    @PageNumber = 1,
    @PageSize = 20
```

### Test 6: Buy Box Winners
```sql
EXEC SpSearchItemsMultiVendor
    @BuyBoxWinnersOnly = 1,
    @PageNumber = 1,
    @PageSize = 20
```

### Test 7: Sort by Price (Ascending)
```sql
EXEC SpSearchItemsMultiVendor
    @SortBy = 'price_asc',
    @PageNumber = 1,
    @PageSize = 20
```

### Test 8: Sort by Price (Descending)
```sql
EXEC SpSearchItemsMultiVendor
    @SortBy = 'price_desc',
    @PageNumber = 1,
    @PageSize = 20
```

### Test 9: Sort by Newest
```sql
EXEC SpSearchItemsMultiVendor
    @SortBy = 'newest',
    @PageNumber = 1,
    @PageSize = 20
```

### Test 10: Combined Filters
```sql
EXEC SpSearchItemsMultiVendor
    @SearchTerm = 'phone',
    @CategoryIds = 'category-guid-1',
    @BrandIds = 'brand-guid-1',
    @MinPrice = 200.00,
    @MaxPrice = 1000.00,
    @VendorIds = 'vendor-guid-1,vendor-guid-2',
    @InStockOnly = 1,
    @OnSaleOnly = 1,
    @SortBy = 'price_asc',
    @PageNumber = 1,
    @PageSize = 20
```

### Test 11: Pagination
```sql
-- Page 1
EXEC SpSearchItemsMultiVendor
    @PageNumber = 1,
    @PageSize = 20

-- Page 2
EXEC SpSearchItemsMultiVendor
    @PageNumber = 2,
    @PageSize = 20

-- Page 3 (should be empty if less than 40 items)
EXEC SpSearchItemsMultiVendor
    @PageNumber = 3,
    @PageSize = 20
```

### Test 12: View Query
```sql
-- Query VwItemBestPrices
SELECT TOP 20 
    ItemId,
    BestPrice,
    TotalStock,
    TotalOffers,
    FastestDelivery
FROM VwItemBestPrices
ORDER BY BestPrice ASC
```

## Performance Testing

### Index Usage Analysis
```sql
-- Check if indexes are being used
SELECT 
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.indexes i
LEFT JOIN sys.dm_db_index_usage_stats s ON i.object_id = s.object_id AND i.index_id = s.index_id
WHERE object_id IN (OBJECT_ID('TbItems'), OBJECT_ID('TbOffers'), OBJECT_ID('TbOfferCombinationPricing'))
ORDER BY s.user_seeks + s.user_scans + s.user_lookups DESC
```

### Query Execution Plan
```sql
-- Enable statistics
SET STATISTICS IO ON
SET STATISTICS TIME ON

-- Execute procedure
EXEC SpSearchItemsMultiVendor
    @SearchTerm = 'test',
    @PageNumber = 1,
    @PageSize = 20

-- Turn off statistics
SET STATISTICS IO OFF
SET STATISTICS TIME OFF
```

## Rollback Procedure

If issues occur, rollback the migration:

```bash
# Rollback to previous migration
dotnet ef database update 20251209153205_replaceCurrentStateWithIsDeletedInViews --project src/Infrastructure/DAL
```

### Manual Rollback (if needed)
```sql
-- Drop view
DROP VIEW IF EXISTS [dbo].[VwItemBestPrices]

-- Drop stored procedure
DROP PROCEDURE IF EXISTS [dbo].[SpSearchItemsMultiVendor]

-- Drop indexes
DROP INDEX IF EXISTS IX_TbItems_TitleAr ON TbItems
DROP INDEX IF EXISTS IX_TbItems_TitleEn ON TbItems
DROP INDEX IF EXISTS IX_TbItems_CategoryId_BrandId ON TbItems
DROP INDEX IF EXISTS IX_TbItems_IsActive_CreatedDate ON TbItems

DROP INDEX IF EXISTS IX_TbOffers_ItemId_VendorId ON TbOffers
DROP INDEX IF EXISTS IX_TbOffers_VisibilityScope ON TbOffers
DROP INDEX IF EXISTS IX_TbOffers_StorgeLocation ON TbOffers
DROP INDEX IF EXISTS IX_TbOffers_VendorId ON TbOffers

DROP INDEX IF EXISTS IX_TbOfferCombinationPricing_OfferId_SalesPrice ON TbOfferCombinationPricing
DROP INDEX IF EXISTS IX_TbOfferCombinationPricing_SalesPrice ON TbOfferCombinationPricing
DROP INDEX IF EXISTS IX_TbOfferCombinationPricing_StockStatus_AvailableQuantity ON TbOfferCombinationPricing
```

## Monitoring After Migration

### Track Index Health
```sql
-- Monthly fragmentation check
SELECT 
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent,
    ips.page_count
FROM sys.indexes i
INNER JOIN sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('TbItems'), NULL, NULL, 'LIMITED') ips ON i.index_id = ips.index_id
WHERE ips.avg_fragmentation_in_percent > 10

-- Rebuild if fragmentation > 30%
-- Reorganize if fragmentation 10-30%
```

### Monitor Procedure Performance
```sql
-- Track execution metrics
SELECT 
    TOP 20
    qt.text,
    qs.execution_count,
    qs.total_elapsed_time / 1000000 as total_seconds,
    qs.total_elapsed_time / qs.execution_count / 1000 as avg_milliseconds
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
WHERE qt.text LIKE '%SpSearchItemsMultiVendor%'
ORDER BY qs.execution_count DESC
```

## Success Criteria

? All 11 indexes created successfully
? Stored procedure executes without errors
? View returns data correctly
? All test cases pass
? Pagination works correctly
? Filters produce expected results
? Sorting options function properly
? No performance regression
? Database backup available

## Support & Troubleshooting

If you encounter issues:

1. **Check Migration Status**: `dotnet ef migrations list`
2. **View Migration History**: Query `__EFMigrationsHistory` table
3. **Review Error Logs**: Check SQL Server error logs
4. **Verify Permissions**: Ensure user has DDL permissions
5. **Check Disk Space**: Ensure sufficient disk space for indexes

## Next Steps

1. Apply migration to Development environment
2. Run all test cases
3. Monitor performance for 24-48 hours
4. Apply to Staging environment
5. Final validation
6. Apply to Production environment
7. Monitor production performance
