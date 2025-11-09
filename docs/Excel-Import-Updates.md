# ??????? ???? Excel Import - Empty Template & Enhanced Validation

## ? ????????? ???????

### 1. **?? Empty Template**
??? ????? ???? ?????? Template ???? ???? ????? ??????:

```razor
<!-- Download Template with Data -->
<button class="btn btn-primary" @onclick="GenerateTemplateWithData">
    Download Template (With Reference Data)
</button>

<!-- Download Empty Template -->
<button class="btn btn-outline-primary" @onclick="GenerateEmptyTemplate">
    Download Empty Template
</button>
```

#### ????? ??? ???????:

| Feature | Template with Data | Empty Template |
|---------|-------------------|----------------|
| ???? Products | ? | ? |
| ???? Categories | ? | ? |
| ???? Brands | ? | ? |
| ???? Units | ? | ? |
| ???? AttributeOptions | ? | ? |
| ???? Instructions | ? | ? |
| ??? ????? | ???? | ???? |
| ????? ????????? | ??? - ???? ?????? ?????? | ????? - ????? ????? ??? IDs |

### 2. **Enhanced Validation**

#### ?. Validation ??? ????? ???? (Row-Level)
```csharp
private List<string> GetRowValidationErrors(ImportedProductDto product)
{
    var errors = new List<string>();

    // Required fields
    if (string.IsNullOrWhiteSpace(product.TitleEn))
        errors.Add("Title (EN) is required");
    
    // Category validation
    if (product.CategoryId == Guid.Empty)
        errors.Add("Category ID is required");
    else if (!categories.Any(c => c.Id == product.CategoryId))
        errors.Add("Invalid Category ID - Category does not exist");

    // Brand validation
    if (!brands.ContainsKey(product.BrandId))
        errors.Add("Invalid Brand ID - Brand does not exist");

    // Unit validation
    if (!units.ContainsKey(product.UnitId))
        errors.Add("Invalid Unit ID - Unit does not exist");

    // Price validation
    if (product.Price <= 0)
        errors.Add("Price must be greater than 0");

    // Quantity validation
    if (product.Quantity < 0)
        errors.Add("Quantity cannot be negative");

    // Validate pricing attribute options exist
    foreach (var pricingAttr in product.PricingAttributeValues)
    {
        var attribute = categoryAttributes.FirstOrDefault(a => a.AttributeId == pricingAttr.Key);
        foreach (var optionId in pricingAttr.Value)
        {
            if (attribute.AttributeOptions == null || 
                !attribute.AttributeOptions.Any(o => o.Id == optionId))
            {
                errors.Add($"Invalid option ID {optionId} for attribute {attribute.TitleEn}");
            }
        }
    }

    return errors;
}
```

#### ?. Validation ???? ??? Parsing
```csharp
public List<ImportedProductDto> ParseProductsFromExcel(Stream fileStream, List<CategoryAttributeDto> categoryAttributes)
{
    // Validate required columns exist
    ValidateRequiredColumns(columnMap);

    // Validate each row
    if (string.IsNullOrWhiteSpace(product.TitleEn))
        throw new Exception($"Row {row.RowNumber()}: TitleEn is required");
    
    if (product.CategoryId == Guid.Empty)
        throw new Exception($"Row {row.RowNumber()}: Valid CategoryId is required");

    if (product.Price <= 0)
        throw new Exception($"Row {row.RowNumber()}: Price must be greater than 0");

    // Validate GUID format for pricing attributes
    var optionIds = value.Split(',')
        .Select(s => 
        {
            if (!Guid.TryParse(s, out var guid))
                throw new Exception($"Row {row.RowNumber()}: Invalid GUID '{s}'");
            return guid;
        })
        .ToList();
}
```

### 3. **Visual Validation Feedback**

#### ???? ???????? ???????:
```razor
<table class="table table-striped table-hover">
    <thead>
        <tr>
            <th>#</th>
            <th>Title (EN)</th>
            <th>Category</th>
            <th>Price</th>
            <th>Quantity</th>
            <th>Stock</th>
            <th>Attributes</th>
            <th>Pricing Combos</th>
            <th>Status</th> <!-- ???? ???? -->
            <th>Action</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var product in importedProducts)
        {
            var rowErrors = GetRowValidationErrors(product);
            var hasErrors = rowErrors.Any();
            <tr class="@(hasErrors ? "table-danger" : "")">
                <!-- ... -->
                <td>
                    @if (hasErrors)
                    {
                        <span class="badge bg-danger" title="@string.Join(", ", rowErrors)">
                            <i class="fas fa-times me-1"></i>
                            @rowErrors.Count errors
                        </span>
                    }
                    else
                    {
                        <span class="badge bg-success">
                            <i class="fas fa-check"></i>
                            Valid
                        </span>
                    }
                </td>
            </tr>
        }
    </tbody>
</table>
```

#### Alert ??? Errors:
```razor
@if (validationErrors.Any())
{
    <div class="alert alert-danger">
        <h6><i class="fas fa-times-circle me-2"></i>Validation Errors Found:</h6>
        <p class="mb-2">Please fix these errors before importing:</p>
        <ul class="mb-0">
            @foreach (var error in validationErrors.Take(20))
            {
                <li>@error</li>
            }
        </ul>
        @if (validationErrors.Count > 20)
        {
            <p class="mb-0 mt-2 text-muted">... and @(validationErrors.Count - 20) more errors</p>
        }
    </div>
}
```

