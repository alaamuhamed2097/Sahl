using Dashboard.Configuration;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Dashboard.Pages.Catalog.Products.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Brand;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;
using Shared.DTOs.ECommerce.Unit;
using Shared.DTOs.Media;
using System.ComponentModel;
using System.Threading.Tasks;
using static Dashboard.Pages.Catalog.Products.Components.AttributeValuesSection;


namespace Dashboard.Pages.Catalog.Products
{
    public partial class Details
    {
        private bool _initialized = false;
        private Guid _lastLoadedId = Guid.Empty;
        // Constants
        protected const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        protected const int MaxImageCount = 10;

        // Parameters
        [Parameter] public Guid Id { get; set; }

        // Injections
        [Inject] protected IItemService ItemService { get; set; } = null!;
        [Inject] protected IBrandService BrandService { get; set; } = null!;
        [Inject] protected ICategoryService CategoryService { get; set; } = null!;
        [Inject] protected IAttributeService AttributeService { get; set; } = null!;
        [Inject] protected IUnitService UnitService { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        //   [Inject] protected IVideoProviderService VideoProviderService { get; set; } = null!
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected string baseUrl = string.Empty;

        // Wizard state
        protected bool isWizardInitialized { get; set; }
        protected int currentStep = 0;
        protected const int TotalSteps = 6;

        // State variables
        protected bool isSaving { get; set; }
        protected bool isProcessing { get; set; }
        protected int processingProgress { get; set; }
        protected ItemDto Model { get; set; } = new()
        {
            Images = new List<ItemImageDto>(),
            ItemAttributes = new List<ItemAttributeDto>(),
            ItemCombinations = new List<ItemCombinationDto>
            {
                // Initialize with a default combination
                new ItemCombinationDto
                {
                    Barcode = "1111111",
                    SKU = "DEFAULT",
                    BasePrice = 0,
                    CombinationAttributes = new List<CombinationAttributeDto>(),
                    IsDefault = true
                }
            }
        };

        // Validation states
        protected bool showValidationErrors = false;
        protected Dictionary<string, bool> fieldValidation = new Dictionary<string, bool>();

        // Data collections
        private IEnumerable<CategoryDto> categories = Array.Empty<CategoryDto>();
        private IEnumerable<UnitDto> units = Array.Empty<UnitDto>();
        private IEnumerable<VideoProviderDto> videoProviders = Array.Empty<VideoProviderDto>();
        protected List<CategoryAttributeDto> categoryAttributes = new();
        private List<BrandDto> brands = new();
        protected bool isLoadingAttributes = false;
        private Dictionary<Guid, string> _optionDisplayMap = new();
        private Dictionary<Guid, List<string>> attributeValues = new();
        private Dictionary<Guid, string> nonPricingAttributeValues = new();
        private Dictionary<Guid, List<string>> pricingAttributeValues = new();
        private Dictionary<Guid, Dictionary<string, PriceModifierInfo>> attributeValuePriceModifiers = new();
        private AttributeValuesSection attributeValuesSectionRef;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            // Initialize validation dictionary
            fieldValidation["CategoryId"] = true;
            fieldValidation["UnitId"] = true;
            // REMOVED: Quantity and ThumbnailImage validation - handled elsewhere
            fieldValidation["ThumbnailImage"] = true;

            await LoadData();
            _initialized = true;
        }

