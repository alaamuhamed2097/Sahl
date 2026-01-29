using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Catalog.Category;
using Shared.DTOs.Catalog.Item;
using System.Text.Json;

namespace Dashboard.Pages.Catalog.VendorItems
{
    public partial class Import
    {
        private enum ImportStep
        {
            SelectCategory,
            UploadFile,
            ReviewData,
            Complete
        }

        private ImportStep currentStep = ImportStep.SelectCategory;
        private List<CategoryDto> categories = new();
        private List<CategoryAttributeDto> categoryAttributes = new();
        private Dictionary<Guid, string> brands = new();
        private Dictionary<Guid, string> units = new();

        private Guid selectedCategoryId = Guid.Empty;
        private string selectedCategoryName = string.Empty;
        private bool isGeneratingTemplate = false;
        private bool isProcessing = false;
        private bool isImporting = false;
        private int importProgress = 0;

        private IBrowserFile? uploadedFile;
        private string uploadError = string.Empty;
        private List<ImportedProductDto> importedProducts = new();
        private List<string> validationErrors = new();
        private List<ImportResult> importResults = new();

        private int successCount = 0;
        private int failedCount = 0;

        protected override async Task OnInitializedAsync()
        {
            // Load Excel helper script
            await ResourceLoaderService.LoadScript("Common/Excel/excelExportHelper.js");

            await LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            try
            {
                // Load categories
                var categoriesResult = await CategoryService.GetAllAsync();
                if (categoriesResult?.Success == true)
                {
                    categories = categoriesResult.Data?.ToList() ?? new();
                }

                // Load brands
                var brandsResult = await BrandService.GetAllAsync();
                if (brandsResult?.Success == true)
                {
                    brands = brandsResult.Data?.ToDictionary(b => b.Id, b => b.Name) ?? new();
                }

                // Load units
                var unitsResult = await UnitService.GetAllAsync();
                if (unitsResult?.Success == true)
                {
                    units = unitsResult.Data?.ToDictionary(u => u.Id, u => u.Title) ?? new();
                }
            }
            catch (Exception ex)
            {
                await ShowError("Error loading data", ex.Message);
            }
        }

        private async Task OnCategorySelected(ChangeEventArgs e)
        {
            if (Guid.TryParse(e.Value?.ToString(), out var categoryId))
            {
                selectedCategoryId = categoryId;
                var category = categories.FirstOrDefault(c => c.Id == categoryId);
                selectedCategoryName = category?.Title ?? "";

                // Load category attributes
                await LoadCategoryAttributes();
            }
            else
            {
                selectedCategoryId = Guid.Empty;
                selectedCategoryName = "";
                categoryAttributes.Clear();
            }
        }

        private async Task LoadCategoryAttributes()
        {
            try
            {
                var result = await AttributeService.GetByCategoryIdAsync(selectedCategoryId);
                if (result?.Success == true)
                {
                    categoryAttributes = result.Data?.ToList() ?? new();
                }
            }
            catch (Exception ex)
            {
                await ShowError("Error loading attributes", ex.Message);
            }
        }

