using Dashboard.Configuration;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Brand;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;
using Shared.DTOs.ECommerce.Unit;
using Shared.DTOs.Media;


namespace Dashboard.Pages.Catalog.Products
{
    public partial class Details
    {
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
        protected const int TotalSteps = 5;

        // State variables
        protected bool isSaving { get; set; }
        protected bool isProcessing { get; set; }
        protected int processingProgress { get; set; }
        protected ItemDto Model { get; set; } = new()
        {
            Images = new List<ItemImageDto>(),
            ItemAttributes = new List<ItemAttributeDto>(),
            ItemAttributeCombinationPricings = new List<ItemAttributeCombinationPricingDto>
            {
                // Initialize with a default combination
                new ItemAttributeCombinationPricingDto
                {
                    AttributeIds = string.Empty,
                    Price = 0,
                    SalesPrice = 0,
                    Quantity = 0,
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

        // ========== Lifecycle Methods ==========
        // ? FIX: متغير لتتبع التهيئة
        private bool _initialized = false;
        private Guid _lastLoadedId = Guid.Empty;

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

                    Console.WriteLine($"✅ Loaded {categoryAttributes.Count} attributes:");
                    foreach (var attr in categoryAttributes)
                    {
                        Console.WriteLine($"  - ID: {attr.Id}");
                        Console.WriteLine($"    AttributeId: {attr.AttributeId}");
                        Console.WriteLine($"    Title: {attr.Title}");
                        Console.WriteLine($"    TitleAr: {attr.TitleAr}");
                        Console.WriteLine($"    TitleEn: {attr.TitleEn}");
                        Console.WriteLine($"    AffectsPricing: {attr.AffectsPricing}");
                        Console.WriteLine($"    FieldType: {attr.FieldType}");
                        Console.WriteLine($"    AttributeOptionsJson: {attr.AttributeOptionsJson}");
                        
                        // Log parsed options
                        if (attr.AttributeOptions != null && attr.AttributeOptions.Any())
                        {
                            Console.WriteLine($"    ✅ Parsed {attr.AttributeOptions.Count} options:");
                            foreach (var opt in attr.AttributeOptions)
                            {
                                Console.WriteLine($"      • {opt.Title} (ID: {opt.Id}, DisplayOrder: {opt.DisplayOrder})");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"    ⚠️ No options available or parsing failed");
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

                // Call the method that exists in Details.CombinationLogic.cs
                await HandleCategoryChangeWithCombinations();

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

        // ========== UI Action Methods ==========
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
                Model.ItemAttributeCombinationPricings = Model.ItemAttributeCombinationPricings
                  .Where(c => !c.AttributeIds.Split(',').Contains(attributeId.ToString()))
              .ToList();
            }
        }

        private void RemoveCombination(ItemAttributeCombinationPricingDto combination)
        {
            // Don't allow removing the default combination if it's the only one
            if (combination.IsDefault && Model.ItemAttributeCombinationPricings.Count == 1)
            {
                ShowErrorMessage(
                    ValidationResources.Error,
                    "Cannot remove the default combination. At least one combination is required.").Wait();
                return;
            }

            Model.ItemAttributeCombinationPricings.Remove(combination);

            // If we removed the default, set another one as default
            if (combination.IsDefault && Model.ItemAttributeCombinationPricings.Any())
            {
                Model.ItemAttributeCombinationPricings.First().IsDefault = true;
            }
        }

        /// <summary>
        /// Sets a combination as the default one
        /// </summary>
        private void SetDefaultCombination(ItemAttributeCombinationPricingDto combination)
        {
            // Unmark all others
            foreach (var combo in Model.ItemAttributeCombinationPricings)
            {
                combo.IsDefault = false;
            }

            // Mark this one as default
            combination.IsDefault = true;
            
            Console.WriteLine($"✅ Set combination as default: {GetCombinationAttributesDisplay(combination.AttributeIds)}");
            StateHasChanged();
        }
        protected async Task Save()
        {
            try
            {
                // Validate required fields
                ValidateForm();
                
                // Validate attributes
                if (!ValidateAttributes())
                {
                    showValidationErrors = true;
                    await ShowErrorMessage(
                        ValidationResources.ValidationError,
                        "Please fill in all required attributes before saving.");
                    return;
                }

                // Validate attribute combinations
                if (!ValidateAttributeCombinations())
                {
                    showValidationErrors = true;
                    return;
                }

                if (!IsFormValid())
                {
                    showValidationErrors = true;
                    // Highlight which fields are invalid
                    if (!fieldValidation["ThumbnailImage"])
                    {
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

                var result = await ItemService.SaveAsync(Model);

                if (result?.Success == true)
                {
                    await ShowSuccessMessage(
                      ValidationResources.Done,
                   NotifiAndAlertsResources.SavedSuccessfully);

                    Navigation.NavigateTo("/Products");
                }
                else
                {
                    await ShowErrorMessage(
                ValidationResources.Failed,
        result?.Message ?? NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(NotifiAndAlertsResources.FailedAlert, ex.Message);
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
            }
        }
        
        /// <summary>
        /// Validates all required attributes have values
        /// </summary>
        private bool ValidateAttributes()
        {
            if (Model.CategoryId == Guid.Empty || !categoryAttributes.Any())
                return true; // No attributes to validate

            foreach (var categoryAttr in categoryAttributes.Where(ca => ca.IsRequired))
            {
                var itemAttr = Model.ItemAttributes.FirstOrDefault(ia => ia.AttributeId == categoryAttr.AttributeId);
                if (itemAttr == null || string.IsNullOrWhiteSpace(itemAttr.Value))
                {
                    Console.WriteLine($"❌ Required attribute '{categoryAttr.Title}' is missing or empty");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates attribute combinations have valid pricing
        /// </summary>
        private bool ValidateAttributeCombinations()
        {
            Console.WriteLine($"🔍 ValidateAttributeCombinations - START");
            Console.WriteLine($"📊 Total combinations: {Model.ItemAttributeCombinationPricings.Count}");
            
            if (!Model.ItemAttributeCombinationPricings.Any())
            {
                Console.WriteLine($"✅ No combinations to validate");
                return true; // No combinations to validate
            }

            var hasErrors = false;
            var errorMessages = new List<string>();

            for (int i = 0; i < Model.ItemAttributeCombinationPricings.Count; i++)
            {
                var combination = Model.ItemAttributeCombinationPricings[i];
                var displayName = GetCombinationAttributesDisplay(combination.AttributeIds);
                
                Console.WriteLine($"🔍 Validating combination {i + 1}: {displayName}");

                // Validate Price
                if (combination.Price <= 0)
                {
                    hasErrors = true;
                    var errorMsg = $"Combination '{displayName}' must have a price greater than 0";
                    errorMessages.Add(errorMsg);
                    Console.WriteLine($"  ❌ {errorMsg}");
                }
                else
                {
                    Console.WriteLine($"  ✅ Price: {combination.Price}");
                }

                // Validate SalesPrice
                if (combination.SalesPrice <= 0)
                {
                    hasErrors = true;
                    var errorMsg = $"Combination '{displayName}' must have a sales price greater than 0";
                    errorMessages.Add(errorMsg);
                    Console.WriteLine($"  ❌ {errorMsg}");
                }
                else
                {
                    Console.WriteLine($"  ✅ Sales Price: {combination.SalesPrice}");
                }

                // Validate Quantity
                if (combination.Quantity < 0)
                {
                    hasErrors = true;
                    var errorMsg = $"Combination '{displayName}' cannot have a negative quantity";
                    errorMessages.Add(errorMsg);
                    Console.WriteLine($"  ❌ {errorMsg}");
                }
                else
                {
                    Console.WriteLine($"  ✅ Quantity: {combination.Quantity}");
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

                case 4: // Attributes (optional)
                    return true;

                default:
                    return true;
            }
        }

        protected async Task MoveToNextStep()
        {
            if (await ValidateCurrentStep())
            {
                if (isWizardInitialized && currentStep < TotalSteps - 1)
                {
                    currentStep++;
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                    StateHasChanged();
                }
            }
            else
            {
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
                4 => ValidationResources.AttributesOptional,
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

        private string GetCombinationAttributesDisplay(string attributeIds)
        {
            if (string.IsNullOrEmpty(attributeIds))
                return string.Empty;

            var ids = attributeIds.Split(',');
            var attributes = new List<string>();

            foreach (var id in ids)
            {
                if (Guid.TryParse(id, out var guid))
                {
                    // Check if we have a display mapping for this ID
                    if (_optionDisplayMap != null && _optionDisplayMap.ContainsKey(guid))
                    {
                        attributes.Add(_optionDisplayMap[guid]);
                    }
                    else
                    {
                        // Fallback: Try to find in attribute options
                        var categoryAttr = categoryAttributes
                             .FirstOrDefault(ca => ca.AttributeOptions?.Any(o => o.Id == guid) == true);

                        if (categoryAttr != null)
                        {
                            var option = categoryAttr.AttributeOptions.First(o => o.Id == guid);
                            attributes.Add($"{categoryAttr.Title}: {option.Title}");
                        }
                    }
                }
            }

            // If we couldn't resolve any IDs, show a placeholder
            if (!attributes.Any())
            {
                return "Combination (IDs: " + string.Join(", ", ids.Take(2)) + (ids.Length > 2 ? "..." : "") + ")";
            }

            return string.Join(" | ", attributes);
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
    }
}