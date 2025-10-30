using Dashboard.Configuration;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.Category;

namespace Dashboard.Pages.ECommerce.Category;

public partial class Category : IDisposable
{
    private IBrowserFile? selectedImage;
    private string? previewImageUrl;
    private IBrowserFile? selectedIcon;
    private string? previewIconUrl;
    private string baseUrl = string.Empty;
    private bool _disposed = false;
    protected bool IsSaving { get; set; }
    protected const long MaxFileSize = 5 * 1024 * 1024; // 5MB
    protected bool showValidationErrors = false;
    private bool select2Initialized = false;
    private bool dataLoaded = false;

    private Guid parent
    {
        get => Model.ParentId;
        set => Model.ParentId = value;
    }

    protected CategoryDto Model { get; set; } = new() { CategoryAttributes = new List<CategoryAttributeDto>() };
    protected List<CategoryDto> AllCategories { get; set; } = new();
    protected IEnumerable<AttributeDto> availableAttributes { get; set; } = Enumerable.Empty<AttributeDto>();
    protected List<CategoryDto> parents => AllCategories.Where(c => c.Id != Model.Id).ToList();

    [Parameter] public Guid Id { get; set; }
    [Inject] protected ICategoryService CategoryService { get; set; } = null!;
    [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
    [Inject] protected IAttributeService AttributeService { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;
    [Inject] IOptions<ApiSettings> ApiOptions { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        baseUrl = ApiOptions.Value.BaseUrl;
        await Task.WhenAll(
            LoadAllCategories(),
            LoadAvailableAttributes()
        );

        dataLoaded = true;

        if (Id == Guid.Empty)
        {
            Model = CreateNewCategory();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ResourceLoaderService.LoadStyleSheet("css/display_order.css");
            await ResourceLoaderService.LoadScriptsSequential("Common/imageHandler/imageHandler.js", "Common/select2Helper.js");
        }

        // Initialize Select2 after component is rendered and data is loaded
        if (dataLoaded && !select2Initialized)
        {
            // Small delay to ensure DOM is ready
            await Task.Delay(100);
            await InitializeSelect2();
            select2Initialized = true;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            previewImageUrl = null;
            previewIconUrl = null;
            await Edit(Id);

            // Reset initialization flag to force reinitialization
            select2Initialized = false;
            StateHasChanged();
        }
    }

    // Initialize Select2 dropdowns
    // In Category.razor.cs, update the InitializeSelect2 method:

    private async Task InitializeSelect2()
    {
        try
        {
            if (_disposed) return;

            // Check if Select2 is already initialized
            var isParentInitialized = await JSRuntime.InvokeAsync<bool>("isSelect2Initialized", ".select2-parent");
            var isAttributeInitialized = await JSRuntime.InvokeAsync<bool>("isSelect2Initialized", ".select2-attribute");

            // Only initialize if not already initialized
            if (!isParentInitialized || !isAttributeInitialized)
            {
                // Create resources object to pass to JavaScript
                var resources = new
                {
                    parentCategoryPlaceholder = ResourceManager.CurrentLanguage == Language.Arabic
                        ? "اختر الفئة الأساسية"
                        : FormResources.Select + " " + ECommerceResources.CategoryParent,
                    attributePlaceholder = ResourceManager.CurrentLanguage == Language.Arabic
                        ? "اختر الخاصية"
                        : FormResources.Select + " " + ECommerceResources.Attribute,
                    noResults = ResourceManager.CurrentLanguage == Language.Arabic
                        ? "لا توجد نتائج"
                        : "No results found",
                    searching = ResourceManager.CurrentLanguage == Language.Arabic
                        ? "جاري البحث..."
                        : "Searching..."
                };

                await JSRuntime.InvokeVoidAsync("initializeSelect2", resources);
            }
        }
        catch (JSDisconnectedException)
        {
            Console.WriteLine("JS circuit disconnected, skipping Select2 initialization");
        }
        catch (ObjectDisposedException)
        {
            Console.WriteLine("Object disposed, skipping Select2 initialization");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Select2: {ex.Message}");
        }
    }

    // Handle parent category selection change
    private async Task OnParentCategoryChanged(ChangeEventArgs e)
    {
        if (Guid.TryParse(e.Value?.ToString(), out var parentId))
        {
            Model.ParentId = parentId;
        }
        else
        {
            Model.ParentId = Guid.Empty;
        }
        StateHasChanged();
    }

    // Handle attribute selection change
    private async Task OnAttributeChanged(ChangeEventArgs e, CategoryAttributeDto categoryAttribute)
    {
        if (Guid.TryParse(e.Value?.ToString(), out var attributeId))
        {
            categoryAttribute.AttributeId = attributeId;
        }
        StateHasChanged();
    }

    private CategoryDto CreateNewCategory()
    {
        return new CategoryDto
        {
            CategoryAttributes = new List<CategoryAttributeDto>(),
            DisplayOrder = GetNextDisplayOrder(),
            IsMainCategory = false,
            IsFeaturedCategory = false
        };
    }

    private int GetNextDisplayOrder()
    {
        return AllCategories.Any() ? AllCategories.Max(c => c.DisplayOrder) + 1 : 1;
    }

    // ========== Image Handling ==========
    private async Task HandleSelectedImage(InputFileChangeEventArgs e)
    {
        await HandleFileUpload(e.File, "image", (url, base64) =>
        {
            selectedImage = e.File;
            previewImageUrl = url;
            Model.ImageUrl = base64;
        });
    }

    private async Task HandleIconUpload(InputFileChangeEventArgs e)
    {
        await HandleFileUpload(e.File, "icon", (url, base64) =>
        {
            selectedIcon = e.File;
            previewIconUrl = url;
            Model.Icon = base64;
        });
    }

    private async Task HandleFileUpload(IBrowserFile file, string fileType, Action<string, string> onSuccess)
    {
        try
        {
            if (!IsValidImageFile(file))
                return;
            var (isValid, errorMessage) = await ValidateFileSize(file);
            if (!isValid)
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, errorMessage);
                return;
            }
            var (previewUrl, base64Data) = await ProcessImageFile(file);
            if (!string.IsNullOrEmpty(previewUrl) && !string.IsNullOrEmpty(base64Data))
            {
                onSuccess(previewUrl, base64Data);
            }
            else
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
            }
        }
        catch (Exception ex)
        {
            await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
            Console.WriteLine($"Error processing {fileType}: {ex.Message}");
        }
    }

    private bool IsValidImageFile(IBrowserFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.Name).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            _ = ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.InvalidImageFormat);
            return false;
        }
        return true;
    }

    private async Task<(bool isValid, string errorMessage)> ValidateFileSize(IBrowserFile file)
    {
        if (file.Size > MaxFileSize)
        {
            return (false, string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / (1024 * 1024)));
        }
        return (true, string.Empty);
    }

    private async Task<(string previewUrl, string base64Data)> ProcessImageFile(IBrowserFile file)
    {
        using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var imageBytes = ms.ToArray();
        var base64 = Convert.ToBase64String(imageBytes);
        var previewUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, file.ContentType);
        var base64Data = previewUrl?.Replace($"data:{file.ContentType};base64,", "") ?? string.Empty;
        return (previewUrl ?? string.Empty, base64Data);
    }

    private async Task Save()
    {
        if (IsSaving) return; // Prevent double submission
        try
        {
            IsSaving = true;
            StateHasChanged();
            // Validate display order for new categories
            if (Model.Id == Guid.Empty && Model.DisplayOrder <= 0)
            {
                Model.DisplayOrder = GetNextDisplayOrder();
            }
            if (Model.CategoryAttributes != null && Model.CategoryAttributes.Any())
            {
                Model.CategoryAttributes.RemoveAll(attribute => attribute.AttributeId == Guid.Empty);
            }
            // Ensure display order doesn't conflict with existing categories
            await ValidateAndAdjustDisplayOrder();
            var result = await CategoryService.SaveAsync(Model);
            if (result.Success)
            {
                await ShowSuccessNotification(ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully);
                await CloseModal();
            }
            else
            {
                await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed);
            }
        }
        catch (Exception ex)
        {
            await ShowErrorNotification(ValidationResources.Error, ex.Message);
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    private async Task ValidateAndAdjustDisplayOrder()
    {
        // Ensure display order is within valid range
        var maxOrder = AllCategories.Any() ? AllCategories.Max(c => c.DisplayOrder) : 0;
        if (Model.DisplayOrder > maxOrder + 1)
        {
            Model.DisplayOrder = maxOrder + 1;
        }
        if (Model.DisplayOrder <= 0)
        {
            Model.DisplayOrder = 1;
        }
    }

    protected async Task Edit(Guid id)
    {
        try
        {
            var result = await CategoryService.GetByIdAsync(id);
            if (!result.Success || result.Data == null)
            {
                await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
                return;
            }
            Model = result.Data;
            // Ensure proper initialization
            Model.CategoryAttributes ??= new List<CategoryAttributeDto>();
            if (Model.DisplayOrder <= 0)
            {
                Model.DisplayOrder = GetNextDisplayOrder();
            }
            // Normalize attribute display orders if they're not properly set
            if (Model.CategoryAttributes.Any() && Model.CategoryAttributes.Any(attr => attr.DisplayOrder <= 0))
            {
                NormalizeAttributeDisplayOrders();
            }
            // Sort attributes by display order
            Model.CategoryAttributes = Model.CategoryAttributes
                .OrderBy(attr => attr.DisplayOrder)
                .ToList();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await ShowErrorNotification(ValidationResources.Error, NotifiAndAlertsResources.FailedAlert);
            Console.WriteLine($"Error loading category {id}: {ex.Message}");
        }
    }

    protected async Task CloseModal()
    {
        Navigation.NavigateTo("/categories");
    }

    protected async Task LoadAllCategories()
    {
        try
        {
            var result = await CategoryService.GetAllAsync();
            AllCategories = result.Data?.OrderBy(x => x.DisplayOrder).ToList() ?? new List<CategoryDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading categories: {ex.Message}");
            AllCategories = new List<CategoryDto>();
        }
    }

    protected async Task LoadAvailableAttributes()
    {
        try
        {
            var result = await AttributeService.GetAllAsync();
            availableAttributes = result.Success ? result.Data ?? Enumerable.Empty<AttributeDto>() : Enumerable.Empty<AttributeDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading attributes: {ex.Message}");
            availableAttributes = Enumerable.Empty<AttributeDto>();
        }
    }

    // ========== Attribute Management ==========
    private async Task AddNewAttribute()
    {
        Model.CategoryAttributes ??= new List<CategoryAttributeDto>();
        var newAttribute = new CategoryAttributeDto
        {
            IsRequired = false,
            AffectsPricing = false,
            CategoryId = Model.Id,
            AttributeId = Guid.Empty,
            DisplayOrder = GetNextAttributeDisplayOrder()
        };
        Model.CategoryAttributes.Add(newAttribute);

        // Reset initialization flag to force reinitialization
        select2Initialized = false;
        StateHasChanged();
    }

    private async Task RemoveAttribute(int index)
    {
        if (index < 0 || Model.CategoryAttributes == null || index >= Model.CategoryAttributes.Count)
            return;
        var confirmed = await ShowConfirmationDialog(
            NotifiAndAlertsResources.AreYouSure,
            NotifiAndAlertsResources.ConfirmDeleteAlert
        );
        if (confirmed)
        {
            Model.CategoryAttributes.RemoveAt(index);
            NormalizeAttributeDisplayOrders();

            // Reset initialization flag to force reinitialization
            select2Initialized = false;
            StateHasChanged();
        }
    }

    // ========== Display Order Management ==========
    private int GetCategoryCurrentIndex()
    {
        return Model.DisplayOrder;
    }

    private void OnDisplayOrderChanged()
    {
        // Validate and adjust display order
        var maxOrder = AllCategories.Any() ? AllCategories.Max(c => c.DisplayOrder) : 0;
        if (Model.DisplayOrder <= 0)
        {
            Model.DisplayOrder = 1;
        }
        else if (Model.DisplayOrder > maxOrder + 1)
        {
            Model.DisplayOrder = maxOrder + 1;
        }
        StateHasChanged();
    }

    // ========== Display Order Management for Attributes ==========
    private async Task MoveAttributeUp(int index)
    {
        if (index <= 0 || Model.CategoryAttributes == null || index >= Model.CategoryAttributes.Count)
            return;
        try
        {
            var currentAttribute = Model.CategoryAttributes[index];
            var previousAttribute = Model.CategoryAttributes[index - 1];
            // Swap display orders
            var tempOrder = currentAttribute.DisplayOrder;
            currentAttribute.DisplayOrder = previousAttribute.DisplayOrder;
            previousAttribute.DisplayOrder = tempOrder;
            // Sort the list by display order to reflect the change
            Model.CategoryAttributes = Model.CategoryAttributes
                .OrderBy(attr => attr.DisplayOrder)
                .ToList();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error moving attribute up: {ex.Message}");
            await ShowErrorNotification("Error", "Failed to reorder attribute");
        }
    }

    private async Task MoveAttributeDown(int index)
    {
        if (Model.CategoryAttributes == null || index < 0 || index >= Model.CategoryAttributes.Count - 1)
            return;
        try
        {
            var currentAttribute = Model.CategoryAttributes[index];
            var nextAttribute = Model.CategoryAttributes[index + 1];
            // Swap display orders
            var tempOrder = currentAttribute.DisplayOrder;
            currentAttribute.DisplayOrder = nextAttribute.DisplayOrder;
            nextAttribute.DisplayOrder = tempOrder;
            // Sort the list by display order to reflect the change
            Model.CategoryAttributes = Model.CategoryAttributes
                .OrderBy(attr => attr.DisplayOrder)
                .ToList();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error moving attribute down: {ex.Message}");
            await ShowErrorNotification("Error", "Failed to reorder attribute");
        }
    }

    private void NormalizeAttributeDisplayOrders()
    {
        if (Model.CategoryAttributes == null) return;
        // Sort by current display order first
        var sortedAttributes = Model.CategoryAttributes
            .OrderBy(attr => attr.DisplayOrder)
            .ToList();
        // Reassign sequential display orders
        for (int i = 0; i < sortedAttributes.Count; i++)
        {
            sortedAttributes[i].DisplayOrder = i + 1;
        }
        Model.CategoryAttributes = sortedAttributes;
    }

    private int GetNextAttributeDisplayOrder()
    {
        if (Model.CategoryAttributes == null || !Model.CategoryAttributes.Any())
            return 1;
        return Model.CategoryAttributes.Max(attr => attr.DisplayOrder) + 1;
    }

    private bool CanMoveUp(int index)
    {
        return index > 0 && Model.CategoryAttributes != null && Model.CategoryAttributes.Count > 1;
    }

    private bool CanMoveDown(int index)
    {
        return Model.CategoryAttributes != null && index < Model.CategoryAttributes.Count - 1 && Model.CategoryAttributes.Count > 1;
    }

    // ========== Notification Helpers ==========
    private async Task ShowErrorNotification(string title, string message)
    {
        await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
    }

    private async Task ShowSuccessNotification(string title, string message)
    {
        await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
    }

    private async Task ShowWarningNotification(string title, string message)
    {
        await JSRuntime.InvokeVoidAsync("swal", title, message, "warning");
    }

    private async Task<bool> ShowConfirmationDialog(string title, string message)
    {
        return await JSRuntime.InvokeAsync<bool>("swal", new
        {
            title,
            text = message,
            icon = "warning",
            buttons = new
            {
                cancel = ActionsResources.Cancel,
                confirm = ActionsResources.Confirm,
            },
            dangerMode = true,
        });
    }

    // ========== Disposal ==========
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // Clean up Select2 instances before disposal
            try
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await JSRuntime.InvokeVoidAsync("destroySelect2", ".select2-parent, .select2-attribute");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error cleaning up Select2 during disposal: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Select2 cleanup: {ex.Message}");
            }
            selectedImage = null;
            selectedIcon = null;
            _disposed = true;
        }
    }
}