using ClosedXML.Excel;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.Category;

namespace Dashboard.Services
{
    /// <summary>
    /// Service for generating and processing Excel templates for product import
    /// </summary>
    public class ExcelTemplateService
    {
        /// <summary>
        /// Generates an empty Excel template (no reference sheets) for product import
        /// </summary>
        public byte[] GenerateEmptyProductTemplate(
            List<CategoryDto> categories,
            List<CategoryAttributeDto> categoryAttributes)
        {
            using var workbook = new XLWorkbook();

            // Sheet 1: Products (with example row)
            var productsSheet = workbook.Worksheets.Add(ECommerceResources.Products);
            GenerateProductsSheet(productsSheet, categoryAttributes);

            // Sheet 2: Instructions ONLY
            var instructionsSheet = workbook.Worksheets.Add("Instructions");
            GenerateInstructionsSheet(instructionsSheet);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Generates an Excel template for product import with dynamic attribute columns
        /// </summary>
        public byte[] GenerateProductTemplate(
            List<CategoryDto> categories,
            List<CategoryAttributeDto> categoryAttributes,
            Dictionary<Guid, string> brands,
            Dictionary<Guid, string> units)
        {
            using var workbook = new XLWorkbook();

            // Sheet 1: Products
            var productsSheet = workbook.Worksheets.Add(ECommerceResources.Products);
            GenerateProductsSheet(productsSheet, categoryAttributes, brands, units, categories);

            // Sheet 2: Categories (Reference)
            var categoriesSheet = workbook.Worksheets.Add(ECommerceResources.Categories);
            GenerateCategoriesSheet(categoriesSheet, categories);

            // Sheet 3: Brands (Reference)
            var brandsSheet = workbook.Worksheets.Add(BrandResources.Brands);
            GenerateBrandsSheet(brandsSheet, brands);

            // Sheet 4: Units (Reference)
            var unitsSheet = workbook.Worksheets.Add(ECommerceResources.Units);
            GenerateUnitsSheet(unitsSheet, units);

            // Sheet 5: Attribute Options (Reference)
            var optionsSheet = workbook.Worksheets.Add("Attribute Options");
            GenerateAttributeOptionsSheet(optionsSheet, categoryAttributes);

            // Sheet 6: Instructions
            var instructionsSheet = workbook.Worksheets.Add("Instructions");
            GenerateInstructionsSheet(instructionsSheet);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Generates the main Products sheet with base columns and dynamic attribute columns
        /// </summary>
        private void GenerateProductsSheet(
            IXLWorksheet sheet,
            List<CategoryAttributeDto> attributes,
            Dictionary<Guid, string>? brands = null,
            Dictionary<Guid, string>? units = null,
            List<CategoryDto>? categories = null)
        {
            int col = 1;

            // Base product fields
            var headers = new List<(string key, string text, int colIndex)>();

            // Add base headers
            headers.Add(("TitleAr*", $"{FormResources.Title} ({GeneralResources.Arabic})*", col++));
            headers.Add(("TitleEn*", $"{FormResources.Title} ({GeneralResources.English})*", col++));
            headers.Add(("ShortDescriptionAr*", $"{FormResources.ShortDescription} ({GeneralResources.Arabic})*", col++));
            headers.Add(("ShortDescriptionEn*", $"{FormResources.ShortDescription} ({GeneralResources.English})*", col++));
            headers.Add(("DescriptionAr*", $"{FormResources.Description} ({GeneralResources.Arabic})*", col++));
            headers.Add(("DescriptionEn*", $"{FormResources.Description} ({GeneralResources.English})*", col++));

            int categoryCol = col;
            headers.Add(("CategoryId*", $"{ECommerceResources.Category}*", col++));

            int brandCol = col;
            headers.Add(("BrandId*", $"{BrandResources.Brand}*", col++));

            int unitCol = col;
            headers.Add(("UnitId*", $"{ECommerceResources.Unit}*", col++));

            headers.Add(("Price*", $"{FormResources.Price}*", col++));
            headers.Add(("Quantity*", $"{ECommerceResources.Quantity}*", col++));

            int stockStatusCol = col;
            headers.Add(("StockStatus*", $"{ECommerceResources.StockStatus}*", col++));

            int isNewArrivalCol = col;
            headers.Add(("IsNewArrival", "Is New Arrival", col++));

            int isBestSellerCol = col;
            headers.Add(("IsBestSeller", "Is Best Seller", col++));

            int isRecommendedCol = col;
            headers.Add(("IsRecommended", "Is Recommended", col++));

            headers.Add(("ThumbnailImagePath", "Thumbnail Image", col++));
            headers.Add(("ImagePath1", $"{FormResources.Image} 1", col++));
            headers.Add(("ImagePath2", $"{FormResources.Image} 2", col++));
            headers.Add(("ImagePath3", $"{FormResources.Image} 3", col++));
            headers.Add(("ImagePath4", $"{FormResources.Image} 4", col++));
            headers.Add(("ImagePath5", $"{FormResources.Image} 5", col++));

            // Create headers
            foreach (var (key, text, colIndex) in headers)
            {
                var cell = sheet.Cell(1, colIndex);
                cell.Value = text;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                cell.Style.Alignment.WrapText = true;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            // Add attribute columns (non-pricing affecting attributes)
            var nonPricingAttributes = attributes.Where(a => !a.AffectsPricing).ToList();
            foreach (var attr in nonPricingAttributes)
            {
                var cell = sheet.Cell(1, col);
                cell.Value = $"{attr.Title}";
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                cell.Style.Alignment.WrapText = true;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Add dropdown for attribute options
                if (attr.AttributeOptions != null && attr.AttributeOptions.Any())
                {
                    AddAttributeOptionsDropdown(sheet, col, attr.AttributeOptions);
                }
                col++;
            }

            // Add pricing-affecting attributes columns
            var pricingAttributes = attributes.Where(a => a.AffectsPricing).ToList();
            foreach (var attr in pricingAttributes)
            {
                var cell = sheet.Cell(1, col);
                cell.Value = $"{attr.Title}* (Pricing Attribute)";
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightYellow;
                cell.Style.Alignment.WrapText = true;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Note: Pricing attributes allow multiple selections (comma-separated)
                if (attr.AttributeOptions != null && attr.AttributeOptions.Any())
                {
                    var comment = cell.CreateComment();
                    comment.Style.Size.SetAutomaticSize();
                    var optionsText = string.Join("\n", attr.AttributeOptions.Select(o => $"• {o.Title}"));
                    comment.AddText($"Enter multiple values separated by comma:\n{optionsText}");
                }
                col++;
            }

            // Apply dropdowns AFTER all columns are created
            if (categories != null && categories.Any())
            {
                AddCategoryDropdown(sheet, categoryCol, categories);
            }

            if (brands != null && brands.Any())
            {
                AddBrandDropdown(sheet, brandCol, brands);
            }

            if (units != null && units.Any())
            {
                AddUnitDropdown(sheet, unitCol, units);
            }

            // Add boolean dropdowns
            AddBooleanDropdown(sheet, stockStatusCol);
            AddBooleanDropdown(sheet, isNewArrivalCol);
            AddBooleanDropdown(sheet, isBestSellerCol);
            AddBooleanDropdown(sheet, isRecommendedCol);

            // Set column widths
            sheet.Column(1).Width = 25; // TitleAr
            sheet.Column(2).Width = 25; // TitleEn
            sheet.Column(3).Width = 30; // ShortDescriptionAr
            sheet.Column(4).Width = 30; // ShortDescriptionEn
            sheet.Column(5).Width = 35; // DescriptionAr
            sheet.Column(6).Width = 35; // DescriptionEn
            sheet.Column(7).Width = 20; // Category
            sheet.Column(8).Width = 20; // Brand
            sheet.Column(9).Width = 15; // Unit
            sheet.Column(10).Width = 12; // Price
            sheet.Column(11).Width = 12; // Quantity
            sheet.Column(12).Width = 15; // StockStatus

            // Freeze header row
            sheet.SheetView.FreezeRows(1);

            // Add example row with instructions (row 2)
            AddExampleRow(sheet, categories?.FirstOrDefault(), brands, units);
        }

        /// <summary>
        /// Adds dropdown validation for Categories
        /// </summary>
        private void AddCategoryDropdown(IXLWorksheet sheet, int column, List<CategoryDto> categories)
        {
            var finalCategories = categories.Where(c => c.IsFinal).ToList();
            if (!finalCategories.Any()) return;

            // Use simple string list for validation (max 255 chars)
            var categoryNames = finalCategories.Select(c => c.Title).ToList();
            var validationList = string.Join(",", categoryNames.Take(Math.Min(categoryNames.Count, 20)));

            // Apply validation to range
            var range = sheet.Range(2, column, 1048576, column);
            range.CreateDataValidation().List(validationList, true);
        }

        /// <summary>
        /// Adds dropdown validation for Brands
        /// </summary>
        private void AddBrandDropdown(IXLWorksheet sheet, int column, Dictionary<Guid, string> brands)
        {
            if (!brands.Any()) return;

            var brandList = string.Join(",", brands.Values.Take(Math.Min(brands.Count, 20)));
            var range = sheet.Range(2, column, 1048576, column);
            range.CreateDataValidation().List(brandList, true);
        }

        /// <summary>
        /// Adds dropdown validation for Units
        /// </summary>
        private void AddUnitDropdown(IXLWorksheet sheet, int column, Dictionary<Guid, string> units)
        {
            if (!units.Any()) return;

            var unitList = string.Join(",", units.Values.Take(Math.Min(units.Count, 20)));
            var range = sheet.Range(2, column, 1048576, column);
            range.CreateDataValidation().List(unitList, true);
        }

        /// <summary>
        /// Adds dropdown validation for Boolean fields (True/False)
        /// </summary>
        private void AddBooleanDropdown(IXLWorksheet sheet, int column)
        {
            var range = sheet.Range(2, column, 1048576, column);
            range.CreateDataValidation().List("True,False", true);
        }

        /// <summary>
        /// Adds dropdown validation for Attribute Options
        /// </summary>
        private void AddAttributeOptionsDropdown(IXLWorksheet sheet, int column, List<AttributeOptionDto> options)
        {
            if (!options.Any()) return;

            var optionList = string.Join(",", options.Select(o => o.Title).Take(Math.Min(options.Count, 20)));
            var range = sheet.Range(2, column, 1048576, column);
            range.CreateDataValidation().List(optionList, true);
        }

        /// <summary>
        /// Helper method to convert column number to Excel column letter
        /// </summary>
        private string GetColumnLetter(int columnNumber)
        {
            string columnLetter = string.Empty;

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnLetter = Convert.ToChar('A' + modulo) + columnLetter;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnLetter;
        }

        /// <summary>
        /// Adds example row with sample data
        /// </summary>
        private void AddExampleRow(IXLWorksheet sheet, CategoryDto? category, Dictionary<Guid, string>? brands, Dictionary<Guid, string>? units)
        {
            int row = 2;
            int col = 1;

            // Style example row differently
            var exampleRange = sheet.Range(row, 1, row, 21);
            exampleRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
            exampleRange.Style.Font.Italic = true;

            sheet.Cell(row, col++).Value = "منتج تجريبي";
            sheet.Cell(row, col++).Value = "Example Product";
            sheet.Cell(row, col++).Value = "وصف قصير للمنتج";
            sheet.Cell(row, col++).Value = "Short description in English";
            sheet.Cell(row, col++).Value = "وصف كامل للمنتج التجريبي";
            sheet.Cell(row, col++).Value = "Full description in English";

            // Category - use name instead of ID
            if (category != null)
            {
                sheet.Cell(row, col++).Value = category.Title;
            }
            else
            {
                sheet.Cell(row, col++).Value = "Select Category";
            }

            // Brand - use name instead of ID
            if (brands != null && brands.Any())
            {
                sheet.Cell(row, col++).Value = brands.Values.First();
            }
            else
            {
                sheet.Cell(row, col++).Value = "Select Brand";
            }

            // Unit - use name instead of ID
            if (units != null && units.Any())
            {
                sheet.Cell(row, col++).Value = units.Values.First();
            }
            else
            {
                sheet.Cell(row, col++).Value = "Select Unit";
            }

            sheet.Cell(row, col++).Value = 100.00;
            sheet.Cell(row, col++).Value = 50;
            sheet.Cell(row, col++).Value = "True";
            sheet.Cell(row, col++).Value = "False";
            sheet.Cell(row, col++).Value = "False";
            sheet.Cell(row, col++).Value = "False";
            sheet.Cell(row, col++).Value = "/images/products/example.jpg";

            // Add note to first cell of example row
            var firstCell = sheet.Cell(row, 1);
            var comment = firstCell.CreateComment();
            comment.Style.Size.SetAutomaticSize();
            comment.AddText("⚠️ DELETE THIS ROW BEFORE IMPORTING\nThis is just an example to show the format.");
        }

        /// <summary>
        /// Generates Categories reference sheet
        /// </summary>
        private void GenerateCategoriesSheet(IXLWorksheet sheet, List<CategoryDto> categories)
        {
            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = $"{FormResources.Title} ({GeneralResources.Arabic})";
            sheet.Cell(1, 3).Value = $"{FormResources.Title} ({GeneralResources.English})";
            sheet.Cell(1, 4).Value = ECommerceResources.IsFinal;

            sheet.Range(1, 1, 1, 4).Style.Font.Bold = true;
            sheet.Range(1, 1, 1, 4).Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var category in categories.Where(c => c.IsFinal))
            {
                sheet.Cell(row, 1).Value = category.Id.ToString();
                sheet.Cell(row, 2).Value = category.TitleAr;
                sheet.Cell(row, 3).Value = category.TitleEn;
                sheet.Cell(row, 4).Value = category.IsFinal ? GeneralResources.Yes : GeneralResources.No;
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// Generates Brands reference sheet
        /// </summary>
        private void GenerateBrandsSheet(IXLWorksheet sheet, Dictionary<Guid, string> brands)
        {
            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = BrandResources.BrandName;

            sheet.Range(1, 1, 1, 2).Style.Font.Bold = true;
            sheet.Range(1, 1, 1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var brand in brands)
            {
                sheet.Cell(row, 1).Value = brand.Key.ToString();
                sheet.Cell(row, 2).Value = brand.Value;
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// Generates Units reference sheet
        /// </summary>
        private void GenerateUnitsSheet(IXLWorksheet sheet, Dictionary<Guid, string> units)
        {
            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = "Unit Name";

            sheet.Range(1, 1, 1, 2).Style.Font.Bold = true;
            sheet.Range(1, 1, 1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var unit in units)
            {
                sheet.Cell(row, 1).Value = unit.Key.ToString();
                sheet.Cell(row, 2).Value = unit.Value;
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// Generates Attribute Options reference sheet
        /// </summary>
        private void GenerateAttributeOptionsSheet(IXLWorksheet sheet, List<CategoryAttributeDto> attributes)
        {
            sheet.Cell(1, 1).Value = $"{ECommerceResources.Attribute} ID";
            sheet.Cell(1, 2).Value = "Attribute Name";
            sheet.Cell(1, 3).Value = "Option ID";
            sheet.Cell(1, 4).Value = $"Option Name ({GeneralResources.Arabic})";
            sheet.Cell(1, 5).Value = $"Option Name ({GeneralResources.English})";
            sheet.Cell(1, 6).Value = ECommerceResources.AffectsPricing;

            sheet.Range(1, 1, 1, 6).Style.Font.Bold = true;
            sheet.Range(1, 1, 1, 6).Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var attr in attributes)
            {
                if (attr.AttributeOptions != null && attr.AttributeOptions.Any())
                {
                    foreach (var option in attr.AttributeOptions)
                    {
                        sheet.Cell(row, 1).Value = attr.AttributeId.ToString();
                        sheet.Cell(row, 2).Value = attr.Title;
                        sheet.Cell(row, 3).Value = option.Id.ToString();
                        sheet.Cell(row, 4).Value = option.TitleAr;
                        sheet.Cell(row, 5).Value = option.TitleEn;
                        sheet.Cell(row, 6).Value = attr.AffectsPricing ? GeneralResources.Yes : GeneralResources.No;
                        row++;
                    }
                }
            }

            sheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// Generates Instructions sheet
        /// </summary>
        private void GenerateInstructionsSheet(IXLWorksheet sheet)
        {
            sheet.Cell(1, 1).Value = "Excel Template Instructions";
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 16;

            int row = 3;

            var instructions = new List<string>
            {
                "1. Required Fields:",
                "   - All fields marked with * are required",
                "   - Provide titles in both Arabic and English",
                "   - Use IDs from reference sheets for Category, Brand, and Unit",
                "   - Price must be a numeric value",
                "   - Quantity must be an integer",
                "   - Stock Status must be True or False",
                "",
                "2. Categories:",
                "   - Check the Categories sheet for available options",
                "   - Only final categories can be selected",
                "   - Select from dropdown",
                "",
                "3. Attributes:",
                "   - Regular attributes: enter a single value",
                "   - Pricing attributes: enter multiple IDs separated by comma",
                "   - Example: option1-id, option2-id",
                "   - Check Attribute Options sheet for valid options",
                "",
                "4. Pricing Combinations:",
                "   - Multiple options will generate combinations",
                "   - Example: Size(S,M,L) and Color(Red,Blue) = 6 combinations",
                "   - Set prices after import",
                "",
                "5. Images:",
                "   - Thumbnail image is required",
                "   - Additional images (1-5) are optional",
                "   - Supported formats: JPG, PNG, WebP",
                "",
                "6. Boolean Fields:",
                "   - Use True or False",
                "   - Select from dropdown",
                "   - Applies to: StockStatus, IsNewArrival, IsBestSeller, IsRecommended",
                "",
                "7. Tips:",
                "   - Delete the example row before importing",
                "   - Save as .xlsx format",
                "   - Maximum 1000 rows per import",
                "   - Review data carefully before import"
            };

            foreach (var instruction in instructions)
            {
                sheet.Cell(row, 1).Value = instruction;
                if (instruction.StartsWith("1.") || instruction.StartsWith("2.") ||
                    instruction.StartsWith("3.") || instruction.StartsWith("4.") ||
                    instruction.StartsWith("5.") || instruction.StartsWith("6.") ||
                    instruction.StartsWith("7."))
                {
                    sheet.Cell(row, 1).Style.Font.Bold = true;
                    sheet.Cell(row, 1).Style.Font.FontColor = XLColor.DarkBlue;
                }
                row++;
            }

            sheet.Column(1).Width = 100;
            sheet.Column(1).Style.Alignment.WrapText = true;
        }

        /// <summary>
        /// Parses an uploaded Excel file and extracts product data with strict validation
        /// </summary>
        public List<ImportedProductDto> ParseProductsFromExcel(
            Stream fileStream,
            List<CategoryAttributeDto> categoryAttributes,
            List<CategoryDto> categories,
            Dictionary<Guid, string> brands,
            Dictionary<Guid, string> units)
        {
            var products = new List<ImportedProductDto>();

            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(ECommerceResources.Products);

            if (worksheet == null)
                throw new Exception($"{ECommerceResources.Products} sheet not found in Excel file");

            var headerRow = worksheet.Row(1);
            var columnMap = BuildColumnMap(headerRow);

            ValidateRequiredColumns(columnMap);

            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                if (row.IsEmpty()) continue;

                try
                {
                    var product = new ImportedProductDto
                    {
                        TitleAr = GetCellValue(row, columnMap, "TitleAr*")?.Trim() ?? string.Empty,
                        TitleEn = GetCellValue(row, columnMap, "TitleEn*")?.Trim() ?? string.Empty,
                        ShortDescriptionAr = GetCellValue(row, columnMap, "ShortDescriptionAr*")?.Trim() ?? string.Empty,
                        ShortDescriptionEn = GetCellValue(row, columnMap, "ShortDescriptionEn*")?.Trim() ?? string.Empty,
                        DescriptionAr = GetCellValue(row, columnMap, "DescriptionAr*")?.Trim() ?? string.Empty,
                        DescriptionEn = GetCellValue(row, columnMap, "DescriptionEn*")?.Trim() ?? string.Empty,
                        CategoryId = ParseCategoryValue(GetCellValue(row, columnMap, "CategoryId*"), categories),
                        BrandId = ParseBrandValue(GetCellValue(row, columnMap, "BrandId*"), brands),
                        UnitId = ParseUnitValue(GetCellValue(row, columnMap, "UnitId*"), units),
                        Price = GetDecimalValue(row, columnMap, "Price*"),
                        Quantity = GetIntValue(row, columnMap, "Quantity*"),
                        StockStatus = GetBoolValue(row, columnMap, "StockStatus*"),
                        IsNewArrival = GetBoolValue(row, columnMap, "IsNewArrival"),
                        IsBestSeller = GetBoolValue(row, columnMap, "IsBestSeller"),
                        IsRecommended = GetBoolValue(row, columnMap, "IsRecommended"),
                        ThumbnailImagePath = GetCellValue(row, columnMap, "ThumbnailImagePath")?.Trim(),
                        ImagePaths = new List<string>(),
                        AttributeValues = new Dictionary<Guid, string>(),
                        PricingAttributeValues = new Dictionary<Guid, List<Guid>>(),
                        RowNumber = row.RowNumber()
                    };

                    // Parse image paths
                    for (int i = 1; i <= 5; i++)
                    {
                        var imagePath = GetCellValue(row, columnMap, $"ImagePath{i}")?.Trim();
                        if (!string.IsNullOrWhiteSpace(imagePath))
                            product.ImagePaths.Add(imagePath);
                    }

                    // Parse regular attributes
                    foreach (var attr in categoryAttributes.Where(a => !a.AffectsPricing))
                    {
                        var value = GetCellValue(row, columnMap, attr.Title)?.Trim();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (attr.FieldType == Common.Enumerations.FieldType.FieldType.List &&
                                attr.AttributeOptions != null)
                            {
                                var option = attr.AttributeOptions.FirstOrDefault(o =>
                                    o.Title.Equals(value, StringComparison.OrdinalIgnoreCase));

                                if (option != null)
                                    value = option.Id.ToString();
                            }

                            product.AttributeValues[attr.AttributeId] = value;
                        }
                    }

                    // Parse pricing-affecting attributes
                    foreach (var attr in categoryAttributes.Where(a => a.AffectsPricing))
                    {
                        var columnName = $"{attr.Title}*";
                        var value = GetCellValue(row, columnMap, columnName)?.Trim();

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            var optionIds = value.Split(',')
                                .Select(s => s.Trim())
                                .Where(s => !string.IsNullOrWhiteSpace(s))
                                .Select(s => ParseAttributeOption(s, attr))
                                .Where(guid => guid != Guid.Empty)
                                .ToList();

                            if (optionIds.Any())
                            {
                                product.PricingAttributeValues[attr.AttributeId] = optionIds;
                            }
                        }
                    }

                    products.Add(product);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error parsing row {row.RowNumber()}: {ex.Message}", ex);
                }
            }

            return products;
        }

        private Guid ParseCategoryValue(string? value, List<CategoryDto> categories)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Guid.Empty;

            if (Guid.TryParse(value, out var guid))
            {
                if (categories.Any(c => c.Id == guid))
                    return guid;
            }

            var category = categories.FirstOrDefault(c =>
                c.Title.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));

            return category?.Id ?? Guid.Empty;
        }

        private Guid ParseBrandValue(string? value, Dictionary<Guid, string> brands)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Guid.Empty;

            if (Guid.TryParse(value, out var guid))
            {
                if (brands.ContainsKey(guid))
                    return guid;
            }

            var brand = brands.FirstOrDefault(b =>
                b.Value.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));

            return brand.Key;
        }

        private Guid ParseUnitValue(string? value, Dictionary<Guid, string> units)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Guid.Empty;

            if (Guid.TryParse(value, out var guid))
            {
                if (units.ContainsKey(guid))
                    return guid;
            }

            var unit = units.FirstOrDefault(u =>
                u.Value.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));

