# Get Attributes by Category ID API

## Overview
Added a new API endpoint to retrieve all attributes associated with a specific category, including their options and category-specific settings.

## Changes Made

### 1. Interface Update (`IAttributeService.cs`)
**Added Method:**
```csharp
Task<IEnumerable<CategoryAttributeDto>> GetByCategoryIdAsync(Guid categoryId);
```

### 2. Service Implementation (`AttributeService.cs`)
**Implemented Method:**
```csharp
public async Task<IEnumerable<CategoryAttributeDto>> GetByCategoryIdAsync(Guid categoryId)
```

**Features:**
- Validates category ID is not empty
- Fetches `TbCategoryAttribute` records for the category
- Retrieves attribute details with options from `VwAttributeWithOptions` view
- Combines data from both sources into `CategoryAttributeDto`
- Returns attributes ordered by `DisplayOrder`
- Includes:
  - Attribute metadata (Title, FieldType, MaxLength, etc.)
  - Category-specific settings (AffectsPricing, IsRequired, DisplayOrder)
  - Attribute options serialized as JSON

### 3. API Controller Update (`AttributeController.cs`)
**New Endpoint:**
```http
GET /api/Attribute/category/{categoryId}
```

**Authorization:** Requires authentication (`[Authorize]`)

**Response Format:**
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Attributes retrieved successfully",
  "data": [
    {
      "id": "guid",
   "categoryId": "guid",
      "attributeId": "guid",
      "titleAr": "?????",
      "titleEn": "Color",
  "fieldType": 6,
      "isRangeFieldType": false,
      "maxLength": null,
      "affectsPricing": true,
      "isRequired": true,
      "displayOrder": 1,
      "attributeOptionsJson": "[{\"id\":\"guid\",\"titleAr\":\"????\",\"titleEn\":\"Red\",\"displayOrder\":1}]"
    }
  ]
}
```

## Data Flow

```
Client Request
    ?
GET /api/Attribute/category/{categoryId}
    ?
AttributeController.GetByCategory(categoryId)
    ?
AttributeService.GetByCategoryIdAsync(categoryId)
    ?
1. Query TbCategoryAttribute table
   - Filter by categoryId and CurrentState = 1
    ?
2. Extract attribute IDs
    ?
3. Query VwAttributeWithOptions view
   - Get attribute details with options
    ?
4. Map and combine data
   - Create CategoryAttributeDto objects
   - Include AttributeOptions as JSON
    ?
5. Return ordered by DisplayOrder
    ?
Response to Client
```

## Usage Examples

### Frontend/Blazor
```csharp
// In a Blazor component or service
var response = await httpClient.GetFromJsonAsync<ResponseModel<IEnumerable<CategoryAttributeDto>>>(
    $"api/Attribute/category/{categoryId}");

if (response.Success)
{
    var attributes = response.Data;
    // Process attributes...
}
```

### JavaScript/TypeScript
```javascript
const response = await fetch(`/api/Attribute/category/${categoryId}`, {
    headers: {
     'Authorization': `Bearer ${token}`
    }
});

const result = await response.json();
if (result.success) {
    const attributes = result.data;
    // Process attributes...
}
```

### cURL
```bash
curl -X GET "https://yourapi.com/api/Attribute/category/{categoryId}" \
     -H "Authorization: Bearer YOUR_TOKEN"
```

## Response Scenarios

### Success (200 OK)
```json
{
  "success": true,
  "statusCode": 200,
"message": "Attributes retrieved successfully",
  "data": [...]
}
```

### No Data Found (200 OK)
```json
{
  "success": true,
  "statusCode": 200,
  "message": "No data found",
  "data": []
}
```

### Invalid Input (400 Bad Request)
```json
{
  "success": false,
  "statusCode": 400,
  "message": "Invalid input"
}
```

### Unauthorized (401 Unauthorized)
```json
{
  "success": false,
  "statusCode": 401,
  "message": "Unauthorized"
}
```

### Server Error (500 Internal Server Error)
```json
{
  "success": false,
  "statusCode": 500,
  "message": "Error getting attributes for category {categoryId}: {error details}"
}
```

## Benefits

1. **Efficient Data Retrieval**: Single endpoint to get all category attributes with their options
2. **Category-Specific Settings**: Includes AffectsPricing, IsRequired, and DisplayOrder per category
3. **Complete Attribute Data**: Returns full attribute metadata including field type, options, etc.
4. **Ordered Results**: Attributes returned in DisplayOrder for proper UI rendering
5. **Reusable**: Can be used by any client (Blazor, mobile app, external API consumers)

## Use Cases

1. **Product Form**: Load attributes when creating/editing products for a category
2. **Dynamic Forms**: Generate dynamic input fields based on category attributes
3. **Variant Generation**: Identify price-affecting attributes for variant combinations
4. **Validation**: Use IsRequired field to enforce required attributes
5. **Admin Panel**: Display category attributes in admin interfaces

## Related Endpoints

- `GET /api/Attribute` - Get all attributes
- `GET /api/Attribute/{id}` - Get single attribute by ID
- `GET /api/Attribute/search` - Search attributes with pagination
- `GET /api/Category/{id}` - Get category with attributes (alternative approach)

## Testing

### Test Cases
1. ? Get attributes for valid category ID
2. ? Get attributes for category with no attributes
3. ? Get attributes for empty GUID (should return 400)
4. ? Get attributes without authentication (should return 401)
5. ? Get attributes for non-existent category (returns empty list)

### Sample Test
```csharp
[Fact]
public async Task GetByCategory_ValidCategoryId_ReturnsAttributes()
{
    // Arrange
    var categoryId = Guid.NewGuid();
    
    // Act
    var response = await _client.GetAsync($"/api/Attribute/category/{categoryId}");
  
    // Assert
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<ResponseModel<IEnumerable<CategoryAttributeDto>>>();
    Assert.True(result.Success);
}
```

## Files Modified

1. **Interface**: `src\Core\BL\Contracts\Service\ECommerce\Category\IAttributeService.cs`
2. **Service**: `src\Core\BL\Service\ECommerce\Category\AttributeService.cs`
3. **Controller**: `src\Presentation\Api\Controllers\Catalog\AttributeController.cs`

## Build Status
? **Build Successful** - All changes compile without errors

## Next Steps

1. Update Dashboard frontend service to use this new endpoint
2. Add Swagger documentation examples
3. Consider caching results for frequently accessed categories
4. Add unit tests for the new method
5. Update API documentation
