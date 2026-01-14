using Common.Enumerations.Pricing;
using Dashboard.Configuration;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Setting;
using Dashboard.Pages.Catalog.Products.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Brand;
using Shared.DTOs.Catalog.Category;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.Catalog.Unit;
using Shared.DTOs.Media;
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
        [Inject] protected IDevelopmentSettingsService DevelopmentSettingsService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected string baseUrl = string.Empty;

        // Wizard state
        protected bool isWizardInitialized { get; set; }
        protected bool isMultiVendorEnabled { get; set; }
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
            //ItemCombinations = new List<ItemCombinationDto>
            //{
            //    // Initialize with a default combination
            //    new ItemCombinationDto
            //    {
            //        BasePrice = 0,
            //        CombinationAttributes = new List<CombinationAttributeDto>(),
            //        IsDefault = true
            //    }
            //}
        };

        // Validation states
        protected bool showValidationErrors = false;
        protected Dictionary<string, bool> fieldValidation = new Dictionary<string, bool>();

        // Data collections
        private IEnumerable<CategoryDto> categories = Array.Empty<CategoryDto>();
        private CategoryDto currentCategory = new CategoryDto();
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

            LoadData();
            _initialized = true;
        }

        protected override void OnParametersSet()
        {
            if (!_initialized)
            {
                return;
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
                    LoadBrands(),
                    CheckIsMultiVendorEnabled()
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

        private async Task CheckIsMultiVendorEnabled()
        {
            var result = await DevelopmentSettingsService.CheckIsMultiVendorEnabledAsync();
            if (result?.Success == true)
            {
                isMultiVendorEnabled = result.Data ;
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
                isLoadingAttributes = true;
                StateHasChanged();

                var result = await AttributeService.GetByCategoryIdAsync(Model.CategoryId);

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
                        Model.ItemAttributes = categoryAttributes
                            .Select(a => new ItemAttributeDto
                            {
                                // FIX: Use AttributeId, not Id
                                AttributeId = a.AttributeId,
                                Value = string.Empty
                            })
                            .ToList();
                    }
                    else
                    {
                        // For existing products, ensure all category attributes have entries
                        foreach (var categoryAttr in categoryAttributes)
                        {
                            // FIX: Use AttributeId, not Id
                            if (!Model.ItemAttributes.Any(ia => ia.AttributeId == categoryAttr.AttributeId))
                            {
                                Model.ItemAttributes.Add(new ItemAttributeDto
                                {
                                    AttributeId = categoryAttr.AttributeId,
                                    Value = string.Empty
                                });
                            }
                        }
                    }
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
                StateHasChanged();

                // Give Blazor time to render
                await Task.Delay(100);
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
                        currentCategory = categories.Where(c => c.Id == Model.CategoryId).FirstOrDefault() ?? new CategoryDto();
                        await LoadCategoryAttributes();
                        // IMPORTANT: Load existing attribute values into the component
                        // Give the component time to initialize after attributes are loaded
                        await Task.Delay(100);

                        if (attributeValuesSectionRef != null && Model.ItemAttributes != null)
                        {
                            attributeValuesSectionRef.LoadExistingValues(Model.ItemAttributes);
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Cannot load values - Ref: {attributeValuesSectionRef != null}, Attributes: {Model.ItemAttributes != null}");
                        }
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
                // Reset attribute values when category changes
                attributeValues = new Dictionary<Guid, List<string>>();
                currentCategory = categories.FirstOrDefault(c => c.Id == Model.CategoryId) ?? new CategoryDto();
                // Load category attributes
                await LoadCategoryAttributes();
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
                var nonPricingValues = attributeValuesSectionRef.GetAllAttributeData();

                // Update the dictionaries
                nonPricingAttributeValues = nonPricingValues;

                // Update the model's ItemAttributes for non-pricing attributes
                UpdateModelAttributes();
            }

            StateHasChanged();
        }

        private void UpdateModelAttributes()
        {

            // Clear existing attributes
            Model.ItemAttributes = new List<ItemAttributeDto>();

            // Add non-pricing attributes from the form
            foreach (var kvp in nonPricingAttributeValues)
            {

                if (!string.IsNullOrWhiteSpace(kvp.Value))
                {
                    // Match using AttributeId instead of Id
                    var categoryAttr = categoryAttributes.FirstOrDefault(ca => ca.AttributeId == kvp.Key);

                    if (categoryAttr != null)
                    {
                        Model.ItemAttributes.Add(new ItemAttributeDto
                        {
                            AttributeId = categoryAttr.AttributeId,
                            Value = kvp.Value
                        });
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Could not find category attribute for AttributeId: {kvp.Key}");
                    }
                }
                else
                {
                    Console.WriteLine($"⏭️ Skipped (empty value)");
                }
            }

            foreach (var attr in Model.ItemAttributes)
            {
                var catAttr = categoryAttributes.FirstOrDefault(ca => ca.AttributeId == attr.AttributeId);
                Console.WriteLine($"    - {catAttr?.Title ?? "Unknown"}: {attr.Value}");
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

        /// <summary>
        /// Checks if a combination contains a specific attribute
        /// </summary>
        private bool CombinationContainsAttribute(ItemCombinationDto combination, Guid attributeId)
        {
            if (combination?.CombinationAttributes == null)
                return false;

            return combination.CombinationAttributes
                .Any(ca => ca.combinationAttributeValue.AttributeId == attributeId);
        }

        protected async Task Save()
        {
            try
            {
                ValidateForm();

                // Step 2: Validate attributes
                if (!ValidateAttributes())
                {
                    Console.WriteLine("  ❌ ValidateAttributes() FAILED");
                    showValidationErrors = true;
                    await ShowErrorMessage(
                        ValidationResources.ValidationError,
                        "Please fill in all required attributes before saving.");
                    return;
                }

                // Step 3: Validate attribute combinations
                //if (!(await ValidateAttributeCombinations()))
                //{
                //    Console.WriteLine("  ❌ ValidateAttributeCombinations() FAILED");
                //    showValidationErrors = true;
                //    return;
                //}

                // Step 3: Check if form is valid
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

                        await ShowErrorMessage(
                            ValidationResources.ValidationError,
                            ValidationResources.PleaseFixValidationErrors);
                    }
                    return;
                }

                isSaving = true;
                StateHasChanged();

                // Step 4: Prepare the model based on pricing strategy
                PrepareModelForSave();

                //if (Model.Id == Guid.Empty)
                //{
                //    foreach (var comb in Model.ItemCombinations ?? new())
                //    {
                //        comb.Id = Guid.Empty;
                //    }
                //}

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
            }
        }

        /// <summary>
        /// Prepares the model for saving based on the category's pricing strategy
        /// This method handles the business logic for what should be created during item save
        /// </summary>
        private void PrepareModelForSave()
        {
            Console.WriteLine("📦 PrepareModelForSave - Start");
            Console.WriteLine($"   Category Pricing Type: {currentCategory.PricingSystemType}");

            // The backend will handle creation of:
            // 1. Default combination (for Simple pricing only)
            // 2. Default offer (for Single-vendor + Simple pricing only)
            // 3. Default offer combination pricing (for Single-vendor + Simple pricing only)

            // For the admin dashboard, we just need to ensure the basic item data is correct
            // The ItemService.SaveAsync will handle the rest based on:
            // - isMultiVendorMode (from settings)
            // - category.PricingSystemType (from selected category)

            // Ensure item attributes are properly set
            if (attributeValuesSectionRef != null)
            {
                var attributeData = attributeValuesSectionRef.GetAllAttributeData();

                Model.ItemAttributes = new List<ItemAttributeDto>();

                foreach (var kvp in attributeData)
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        Model.ItemAttributes.Add(new ItemAttributeDto
                        {
                            AttributeId = kvp.Key,
                            Value = kvp.Value
                        });
                    }
                }

                Console.WriteLine($"   ✅ Prepared {Model.ItemAttributes.Count} item attributes");
            }

            // For Simple pricing, ensure base price is set
            if (currentCategory.PricingSystemType == PricingStrategyType.Simple)
            {
                if (!Model.BasePrice.HasValue || Model.BasePrice <= 0)
                {
                    Console.WriteLine("   ⚠️ Base price not set for Simple pricing - this will fail validation");
                }
                else
                {
                    Console.WriteLine($"   ✅ Base price set: {Model.BasePrice}");
                }
            }

            // Log what will be created on the backend
            Console.WriteLine("📋 Expected backend behavior:");
            Console.WriteLine($"   - Item will be created with {Model.Images?.Count ?? 0} images");
            Console.WriteLine($"   - Item has {Model.ItemAttributes?.Count ?? 0} attributes");

            if (currentCategory.PricingSystemType == PricingStrategyType.Simple)
            {
                Console.WriteLine($"   - Default combination will be created (BasePrice: {Model.BasePrice})");
                Console.WriteLine($"   - Default offer creation depends on multi-vendor mode");
            }
            else
            {
                Console.WriteLine($"   - No default combination/offer will be created");
                Console.WriteLine($"   - Combinations must be created separately");
            }

            Console.WriteLine("📦 PrepareModelForSave - Complete");
        }


        /// <summary>
        /// Validates all required attributes have values
        /// </summary>
        private bool ValidateAttributes()
        {
            if (Model.CategoryId == Guid.Empty || !categoryAttributes.Any())
                return true; // No attributes to validate

            // Only validate non-pricing attributes (pricing attributes are handled in combinations)
            foreach (var categoryAttr in categoryAttributes.Where(ca => ca.IsRequired && !ca.AffectsPricing))
            {
                var itemAttr = Model.ItemAttributes?.FirstOrDefault(ia => ia.AttributeId == categoryAttr.AttributeId);
                if (itemAttr == null || string.IsNullOrWhiteSpace(itemAttr.Value))
                {
                    return false;
                }
            }
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
                case 4: //Default pricing
                    if (currentCategory.PricingSystemType == PricingStrategyType.Simple)
                        return Model.BasePrice.HasValue &&
                               Model.BasePrice.Value > 0;
                    else
                        return true;
                case 5: // Attributes - FIXED VALIDATION
                        // Sync the attribute values first
                    if (attributeValuesSectionRef != null)
                    {
                        await OnAttributeValuesChanged();

                        var nonPricing = attributeValuesSectionRef.GetAllAttributeData();

                    }
                    return true;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Gets a user-friendly message explaining what will be created
        /// </summary>
        private string GetPricingStrategyMessage()
        {
            if (currentCategory.PricingSystemType == PricingStrategyType.Simple)
            {
                return "A default combination and pricing will be created automatically. " +
                       "This item will be available for sale immediately after approval.";
            }
            else if (currentCategory.PricingSystemType == PricingStrategyType.CombinationBased)
            {
                return "You will need to create item combinations with specific attribute values " +
                       "after saving this item. Vendors can then add their offers to these combinations.";
            }
            else if (currentCategory.PricingSystemType == PricingStrategyType.Hybrid)
            {
                return "This category supports both simple and combination-based pricing. " +
                       "You can create multiple combinations with different attribute values.";
            }

            return "Item will be created based on the category's pricing strategy.";
        }


        protected async Task MoveToNextStep()
        {

            if (await ValidateCurrentStep())
            {

                // If moving from step 4 (attributes), sync the data
                if (currentStep == 4 && attributeValuesSectionRef != null)
                {
                    await OnAttributeValuesChanged();
                }

                if (isWizardInitialized)
                {

                    if (currentStep < TotalSteps - 1)
                    {
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


        private string GetStepValidationMessage(int step)
        {
            return step switch
            {
                0 => ValidationResources.FillBasicInformationFields,
                1 => ValidationResources.FillSEOFields,
                2 => ValidationResources.SelectCategoryBrandUnit,
                3 => ValidationResources.UploadThumbnailOrImages,
                4 => "Please enter item base price.",
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
        //protected async Task AddCombination(ItemCombinationDto combination)
        //{
        //    // Generate a new ID for the combination
        //    combination.Id = Guid.NewGuid();
        //    Model.ItemCombinations.Add(combination);

        //    // If this is set as default, unset others
        //    if (combination.IsDefault)
        //    {
        //        foreach (var combo in Model.ItemCombinations)
        //        {
        //            if (combo.Id != combination.Id)
        //                combo.IsDefault = false;
        //        }
        //    }

        //    StateHasChanged();
        //}

        //protected async Task UpdateCombination(ItemCombinationDto combination)
        //{
        //    // Find and update the existing combination
        //    var existing = Model.ItemCombinations.FirstOrDefault(c => c.Id == combination.Id);
        //    if (existing != null)
        //    {
        //        // Update properties
        //        existing.Barcode = combination.Barcode;
        //        existing.SKU = combination.SKU;
        //        existing.BasePrice = combination.BasePrice;
        //        existing.IsDefault = combination.IsDefault;
        //        existing.CombinationAttributes = combination.CombinationAttributes;

        //        // If this is set as default, unset others
        //        if (combination.IsDefault)
        //        {
        //            foreach (var combo in Model.ItemCombinations)
        //            {
        //                if (combo.Id != combination.Id)
        //                    combo.IsDefault = false;
        //            }
        //        }
        //    }

        //    StateHasChanged();
        //}

        //protected async Task RemoveCombination(ItemCombinationDto combination)
        //{
        //    Model.ItemCombinations.Remove(combination);

        //    // If we removed the default and there are others, set a new default
        //    if (combination.IsDefault && Model.ItemCombinations.Any())
        //    {
        //        Model.ItemCombinations.First().IsDefault = true;
        //    }

        //    StateHasChanged();
        //}

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

        //private string GetCombinationAttributesDisplay(ItemCombinationDto combination)
        //{
        //    if (combination?.CombinationAttributes == null || !combination.CombinationAttributes.Any())
        //        return "Default Combination";

        //    var attributes = new List<string>();

        //    foreach (var combinationAttr in combination.CombinationAttributes)
        //    {
        //        foreach (var attrValue in combinationAttr.combinationAttributeValueDtos)
        //        {
        //            var categoryAttr = categoryAttributes
        //                .FirstOrDefault(ca => ca.AttributeId == attrValue.AttributeId);

        //            if (categoryAttr != null && !string.IsNullOrEmpty(attrValue.Value))
        //            {
        //                attributes.Add($"{categoryAttr.Title}: {attrValue.Value}");
        //            }
        //        }
        //    }

        //    return attributes.Any() ? string.Join(" | ", attributes) : "Default Combination";
        //}


        /// <summary>
        /// Sets a combination as the default one
        /// </summary>
        //private void SetDefaultCombination(ItemCombinationDto combination)
        //{
        //    // Unmark all others
        //    foreach (var combo in Model.ItemCombinations)
        //    {
        //        combo.IsDefault = false;
        //    }

        //    // Mark this one as default
        //    combination.IsDefault = true;

        //    Console.WriteLine($"✅ Set combination as default: {GetCombinationAttributesDisplay(combination)}");
        //    StateHasChanged();
        //}

        /// <summary>
        /// Validates attribute combinations have valid pricing
        /// </summary>
        //private async Task<bool> ValidateAttributeCombinations()
        //{
        //    Console.WriteLine($"🔍 ValidateAttributeCombinations - START");
        //    Console.WriteLine($"📊 Total combinations: {Model.ItemCombinations.Count}");

        //    if (!Model.ItemCombinations.Any())
        //    {
        //        Console.WriteLine($"❌ No combinations found - at least one required");
        //        await ShowErrorMessage(
        //            ValidationResources.ValidationError,
        //            "At least one attribute combination is required.");
        //        return false;
        //    }

        //    var hasErrors = false;
        //    var errorMessages = new List<string>();

        //    // Check if at least one combination is marked as default
        //    if (!Model.ItemCombinations.Any(c => c.IsDefault))
        //    {
        //        hasErrors = true;
        //        errorMessages.Add("At least one combination must be marked as default");
        //        Console.WriteLine($"❌ No default combination found");
        //    }

        //    for (int i = 0; i < Model.ItemCombinations.Count; i++)
        //    {
        //        var combination = Model.ItemCombinations[i];
        //        var displayName = GetCombinationAttributesDisplay(combination);

        //        Console.WriteLine($"🔍 Validating combination {i + 1}: {displayName}");

        //        // Validate BasePrice
        //        if (combination.BasePrice <= 0)
        //        {
        //            hasErrors = true;
        //            var errorMsg = $"Combination '{displayName}' must have a base price greater than 0";
        //            errorMessages.Add(errorMsg);
        //            Console.WriteLine($"  ❌ {errorMsg}");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"  ✅ Base Price: {combination.BasePrice}");
        //        }
        //    }

        //    if (hasErrors)
        //    {
        //        Console.WriteLine($"❌ Validation FAILED - {errorMessages.Count} error(s)");

        //        // Show all errors in a single message
        //        var errorMessage = string.Join("\n", errorMessages);
        //        ShowErrorMessage(
        //            ValidationResources.ValidationError,
        //            $"Please fix the following errors in attribute combinations:\n\n{errorMessage}").Wait();

        //        return false;
        //    }

        //    Console.WriteLine($"✅ ValidateAttributeCombinations - PASSED");
        //    return true;
        //}
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
    }
}