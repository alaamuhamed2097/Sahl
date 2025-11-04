# Attribute Combination System - Implementation Guide

## Overview
This document explains the implementation of an automatic attribute combination generation system for product variants in your Blazor e-commerce application. The system automatically generates all possible combinations of price-affecting attributes using a Cartesian product algorithm.

## Key Features

### 1. **Automatic Combination Generation**
- The system automatically generates all possible combinations of attributes marked as "Affecting Price" (IsAffectingPrice = true)
- Uses Cartesian product algorithm to create combinations
- Preserves existing pricing data when regenerating combinations

### 2. **Multi-Select Support for Price-Affecting Attributes**
- Price-affecting attributes with List or MultiSelectList types now support multiple selections
- Users can select multiple options (e.g., Red, Blue, Green for Color attribute)
- Each selected option creates a separate variant combination

### 3. **Real-Time Updates**
- Combinations are regenerated automatically when:
  - Category changes
  - Attribute values change
  - "Generate Combinations" button is clicked

## Implementation Details

### Files Created/Modified

#### 1. **ItemAttributesSection.razor & .cs**
Location: `src\Presentation\Dashboard\Pages\Catalog\Products\Components\`

**Purpose**: Enhanced component for managing product attributes with multi-select support

**Key Features**:
- Multi-checkbox selection for price-affecting list attributes
- Visual indicator (badge) showing which attributes affect pricing
- "Generate Combinations" button for manual trigger
- Real-time value change detection

**Usage Example**:
```razor
<ItemAttributesSection 
    CategoryAttributes="categoryAttributes"
    ItemAttributes="Model.ItemAttributes"
    OnRemoveAttribute="RemoveAttribute"
    OnGenerateCombinations="GenerateAttributeCombinations"
    OnAttributeValueChanged="StateHasChanged" />
```

#### 2. **Details.CombinationLogic.cs**
Location: `src\Presentation\Dashboard\Pages\Catalog\Products\`

**Purpose**: Contains the core combination generation logic

**Key Methods**:

##### `GenerateAttributeCombinations()`
- Main method that orchestrates combination generation
- Filters price-affecting attributes
- Handles different field types (List, MultiSelectList, Text, Number, etc.)
- Creates or preserves existing combinations

##### `GenerateCartesianProduct()`
- Core algorithm for generating all possible combinations
- Takes a list of attribute option sets
- Returns all possible combinations using Cartesian product

##### `GetCombinationAttributesDisplay()`
- Formats combination display strings
- Handles both option IDs and direct attribute values
- Shows "AttributeName: Value" format

### 3. **Details.razor.cs (Modified)**
Location: `src\Presentation\Dashboard\Pages\Catalog\Products\`

**Changes**:
- Updated `HandleCategoryChange()` to call `HandleCategoryChangeWithCombinations()`
- Updated `LoadProduct()` to load category attributes when loading existing products
- Enhanced `GetCombinationAttributesDisplay()` to support both option IDs and attribute IDs

### 4. **Details.razor (Modified)**
Location: `src\Presentation\Dashboard\Pages\Catalog\Products\`

**Changes**:
- Integrated the new `ItemAttributesSection` component
- Added combination table display with editable pricing and quantities
- Shows combination count badge
- Allows per-combination image upload

## How It Works

### Workflow Example:

**Category Setup**:
```
Category: T-Shirts
Attributes:
  - Color (List, AffectsPricing=true): Red, Blue, Green
  - Size (List, AffectsPricing=true): S, M, L, XL
  - Brand (Text, AffectsPricing=false): Nike
```

**User Interaction**:
1. User selects category "T-Shirts"
2. System loads category attributes
3. User selects multiple colors: Red, Blue, Green
4. User selects multiple sizes: M, L
5. User clicks "Generate Combinations"

**System Response**:
```
Generated Combinations:
1. Red-M    ? Price: $20.00, Qty: 0
2. Red-L    ? Price: $20.00, Qty: 0
3. Blue-M   ? Price: $20.00, Qty: 0
4. Blue-L   ? Price: $20.00, Qty: 0
5. Green-M  ? Price: $20.00, Qty: 0
6. Green-L  ? Price: $20.00, Qty: 0
```

**Total combinations** = 3 colors × 2 sizes = **6 combinations**

### Algorithm Flow:

```csharp
// Step 1: Get price-affecting attributes
var priceAffectingAttributes = categoryAttributes
    .Where(ca => ca.AffectsPricing)
    .ToList();

// Step 2: Build option sets for each attribute
attributeOptionSets = [
    [ {Red}, {Blue}, {Green} ],  // Color options
    [ {M}, {L} ]   // Size options
]

// Step 3: Generate Cartesian product
result = [
    [Red, M], [Red, L],
    [Blue, M], [Blue, L],
    [Green, M], [Green, L]
]

// Step 4: Create pricing entries
foreach combination:
create ItemAttributeCombinationPricingDto with:
      - AttributeIds: "Red-M" (comma-separated IDs)
        - FinalPrice: default from Model.Price
        - Quantity: 0
        - Image: null
