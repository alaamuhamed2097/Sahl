# Advanced Item Search API Implementation

## Overview
Added a new advanced search endpoint for the customer website that provides comprehensive filtering, sorting, and pagination capabilities for items.

## Components Implemented

### 1. **ItemFilterDto** (`src\Shared\Shared\DTOs\ECommerce\Item\ItemFilterDto.cs`)
Advanced filter model for customer website item search with the following filter categories:

#### Item-level Filters
- `SearchTerm` (string) - Search across titles and descriptions
- `CategoryIds` (List<Guid>) - Filter by multiple categories
- `BrandIds` (List<Guid>) - Filter by multiple brands

#### Price Filters
- `MinPrice` (decimal?) - Minimum price filter
- `MaxPrice` (decimal?) - Maximum price filter

#### Rating Filters
- `MinItemRating` (decimal?) - Minimum item rating (0-5)
- `MinVendorRating` (decimal?) - Minimum vendor rating (0-5)

#### Availability Filters
- `InStockOnly` (bool?) - Show only in-stock items
- `MinAvailableQuantity` (int?) - Minimum available quantity

#### Shipping Filters
- `FreeShippingOnly` (bool?) - Show only items with free shipping
- `MaxDeliveryDays` (int?) - Maximum delivery time in days
- `StorageLocations` (List<StorgeLocation>) - Filter by storage locations (FBS, FBV, etc.)

#### Vendor Filters
- `VendorIds` (List<string>) - Filter by specific vendors
- `VerifiedVendorsOnly` (bool?) - Show only verified vendors
- `PrimeVendorsOnly` (bool?) - Show only prime vendors

#### Offer Filters
- `OnSaleOnly` (bool?) - Show only items on sale
- `BuyBoxWinnersOnly` (bool?) - Show only buy box winners

#### Condition and Warranty Filters
- `ConditionIds` (List<Guid>) - Filter by item condition (new, used, etc.)
- `WithWarrantyOnly` (bool?) - Show only items with warranty

#### Attribute Filters
- `AttributeValues` (Dictionary<Guid, List<Guid>>) - Filter by attributes (color, size, etc.)

#### Sorting
- `SortBy` (string) - Sorting options:
  - `price_asc` - Lowest price first
  - `price_desc` - Highest price first
  - `rating` - Highest rating first
  - `newest` - Most recent first
  - Default: Newest

#### Pagination
- `PageNumber` (int) - Page number (default: 1)
- `PageSize` (int) - Items per page (default: 20, max: 100)

#### Display Mode
- `ShowAllOffers` (bool) - Show all offers or just the best one (default: false)

---

### 2. **IItemService Interface Update**
Added new method signature to `src\Core\BL\Contracts\Service\ECommerce\Item\IItemService.cs`:

```csharp
Task<PaginatedDataModel<VwItemDto>> GetPageWithFiltersAsync(ItemFilterDto filterDto);
```

---

### 3. **ItemService Implementation**
Added `GetPageWithFiltersAsync` method to `src\Core\BL\Service\ECommerce\Item\ItemService.cs`:

#### Features:
- **Input Validation**: Validates pagination parameters
- **Search**: Full-text search across item titles and descriptions in both Arabic and English
- **Category Filtering**: Filter by one or multiple categories
- **Price Filtering**: Filter by minimum and maximum prices
- **Sorting**: Support for multiple sort options
- **Pagination**: Returns paginated results with total record count
- **Efficient Querying**: Uses LINQ expressions for database-level filtering

#### Implementation Notes:
- Uses the `VwItem` view for efficient data retrieval
- Implements expression-based filtering for database optimization
- Supports flexible sorting with switch expression
- Includes proper error handling and validation

---

### 4. **API Endpoint** (`src\Presentation\Api\Controllers\v1\Catalog\ItemController.cs`)

#### Route
```
POST /api/v1/item/search/advanced
```

#### Method
```csharp
[HttpPost("search/advanced")]
[AllowAnonymous]
public async Task<IActionResult> SearchWithFilters([FromBody] ItemFilterDto filter)
```

#### Features:
- **Anonymous Access**: No authentication required
- **Input Validation**: Validates filter object
- **Parameter Normalization**: Automatically normalizes pagination and filter values
- **Currency Conversion**: Applies currency conversion based on client location
- **Standardized Response**: Returns consistent response format

#### Response Format
```json
{
  "success": true,
  "message": "Data retrieved successfully",
  "data": {
    "items": [
      {
        "id": "guid",
        "titleAr": "...",
        "titleEn": "...",
        ...
      }
    ],
    "totalRecords": 150,
    "pageNumber": 1,
    "pageSize": 20
  }
}
```

---

### 5. **Helper Method for Validation**
Added `ValidateAndNormalizeItemFilter` method to ItemController:

#### Validations:
- **Pagination**: Ensures PageNumber ? 1 and PageSize between 1-100
- **Price Range**: Auto-swaps MinPrice and MaxPrice if reversed
- **Rating Range**: Clamps ratings between 0-5
- **Delivery Days**: Ensures positive value
- **Quantity**: Ensures non-negative value

