using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Resources.Services;
using Shared.DTOs;
using Shared.DTOs.User;
using Shared.DTOs.User.Marketer;
using UI.Configuration;
using UI.Contracts;
using UI.Contracts.General;
using UI.Contracts.Location;
using UI.Contracts.Team;
using UI.Contracts.User.Marketer;
using UI.Services;
using UI.Services.General;

namespace UI.Pages.User.Marketer
{
    public partial class RegisterMarketer
    {
        [Inject] protected IMarketerService MarketerService { get; set; }
        [Inject] protected ICountryService CountryService { get; set; }
        [Inject] protected IStateService StateService { get; set; }
        [Inject] protected ICityService CityService { get; set; }
        [Inject] protected ITeamService TeamService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] private LanguageService LanguageService { get; set; }
        [Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

        [Parameter] public string recruitmentLinkCode { get; set; } = null!;

        protected MarketerRegistrationDto Model { get; set; } = new() { NationalIdImageFiles = new()};
        protected SponsorDto Sponsor { get; set; } = new SponsorDto();

        protected string SponsorFullName
        {
            get
            {
                var fullName = Sponsor.FirstName + " " + Sponsor.LastName;
                if (fullName.Length < 18)
                    return fullName;
                else
                    return fullName.Substring(0, 15) + "...";
            }
        }
        protected string baseUrl = string.Empty;
        protected int    processingProgress;
        protected bool   isSaving = false;
        protected bool   isLoading = true;
        protected bool   isProcessing;
        protected bool   isLoadingLocations = false;

        // Location data
        protected List<CountryDto> Countries { get; set; } = new List<CountryDto>();
        protected List<StateDto> States { get; set; } = new List<StateDto>();
        protected List<CityDto> Cities { get; set; } = new List<CityDto>();
        private IEnumerable<CountryInfo>? countries;
        protected List<dropdownDto> Teams { get; set; } = new();

        protected bool isCountrySelected;
        protected bool isStateSelected;

        // Constants
        protected const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        protected const int MaxImageCount = 2;

        // Functions
        protected override async Task OnInitializedAsync()
        {
            if (recruitmentLinkCode != null)
            {
                var task1 = LoadLinkSpecificData();
                var task2 = LoadSharedResources();
                await Task.WhenAll(task1, task2);

                if(Sponsor.TeamId != Guid.Empty)
                    Model.TeamId = Sponsor.TeamId;

                isLoading = false;
                StateHasChanged();

                LanguageService.OnLanguageChanged += HandleLanguageChanged;
                await InitializeLanguageFromStorage();
            }
        }

        private async Task LoadLinkSpecificData()
        {
            try
            {
                await IsCodeValid();
                await LoadSponsor();
            }
            catch (Exception ex)
            {
                NavigationManager.NavigateTo($"/NotFoundPage/{ex.Message}", forceLoad: false);
            }
        }

        private async Task LoadSharedResources()
        {
            var task1 = LoadCountries();
            var task2 = LoadCountriesAsync();
            var task3 = LoadTeams();
            await Task.WhenAll(task1, task2, task3);

            baseUrl = ApiOptions.Value.BaseUrl;
            isCountrySelected = false;
            isStateSelected = false;
        }

        private async Task IsCodeValid()
        {
            var isValid = await MarketerService.IsValidLink(recruitmentLinkCode);
            if (!isValid.Success)
            {
                throw new InvalidOperationException(Uri.EscapeDataString(isValid.Message));
            }
        }

        private async Task LoadSponsor()
        {
            var result = await MarketerService.GetSponsorDto(recruitmentLinkCode);
            if (!result.Success || result.Data == null)
            {
                throw new InvalidOperationException(Uri.EscapeDataString(result.Message));
            }
            Sponsor = result.Data;
        }
   
        private async Task LoadCountriesAsync()
        {
            try
            {
                if (CountryPhoneCodeService != null)
                {
                    countries = CountryPhoneCodeService.GetAllCountries(ResourceManager.CurrentLanguage == Language.Arabic ? "ar" : "en");
                }
            }
            catch (Exception)
            {
                // Fallback to basic countries if service fails
                countries = CountryPhoneCodeService.GetFallbackCountries();
            }
        }

        private async Task LoadTeams()
        {
            try
            {
                var response = await TeamService.GetDropDownList();
                if (response?.Success == true)
                {
                    Teams = response.Data.ToList();
                }
                else
                {
                    NavigationManager.NavigateTo($"/NotFoundPage/can not load teams", forceLoad: false);
                }
            }
            catch (Exception ex)
            {
                NavigationManager.NavigateTo($"/NotFoundPage/{ex.Message}", forceLoad: false);
            }
        }

        // Locations
        private async Task LoadCountries()
        {
            isLoadingLocations = true;
            StateHasChanged();

            try
            {
                var response = await CountryService.GetAllAsync();
                if (response?.Success == true)
                {
                    Countries = response.Data.ToList();
                    if(string.IsNullOrEmpty(Model.PhoneCode)) Model.PhoneCode = "+20"; // Default to Egypt
                }
            }
            finally
            {
                isLoadingLocations = false;
                StateHasChanged();
            }
        }

        protected async Task OnCountryChanged()
        {
            if (Model.CountryId == Guid.Empty) return;

            isLoadingLocations = true;
            isCountrySelected = true;
            isStateSelected = false;
            StateHasChanged();

            States.Clear();
            Cities.Clear();

            try
            {
                // Call API to load states by selected country
                var response = await StateService.GetAllAsync();
                if (response?.Success == true)
                {
                    States = response.Data.Where(s=>s.CountryId==Model.CountryId).ToList();
                }
            }
            finally
            {
                isLoadingLocations = false;
                StateHasChanged();
            }
        }

        protected async Task OnStateChanged()
        {
            if (Model.StateId == Guid.Empty) return;

            isLoadingLocations = true;
            isStateSelected = true;
            StateHasChanged();

            Cities.Clear();
            try
            {
                // Call API to load cities by selected state
                var response = await CityService.GetAllAsync();
                if (response?.Success == true)
                {
                    Cities = response.Data.Where(c=>c.StateId == Model.StateId).ToList();
                }
            }
            finally
            {
                isLoadingLocations = false;
                StateHasChanged();
            }
        }

        // Submit 
        protected async Task HandleValidSubmit()
        {
            isSaving = true;
            try
            {
                var response = await MarketerService.Register(recruitmentLinkCode, Model);
                if(response.Success)
                {
                    NavigationManager.NavigateTo("/login");
                    await JSRuntime.InvokeVoidAsync("swal", response.Message, NotifiAndAlertsResources.RegistrationSuccessful, "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", response.Message, NotifiAndAlertsResources.RegistrationFailed, "error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                isSaving = false;
            }
        }
        
        // Handle images
        private async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            isProcessing = true;
            processingProgress = 0;

            try
            {
                if (e.FileCount == 0) return;

                StateHasChanged();

                // Initialize if null
                Model.NationalIdImageFiles ??= new List<string>();

                // Get actual files to process (respect the limit)
                var filesToProcess = e.GetMultipleFiles(Math.Min(2, e.FileCount));
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
                        Model.NationalIdImageFiles.Add(base64Image);
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

        private async Task DeleteImage(string image)
        {
            try
            {
                var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
                {
                    title = NotifiAndAlertsResources.ConfirmDeleteImage,
                    icon = "warning",
                    buttons = new { cancel = true, confirm = true },
                    dangerMode = true
                });

                if (confirmed)
                {
                    Model.NationalIdImageFiles.Remove(image);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ValidationResources.Error, ex.Message);
            }
        }

        private async Task ShowErrorMessage(string title, string message, string type = "error")
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, type);
        }

        // Language
        private async Task ChangeLanguage()
        {
            ResourceManager.ChangeLanguage();
            var languageCode = ResourceManager.GetCultureName(ResourceManager.CurrentLanguage);
            await JSRuntime.InvokeVoidAsync("localization.setLanguage", languageCode);
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

        private void HandleLanguageChanged()
        {
            // Force component re-render
            InvokeAsync(StateHasChanged);
        }

        private async Task InitializeLanguageFromStorage()
        {
            var lang = await JSRuntime.InvokeAsync<string>("localization.getCurrentLanguage");
            ResourceManager.CurrentLanguage = lang.StartsWith("ar") ? Language.Arabic : Language.English;
        }

        public void Dispose()
        {
            // Unsubscribe when component is removed
            LanguageService.OnLanguageChanged -= HandleLanguageChanged;
        }
    }
}