        private async Task GenerateTemplateWithData()
        {
            try
            {
                isGeneratingTemplate = true;
                StateHasChanged();

                var category = categories.First(c => c.Id == selectedCategoryId);

                // Prepare data for JavaScript
                var templateConfig = new
                {
                    headers = new[]
                    {
                    $"{FormResources.Title} ({GeneralResources.Arabic})*",
                    $"{FormResources.Title} ({GeneralResources.English})*",
                    $"{FormResources.ShortDescription} ({GeneralResources.Arabic})*",
                    $"{FormResources.ShortDescription} ({GeneralResources.English})*",
                    $"{FormResources.Description} ({GeneralResources.Arabic})*",
                    $"{FormResources.Description} ({GeneralResources.English})*",
                    $"{ECommerceResources.Category}*",
                    $"{BrandResources.Brand}*",
                    $"{ECommerceResources.Unit}*",
                    $"{FormResources.Price}*",
                    $"{ECommerceResources.Quantity}*",
                    $"{ECommerceResources.StockStatus}*",
                    "Thumbnail Image",
                    $"{FormResources.Image} 1",
                    $"{FormResources.Image} 2",
                    $"{FormResources.Image} 3",
                    $"{FormResources.Image} 4",
                    $"{FormResources.Image} 5"
                },
                    exampleRow = new[]
                    {
                    "منتج تجريبي",
                    "Example Product",
                    "وصف قصير",
                    "Short description",
                    "وصف كامل",
                    "Full description",
                    category.Title,
                    brands.Values.FirstOrDefault() ?? "Brand Name",
                    units.Values.FirstOrDefault() ?? "Unit Name",
                    "100.00",
                    "50",
                    "True",
                    "False",
                    "False",
                    "False",
                    "/images/products/example.jpg",
                    "",
                    "",
                    "",
                    "",
                    ""
                },
                    categories = categories.Where(c => c.IsFinal).Select(c => new
                    {
                        id = c.Id.ToString(),
                        title = c.Title,
                        titleAr = c.TitleAr,
                        titleEn = c.TitleEn,
                        isFinal = c.IsFinal
                    }).ToArray(),
                    brands = brands.Select(b => new
                    {
                        id = b.Key.ToString(),
                        name = b.Value
                    }).ToArray(),
                    units = units.Select(u => new
                    {
                        id = u.Key.ToString(),
                        title = u.Value
                    }).ToArray(),
                    attributes = categoryAttributes.Select(a => new
                    {
                        id = a.AttributeId.ToString(),
                        title = a.Title,
                        affectsPricing = a.AffectsPricing,
                        options = a.AttributeOptions?.Select(o => new
                        {
                            id = o.Id.ToString(),
                            title = o.Title
                        }).ToArray() ?? Array.Empty<object>()
                    }).ToArray()
                };

                // Call JavaScript to generate template with REAL DROPDOWNS
                var success = await JSRuntime.InvokeAsync<bool>(
                    "excelExportHelper.generateProductImportTemplate",
                    $"ProductImportTemplate_{category.TitleEn}_{DateTime.Now:yyyyMMdd}.xlsx",
                    templateConfig
                );

                if (success)
                {
                    // Move to next step
                    currentStep = ImportStep.UploadFile;
                }
                else
                {
                    await ShowError(NotifiAndAlertsResources.Error, "Failed to generate template");
                }
            }
            catch (Exception ex)
            {
                await ShowError(NotifiAndAlertsResources.Error, ex.Message);
            }
            finally
            {
                isGeneratingTemplate = false;
                StateHasChanged();
            }
        }

        private async Task GenerateEmptyTemplate()
        {
            try
            {
                isGeneratingTemplate = true;
                StateHasChanged();

                var category = categories.First(c => c.Id == selectedCategoryId);

                // Prepare data for JavaScript (empty template - no reference data)
                var templateConfig = new
                {
                    headers = new[]
                    {
                    $"{FormResources.Title} ({GeneralResources.Arabic})*",
                    $"{FormResources.Title} ({GeneralResources.English})*",
                    $"{FormResources.ShortDescription} ({GeneralResources.Arabic})*",
                    $"{FormResources.ShortDescription} ({GeneralResources.English})*",
                    $"{FormResources.Description} ({GeneralResources.Arabic})*",
                    $"{FormResources.Description} ({GeneralResources.English})*",
                    $"{ECommerceResources.Category}*",
                    $"{BrandResources.Brand}*",
                    $"{ECommerceResources.Unit}*",
                    $"{FormResources.Price}*",
                    $"{ECommerceResources.Quantity}*",
                    $"{ECommerceResources.StockStatus}*",
                    "Is New Arrival",
                    "Is Best Seller",
                    "Is Recommended",
                    "Thumbnail Image",
                    $"{FormResources.Image} 1",
                    $"{FormResources.Image} 2",
                    $"{FormResources.Image} 3",
                    $"{FormResources.Image} 4",
                    $"{FormResources.Image} 5"
                },
                    exampleRow = new[]
                    {
                    "منتج تجريبي",
                    "Example Product",
                    "وصف قصير",
                    "Short description",
                    "وصف كامل",
                    "Full description",
                    "Category Name",
                    "Brand Name",
                    "Unit Name",
                    "100.00",
                    "50",
                    "True",
                    "False",
                    "False",
                    "False",
                    "/images/products/example.jpg",
                    "",
                    "",
                    "",
                    "",
                    ""
                },
                    categories = Array.Empty<object>(),  // Empty for basic template
                    brands = Array.Empty<object>(),
                    units = Array.Empty<object>(),
                    attributes = Array.Empty<object>()
                };

                // Call JavaScript
                var success = await JSRuntime.InvokeAsync<bool>(
                    "excelExportHelper.generateProductImportTemplate",
                    $"ProductImportEmpty_{category.TitleEn}_{DateTime.Now:yyyyMMdd}.xlsx",
                    templateConfig
                );

                if (success)
                {
                    currentStep = ImportStep.UploadFile;
                }
                else
                {
                    await ShowError(NotifiAndAlertsResources.Error, "Failed to generate template");
                }
            }
            catch (Exception ex)
            {
                await ShowError(NotifiAndAlertsResources.Error, ex.Message);
            }
            finally
            {
                isGeneratingTemplate = false;
                StateHasChanged();
            }
        }

