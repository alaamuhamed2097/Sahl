# Quick Start Guide - Attribute Combinations

## For End Users (Administrators)

### Setting Up Product Variants

#### Step 1: Configure Category
1. Go to **Categories** section
2. Create or edit a category (e.g., "T-Shirts")
3. Add attributes:
   - ? Check "Is Affect Price" for variant attributes (Color, Size)
   - ? Uncheck for common attributes (Brand, Material)
4. Save category

#### Step 2: Create/Edit Product
1. Go to **Products** ? **Add/Edit Product**
2. Fill basic information (name, description, price, etc.)
3. Select **Category** ? Category attributes load automatically
4. Navigate to **Attributes** section

#### Step 3: Select Attribute Values
**For Price-Affecting Attributes**:
- You'll see checkboxes for multiple selection
- Example for "Color": ? Red, ? Blue, ? Green
- Example for "Size": ? M, ? L

**For Non-Price-Affecting Attributes**:
- Fill as normal (single value)

#### Step 4: Generate Combinations
1. Click **"Add Attribute Combinations"** button
2. System generates all combinations automatically
3. A table appears with all combinations

#### Step 5: Configure Each Combination
For each combination row:
- **Price**: Set specific price for this variant
- **Quantity**: Set stock quantity
- **Image**: (Optional) Upload variant-specific image

Example:
```
| Combination  | Price  | Quantity | Image          |
|--------------|--------|----------|----------------|
| Red-M        | $20.00 | 100      | red-m.jpg      |
| Red-L    | $22.00 | 50       | red-l.jpg      |
| Blue-M       | $20.00 | 75       | blue-m.jpg     |
| Blue-L       | $22.00 | 30       | blue-l.jpg     |
```

#### Step 6: Save Product
- Click **Save** button
- All combinations are stored

### Updating Combinations

**To Add New Variant**:
1. Select additional attribute value (e.g., add "Green" color)
2. Click "Add Attribute Combinations"
3. System adds only new combinations (existing prices preserved)

**To Remove Variant**:
1. Uncheck attribute value
2. Regenerate combinations
3. Removed variants disappear from table

### Tips & Best Practices

? **DO**:
- Limit price-affecting attributes to 2-3
- Use meaningful attribute names
- Set realistic stock quantities
- Upload variant images for better customer experience

? **DON'T**:
- Don't mark too many attributes as "affecting price" (causes combination explosion)
- Don't leave prices at $0.00
- Don't forget to set stock quantities

## For Developers

### Quick Integration

The system is already integrated. If you need to customize:

#### Change Combination Display Format
Edit `Details.razor.cs`:
```csharp
private string GetCombinationAttributesDisplay(string attributeIds)
{
  // Current: "Color: Red | Size: M"
    // Modify the format here
    return string.Join(" - ", attributes);  // "Color: Red - Size: M"
}
```

#### Add Validation Rules
Edit `Details.CombinationLogic.cs`:
```csharp
public void GenerateAttributeCombinations()
{
    // Add validation
    if (attributeOptionSets.Count > 3)
    {
        await ShowErrorMessage("Warning", "Too many price-affecting attributes!");
  return;
    }
    
    // Rest of method...
}
```

#### Customize Default Prices
Edit `Details.CombinationLogic.cs`:
```csharp
newCombinations.Add(new ItemAttributeCombinationPricingDto
{
    AttributeIds = attributeIds,
    FinalPrice = Model.Price ?? 0,  // Change this
    Quantity = 10,          // Or this
    Image = null
});
```

### API Endpoint Requirements

Ensure your backend API handles:

**Save Product Request**:
```json
{
  "id": "guid",
  "title": "Product Name",
  "itemAttributes": [
    { "attributeId": "guid1", "value": "Red,Blue" }
  ],
  "itemAttributeCombinationPricings": [
    {
      "attributeIds": "guid1,guid2",
    "finalPrice": 20.00,
      "quantity": 100,
      "image": "base64string"
    }
  ]
}
```

**Load Product Response**:
Same structure as request. System will:
1. Load itemAttributes
2. Load categoryAttributes (from category)
3. Display existing combinations

### Database Schema

Ensure table exists:
```sql
CREATE TABLE TbItemAttributeCombinationPricing (
 Id uniqueidentifier PRIMARY KEY,
    ItemId uniqueidentifier NOT NULL,
    AttributeIds nvarchar(500) NOT NULL,
    FinalPrice decimal(18,2) NOT NULL,
    Quantity int DEFAULT 0,
    Image nvarchar(255),
    CurrentState int DEFAULT 1,
 CreatedDateUtc datetime2 NOT NULL
);
```

## Troubleshooting

### Issue: Combinations Not Generating
**Solution**: 
- Verify attributes have "Is Affect Price" checked in category
- Ensure attribute values are selected
- Check browser console for errors

### Issue: Too Many Combinations
**Problem**: 100+ combinations generated
**Solution**: 
- Reduce number of selected options
- Consider if all attributes need to affect pricing
- Use attribute dependencies (future enhancement)

### Issue: Prices Reset After Update
**Cause**: Attribute IDs changed
**Solution**: 
- Don't regenerate unless necessary
- System preserves matches by AttributeIds string

### Issue: Images Not Saving
**Check**:
- File size < 10MB
- Valid image format (jpg, png, gif)
- Base64 conversion working properly

## Example Scenarios

### Scenario 1: Simple T-Shirt
```
Attributes:
- Color: Red, Blue (affects price)
- Size: S, M, L (affects price)

Combinations: 2 × 3 = 6
```

### Scenario 2: Complex Shoe
```
Attributes:
- Color: Black, Brown, White (affects price)
- Size: 7, 8, 9, 10, 11 (affects price)
- Material: Leather (doesn't affect price)

Combinations: 3 × 5 = 15
```

### Scenario 3: Electronics Bundle
```
Attributes:
- Color: Black, Silver (affects price)
- Storage: 64GB, 128GB, 256GB (affects price)
- Warranty: 1 year, 2 years (affects price)

Combinations: 2 × 3 × 2 = 12
```

## Support

For additional help:
1. Review `ATTRIBUTE_COMBINATION_IMPLEMENTATION.md` for detailed technical documentation
2. Check `IMPLEMENTATION_SUMMARY.md` for overview
3. Contact development team for custom requirements

---

**Quick Reference**:
- ?? Full Documentation: `ATTRIBUTE_COMBINATION_IMPLEMENTATION.md`
- ?? Summary: `IMPLEMENTATION_SUMMARY.md`
- ?? This Guide: `QUICK_START_GUIDE.md`
