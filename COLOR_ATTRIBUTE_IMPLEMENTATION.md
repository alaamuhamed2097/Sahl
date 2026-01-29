# Color Attribute Type Implementation

## Overview
This document describes the implementation of a new **Color** attribute type that can affect pricing, similar to how List attributes work.

## Changes Made

### 1. Backend Changes

#### 1.1 Enumeration Update
**File:** `src/Shared/Common/Enumerations/FieldType/FieldType.cs`
- Added `Color = 10` to the `FieldType` enum

#### 1.2 Entity Updates
**File:** `src/Core/Domains/Entities/Catalog/Attribute/TbAttributeOption.cs`
- Added `Value` property (string, max 200 chars, nullable) to store color hex codes (e.g., #FF5733)

#### 1.3 DTO Updates
**File:** `src/Shared/Shared/DTOs/Catalog/Category/AttributeDto.cs`
- Updated validation range to include Color field type (1-10)

**File:** `src/Shared/Shared/DTOs/Catalog/Category/AttributeOptionDto.cs`
- Added `Value` property to store color hex codes

#### 1.4 Database Migration
**File:** `src/Infrastructure/DAL/Migrations/20260130001400_AddValueColumnToTbAttributeOption.cs`
- Created migration to add `Value` column to `TbAttributeOption` table
- Column type: `nvarchar(200)`, nullable

### 2. Frontend Changes (Dashboard)

#### 2.1 Attribute Management Page
**File:** `src/Presentation/Dashboard/Pages/Catalog/Attributes/Details.razor`
- Added "Color" option to the field type dropdown
- Updated attribute options section to show color picker and preview for Color type attributes
- Color options display:
  - Color preview swatch (40x40px rounded box)
  - Color picker input
  - Arabic and English title fields

#### 2.2 Product Attribute Values Section
**File:** `src/Presentation/Dashboard/Pages/Catalog/Products/Components/AttributeValuesSection.razor`
- Added color input support for non-pricing Color attributes:
  - Native HTML color picker (80x40px)
  - Text input for hex code (#000000 format)
- Added color preview for dropdown selections:
  - Shows color swatch (30x30px) when color option is selected
  - Displays hex code value

**File:** `src/Presentation/Dashboard/Pages/Catalog/Products/Components/AttributeValuesSection.razor.cs`
- Added Color field type handling in `LoadExistingValues()` method
- Added Color field type handling in `GetAllAttributeData()` method

## How It Works

### Creating a Color Attribute

1. **Navigate to Attributes Management**
   - Go to Catalog → Attributes → Add New Attribute

2. **Configure the Attribute**
   - Enter Arabic and English titles (e.g., "لون المنتج" / "Product Color")
   - Select "Color" from the Field Type dropdown
   - Add color options:
     - Click "Add" to create a new option
     - Use the color picker to select a color
     - Enter Arabic and English names (e.g., "أحمر" / "Red")
     - The hex code is automatically stored in the Value field

3. **Assign to Category**
   - Go to Categories → Edit Category
   - Add the color attribute
   - Set whether it affects pricing (AffectsPricing = true/false)

### Using Color Attributes in Products

#### Non-Pricing Color Attributes
- When creating/editing a product, color attributes appear with:
  - A color picker for direct color selection
  - A text input for manual hex code entry
  - Both inputs are synchronized

#### Pricing Color Attributes (Like List)
- Color attributes with `AffectsPricing = true` work like List attributes
- Each color option can have:
  - Different prices (via combinations)
  - Different stock levels
  - Different SKUs/barcodes
- Color options appear in dropdown with visual indicators
- Selected colors show a preview swatch

## Database Schema

### TbAttributeOption Table
```sql
ALTER TABLE TbAttributeOption
ADD Value NVARCHAR(200) NULL;
```

The `Value` column stores:
- Color hex codes (e.g., #FF5733, #00A8E8)
- Can be extended for other attribute types in the future

## API Compatibility

All existing API endpoints remain compatible:
- `GET /api/v1/Attribute` - Returns attributes with Color type
- `GET /api/v1/Attribute/{id}` - Returns attribute details including color options
- `GET /api/v1/Attribute/category/{categoryId}` - Returns category attributes with colors
- `POST /api/v1/Attribute/save` - Saves color attributes with options

## Migration Instructions

### To Apply Changes:

1. **Build the Solution**
   ```powershell
   dotnet build
   ```

2. **Apply Database Migration**
   ```powershell
   cd src/Infrastructure/DAL
   dotnet ef database update --project ../../../Infrastructure/DAL --startup-project ../../../Presentation/Api
   ```

3. **Restart the Application**
   - Stop the running application
   - Start it again to load the new changes

## Usage Examples

### Example 1: T-Shirt Colors (Non-Pricing)
- Attribute: "T-Shirt Color" (AffectsPricing = false)
- Options:
  - Red (#FF0000)
  - Blue (#0000FF)
  - Green (#00FF00)
- Customer selects color, but price remains the same

### Example 2: Phone Colors (Pricing)
- Attribute: "Phone Color" (AffectsPricing = true)
- Options:
  - Midnight Black (#000000) - Base Price
  - Rose Gold (#B76E79) - +$50
  - Ocean Blue (#4A90E2) - +$30
- Each color creates a different combination with different pricing

## Benefits

1. **Visual Color Selection**: Users can see actual colors instead of just text
2. **Consistent Color Values**: Hex codes ensure accurate color representation
3. **Flexible Pricing**: Colors can affect product pricing like size/material
4. **Better UX**: Color swatches provide better visual feedback
5. **Extensible**: The Value field can be used for other attribute types in the future

## Future Enhancements

Potential improvements:
- RGB/RGBA color support
- Color palette presets
- Color name validation
- Color accessibility checks (contrast ratios)
- Image-based color swatches

## Testing Checklist

- [ ] Create a new Color attribute
- [ ] Add color options with different hex codes
- [ ] Assign color attribute to a category (non-pricing)
- [ ] Create a product with color selection
- [ ] Verify color picker works correctly
- [ ] Verify hex code input works correctly
- [ ] Create pricing color attribute
- [ ] Create product combinations with different colors
- [ ] Verify color swatches display correctly
- [ ] Test API endpoints with color attributes
- [ ] Verify database migration applied successfully

## Support

For issues or questions, please refer to the main project documentation or contact the development team.
