using Common.Enumerations.IdentificationType;
using Dashboard.Configuration;
using Dashboard.Contracts.Location;
using Dashboard.Contracts.Vendor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Location;
using Shared.DTOs.Vendor;

namespace Dashboard.Pages.UserManagement.Vendors
{
    public partial class Edit
    {
        [Parameter] public Guid Id { get; set; }

        protected VendorUpdateRequestDto Model { get; set; } = new();

        [Inject] protected IVendorService _vendorService { get; set; } = null!;
        [Inject] protected ICityService CityService { get; set; } = null!;
        [Inject] protected IStateService StateService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        protected List<CityDto> AllCities { get; set; } = new();
        protected List<CityDto> FilteredCities { get; set; } = new();
        protected List<StateDto> States { get; set; } = new();

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

        private bool isSaving { get; set; }
        private bool isLoading = true;
        protected string baseUrl = string.Empty;
        private bool isLoadingCities { get; set; }

        // Constants
        protected const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            await LoadInitialData();
            await InitializeModel(Id);
        }

        private async Task LoadInitialData()
        {
            try
            {
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

        protected async Task InitializeModel(Guid id)
        {
            try
            {
                var result = await _vendorService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }

                if (result.Data != null)
                {
                    Model = new VendorUpdateRequestDto
                    {
                        VendorId = result.Data.Id,
                        UserId = result.Data.UserId,
                        FirstName = result.Data.AdministratorFirstName,
                        LastName = result.Data.AdministratorLastName,
                        BirthDate = result.Data.BirthDate,
                        IdentificationType = result.Data.IdentificationType,
                        IdentificationNumber = result.Data.IdentificationNumber,
                        IdentificationImageFrontPath = result.Data.IdentificationImageFrontPath,
                        IdentificationImageBackPath = result.Data.IdentificationImageBackPath,
                        StoreName = result.Data.StoreName,
                        VendorType = result.Data.VendorType,
                        Address = result.Data.Address,
                        PostalCode = result.Data.PostalCode,
                        CityId = result.Data.CityId,
                        Notes = result.Data.Notes,
                        IsRealEstateRegistered = result.Data.IsRealEstateRegistered
                    };

                    // Set state based on city if available
                    if (result.Data.CityId != Guid.Empty)
                    {
                        var city = AllCities.FirstOrDefault(c => c.Id == result.Data.CityId);
                        if (city != null)
                        {
                            _selectedStateId = city.StateId ?? Guid.Empty;
                            // Trigger city filtering
                            OnStateChanged();
                        }
                    }
                }

                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
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
                    FilteredCities = new List<CityDto>(); // Initially empty until State is selected
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
                Model.CityId = Guid.Empty; // Reset selected city
            }
            else
            {
                FilteredCities = AllCities.Where(c => c.StateId == SelectedStateId).ToList();
                // Only reset CityId if current city is not in filtered list
                if (Model.CityId != Guid.Empty && !FilteredCities.Any(c => c.Id == Model.CityId))
                {
                    Model.CityId = Guid.Empty;
                }
            }
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
                        Model.IdentificationImageFrontPath = base64Image;
                    else
                        Model.IdentificationImageBackPath = base64Image;

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
                return string.Empty;
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

            // If it's a relative path or URL
            return baseUrl + '/' + imagePath;
        }

        protected async Task Save()
        {
            // Validate State selection
            if (SelectedStateId == Guid.Empty)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    "Please select a State",
                    "error");
                return;
            }

            // Manual Validation for Back Image
            if (Model.IdentificationType != IdentificationType.Passport &&
                string.IsNullOrEmpty(Model.IdentificationImageBackPath))
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    "Back identification image is required for non-passport documents",
                    "error");
                return;
            }

            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner

                var result = await _vendorService.UpdateVendorAsync(Model);

                if (result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.SaveSuccess,
                        NotifiAndAlertsResources.SavedSuccessfully,
                        "success");

                    // Navigate back to list after successful save
                    Navigation.NavigateTo("/users/vendors");
                }
                else
                {
                    var errorMessage = result.Message;
                    if (result.Errors != null && result.Errors.Any())
                    {
                        errorMessage = string.Join("<br/>", result.Errors);
                    }
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        errorMessage,
                        "error");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.FailedAlert,
                    ex.Message,
                    "error");
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
            }
        }

        protected void CloseModal()
        {
            Navigation.NavigateTo("/users/vendors");
        }
    }
}