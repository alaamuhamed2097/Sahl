# Advanced Item Search API - Quick Reference Guide

## API Endpoint

```
POST /api/v1/item/search/advanced
```

## Authentication
- **Required**: No (AllowAnonymous)
- **Currency Conversion**: Applied based on user role

## Request Body

```csharp
public class ItemFilterDto
{
    // Item Filters
    public string SearchTerm { get; set; }
    public List<Guid> CategoryIds { get; set; }
    public List<Guid> BrandIds { get; set; }
    
    // Price Range
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    
    // Rating Filters
    public decimal? MinItemRating { get; set; }           // 0-5
    public decimal? MinVendorRating { get; set; }         // 0-5
    
    // Availability
    public bool? InStockOnly { get; set; }
    public int? MinAvailableQuantity { get; set; }
    
    // Shipping
    public bool? FreeShippingOnly { get; set; }
    public int? MaxDeliveryDays { get; set; }
    public List<StorgeLocation> StorageLocations { get; set; }
    
    // Vendor Filters
    public List<string> VendorIds { get; set; }
    public bool? VerifiedVendorsOnly { get; set; }
    public bool? PrimeVendorsOnly { get; set; }
    
    // Offer Filters
    public bool? OnSaleOnly { get; set; }
    public bool? BuyBoxWinnersOnly { get; set; }
    
    // Condition & Warranty
    public List<Guid> ConditionIds { get; set; }
    public bool? WithWarrantyOnly { get; set; }
    
    // Attributes (Color, Size, etc.)
    public Dictionary<Guid, List<Guid>> AttributeValues { get; set; }
    
    // Sorting
    public string SortBy { get; set; }
    // Options: "price_asc", "price_desc", "rating", "newest"
    
    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;  // Max: 100
    
    // All Offers
    public bool ShowAllOffers { get; set; } = false;
}
```

## Sort Options

| Value | Description |
|-------|-------------|
| `price_asc` | Lowest price first |
| `price_desc` | Highest price first |
| `rating` | Highest rating first |
| `newest` | Most recent first (default) |

## Response Format

```json
{
  "success": true,
  "message": "Data retrieved successfully",
  "data": {
    "items": [
      {
        "id": "item-uuid",
        "titleAr": "????? ?????? ????????",
        "titleEn": "Product Title in English",
        "shortDescriptionAr": "...",
        "shortDescriptionEn": "...",
        "categoryId": "category-uuid",
        "categoryTitleAr": "?????",
        "categoryTitleEn": "Category",
        "minimumPrice": 99.99,
        "maximumPrice": 199.99,
        "thumbnailImage": "/images/product.jpg",
        "createdDateUtc": "2024-01-15T10:30:00Z"
      }
    ],
    "totalRecords": 150,
    "pageNumber": 1,
    "pageSize": 20
  }
}
```

## Common Use Cases

### 1. Simple Search
```json
{
  "searchTerm": "iPhone",
  "pageNumber": 1,
  "pageSize": 20
}
```

### 2. Price Range Search
```json
{
  "searchTerm": "laptop",
  "minPrice": 500,
  "maxPrice": 1500,
  "sortBy": "price_asc"
}
```

### 3. Category Filter
```json
{
  "categoryIds": ["category-id-1", "category-id-2"],
  "pageNumber": 1,
  "pageSize": 20
}
```

### 4. Stock Availability
```json
{
  "inStockOnly": true,
  "minAvailableQuantity": 5,
  "sortBy": "newest"
}
```

### 5. Budget Shopping
```json
{
  "maxPrice": 200,
  "inStockOnly": true,
  "freeShippingOnly": true,
  "sortBy": "price_asc"
}
```

### 6. Multi-Brand Search
```json
{
  "brandIds": ["apple-uuid", "samsung-uuid"],
  "minPrice": 300,
  "maxPrice": 1200,
  "sortBy": "rating"
}
```

### 7. Fast Delivery
```json
{
  "maxDeliveryDays": 3,
  "inStockOnly": true,
  "sortBy": "price_asc"
}
```

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 400 | Invalid filter parameters |
| 500 | Server error |

## Validation Rules

- **PageNumber**: Must be ? 1 (auto-corrected to 1 if less)
- **PageSize**: Must be 1-100 (auto-corrected to valid range)
- **MinPrice/MaxPrice**: Auto-swapped if reversed
- **Ratings**: Auto-clamped to 0-5
- **DeliveryDays**: Must be positive (auto-corrected)
- **Quantity**: Must be non-negative (auto-corrected)

## Notes

- All filters are optional
- Leave null/empty to ignore a filter
- Search is case-insensitive
- Pagination is zero-based in calculations but 1-based in API
- Maximum of 100 items per page
- Empty search results return with message "No data found" but success=true

## Integration Example (C# HttpClient)

```csharp
using var client = new HttpClient();
var filter = new ItemFilterDto
{
    SearchTerm = "laptop",
    MinPrice = 500,
    MaxPrice = 1500,
    CategoryIds = new List<Guid> { categoryId },
    SortBy = "price_asc",
    PageNumber = 1,
    PageSize = 20
};

var json = JsonSerializer.Serialize(filter);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(
    "https://api.example.com/api/v1/item/search/advanced",
    content
);

var responseBody = await response.Content.ReadAsStringAsync();
var result = JsonSerializer.Deserialize<ResponseModel<PaginatedDataModel<VwItemDto>>>(responseBody);
```

## Frontend Integration Example (JavaScript/TypeScript)

```javascript
// Fetch with advanced filters
const filters = {
  searchTerm: "laptop",
  minPrice: 500,
  maxPrice: 1500,
  sortBy: "price_asc",
  pageNumber: 1,
  pageSize: 20
};

const response = await fetch('/api/v1/item/search/advanced', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(filters)
});

const data = await response.json();

if (data.success) {
  console.log(`Found ${data.data.totalRecords} items`);
  console.log('Items:', data.data.items);
} else {
  console.error('Search failed:', data.message);
}
```

## Troubleshooting

### Issue: No results found
- Verify filter values are correct
- Check if searchTerm is too specific
- Try removing some filters to expand results

### Issue: Pagination not working
- Ensure PageNumber and PageSize are positive
- Check total records count
- Verify PageNumber doesn't exceed total pages

### Issue: Wrong sort order
- Verify SortBy value spelling (case-sensitive)
- Use exact values: "price_asc", "price_desc", "rating", "newest"

### Issue: Invalid filter error
- Check that all Guid values are valid
- Verify decimal values for price/rating
- Ensure array fields are properly formatted