        private async Task HandleFileUpload(InputFileChangeEventArgs e)
        {
            uploadedFile = e.File;
            uploadError = string.Empty;

            if (uploadedFile.Size > 10 * 1024 * 1024) // 10MB limit
            {
                uploadError = "File size exceeds 10MB limit";
                uploadedFile = null;
            }
            else if (!uploadedFile.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                uploadError = "Only .xlsx files are supported";
                uploadedFile = null;
            }
        }

        private async Task ParseExcelFile()
        {
            if (uploadedFile == null) return;

            try
            {
                isProcessing = true;
                validationErrors.Clear();
                StateHasChanged();

                // Read file as base64
                using var stream = uploadedFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var base64File = Convert.ToBase64String(fileBytes);

                // Call JavaScript to parse Excel
                var parsedJson = await JSRuntime.InvokeAsync<string>(
                    "excelExportHelper.parseProductsFromExcel",
                    base64File,
                    new
                    {
                        categoryAttributes = categoryAttributes.Select(a => new
                        {
                            attributeId = a.AttributeId.ToString(),
                            title = a.Title,
                            affectsPricing = a.AffectsPricing,
                            fieldType = a.FieldType.ToString(),
                            options = a.AttributeOptions?.Select(o => new
                            {
                                id = o.Id.ToString(),
                                title = o.Title
                            }).ToArray() ?? Array.Empty<object>()
                        }).ToArray(),
                        categories = categories.Select(c => new
                        {
                            id = c.Id.ToString(),
                            title = c.Title
                        }).ToArray(),
                        brands = brands.Select(b => new
                        {
                            id = b.Key.ToString(),
                            name = b.Value
                        }).ToArray(),
                        units = units.Select(u => new
                        {
                            id = u.Key.ToString(),
                            title = u.Value
                        }).ToArray()
                    }
                );

                // Parse JSON result
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                importedProducts = JsonSerializer.Deserialize<List<ImportedProductDto>>(parsedJson, options) ?? new();

                // Validate products
                ValidateImportedProducts();

                currentStep = ImportStep.ReviewData;
            }
            catch (Exception ex)
            {
                await ShowError(NotifiAndAlertsResources.Error, ex.Message);
            }
            finally
            {
                isProcessing = false;
                StateHasChanged();
            }
        }

        private List<string> GetRowValidationErrors(ImportedProductDto product)
        {
            var errors = new List<string>();

            // Required fields validation
            if (string.IsNullOrWhiteSpace(product.TitleEn))
                errors.Add("Title (EN) is required");

            if (string.IsNullOrWhiteSpace(product.TitleAr))
                errors.Add("Title (AR) is required");

            if (string.IsNullOrWhiteSpace(product.ShortDescriptionEn))
                errors.Add("Short Description (EN) is required");

            if (string.IsNullOrWhiteSpace(product.ShortDescriptionAr))
                errors.Add("Short Description (AR) is required");

            if (string.IsNullOrWhiteSpace(product.DescriptionEn))
                errors.Add("Description (EN) is required");

            if (string.IsNullOrWhiteSpace(product.DescriptionAr))
                errors.Add("Description (AR) is required");

            // Category validation
            if (product.CategoryId == Guid.Empty)
                errors.Add("Category ID is required");
            else if (!categories.Any(c => c.Id == product.CategoryId))
                errors.Add("Invalid Category ID - Category does not exist");

            // Brand validation
            if (product.BrandId == Guid.Empty)
                errors.Add("Brand ID is required");
            else if (!brands.ContainsKey(product.BrandId))
                errors.Add("Invalid Brand ID - Brand does not exist");

            // Unit validation
            if (product.UnitId == Guid.Empty)
                errors.Add("Unit ID is required");
            else if (!units.ContainsKey(product.UnitId))
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
                if (attribute == null)
                {
                    errors.Add($"Pricing attribute {pricingAttr.Key} not found in category");
                    continue;
                }

                foreach (var optionId in pricingAttr.Value)
                {
                    if (attribute.AttributeOptions == null ||
                        !attribute.AttributeOptions.Any(o => o.Id == optionId))
                    {
                        errors.Add($"Invalid option ID {optionId} for attribute {attribute.TitleEn}");
                    }
                }
            }

            // Validate regular attribute values
            foreach (var attr in product.AttributeValues)
            {
                var attribute = categoryAttributes.FirstOrDefault(a => a.AttributeId == attr.Key);
                if (attribute == null)
                {
                    errors.Add($"Attribute {attr.Key} not found in category");
                }
            }