```

## Data Structure

### ItemAttributeCombinationPricingDto
```csharp
public class ItemAttributeCombinationPricingDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string AttributeIds { get; set; }  // Comma-separated GUIDs
    public decimal FinalPrice { get; set; }
    public int Quantity { get; set; }
    public string? Image { get; set; }        // Base64 or path
}
```

### CategoryAttributeDto
```csharp
public class CategoryAttributeDto
{
    public Guid Id { get; set; }
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public bool AffectsPricing { get; set; }     // Key flag
    public bool IsRequired { get; set; }
    public FieldType FieldType { get; set; }
    public List<AttributeOptionDto> AttributeOptions { get; set; }
}
```

## Database Schema

### TbItemAttributeCombinationPricing
```sql
CREATE TABLE TbItemAttributeCombinationPricing (
    Id uniqueidentifier PRIMARY KEY,
    ItemId uniqueidentifier NOT NULL,
    AttributeIds nvarchar(500) NOT NULL,     -- "guid1,guid2,guid3"
    FinalPrice decimal(18,2) NOT NULL,
    Quantity int DEFAULT 0,
    Image nvarchar(255) NULL,
    CurrentState int DEFAULT 1,
    CreatedDateUtc datetime2 NOT NULL,
    FOREIGN KEY (ItemId) REFERENCES TbItem(Id) ON DELETE CASCADE
);
```

## Usage Guide

### For Administrators:

1. **Configure Category Attributes**:
   - Go to Categories management
   - Add/edit a category
   - Add attributes and mark which ones affect pricing
   - Example: Color and Size should affect pricing

2. **Create/Edit Product**:
   - Select category
   - System loads category attributes
   - Fill in attribute values (select multiple for price-affecting attributes)
   - Click "Generate Combinations" button
   - System creates all combinations
   - Edit price and quantity for each combination
   - Optionally upload image for each combination

3. **Update Combinations**:
   - Change attribute values
   - Click "Generate Combinations" again
   - System updates combinations (preserving existing prices where possible)

### For Developers:

#### Adding New Field Types:
To support additional field types in combinations, update `GenerateAttributeCombinations()`:

```csharp
// In Details.CombinationLogic.cs
else if (attr.FieldType == FieldType.YourNewType)
{
    // Handle your new field type
    // Extract options/values
    // Add to options list
}
```

#### Customizing Combination Display:
Modify `GetCombinationAttributesDisplay()` to change how combinations are shown:

```csharp
// Current format: "Color: Red | Size: M"
// Custom format example:
attributes.Add($"{attribute.Title}={value}");
return string.Join("&", attributes);  // "Color=Red&Size=M"
```

## Performance Considerations

### Combination Explosion:
Be aware of combinations growing exponentially:
- 3 colors × 3 sizes = 9 combinations ?
- 5 colors × 4 sizes × 3 materials = 60 combinations ?
- 10 colors × 5 sizes × 4 materials × 3 styles = 600 combinations ?

**Recommendations**:
1. Limit price-affecting attributes to 2-3 maximum
2. Consider attribute dependency (e.g., some colors only available in certain sizes)
3. Implement combination limits (max 50-100 combinations per product)

### Future Enhancements:

1. **Conditional Combinations**:
   ```csharp
   // Only generate valid combinations
   if (color == "Red" && size == "XL") 
       continue; // Skip invalid combination
   ```

2. **Bulk Price Updates**:
   ```csharp
   // Apply same price adjustment to all combinations
   foreach (var combo in combinations)
       combo.FinalPrice *= 1.10; // 10% increase
   ```

3. **Import/Export**:
   - Export combinations to CSV/Excel
   - Import pricing from spreadsheet

4. **Stock Synchronization**:
   - Sync combination quantities with inventory system
   - Alert when combination stock is low

## Troubleshooting

### Common Issues:

1. **Combinations not generating**:
   - Verify attributes are marked with `AffectsPricing = true`
   - Ensure attribute values are selected
   - Check that `GenerateAttributeCombinations()` is being called

2. **Duplicate combinations**:
   - System checks for existing combinations by AttributeIds
   - Ensure AttributeIds are consistently formatted (comma-separated, no spaces)

3. **Lost pricing data**:
   - System preserves existing combinations when regenerating
   - Only adds new combinations, doesn't overwrite existing prices

## Testing Checklist

- [ ] Create product with 2 price-affecting attributes
- [ ] Verify combinations generate correctly
- [ ] Test combination count calculation
- [ ] Edit attribute values and regenerate
- [ ] Verify existing prices are preserved
- [ ] Test with different field types (List, Text, Number)
- [ ] Test multi-select for price-affecting attributes
- [ ] Upload images for specific combinations
- [ ] Save product and verify combinations are stored
- [ ] Load existing product and verify combinations display correctly
- [ ] Test removing attributes
- [ ] Test changing category

## API Integration Notes

The system integrates with your existing services:
- `ICategoryService` - Loads category attributes
- `IItemService` - Saves product with combinations
- Backend should handle JSON serialization of AttributeIds

Ensure your backend service properly processes the `ItemAttributeCombinationPricings` collection when saving items.

## Conclusion

This implementation provides a robust, user-friendly system for managing product variants through automatic combination generation. It handles the complexity of Cartesian products while providing flexibility for manual adjustments and supporting multiple field types and selection modes.
