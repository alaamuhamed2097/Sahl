using Dashboard.Configuration;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Dashboard.Services.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.ECommerce;

namespace Dashboard.Pages.Shipping.Companies
{
    public partial class Details
    {
        private bool isSaving { get; set; }
        protected string baseUrl = string.Empty;
        private IBrowserFile? selectedLogoFile;
        private string? previewLogoUrl;
        private IEnumerable<CountryInfo>? countries;

        protected ShippingCompanyDto Model { get; set; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IShippingCompanyService ShippingCompanyService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            Model = new();
            await LoadCountriesAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
            }
            return Task.CompletedTask;
        }

        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                previewLogoUrl = null;
                Edit(Id);
            }
        }

        private async Task LoadCountriesAsync()
        {
            try
            {
                if (CountryPhoneCodeService != null)
                {
                    countries = CountryPhoneCodeService.GetAllCountries(ResourceManager.CurrentLanguage == Language.Arabic ? "ar" : "en");
                    if (string.IsNullOrEmpty(Model.PhoneCode)) Model.PhoneCode = "+20"; // Default to Egypt
                }
            }
            catch (Exception)
            {
                // Fallback to basic countries if service fails
                countries = CountryPhoneCodeService.GetFallbackCountries();
            }
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged();

                var result = await ShippingCompanyService.SaveAsync(Model);

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

        protected async Task Edit(Guid id)
        {
            try
            {
                var result = await ShippingCompanyService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
                    return;
                }

                Model = result.Data ?? new();
                StateHasChanged();
            }
            catch (Exception)
            {
                await ShowErrorNotification(ValidationResources.Error, NotifiAndAlertsResources.SomethingWentWrong);
            }
        }

        // Upload and convert image to base64
        protected async Task HandleSelectedLogo(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.File;

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
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
                        Model.Base64Image = $"data:{file.ContentType};base64,{base64}";
                        previewLogoUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, file.ContentType);

                        if (string.IsNullOrEmpty(previewLogoUrl))
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
                    selectedLogoFile = null;
                    previewLogoUrl = null;

                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(
                NotifiAndAlertsResources.Error,
                ValidationResources.ErrorProcessingImage);
                selectedLogoFile = null;
                previewLogoUrl = null;
            }

        }

        private void RemoveSelectedImage()
        {
            previewLogoUrl = null;
            Model.Base64Image = null;
            selectedLogoFile = null;
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/shippingCompanies", true);
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