            return errors;
        }

        private void ValidateImportedProducts()
        {
            validationErrors.Clear();

            foreach (var product in importedProducts)
            {
                var rowErrors = GetRowValidationErrors(product);
                foreach (var error in rowErrors)
                {
                    validationErrors.Add($"Row {product.RowNumber}: {error}");
                }
            }
        }

        private async Task ImportProducts()
        {
            try
            {
                isImporting = true;
                importProgress = 0;
                importResults.Clear();
                successCount = 0;
                failedCount = 0;
                StateHasChanged();

                int processed = 0;
                foreach (var importedProduct in importedProducts)
                {
                    try
                    {
                        var itemDto = ConvertToItemDto(importedProduct);
                        var result = await ItemService.SaveAsync(itemDto);

                        if (result?.Success == true)
                        {
                            successCount++;
                            importResults.Add(new ImportResult
                            {
                                RowNumber = importedProduct.RowNumber,
                                Success = true,
                                Message = "Imported successfully"
                            });
                        }
                        else
                        {
                            failedCount++;
                            importResults.Add(new ImportResult
                            {
                                RowNumber = importedProduct.RowNumber,
                                Success = false,
                                Message = result?.Message ?? "Unknown error"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        importResults.Add(new ImportResult
                        {
                            RowNumber = importedProduct.RowNumber,
                            Success = false,
                            Message = ex.Message
                        });
                    }

                    processed++;
                    importProgress = (processed * 100) / importedProducts.Count;
                    StateHasChanged();
                }

                currentStep = ImportStep.Complete;
            }
            catch (Exception ex)
            {
                await ShowError("Error importing products", ex.Message);
            }
            finally
            {
                isImporting = false;
                StateHasChanged();
            }
        }

        private ItemDto ConvertToItemDto(ImportedProductDto imported)
        {
            var item = new ItemDto
            {
                TitleAr = imported.TitleAr,
                TitleEn = imported.TitleEn,
                ShortDescriptionAr = imported.ShortDescriptionAr,
                ShortDescriptionEn = imported.ShortDescriptionEn,
                DescriptionAr = imported.DescriptionAr,
                DescriptionEn = imported.DescriptionEn,
                CategoryId = imported.CategoryId,
                BrandId = imported.BrandId,
                UnitId = imported.UnitId,
                ThumbnailImage = imported.ThumbnailImagePath ?? string.Empty,
                Images = new List<ItemImageDto>(),
                ItemAttributes = new List<ItemAttributeDto>()
            };

            // Add item attributes
            foreach (var attr in imported.AttributeValues)
            {
                item.ItemAttributes.Add(new ItemAttributeDto
                {
                    AttributeId = attr.Key,
                    Value = attr.Value
                });
            }

            return item;
        }

        private List<List<Guid>> GeneratePricingCombinations(Dictionary<Guid, List<Guid>> attributeValues)
        {
            if (!attributeValues.Any()) return new();

            var result = new List<List<Guid>> { new List<Guid>() };

            foreach (var kvp in attributeValues)
            {
                var newResult = new List<List<Guid>>();
                foreach (var existing in result)
                {
                    foreach (var optionId in kvp.Value)
                    {
                        var newCombo = new List<Guid>(existing) { optionId };
                        newResult.Add(newCombo);
                    }
                }
                result = newResult;
            }

            return result;
        }

        private int GetCombinationCount(Dictionary<Guid, List<Guid>> pricingAttributes)
        {
            if (!pricingAttributes.Any()) return 0;
            return pricingAttributes.Values.Aggregate(1, (acc, list) => acc * list.Count);
        }

        private void RemoveProduct(ImportedProductDto product)
        {
            importedProducts.Remove(product);
        }

        private void BackToSelectCategory()
        {
            currentStep = ImportStep.SelectCategory;
            uploadedFile = null;
            uploadError = string.Empty;
        }

        private void BackToUpload()
        {
            currentStep = ImportStep.UploadFile;
            importedProducts.Clear();
            validationErrors.Clear();
        }

        private void StartNewImport()
        {
            currentStep = ImportStep.SelectCategory;
            selectedCategoryId = Guid.Empty;
            selectedCategoryName = string.Empty;
            uploadedFile = null;
            uploadError = string.Empty;
            importedProducts.Clear();
            validationErrors.Clear();
            importResults.Clear();
            successCount = 0;
            failedCount = 0;
        }

        private void GoToProducts()
        {
            Navigation.NavigateTo("/products");
        }

        private async Task ShowError(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private class ImportResult
        {
            public int RowNumber { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
        }

        // DTO for imported products from Excel
        private class ImportedProductDto
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
}