using Common.Enumerations.IdentificationType;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Location;
using Dashboard.Contracts.Vendor;
using Dashboard.Pages.Base;
using Dashboard.Services.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Location;
using Shared.DTOs.Vendor;

namespace Dashboard.Pages.UserManagement.Vendors
{
    public partial class Create : LocalizedComponentBase
    {
        protected VendorRegistrationRequestDto Model { get; set; } = new();
        private bool isSaving { get; set; }
        private bool isLoadingCities { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IVendorService _vendorService { get; set; } = null!;
        [Inject] protected ICityService CityService { get; set; } = null!;
        [Inject] protected IStateService StateService { get; set; } = null!;
        [Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

        protected List<CityDto> AllCities { get; set; } = new();
        protected List<CityDto> FilteredCities { get; set; } = new();
        protected List<StateDto> States { get; set; } = new();
        private IEnumerable<CountryInfo>? countries;

        private Guid _selectedStateId;
        protected Guid SelectedStateId
        {
            get => _selectedStateId;
            set
            {
                if (_selectedStateId != value)
                {
                    _selectedStateId = value;
                    OnStateChanged();
                }
            }
        }


        // Constants
        protected const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        protected override async Task OnInitializedAsync()
        {
            await LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            try
            {
                // Load countries for phone codes
                if (CountryPhoneCodeService != null)
                {
                    countries = CountryPhoneCodeService.GetAllCountries(
                        ResourceManager.CurrentLanguage == Language.Arabic ? "ar" : "en");
                }
                else
                {
                    countries = CountryPhoneCodeService.GetFallbackCountries();
                }

                // Set default phone code
                if (string.IsNullOrEmpty(Model.PhoneCode))
                    Model.PhoneCode = "+20"; // Default to Egypt

                // Load States
                await LoadStates();

                // Load Cities (all of them, then we filter)
                await LoadCities();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.FailedAlert,
                    ex.Message,
                    "error");
            }
        }

        private async Task LoadStates()
        {
            var response = await StateService.GetAllAsync();
            if (response?.Success == true)
            {
                States = response.Data.ToList();
            }
        }

        private async Task LoadCities()
        {
            isLoadingCities = true;
            StateHasChanged();

            try
            {
                var response = await CityService.GetAllAsync();
                if (response?.Success == true)
                {
                    AllCities = response.Data.ToList();
                    FilteredCities = new List<CityDto>(); // Verify context, initially empty until State selected
                }
            }
            finally
            {
                isLoadingCities = false;
                StateHasChanged();
            }
        }

        private void OnStateChanged()
        {
            if (SelectedStateId == Guid.Empty)
            {
                FilteredCities = new List<CityDto>();
            }
            else
            {
                FilteredCities = AllCities.Where(c => c.StateId == SelectedStateId).ToList();
            }
            Model.CityId = Guid.Empty; // Reset selected city
            StateHasChanged();
        }

        private async Task HandleImageUpload(InputFileChangeEventArgs e, bool isFront)
        {
            try
            {
                if (e.FileCount == 0) return;

                var file = e.File;

                // Validate file size
                if (file.Size > MaxFileSize)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Warning,
                        $"{file.Name} {string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / 1024 / 1024)}",
                        "warning");
                    return;
                }

                // Validate content type
                if (!file.ContentType.StartsWith("image/"))
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Warning,
                        $"{file.Name} {ValidationResources.InvalidImageFormat}",
                        "warning");
                    return;
                }

                // Convert to base64
                var base64Image = await ConvertFileToBase64(file);
                if (!string.IsNullOrEmpty(base64Image))
                {
                    if (isFront)
                        Model.IdentificationImageFront = base64Image;
                    else
                        Model.IdentificationImageBack = base64Image;

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task<string> ConvertFileToBase64(IBrowserFile file)
        {
            try
            {
                using var stream = file.OpenReadStream(MaxFileSize);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    $"{ValidationResources.ErrorProcessingFile}: {ex.Message}",
                    "error");
                return null;
            }
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

            return string.Empty;
        }

        protected async Task Save()
        {
            // Manual Validation for Back Image
            if (Model.IdentificationType != IdentificationType.Passport && string.IsNullOrEmpty(Model.IdentificationImageBack))
            {
                await JSRuntime.InvokeVoidAsync("swal",
                       ValidationResources.Failed,
                       ValidationResources.FieldRequired, // Should say "Back Image Required" but generic required is ok or custom message
                       "error");
                return;
            }

            try
            {
                isSaving = true;
                StateHasChanged();

                var result = await _vendorService.CreateVendorAsync(Model);
                isSaving = false;

                if (result.Success)
                {
                    await CloseModal();
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Done,
                        NotifiAndAlertsResources.SavedSuccessfully,
                        "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        result.Message ?? NotifiAndAlertsResources.SaveFailed,
                        "error");
                }
            }
            catch (Exception ex)
            {
                isSaving = false;
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.FailedAlert,
                    ex.Message,
                    "error");
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/users/vendors");
        }
    }
}
