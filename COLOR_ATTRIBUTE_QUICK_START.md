# Color Attribute - Quick Start Guide

## What's New?
A new **Color** attribute type has been added that allows you to:
- Create color-based product variations
- Affect product pricing based on color selection
- Display visual color swatches in the UI

## Quick Setup (5 Steps)

### Step 1: Create a Color Attribute
1. Navigate to **Catalog → Attributes**
2. Click **Add New Attribute**
3. Fill in the details:
   - **Arabic Title**: لون المنتج
   - **English Title**: Product Color
   - **Field Type**: Select **Color**

### Step 2: Add Color Options
1. Click **Add** to create color options
2. For each color:
   - Use the **color picker** to select the color
   - Enter **Arabic name** (e.g., أحمر)
   - Enter **English name** (e.g., Red)
   - The hex code is automatically saved
3. Reorder colors using the up/down arrows

### Step 3: Assign to Category
1. Go to **Catalog → Categories**
2. Edit your category
3. Add the color attribute
4. Set **Affects Pricing** based on your needs:
   - ✅ **Yes** - Different colors have different prices
   - ❌ **No** - All colors have the same price

### Step 4: Use in Products
1. Create or edit a product
2. If **Non-Pricing Color**:
   - Select a color from the dropdown
   - Or use the color picker to choose a custom color
3. If **Pricing Color**:
   - Go to the Combinations tab
   - Create combinations for each color
   - Set different prices for each color

### Step 5: Test It!
- View your product in the dashboard
- Check that colors display correctly
- Verify pricing works as expected

## Example Use Cases

### Case 1: T-Shirt Colors (Same Price)
```
Attribute: T-Shirt Color
Affects Pricing: No
Options:
  - Black (#000000)
  - White (#FFFFFF)
  - Navy (#001F3F)
  - Red (#FF4136)
```
**Result**: Customer can choose any color, price stays the same.

### Case 2: Phone Colors (Different Prices)
```
Attribute: Phone Color
Affects Pricing: Yes
Combinations:
  - Midnight Black (#000000) → $999
  - Rose Gold (#B76E79) → $1,049 (+$50)
  - Ocean Blue (#4A90E2) → $1,029 (+$30)
```
**Result**: Each color has its own price.

## Tips & Tricks

### ✨ Best Practices
- Use standard hex codes (#RRGGBB format)
- Name colors clearly in both languages
- Order colors logically (e.g., light to dark)
- Use color names that match the actual color

### 🎨 Color Naming Ideas
- **Basic**: Red, Blue, Green, Yellow
- **Descriptive**: Midnight Black, Ocean Blue, Rose Gold
- **Branded**: Ferrari Red, Tiffany Blue
- **Natural**: Forest Green, Sky Blue, Sand Beige

### ⚡ Quick Tips
- The color picker and hex input are synchronized
- You can copy/paste hex codes between options
- Color swatches appear automatically in dropdowns
- Preview shows the actual color before saving

## Troubleshooting

**Q: Color picker not showing?**
- Make sure Field Type is set to "Color"
- Check that your browser supports HTML5 color input

**Q: Colors not affecting price?**
- Verify "Affects Pricing" is checked in category attributes
- Create combinations in the product's Combinations tab

**Q: Color swatch not displaying?**
- Ensure the hex code is valid (starts with #)
- Check that the Value field is populated

## Need Help?
Refer to the full documentation in `COLOR_ATTRIBUTE_IMPLEMENTATION.md`