### 4. **??? ????????? ??? ???? ?????**

```razor
<button class="btn btn-success" 
        @onclick="ImportProducts" 
        disabled="@(isImporting || importedProducts.Count == 0 || validationErrors.Any())">
    @if (isImporting)
    {
        <span class="spinner-border spinner-border-sm me-2"></span>
        <text>Importing @importProgress%</text>
    }
    else
    {
        <i class="fas fa-upload me-2"></i>
        <text>Import @importedProducts.Count Products</text>
    }
</button>

@if (validationErrors.Any())
{
    <div class="alert alert-warning mt-3">
        <i class="fas fa-exclamation-triangle me-2"></i>
        <strong>Cannot import:</strong> Please fix all validation errors first or remove invalid rows.
    </div>
}
```

## ?? ????? ??? Validation

### 1. **Required Fields Validation**
- TitleAr/TitleEn
- ShortDescriptionAr/ShortDescriptionEn
- DescriptionAr/DescriptionEn
- CategoryId
- BrandId
- UnitId
- Price
- Quantity
- StockStatus

### 2. **Data Type Validation**
- GUIDs must be valid format
- Price must be decimal > 0
- Quantity must be integer >= 0
- Boolean fields (True/False only)

### 3. **Reference Validation**
- CategoryId must exist in categories
- BrandId must exist in brands
- UnitId must exist in units
- Attribute Option IDs must exist in attribute options

### 4. **Business Logic Validation**
- Price > 0
- Quantity >= 0
- Pricing attribute combinations must be valid

### 5. **Column Structure Validation**
```csharp
private void ValidateRequiredColumns(Dictionary<string, int> columnMap)
{
    var requiredColumns = new List<string>
    {
        "TitleAr*",
        "TitleEn*",
        "CategoryId*",
        // ... etc
    };

    var missingColumns = requiredColumns.Where(col => !columnMap.ContainsKey(col)).ToList();
    
    if (missingColumns.Any())
    {
        throw new Exception($"Missing required columns: {string.Join(", ", missingColumns)}");
    }
}
```

## ?? ???? ??? Error Messages

### ?? ????? Parsing:
```
Error parsing row 5: TitleEn is required
Error parsing row 8: Invalid GUID 'abc-123' for pricing attribute Color
Error parsing row 12: Price must be greater than 0
Missing required columns: CategoryId*, BrandId*
```

### ?? ????? Review:
```
Row 3: Title (EN) is required
Row 5: Invalid Category ID - Category does not exist
Row 7: Price must be greater than 0
Row 9: Invalid option ID guid-xxx for attribute Size
```

## ?? User Experience Flow

### 1. **Select Category**
```
Choose Category ? See 2 buttons:
- Download Template (With Reference Data) [Primary]
- Download Empty Template [Secondary]
```

### 2. **Download Template**
```
Template with Data:
? 6 sheets (Products + 5 reference sheets)
? Pre-filled reference data
? Easy to use

Empty Template:
? 2 sheets (Products + Instructions)
? Smaller file size
? For advanced users
```

### 3. **Upload File**
```
Validation:
? File size < 10MB
? .xlsx format only
? File name displayed
```

### 4. **Review Data**
```
Table View:
? Color-coded rows (red = errors, normal = valid)
? Status badges (? Valid, ? X errors)
? Detailed error messages
? Category/Brand/Unit name display
? Combination count display

Actions:
? Remove invalid rows
? Fix in Excel and re-upload
? Import button disabled if errors exist
```

### 5. **Import**
```
Progress:
? Real-time progress percentage
? Row-by-row status updates
? Success/failure count
? Detailed error messages for failed rows
```

## ?? ????? ??????

### 1. **Smart Validation**
- ????? ?? ???? ??? IDs ??? ?????????
- ???? ????? ??? ??? ?????
- ???? ????? ??? ?????

### 2. **Better UX**
- ???? ?????? ??? Template types
- Alert boxes ???????
- Color coding ?????? ???????
- Tooltips ??? errors

### 3. **Performance**
- Empty template ??? ?????
- Validation ???? ???? ??? parsing
- Progress indicator real-time

## ?? ??????? ????

?? **??????????:**
1. ?????? **Template with Data** ??? ??? ?????
2. ?????? **Empty Template** ??? ??? ???? ??? IDs
3. ???? **Instructions sheet** ?? ?? ???????
4. **?? ???? ?????????** ??? ??? ???? ????? validation

? **????????:**
1. ?? ??? validation methods ?????? ?? `Import.razor` ? `ExcelTemplateService.cs`
2. ??? validation ???? ?? ???????: Parsing + Review
3. ??? UI ???? ??????? ???? ????
4. ???? Import ?????? ???????? ??? ???? ?????

## ?? ???????

???? ??????:
- ? ???? 2 ??? ?? ??? Templates
- ? ???? ????? ?????? ?????
- ? ???? ????? ??? ?????
- ? ???? visual feedback ????
- ? ???? ????????? ??? ???? ?????
- ? ???? validation ????
