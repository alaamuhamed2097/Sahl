using Dashboard.Configuration;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
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


namespace Dashboard.Pages.ECommerce.Item
{
    public partial class Item
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
        [Inject] protected IUnitService UnitService { get; set; } = null!;
        //   [Inject] protected IVideoProviderService VideoProviderService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected string baseUrl = string.Empty;
        // State variables
        protected bool isSaving { get; set; }
        protected bool isProcessing { get; set; }
        protected int processingProgress { get; set; }
        protected ItemDto Model { get; set; } = new() { Images = new List<ItemImageDto>() };

        // Validation states
        protected bool showValidationErrors = false;
        protected Dictionary<string, bool> fieldValidation = new Dictionary<string, bool>();

        // Data collections
        private IEnumerable<CategoryDto> categories = Array.Empty<CategoryDto>();
        private IEnumerable<UnitDto> units = Array.Empty<UnitDto>();
        private IEnumerable<VideoProviderDto> videoProviders = Array.Empty<VideoProviderDto>();
        private List<CategoryAttributeDto> categoryAttributes = new();
        private List<BrandDto> brands = new();
        private bool isLoadingAttributes = false;

        // ========== Lifecycle Methods ==========
        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            // Initialize validation dictionary
            fieldValidation["CategoryId"] = true;
            fieldValidation["UnitId"] = true;
            fieldValidation["Quantity"] = true;
            fieldValidation["ThumbnailImage"] = true;

            await LoadData();
        }

        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                _ = LoadProduct(Id);
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
                isLoadingAttributes = true;
                var result = await CategoryService.GetByIdAsync(Model.CategoryId);

                if (result?.Success == true)
                {
                    categoryAttributes = result.Data?.CategoryAttributes ?? new List<CategoryAttributeDto>();

                    // Initialize attributes if this is a new product
                    if (Id == Guid.Empty)
                    {
                        Model.ItemAttributes = categoryAttributes
                            .Select(a => new ItemAttributeDto
                            {
                                AttributeId = a.Id,
                                Value = string.Empty
                            })
                            .ToList();
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
            finally
            {
                isLoadingAttributes = false;
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
            //fieldValidation["CategoryId"] = Model.CategoryId != Guid.Empty;

            //if (Model.CategoryId != Guid.Empty)
            //{
            //    await LoadCategoryAttributes();
            //}
            //else
            //{
            //    categoryAttributes.Clear();
            //}
            //StateHasChanged();
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

        private async Task HandleCombinationImageUpload(InputFileChangeEventArgs e, ItemAttributeCombinationPricingDto combination)
        {
            try
            {
                if (e.File == null) return;

                // Validate file size
                if (e.File.Size > MaxFileSize)
                {
                    await ShowErrorMessage(
                        ValidationResources.Error,
                        $"{e.File.Name} {string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / 1024 / 1024)} {MaxFileSize / 1024 / 1024}MB");
                    return;
                }

                // Validate content type
                if (!e.File.ContentType.StartsWith("image/"))
                {
                    await ShowErrorMessage(
                        ValidationResources.Error,
                        $"{e.File.Name} {ValidationResources.NotValidImage}");
                    return;
                }

                combination.Image = await ConvertFileToBase64(e.File);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        //private void UpdateAttributeValue(Guid attributeId, ChangeEventArgs e)
        //{
        //    try
        //    {
        //        Console.WriteLine(e.Value);
        //        var value = e.Value?.ToString() ?? string.Empty;
        //        var attribute = Model.ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId);
        //        if (attribute != null)
        //        {
        //            attribute.Value = value;
        //        }
        //        else
        //        {
        //            Model.ItemAttributes.Add(new ItemAttributeDto
        //            {
        //                AttributeId = attributeId,
        //                Value = value
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

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
            Model.ItemAttributeCombinationPricings.Remove(combination);
        }

        protected async Task Save()
        {
            try
            {
                // Validate required fields
                ValidateForm();

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

        protected void CloseModal()
        {
            Navigation.NavigateTo("/Products");
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
            // Quantity validation - only validate if stock status is true (in stock)
            fieldValidation["Quantity"] = !Model.StockStatus || Model.Quantity > 0;

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
                if (Guid.TryParse(id, out var attributeId))
                {
                    var attribute = categoryAttributes.FirstOrDefault(a => a.Id == attributeId);
                    var value = Model.ItemAttributes.FirstOrDefault(a => a.AttributeId == attributeId)?.Value;

                    if (attribute != null && !string.IsNullOrEmpty(value))
                    {
                        attributes.Add($"{attribute.Title}: {value}");
                    }
                }
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