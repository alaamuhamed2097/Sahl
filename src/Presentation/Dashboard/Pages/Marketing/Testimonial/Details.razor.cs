using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Testimonial;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Testimonial;

namespace Dashboard.Pages.Marketing.Testimonial
{
    public partial class Details : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }

        [Inject] private HttpClient Http { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private ITestimonialService TestimonialService { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        private TestimonialDto model = new();
        private bool isSaving = false;
        private string errorMessage = string.Empty;
        private string successMessage = string.Empty;
        private string? previewImageUrl = null;
        private IBrowserFile? selectedImageFile = null;
        protected string baseUrl = string.Empty;

        // Constants for file validation
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] AllowedImageTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            if (Id != Guid.Empty)
            {
                previewImageUrl = null;
                await LoadTestimonial();
            }
            else
            {
                // For new testimonials, set display order automatically
                await SetNextDisplayOrder();
            }
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
            }
            return Task.CompletedTask;
        }

        private async Task SetNextDisplayOrder()
        {
            try
            {
                var allTestimonials = await TestimonialService.GetAllAsync();
                if (allTestimonials.Success && allTestimonials.Data != null)
                {
                    var maxOrder = allTestimonials.Data.Any() ? allTestimonials.Data.Max(t => t.DisplayOrder) : 0;
                    model.DisplayOrder = maxOrder + 1;
                }
                else
                {
                    model.DisplayOrder = 1;
                }
            }
            catch
            {
                model.DisplayOrder = 1; // Fallback
            }
        }

        private async Task LoadTestimonial()
        {
            try
            {
                var response = await TestimonialService.GetByIdAsync(Id);
                if (response?.Success == true && response.Data != null)
                {
                    model = response.Data;
                    StateHasChanged();
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(ValidationResources.Error, NotifiAndAlertsResources.SomethingWentWrong);
            }
        }

        // Upload and convert image to base64 (same as ShippingCompany HandleSelectedLogo)
        protected async Task HandleSelectedImage(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.File;

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                var extension = Path.GetExtension(file.Name).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.InvalidImageFormat);
                    file = null;
                    return;
                }

                const int maxFileSize = 5 * 1024 * 1024; // 5MB
                if (file.Size > maxFileSize)
                {
                    await ShowErrorNotification(
                        NotifiAndAlertsResources.Error,
                        string.Format(ValidationResources.ImageSizeLimitExceeded, 5));
                    file = null;
                    return;
                }

                try
                {
                    if (file != null)
                    {
                        using var stream = file.OpenReadStream(maxAllowedSize: maxFileSize); // Max 5MB
                        using var ms = new MemoryStream();
                        await stream.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        var base64 = Convert.ToBase64String(bytes);
                        model.Base64Image = $"data:{file.ContentType};base64,{base64}";
                        previewImageUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, file.ContentType);

                        if (string.IsNullOrEmpty(previewImageUrl))
                        {
                            await ShowErrorNotification(
                                NotifiAndAlertsResources.Error,
                                ValidationResources.ErrorProcessingImage);
                        }
                    }
                }
                catch (Exception)
                {
                    await ShowErrorNotification(
                        NotifiAndAlertsResources.Error,
                        ValidationResources.ErrorProcessingImage);
                    selectedImageFile = null;
                    previewImageUrl = null;
                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(
                    NotifiAndAlertsResources.Error,
                    ValidationResources.ErrorProcessingImage);
                selectedImageFile = null;
                previewImageUrl = null;
            }
        }

        private void RemoveSelectedImage()
        {
            previewImageUrl = null;
            model.Base64Image = null;
            selectedImageFile = null;
        }

        private async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged();

                var result = await TestimonialService.SaveAsync(model);

                isSaving = false;
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
                    await CloseModal();
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, NotifiAndAlertsResources.SomethingWentWrong);
            }
        }

        private async Task CloseModal()
        {
            Navigation.NavigateTo("/testimonials", true);
        }

        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task ShowSuccessNotification(string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
        }
    }
}