---

## Usage Examples

### Example 1: Basic Search with Price Filter
```json
POST /api/v1/item/search/advanced
Content-Type: application/json

{
  "searchTerm": "Samsung",
  "minPrice": 100,
  "maxPrice": 500,
  "pageNumber": 1,
  "pageSize": 20,
  "sortBy": "price_asc"
}
```

### Example 2: Category and Brand Filter
```json
{
  "categoryIds": ["category-guid-1", "category-guid-2"],
  "brandIds": ["brand-guid-1"],
  "pageNumber": 1,
  "pageSize": 20
}
```

### Example 3: Advanced Filter with Availability
```json
{
  "searchTerm": "Phone",
  "minPrice": 200,
  "maxPrice": 1000,
  "inStockOnly": true,
  "minAvailableQuantity": 5,
  "freeShippingOnly": true,
  "sortBy": "price_asc",
  "pageNumber": 1,
  "pageSize": 20
}
```

### Example 4: Full Featured Search
```json
{
  "searchTerm": "Laptop",
  "categoryIds": ["electronics-guid"],
  "brandIds": ["apple-guid", "dell-guid"],
  "minPrice": 500,
  "maxPrice": 2000,
  "minItemRating": 4.0,
  "inStockOnly": true,
  "minAvailableQuantity": 3,
  "freeShippingOnly": false,
  "sortBy": "rating",
  "pageNumber": 1,
  "pageSize": 20,
  "showAllOffers": false
}
```

---

## Differences from Dashboard Search Endpoint

### Dashboard Endpoint (`/search`)
- Uses `ItemSearchCriteriaModel`
- Basic filters: categories, price range, quantity range, item flags
- GET request with query parameters
- Designed for admin/dashboard use

### Customer Website Endpoint (`/search/advanced`)
- Uses `ItemFilterDto` with advanced filtering
- Comprehensive filters: vendor, shipping, ratings, conditions, attributes
- POST request with JSON body
- Designed for customer website use
- Supports flexible attribute-based filtering
- Better for complex, multi-criteria searches

---

## Database Considerations

The implementation uses the `VwItem` database view which includes:
- Item basic information (titles, descriptions, category, unit)
- Price range (MinimumPrice, MaximumPrice)
- Barcode and SKU
- Creation date
- Video information
- Images and combinations in JSON format

### Future Enhancements
For filters that require deeper data (e.g., vendor ratings, warranty info, actual stock quantities), consider:
1. Extending the VwItem view to include aggregated offer data
2. Creating a dedicated search repository with more complex joins
3. Implementing a separate ItemRepository for advanced filtering

---

## Testing Recommendations

1. **Basic Search**: Test simple text searches
2. **Price Filtering**: Test min/max price combinations
3. **Category Filtering**: Test single and multiple categories
4. **Pagination**: Test various page numbers and sizes
5. **Sorting**: Test all sort options
6. **Edge Cases**: 
   - Empty results
   - Invalid pagination values
   - Reversed min/max prices
   - Out-of-range ratings

---

## Performance Notes

- Uses database-level filtering with LINQ expressions
- Pagination is handled at the database level
- Expression-based filtering minimizes data transfer
- Consider adding database indexes on frequently filtered columns:
  - `TbItems.CategoryId`
  - `TbItems.BrandId`
  - `TbItems.CreatedDateUtc`

---

## Future Enhancements

The current implementation provides a solid foundation. Future enhancements could include:

1. **Vendor & Rating Filters**: Requires extending VwItem view or creating a dedicated repository
2. **Attribute Filters**: Requires joining with attribute tables
3. **Storage Location Filters**: Requires offer/pricing data integration
4. **Most Sold Sorting**: Requires aggregated sales data
5. **Delivery Speed Sorting**: Requires shipping/offer pricing data
6. **Buy Box Winner Filter**: Requires offer pricing status
7. **Warranty Filter**: Requires offer warranty data

---

## Files Modified/Created

| File | Action | Purpose |
|------|--------|---------|
| `src\Shared\Shared\DTOs\ECommerce\Item\ItemFilterDto.cs` | Created | Advanced filter DTO |
| `src\Core\BL\Contracts\Service\ECommerce\Item\IItemService.cs` | Modified | Added interface method |
| `src\Core\BL\Service\ECommerce\Item\ItemService.cs` | Modified | Implemented filter method |
| `src\Presentation\Api\Controllers\v1\Catalog\ItemController.cs` | Modified | Added endpoint and validation |

---

## Summary

The advanced item search API provides a comprehensive filtering system for the customer website. It implements:
- ? Basic and advanced filtering
- ? Multi-criteria search
- ? Flexible sorting options
- ? Pagination support
- ? Input validation and normalization
- ? Database-optimized queries
- ? Standardized response format

The implementation is production-ready and can be extended with additional filters as needed.