        protected override void OnParametersSet()
        {
            // ? FIX: تحقق من التهيئة ومن تغيير الـ Id لمنع infinite loop
            if (!_initialized)
            {
                return; // سينفذ OnInitializedAsync
            }

            if (Id != Guid.Empty && Id != _lastLoadedId)
            {
                _lastLoadedId = Id;
                _ = LoadProduct(Id);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;
            try
            {
                await ResourceLoaderService.LoadStyleSheets([
             "assets/plugins/smart-wizard/css/smart_wizard.min.css",
             "assets/plugins/smart-wizard/css/smart_wizard_theme_arrows.min.css"
            ]);

                await ResourceLoaderService.LoadScriptsSequential(
                 "assets/plugins/smart-wizard/js/jquery.smartWizard.min.js",
                 "assets/js/pages/product-wizard.js"
                   );

                await JSRuntime.InvokeVoidAsync("initializeSmartWizard", 0);
                isWizardInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing wizard: {ex.Message}");
            }
        }

        // ========== Data Loading Methods ==========
        private async Task LoadData()
        {
            try
            {
                var tasks = new[]
                {
                    LoadCategoriesAsync(),
                    LoadUnitsAsync(),
                    LoadBrands()
                };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var result = await CategoryService.GetAllAsync();
            if (result?.Success == true)
            {
                categories = result.Data ?? Array.Empty<CategoryDto>();
            }
        }

        private async Task LoadUnitsAsync()
        {
            var result = await UnitService.GetAllAsync();
            if (result?.Success == true)
            {
                units = result.Data ?? Array.Empty<UnitDto>();
            }
        }

        private async Task LoadCategoryAttributes()
        {
            try
            {
                Console.WriteLine($"🔍 LoadCategoryAttributes - START");
                isLoadingAttributes = true;
                StateHasChanged();

                Console.WriteLine($"📡 Calling API: AttributeService.GetByCategoryIdAsync({Model.CategoryId})");
                var result = await AttributeService.GetByCategoryIdAsync(Model.CategoryId);

                Console.WriteLine($"📥 API Response - Success: {result?.Success}, Data count: {result?.Data?.Count()}");

                if (result?.Success == true && result.Data != null)
                {
                    categoryAttributes = result.Data.ToList();
                    // Initialize attribute values for pricing attributes
                    foreach (var attr in categoryAttributes.Where(ca => ca.AffectsPricing))
                    {
                        if (!attributeValues.ContainsKey(attr.Id))
                        {
                            attributeValues[attr.Id] = new List<string>();
                        }
                    }

                    // Initialize ItemAttributes list if null
                    if (Model.ItemAttributes == null)
                    {
                        Model.ItemAttributes = new List<ItemAttributeDto>();
                    }

                    // Initialize attributes if this is a new product
                    if (Id == Guid.Empty)
                    {
                        Console.WriteLine($"🆕 New product - initializing attributes");
                        Model.ItemAttributes = categoryAttributes
                            .Select(a => new ItemAttributeDto
                            {
                                // FIX: Use AttributeId, not Id
                                AttributeId = a.AttributeId,
                                Value = string.Empty
                            })
                            .ToList();

                        Console.WriteLine($"✅ Initialized {Model.ItemAttributes.Count} item attributes");
                    }
                    else
                    {
                        Console.WriteLine($"📝 Existing product - merging attributes");
                        // For existing products, ensure all category attributes have entries
                        foreach (var categoryAttr in categoryAttributes)
                        {
                            // FIX: Use AttributeId, not Id
                            if (!Model.ItemAttributes.Any(ia => ia.AttributeId == categoryAttr.AttributeId))
                            {
                                Console.WriteLine($"  ➕ Adding missing attribute: {categoryAttr.Title}");
                                Model.ItemAttributes.Add(new ItemAttributeDto
                                {
                                    AttributeId = categoryAttr.AttributeId,
                                    Value = string.Empty
                                });
                            }
                        }
                        Console.WriteLine($"✅ Total item attributes: {Model.ItemAttributes.Count}");
                    }

                    Console.WriteLine($"🔄 About to call StateHasChanged - categoryAttributes.Count: {categoryAttributes.Count}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to load attributes: {result?.Message ?? "Unknown error"}");
                    categoryAttributes = new List<CategoryAttributeDto>();
                    await ShowErrorMessage(
                        "Error",
                        result?.Message ?? "Failed to load category attributes");
                }

                // Force UI update
                isLoadingAttributes = false;
                Console.WriteLine($"🔄 Calling StateHasChanged - isLoadingAttributes: {isLoadingAttributes}");
                StateHasChanged();

                // Give Blazor time to render
                await Task.Delay(100);
                Console.WriteLine($"✅ LoadCategoryAttributes - COMPLETE - Attributes visible: {categoryAttributes.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in LoadCategoryAttributes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                categoryAttributes = new List<CategoryAttributeDto>();
                await ShowErrorMessage("Error", ex.Message);
            }
            finally
            {
                isLoadingAttributes = false;
                StateHasChanged();
            }
        }

        private async Task LoadProduct(Guid id)
        {
            try
            {
                var result = await ItemService.GetByIdAsync(id);

                if (result?.Success == true)
                {
                    Model = result.Data ?? new ItemDto() { Images = new List<ItemImageDto>() };

                    // Ensure images are properly loaded
                    if (Model.Images == null)
                    {
                        Model.Images = new List<ItemImageDto>();
                    }

                    // Mark existing images as not new to handle them differently from uploaded ones
                    foreach (var image in Model.Images)
                    {
                        image.IsNew = false;
                    }

                    // Load category attributes if category is set
                    if (Model.CategoryId != Guid.Empty)
                    {
                        await LoadCategoryAttributes();
                    }

                    StateHasChanged();
                }
                else
                {
                    await ShowErrorMessage(
             ValidationResources.Failed,
             NotifiAndAlertsResources.FailedToRetrieveData);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        private async Task LoadBrands()
        {
            try
            {
                var result = await BrandService.GetAllAsync();

                if (result?.Success == true)
                {
                    brands = result.Data?.ToList() ?? new List<BrandDto>();
                    StateHasChanged();
                }
                else
                {
                    await ShowErrorMessage(
                     ValidationResources.Failed,
                NotifiAndAlertsResources.FailedToRetrieveData);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        // ========== Event Handlers ==========
        private async Task HandleCategoryChange()
        {
            try
            {
                Console.WriteLine($"🔍 HandleCategoryChange called - CategoryId: {Model.CategoryId}");

                // Reset attribute values when category changes
                attributeValues = new Dictionary<Guid, List<string>>();

                // Load category attributes
                await LoadCategoryAttributes();

                Console.WriteLine($"✅ HandleCategoryChange completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in HandleCategoryChange: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                await ShowErrorMessage("Error", ex.Message);
            }
        }

        private async Task HandleThumbnailUpload(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.File == null) return;

                // Validate file size
                if (e.File.Size > MaxFileSize)
                {
                    fieldValidation["ThumbnailImage"] = false;
                    await ShowErrorMessage(
                      ValidationResources.Error,
                           $"{e.File.Name} {string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / 1024 / 1024)} {MaxFileSize / 1024 / 1024}MB");
                    return;
                }

                // Validate content type
                if (!e.File.ContentType.StartsWith("image/"))
                {
                    fieldValidation["ThumbnailImage"] = false;
                    await ShowErrorMessage(
                     ValidationResources.Error,
                         $"{e.File.Name} {ValidationResources.NotValidImage}");
                    return;
                }

                // Process thumbnail
                Model.ThumbnailImage = await ConvertFileToBase64(e.File);
                fieldValidation["ThumbnailImage"] = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                fieldValidation["ThumbnailImage"] = false;
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        private async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount == 0) return;

                isProcessing = true;
                processingProgress = 0;
                StateHasChanged();

                // Validate number of images
                int availableSlots = MaxImageCount - Model.Images.Count;
                if (availableSlots <= 0)
                {
                    await ShowErrorMessage(
                         ValidationResources.Error,
                        $"{ValidationResources.MaximumOf} {MaxImageCount} {ValidationResources.ImagesAllowed}");
                    return;
                }

                // Initialize if null
                Model.Images ??= new List<ItemImageDto>();

                // Get actual files to process (respect the limit)
                var filesToProcess = e.GetMultipleFiles(Math.Min(availableSlots, e.FileCount));
                int processedCount = 0;

                foreach (var file in filesToProcess)
                {
                    // Validate file size
                    if (file.Size > MaxFileSize)
                    {
                        await ShowErrorMessage(
                            NotifiAndAlertsResources.Warning,
                            $"{file.Name} {string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / 1024 / 1024)} {MaxFileSize / 1024 / 1024}MB");
                        continue;
                    }

                    // Validate content type
                    if (!file.ContentType.StartsWith("image/"))
                    {
                        await ShowErrorMessage(
                            NotifiAndAlertsResources.Warning,
                            $"{file.Name} {ValidationResources.InvalidImageFormat}");
                        continue;
                    }

                    // Process image
                    var base64Image = await ConvertFileToBase64(file);
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        Model.Images.Add(new ItemImageDto
                        {
                            Path = base64Image,
                            IsNew = true
                        });
                    }

                    // Update progress
                    processedCount++;
                    processingProgress = (processedCount * 100) / filesToProcess.Count;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
            finally
            {
                isProcessing = false;
                processingProgress = 0;
                StateHasChanged();
            }
        }
        // Add this method
        private async Task OnAttributeValuesChanged()
        {
            if (attributeValuesSectionRef != null)
            {
                // Get the attribute values from the component
                var (nonPricingValues, pricingValues, priceModifiers) = attributeValuesSectionRef.GetAllAttributeData();

                // Update the dictionaries
                nonPricingAttributeValues = nonPricingValues;
                pricingAttributeValues = pricingValues;
                attributeValuePriceModifiers = priceModifiers;

                // Update the model's ItemAttributes for non-pricing attributes
                UpdateModelAttributes();
            }

            StateHasChanged();
        }

        private void UpdateModelAttributes()
        {
            Console.WriteLine("🔍 UpdateModelAttributes - START");

            // Clear existing attributes
            Model.ItemAttributes = new List<ItemAttributeDto>();

            // Add non-pricing attributes from the form
            foreach (var kvp in nonPricingAttributeValues)
            {
                Console.WriteLine($"  Processing attribute {kvp.Key}: '{kvp.Value}'");

                if (!string.IsNullOrWhiteSpace(kvp.Value))
                {
                    // FIX: Match using AttributeId instead of Id
                    var categoryAttr = categoryAttributes.FirstOrDefault(ca => ca.AttributeId == kvp.Key);

                    if (categoryAttr != null)
                    {
                        Console.WriteLine($"    ✅ Found category attribute: {categoryAttr.Title}");
                        Model.ItemAttributes.Add(new ItemAttributeDto
                        {
                            AttributeId = categoryAttr.AttributeId,
                            Value = kvp.Value
                        });
                    }
                    else
                    {
                        Console.WriteLine($"    ⚠️ Could not find category attribute for AttributeId: {kvp.Key}");
                    }
                }
                else
                {
                    Console.WriteLine($"    ⏭️ Skipped (empty value)");
                }
            }

            Console.WriteLine($"✅ UpdateModelAttributes - END - Added {Model.ItemAttributes.Count} attributes to Model");
            foreach (var attr in Model.ItemAttributes)
            {
                var catAttr = categoryAttributes.FirstOrDefault(ca => ca.AttributeId == attr.AttributeId);
                Console.WriteLine($"    - {catAttr?.Title ?? "Unknown"}: {attr.Value}");
            }
        }

        private void UpdateCombinationsBasedOnAttributeValues()
        {
            // If we have attribute values but no combinations, create default combinations
            if (attributeValues.Any() && !Model.ItemCombinations.Any())
            {
                Model.ItemCombinations = new List<ItemCombinationDto>();

                // Create a default combination
                var defaultCombination = new ItemCombinationDto
                {
                    Id = Guid.NewGuid(),
                    Barcode = "DEFAULT",
                    SKU = "DEFAULT",
                    BasePrice = 0,
                    IsDefault = true,
                    CombinationAttributes = new List<CombinationAttributeDto>()
                };

                // Add attributes to the default combination
                foreach (var attr in attributeValues)
                {
                    if (attr.Value.Any())
                    {
                        var combinationAttr = new CombinationAttributeDto
                        {
                            combinationAttributeValueDtos = new List<CombinationAttributeValueDto>
                    {
                        new CombinationAttributeValueDto
                        {
                            AttributeId = attr.Key,
                            Value = attr.Value.First() // Use the first value
                        }
                    }
                        };
                        defaultCombination.CombinationAttributes.Add(combinationAttr);
                    }
                }

                Model.ItemCombinations.Add(defaultCombination);
                Console.WriteLine($"✅ Created default combination with attributes");
            }
        }

        protected async Task RemoveThumbnail()
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.ConfirmDeleteImage,
                icon = "warning",
                buttons = new { confirm = true },
                dangerMode = true
            });

            if (confirmed)
            {
                Model.ThumbnailImage = null;
                StateHasChanged();
            }
        }

        private async Task DeleteImage(ItemImageDto image)
        {
            try
            {
                var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
                {
                    title = NotifiAndAlertsResources.ConfirmDeleteImage,
                    icon = "warning",
                    buttons = new { confirm = true },
                    dangerMode = true
                });

                if (confirmed)
                {
                    Model.Images.Remove(image);
                    // Update validation state - if we have no images, we need a thumbnail
                    fieldValidation["ThumbnailImage"] = !string.IsNullOrEmpty(Model.ThumbnailImage) || Model.Images.Count > 0;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        private void RemoveAttribute(Guid attributeId)
        {
            var attribute = Model.ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId);
            if (attribute != null)
            {
                Model.ItemAttributes.Remove(attribute);

                // Remove any combinations that include this attribute
                Model.ItemCombinations = Model.ItemCombinations
                    .Where(c => !CombinationContainsAttribute(c, attributeId))
                    .ToList();

                // If we removed all combinations or the default combination, ensure we have at least one default
                if (!Model.ItemCombinations.Any())
                {
                    // Add back a default combination
                    Model.ItemCombinations.Add(new ItemCombinationDto
                    {
                        Barcode = "DEFAULT",
                        SKU = "DEFAULT",
                        BasePrice = 0,
                        CombinationAttributes = new List<CombinationAttributeDto>(),
                        IsDefault = true
                    });
                }
                else if (!Model.ItemCombinations.Any(c => c.IsDefault))
                {
                    // If no default exists, set the first one as default
                    Model.ItemCombinations.First().IsDefault = true;
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// Checks if a combination contains a specific attribute
        /// </summary>
        private bool CombinationContainsAttribute(ItemCombinationDto combination, Guid attributeId)
        {
            if (combination?.CombinationAttributes == null)
                return false;

            return combination.CombinationAttributes
                .Any(ca => ca.combinationAttributeValueDtos
                    .Any(cav => cav.AttributeId == attributeId));
        }
        
        //protected async Task Save()
        //{
        //    try
        //    {
        //        // Validate required fields
        //        ValidateForm();

        //        // Validate attributes
        //        if (!ValidateAttributes())
        //        {
        //            showValidationErrors = true;
        //            await ShowErrorMessage(
        //                ValidationResources.ValidationError,
        //                "Please fill in all required attributes before saving.");
        //            return;
        //        }

        //        // Validate attribute combinations
        //        if (!(await ValidateAttributeCombinations()))
        //        {
        //            showValidationErrors = true;
        //            return;
        //        }

        //        if (!IsFormValid())
        //        {
        //            showValidationErrors = true;
        //            // Highlight which fields are invalid
        //            if (!fieldValidation["ThumbnailImage"])
        //            {
        //                await ShowErrorMessage(
        //                ValidationResources.ValidationError,
        //                    ValidationResources.ImageRequired);
        //            }
        //            else
        //            {
        //                await ShowErrorMessage(
        //                  ValidationResources.ValidationError,
        //                     ValidationResources.PleaseFixValidationErrors);
        //            }
        //            return;
        //        }

        //        isSaving = true;
        //        StateHasChanged();

        //        var result = await ItemService.SaveAsync(Model);

        //        if (result?.Success == true)
        //        {
        //            await ShowSuccessMessage(
        //              ValidationResources.Done,
        //           NotifiAndAlertsResources.SavedSuccessfully);

        //            Navigation.NavigateTo("/Products");
        //        }
        //        else
        //        {
        //            await ShowErrorMessage(
        //        ValidationResources.Failed,
        //result?.Message ?? NotifiAndAlertsResources.SaveFailed);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await ShowErrorMessage(NotifiAndAlertsResources.FailedAlert, ex.Message);
        //    }
        //    finally
        //    {
        //        isSaving = false;
        //        StateHasChanged();
        //    }
        //}
        protected async Task Save()
        {
            try
            {
                Console.WriteLine("🔍 === SAVE METHOD STARTED ===");

                // Step 1: Validate form fields
                Console.WriteLine("📋 Step 1: ValidateForm()");
                ValidateForm();
                Console.WriteLine($"  fieldValidation results:");
                foreach (var kvp in fieldValidation)
                {
                    Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
                }

                // Step 2: Validate attributes
                Console.WriteLine("📋 Step 2: ValidateAttributes()");
                if (!ValidateAttributes())
                {
                    Console.WriteLine("  ❌ ValidateAttributes() FAILED");
                    showValidationErrors = true;
                    await ShowErrorMessage(
                        ValidationResources.ValidationError,
                        "Please fill in all required attributes before saving.");
                    return;
                }
                Console.WriteLine("  ✅ ValidateAttributes() PASSED");

                // Step 3: Validate attribute combinations
                Console.WriteLine("📋 Step 3: ValidateAttributeCombinations()");
                if (!(await ValidateAttributeCombinations()))
                {
                    Console.WriteLine("  ❌ ValidateAttributeCombinations() FAILED");
                    showValidationErrors = true;
                    return;
                }
                Console.WriteLine("  ✅ ValidateAttributeCombinations() PASSED");

                // Step 4: Check if form is valid
                Console.WriteLine("📋 Step 4: IsFormValid()");
                if (!IsFormValid())
                {
                    Console.WriteLine("  ❌ IsFormValid() FAILED");
                    showValidationErrors = true;

                    // Provide specific error message
                    if (!fieldValidation["ThumbnailImage"])
                    {
                        Console.WriteLine("    Reason: ThumbnailImage validation failed");
                        await ShowErrorMessage(
                            ValidationResources.ValidationError,
                            ValidationResources.ImageRequired);
                    }
                    else
                    {
                        Console.WriteLine("    Reason: Unknown validation failure");
                        Console.WriteLine($"    ThumbnailImage: {!string.IsNullOrEmpty(Model.ThumbnailImage)}");
                        Console.WriteLine($"    Images count: {Model.Images?.Count ?? 0}");

                        await ShowErrorMessage(
                            ValidationResources.ValidationError,
                            ValidationResources.PleaseFixValidationErrors);
                    }
                    return;
                }
                Console.WriteLine("  ✅ IsFormValid() PASSED");

                Console.WriteLine("🚀 All validations passed, proceeding to save...");
                Console.WriteLine($"  Model.ItemCombinations count: {Model.ItemCombinations?.Count ?? 0}");
                Console.WriteLine($"  Model.ItemAttributes count: {Model.ItemAttributes?.Count ?? 0}");

                isSaving = true;
                StateHasChanged();

                var result = await ItemService.SaveAsync(Model);

                if (result?.Success == true)
                {
                    Console.WriteLine("✅ Save successful!");
                    await ShowSuccessMessage(
                        ValidationResources.Done,
                        NotifiAndAlertsResources.SavedSuccessfully);

                    Navigation.NavigateTo("/Products");
                }
                else
                {
                    Console.WriteLine($"❌ Save failed: {result?.Message}");
                    await ShowErrorMessage(
                        ValidationResources.Failed,
                        result?.Message ?? NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in Save(): {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                await ShowErrorMessage(NotifiAndAlertsResources.FailedAlert, ex.Message);
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
                Console.WriteLine("🔍 === SAVE METHOD ENDED ===");
            }
        }

        /// <summary>
        /// Validates all required attributes have values
        /// </summary>
        private bool ValidateAttributes()
        {
            if (Model.CategoryId == Guid.Empty || !categoryAttributes.Any())
                return true; // No attributes to validate

            // FIXED: Only validate NON-PRICING required attributes
            // Pricing attributes are validated separately in combinations
            foreach (var categoryAttr in categoryAttributes.Where(ca => ca.IsRequired && !ca.AffectsPricing))
            {
                var itemAttr = Model.ItemAttributes?.FirstOrDefault(ia => ia.AttributeId == categoryAttr.AttributeId);
                if (itemAttr == null || string.IsNullOrWhiteSpace(itemAttr.Value))
                {
                    Console.WriteLine($"❌ Required non-pricing attribute '{categoryAttr.Title}' is missing or empty");
                    return false;
                }
            }

            Console.WriteLine($"✅ All required non-pricing attributes are valid");
            return true;
        }

        protected void CloseModal()
        {
            Navigation.NavigateTo("/Products");
        }

        // ========== Wizard Navigation Methods ==========
        protected async Task<bool> ValidateCurrentStep()
        {
            switch (currentStep)
            {
                case 0: // Basic Information
                    return !string.IsNullOrWhiteSpace(Model.TitleAr) &&
                           !string.IsNullOrWhiteSpace(Model.TitleEn) &&
                           !string.IsNullOrWhiteSpace(Model.ShortDescriptionAr) &&
                           !string.IsNullOrWhiteSpace(Model.ShortDescriptionEn) &&
                           !string.IsNullOrWhiteSpace(Model.DescriptionAr) &&
                           !string.IsNullOrWhiteSpace(Model.DescriptionEn);

                case 1: // SEO
                    return !string.IsNullOrWhiteSpace(Model.SEOTitle) &&
                           !string.IsNullOrWhiteSpace(Model.SEODescription) &&
                           !string.IsNullOrWhiteSpace(Model.SEOMetaTags);

                case 2: // Classification
                    return Model.CategoryId != Guid.Empty &&
                           Model.BrandId != Guid.Empty &&
                           Model.UnitId != Guid.Empty;

                case 3: // Media
                    return !string.IsNullOrEmpty(Model.ThumbnailImage) ||
                           (Model.Images != null && Model.Images.Count > 0);

                case 4: // Attributes - FIXED VALIDATION
                        // Sync the attribute values first
                    if (attributeValuesSectionRef != null)
                    {
                        await OnAttributeValuesChanged();

                        var (nonPricing, pricing, modifiers) = attributeValuesSectionRef.GetAllAttributeData();

                        Console.WriteLine($"🔍 Step 4 Validation:");
                        Console.WriteLine($"  Total pricing attributes: {categoryAttributes.Count(a => a.AffectsPricing)}");
                        Console.WriteLine($"  Pricing dictionary keys: {pricing.Count}");

                        // If there are pricing attributes, ensure they have values
                        if (categoryAttributes.Any(a => a.AffectsPricing))
                        {
                            // Check if all REQUIRED pricing attributes have at least one non-empty value
                            var requiredPricingAttrs = categoryAttributes.Where(a => a.AffectsPricing && a.IsRequired).ToList();

                            foreach (var attr in requiredPricingAttrs)
                            {
                                Console.WriteLine($"  Checking required attribute: {attr.Title} (AttributeId: {attr.AttributeId})");

                                // FIX: Use AttributeId to look up values
                                if (!pricing.ContainsKey(attr.AttributeId))
                                {
                                    Console.WriteLine($"    ❌ Not found in pricing dictionary");
                                    await ShowErrorMessage(
                                        ValidationResources.ValidationError,
                                        $"Please add at least one value for the required attribute: {attr.Title}");
                                    return false;
                                }

                                var values = pricing[attr.AttributeId].Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
                                Console.WriteLine($"    Values count: {values.Count}");

                                if (!values.Any())
                                {
                                    Console.WriteLine($"    ❌ No non-empty values");
                                    await ShowErrorMessage(
                                        ValidationResources.ValidationError,
                                        $"Please add at least one value for the required attribute: {attr.Title}");
                                    return false;
                                }

                                Console.WriteLine($"    ✅ Has {values.Count} value(s)");
                            }

                            // Check if at least one pricing attribute has values
                            var hasAnyValues = pricing.Any(kvp => kvp.Value.Any(v => !string.IsNullOrWhiteSpace(v)));
                            Console.WriteLine($"  Has any pricing values: {hasAnyValues}");

                            if (!hasAnyValues)
                            {
                                await ShowErrorMessage(
                                    ValidationResources.ValidationError,
                                    "Please add values for at least one pricing attribute before creating combinations.");
                                return false;
                            }
                        }
                    }
                    return true;

                default:
                    return true;
            }
        }

        protected async Task MoveToNextStep()
        {
            Console.WriteLine($"🔍 MoveToNextStep called - currentStep: {currentStep}, TotalSteps: {TotalSteps}");

            if (await ValidateCurrentStep())
            {
                Console.WriteLine($"✅ Current step validation passed");

                // If moving from step 4 (attributes), sync the data
                if (currentStep == 4 && attributeValuesSectionRef != null)
                {
                    Console.WriteLine($"🔄 Syncing attribute values");
                    await OnAttributeValuesChanged();
                }

                if (isWizardInitialized)
                {
                    Console.WriteLine($"✅ Wizard is initialized");

                    if (currentStep < TotalSteps - 1)
                    {
                        Console.WriteLine($"➡️ Moving to next step: {currentStep} -> {currentStep + 1}");
                        currentStep++;
                        await JSRuntime.InvokeVoidAsync("moveToNextStep");
                        StateHasChanged();
                    }
                    else
                    {
                        Console.WriteLine($"🏁 This is the last step, saving product");
                        // This is the last step, save the product
                        await Save();
                    }
                }
                else
                {
                    Console.WriteLine($"⚠️ Wizard is not initialized");
                }
            }
            else
            {
                Console.WriteLine($"❌ Current step validation failed");
                // Show specific validation message based on current step
                string errorMessage = GetStepValidationMessage(currentStep);
                await ShowErrorMessage(
                    ValidationResources.ValidationError,
                    errorMessage);
            }
        }
        // protected async Task MoveToNextStep()
        // {
        //     if (await ValidateCurrentStep())
        //     {
        //         // If moving from step 5 (attributes), sync the data
        //         if (currentStep == 4 && attributeValuesSectionRef != null)
        //         {
        //             await OnAttributeValuesChanged();
        //         }
        //         if (isWizardInitialized && currentStep < TotalSteps - 1)
        //         {
        //             currentStep++;
        //             await JSRuntime.InvokeVoidAsync("moveToNextStep");
        //             StateHasChanged();
        //         }
        //     }
        //     else
        //     {
        //         // Show specific validation message based on current step
        //         string errorMessage = GetStepValidationMessage(currentStep);
        //         await ShowErrorMessage(
        //ValidationResources.ValidationError,
        // errorMessage);
        //     }
        // }

        // Add these methods to Details.razor.cs

        protected async Task AddCombination(ItemCombinationDto combination)
        {
            // Generate a new ID for the combination
            combination.Id = Guid.NewGuid();
            Model.ItemCombinations.Add(combination);

            // If this is set as default, unset others
            if (combination.IsDefault)
            {
                foreach (var combo in Model.ItemCombinations)
                {
                    if (combo.Id != combination.Id)
                        combo.IsDefault = false;
                }
            }

            StateHasChanged();
        }

        protected async Task UpdateCombination(ItemCombinationDto combination)
        {
            // Find and update the existing combination
            var existing = Model.ItemCombinations.FirstOrDefault(c => c.Id == combination.Id);
            if (existing != null)
            {
                // Update properties
                existing.Barcode = combination.Barcode;
                existing.SKU = combination.SKU;
                existing.BasePrice = combination.BasePrice;
                existing.IsDefault = combination.IsDefault;
                existing.CombinationAttributes = combination.CombinationAttributes;

                // If this is set as default, unset others
                if (combination.IsDefault)
                {
                    foreach (var combo in Model.ItemCombinations)
                    {
                        if (combo.Id != combination.Id)
                            combo.IsDefault = false;
                    }
                }
            }

            StateHasChanged();
        }

        protected async Task RemoveCombination(ItemCombinationDto combination)
        {
            Model.ItemCombinations.Remove(combination);

            // If we removed the default and there are others, set a new default
            if (combination.IsDefault && Model.ItemCombinations.Any())
            {
                Model.ItemCombinations.First().IsDefault = true;
            }

            StateHasChanged();
        }

        private string GetStepValidationMessage(int step)
        {
            return step switch
            {
                0 => ValidationResources.FillBasicInformationFields,
                1 => ValidationResources.FillSEOFields,
                2 => ValidationResources.SelectCategoryBrandUnit,
                3 => ValidationResources.UploadThumbnailOrImages,
                4 => "Please add values for all required attributes. For pricing attributes, you must add at least one value to create combinations.",
                _ => ValidationResources.PleaseFixValidationErrors
            };
        }

        protected async Task MoveToPreviousStep()
        {
            if (isWizardInitialized && currentStep > 0)
            {
                currentStep--;
                await JSRuntime.InvokeVoidAsync("moveToPreviousStep");
                StateHasChanged();
            }
        }

        // ========== Helper Methods ==========
        private async Task<string> ConvertFileToBase64(IBrowserFile file)
        {
            try
            {
                using var stream = file.OpenReadStream(MaxFileSize);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return $"{Convert.ToBase64String(memoryStream.ToArray())}";
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(
                ValidationResources.Error,
                        $"{ValidationResources.ErrorProcessingFile}: {ex.Message}");
                return null;
            }
        }

        private async Task ShowErrorMessage(string title, string message, string type = "error")
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, type);
        }

        private async Task ShowSuccessMessage(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
        }

        private void ValidateForm()
        {
            // REMOVED: Quantity validation - now handled by combinations
            // Thumbnail/Image validation - either thumbnail or at least one image is required
            fieldValidation["ThumbnailImage"] = !string.IsNullOrEmpty(Model.ThumbnailImage) ||
                 (Model.Images != null && Model.Images.Count > 0);
        }

        private bool IsFormValid()
        {
            // Check all validation fields
            var basicValidations = fieldValidation.Values.All(valid => valid);

            // Additional check for images if needed
            bool imagesValid = !string.IsNullOrEmpty(Model.ThumbnailImage) ||
        (Model.Images != null && Model.Images.Count > 0);

            return basicValidations && imagesValid;
        }

        private string GetCombinationAttributesDisplay(ItemCombinationDto combination)
        {
            if (combination?.CombinationAttributes == null || !combination.CombinationAttributes.Any())
                return "Default Combination";

            var attributes = new List<string>();

            foreach (var combinationAttr in combination.CombinationAttributes)
            {
                foreach (var attrValue in combinationAttr.combinationAttributeValueDtos)
                {
                    var categoryAttr = categoryAttributes
                        .FirstOrDefault(ca => ca.AttributeId == attrValue.AttributeId);

                    if (categoryAttr != null && !string.IsNullOrEmpty(attrValue.Value))
                    {
                        attributes.Add($"{categoryAttr.Title}: {attrValue.Value}");
                    }
                }
            }

            return attributes.Any() ? string.Join(" | ", attributes) : "Default Combination";
        }


        /// <summary>
        /// Sets a combination as the default one
        /// </summary>
        private void SetDefaultCombination(ItemCombinationDto combination)
        {
            // Unmark all others
            foreach (var combo in Model.ItemCombinations)
            {
                combo.IsDefault = false;
            }

            // Mark this one as default
            combination.IsDefault = true;

            Console.WriteLine($"✅ Set combination as default: {GetCombinationAttributesDisplay(combination)}");
            StateHasChanged();
        }

        /// <summary>
        /// Validates attribute combinations have valid pricing
        /// </summary>
        private async Task<bool> ValidateAttributeCombinations()
        {
            Console.WriteLine($"🔍 ValidateAttributeCombinations - START");
            Console.WriteLine($"📊 Total combinations: {Model.ItemCombinations.Count}");

            if (!Model.ItemCombinations.Any())
            {
                Console.WriteLine($"❌ No combinations found - at least one required");
                await ShowErrorMessage(
                    ValidationResources.ValidationError,
                    "At least one attribute combination is required.");
                return false;
            }

            var hasErrors = false;
            var errorMessages = new List<string>();

            // Check if at least one combination is marked as default
            if (!Model.ItemCombinations.Any(c => c.IsDefault))
            {
                hasErrors = true;
                errorMessages.Add("At least one combination must be marked as default");
                Console.WriteLine($"❌ No default combination found");
            }

            for (int i = 0; i < Model.ItemCombinations.Count; i++)
            {
                var combination = Model.ItemCombinations[i];
                var displayName = GetCombinationAttributesDisplay(combination);

                Console.WriteLine($"🔍 Validating combination {i + 1}: {displayName}");

                // Validate BasePrice
                if (combination.BasePrice <= 0)
                {
                    hasErrors = true;
                    var errorMsg = $"Combination '{displayName}' must have a base price greater than 0";
                    errorMessages.Add(errorMsg);
                    Console.WriteLine($"  ❌ {errorMsg}");
                }
                else
                {
                    Console.WriteLine($"  ✅ Base Price: {combination.BasePrice}");
                }
            }

            if (hasErrors)
            {
                Console.WriteLine($"❌ Validation FAILED - {errorMessages.Count} error(s)");

                // Show all errors in a single message
                var errorMessage = string.Join("\n", errorMessages);
                ShowErrorMessage(
                    ValidationResources.ValidationError,
                    $"Please fix the following errors in attribute combinations:\n\n{errorMessage}").Wait();

                return false;
            }

            Console.WriteLine($"✅ ValidateAttributeCombinations - PASSED");
            return true;
        }
        private string GetImageSourceForDisplay(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return string.Empty;

            // Check if it's already a full data URL
            if (imagePath.StartsWith("data:image/"))
                return imagePath;

            // Check if it's a base64 string (new uploads)
            if (imagePath.Length > 200)
                return $"data:image/png;base64,{imagePath}";

            // If it's a path to an image on the server
            return baseUrl + imagePath;
        }
        // Add this method to your Details.razor.cs class
        // Add this method to your Details.razor.cs class


    }
}