            return unit.Key;
        }

        private Guid ParseAttributeOption(string value, CategoryAttributeDto attribute)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Guid.Empty;

            if (Guid.TryParse(value, out var guid))
            {
                if (attribute.AttributeOptions?.Any(o => o.Id == guid) == true)
                    return guid;
            }

            if (attribute.AttributeOptions != null)
            {
                var option = attribute.AttributeOptions.FirstOrDefault(o =>
                    o.Title.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));

                if (option != null)
                    return option.Id;
            }

            return Guid.Empty;
        }

        private void ValidateRequiredColumns(Dictionary<string, int> columnMap)
        {
            var requiredColumns = new List<string>
            {
                "TitleAr*", "TitleEn*", "ShortDescriptionAr*", "ShortDescriptionEn*",
                "DescriptionAr*", "DescriptionEn*", "CategoryId*", "BrandId*",
                "UnitId*", "Price*", "Quantity*", "StockStatus*"
            };

            var missingColumns = requiredColumns.Where(col => !columnMap.ContainsKey(col)).ToList();

            if (missingColumns.Any())
            {
                throw new Exception($"Missing required columns: {string.Join(", ", missingColumns)}");
            }
        }

        private Dictionary<string, int> BuildColumnMap(IXLRow headerRow)
        {
            var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int col = 1; col <= headerRow.CellsUsed().Count(); col++)
            {
                var cellValue = headerRow.Cell(col).GetValue<string>();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    columnMap[cellValue.Trim()] = col;
                }
            }

            return columnMap;
        }

        private string? GetCellValue(IXLRow row, Dictionary<string, int> columnMap, string columnName)
        {
            if (columnMap.TryGetValue(columnName, out int columnIndex))
            {
                var cell = row.Cell(columnIndex);
                if (!cell.IsEmpty())
                {
                    return cell.GetValue<string>();
                }
            }
            return null;
        }

        private decimal GetDecimalValue(IXLRow row, Dictionary<string, int> columnMap, string columnName)
        {
            var value = GetCellValue(row, columnMap, columnName);
            if (decimal.TryParse(value, out var result))
            {
                return result;
            }
            return 0;
        }

        private int GetIntValue(IXLRow row, Dictionary<string, int> columnMap, string columnName)
        {
            var value = GetCellValue(row, columnMap, columnName);
            if (int.TryParse(value, out var result))
            {
                return result;
            }
            return 0;
        }

        private bool GetBoolValue(IXLRow row, Dictionary<string, int> columnMap, string columnName)
        {
            var value = GetCellValue(row, columnMap, columnName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim().ToLower();
            return value == "true" || value == "yes" || value == "1";
        }
    }

    /// <summary>
    /// DTO for imported product data from Excel
    /// </summary>
    public class ImportedProductDto
    {
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string ShortDescriptionAr { get; set; } = string.Empty;
        public string ShortDescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool StockStatus { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }
        public string? ThumbnailImagePath { get; set; }
        public List<string> ImagePaths { get; set; } = new();
        public Dictionary<Guid, string> AttributeValues { get; set; } = new();
        public Dictionary<Guid, List<Guid>> PricingAttributeValues { get; set; } = new();
        public int RowNumber { get; set; }
    }
}
