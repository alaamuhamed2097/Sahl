using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Setting;
using Dashboard.Services.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Resources.Services;
using Shared.DTOs.Setting;

namespace Dashboard.Pages.Settings
{
    public partial class Settings : ComponentBase, IDisposable
    {
        [Inject] private ISettingService SettingService { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private ILogger<Settings> Logger { get; set; } = null!;
        [Inject] private IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] private ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected LanguageService LanguageService { get; set; } = null!;

        // Add these properties to the Settings.razor.cs class
        protected string baseUrl = string.Empty;
        private string? previewBannerUrl;

        // Component state
        protected GeneralSettingsDto Model { get; set; } = new();
        protected bool IsLoading { get; set; } = true;
        protected bool IsSaving { get; set; } = false;
        protected string ErrorMessage { get; set; } = string.Empty;
        protected string SuccessMessage { get; set; } = string.Empty;
        protected string ActiveTab { get; set; } = "contact";

        // Validation context for checking errors
        private EditContext? editContext;
        private ValidationMessageStore? messageStore;

        // Phone number enhancement variables
        private IEnumerable<CountryInfo>? countries;
        private CountryInfo? selectedCountry;
        private CountryInfo? selectedWhatsAppCountry;
        private string phoneFormatExample = "";
        private string whatsAppFormatExample = "";
        private bool isPhoneValid = false;
        private bool isWhatsAppValid = false;
        private bool hasValidatedPhone = false;
        private bool hasValidatedWhatsApp = false;

        // Tab validation state
        private Dictionary<string, bool> tabValidationErrors = new()
        {
            { "contact", false },
            { "social", false },
            { "order", false },
            { "seo", false },
            { "banner", false }
        };

        // Lifecycle methods
        protected override async Task OnInitializedAsync()
        {
            // Add this line to set baseUrl
            baseUrl = ApiOptions.Value.BaseUrl; // Assuming you have ApiOptions injected

            LanguageService.OnLanguageChanged += HandleLanguageChanged;
            await InitializeLanguageFromStorage();

            await LoadCountriesAsync();
            await LoadSettings();

            // Initialize validation context
            editContext = new EditContext(Model);
            messageStore = new ValidationMessageStore(editContext);
            editContext.OnValidationStateChanged += OnValidationStateChanged;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
                ResourceLoaderService.LoadStyleSheet("css/setting_page.css");
            }
            return Task.CompletedTask;
        }

        private void OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            UpdateTabValidationState();
            StateHasChanged();
        }

        protected void UpdateTabValidationState()
        {
            if (editContext == null) return;

            // Reset all tabs
            foreach (var key in tabValidationErrors.Keys.ToList())
            {
                tabValidationErrors[key] = false;
            }

            // Check for contact tab errors
            var contactFields = new[] {
                editContext.Field(nameof(Model.Email)),
                editContext.Field(nameof(Model.Phone)),
                editContext.Field(nameof(Model.Address)),
                editContext.Field(nameof(Model.PhoneCode))
            };

            tabValidationErrors["contact"] = contactFields.Any(field =>
                editContext.GetValidationMessages(field).Any()) ||
                !isPhoneValid && hasValidatedPhone && !string.IsNullOrEmpty(Model.Phone) ||
                !isWhatsAppValid && hasValidatedWhatsApp && !string.IsNullOrEmpty(Model.WhatsAppNumber) ||
                string.IsNullOrWhiteSpace(Model.Email) ||
                string.IsNullOrWhiteSpace(Model.Address);

            // Check for social tab errors (these might have URL validation)
            var socialFields = new[] {
                editContext.Field(nameof(Model.FacebookUrl)),
                editContext.Field(nameof(Model.InstagramUrl)),
                editContext.Field(nameof(Model.TwitterUrl)),
                editContext.Field(nameof(Model.LinkedInUrl))
            };

            tabValidationErrors["social"] = socialFields.Any(field =>
                editContext.GetValidationMessages(field).Any()) ||
                HasInvalidUrl(Model.FacebookUrl) ||
                HasInvalidUrl(Model.InstagramUrl) ||
                HasInvalidUrl(Model.TwitterUrl) ||
                HasInvalidUrl(Model.LinkedInUrl);

            // Check for order tab errors
            //var orderFields = new[] {
            //    editContext.Field(nameof(Model.ShippingAmount)),
            //    editContext.Field(nameof(Model.OrderTaxPercentage)),
            //    editContext.Field(nameof(Model.OrderExtraCost))
            //};

            //tabValidationErrors["order"] = orderFields.Any(field =>
            //    editContext.GetValidationMessages(field).Any()) ||
            //    Model.ShippingAmount < 0 ||
            //    Model.OrderTaxPercentage < 0 || Model.OrderTaxPercentage > 100 ||
            //    Model.OrderExtraCost < 0;

            // Check for SEO tab errors
            var seoFields = new[] {
                editContext.Field(nameof(Model.SEOTitle)),
                editContext.Field(nameof(Model.SEODescription)),
                editContext.Field(nameof(Model.SEOMetaTags))
            };

            tabValidationErrors["seo"] = seoFields.Any(field =>
                editContext.GetValidationMessages(field).Any()) ||
                string.IsNullOrWhiteSpace(Model.SEOTitle) ||
                string.IsNullOrWhiteSpace(Model.SEODescription);

            // Check for banner tab errors (optional - only if you want to enforce banner requirement)
            //var bannerFields = new[] {
            //    editContext.Field(nameof(Model.Base64Image))
            //};

            //tabValidationErrors["banner"] = bannerFields.Any(field =>
            //    editContext.GetValidationMessages(field).Any());
            // Note: Banner is optional, so no additional validation needed unless you want to make it required
        }

        private bool HasInvalidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false; // Empty is valid for optional fields

            return !Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   || uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps;
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
                countries = GetFallbackCountries();
            }
        }

        private IEnumerable<CountryInfo> GetFallbackCountries()
        {
            return new[]
            {
                // Arab Countries
                new CountryInfo("Egypt", "???", "+20", "????", "### ### ####"),
                new CountryInfo("Saudi Arabia", "??????? ??????? ???????", "+966", "????", "## ### ####"),
                new CountryInfo("United Arab Emirates", "???????? ??????? ???????", "+971", "????", "## ### ####"),
                new CountryInfo("Iraq", "??????", "+964", "????", "### ### ####"),
                new CountryInfo("Algeria", "???????", "+213", "????", "## ### ####"),
                new CountryInfo("Morocco", "??????", "+212", "????", "## ### ####"),
                new CountryInfo("Sudan", "???????", "+249", "????", "### ### ####"),
                new CountryInfo("Jordan", "??????", "+962", "????", "## ### ####"),
                new CountryInfo("Lebanon", "?????", "+961", "????", "## ### ###"),
                new CountryInfo("Libya", "?????", "+218", "????", "## ### ####"),
                new CountryInfo("Tunisia", "????", "+216", "????", "## ### ###"),
                new CountryInfo("Oman", "????", "+968", "????", "#### ####"),
                new CountryInfo("Kuwait", "??????", "+965", "????", "#### ####"),
                new CountryInfo("Qatar", "???", "+974", "????", "#### ####"),
                new CountryInfo("Bahrain", "???????", "+973", "????", "#### ####"),
                new CountryInfo("Yemen", "?????", "+967", "????", "### ### ###"),
                new CountryInfo("Syria", "?????", "+963", "????", "### ### ###"),
                new CountryInfo("Palestine", "??????", "+970", "????", "## ### ####"),
                new CountryInfo("Mauritania", "?????????", "+222", "????", "## ## ####"),
                new CountryInfo("Somalia", "???????", "+252", "????", "## ### ###"),
                new CountryInfo("Djibouti", "??????", "+253", "????", "## ## ## ##"),
                new CountryInfo("Comoros", "??? ?????", "+269", "????", "### ####"),
                // Other Popular Countries
                new CountryInfo("United States", "???????? ???????", "+1", "????", "(###) ###-####"),
                new CountryInfo("United Kingdom", "??????? ???????", "+44", "????", "## #### ####"),
                new CountryInfo("Germany", "???????", "+49", "????", "### ### ####"),
                new CountryInfo("France", "?????", "+33", "????", "## ## ## ## ##"),
                new CountryInfo("Italy", "???????", "+39", "????", "### ### ####"),
                new CountryInfo("Spain", "???????", "+34", "????", "### ### ###"),
                new CountryInfo("Netherlands", "??????", "+31", "????", "## ### ####"),
                new CountryInfo("Japan", "???????", "+81", "????", "##-####-####"),
                new CountryInfo("Australia", "????????", "+61", "????", "#### ### ###"),
                new CountryInfo("Canada", "????", "+1", "????", "(###) ###-####"),
                new CountryInfo("China", "?????", "+86", "????", "### #### ####"),
                new CountryInfo("India", "?????", "+91", "????", "#### ### ###"),
                new CountryInfo("Brazil", "????????", "+55", "????", "## #####-####"),
                new CountryInfo("Russia", "?????", "+7", "????", "### ###-##-##"),
                new CountryInfo("Turkey", "?????", "+90", "????", "### ### ####"),
                new CountryInfo("South Korea", "????? ????????", "+82", "????", "##-####-####")
            };
        }

        // Data loading
        private async Task LoadSettings()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var result = await SettingService.GetSettingsAsync();

                if (result.Success && result.Data != null)
                {
                    Model = result.Data;
                    UpdateSelectedCountries();
                }
                else
                {
                    ErrorMessage = result.Message ?? "Failed to load settings";
                    // Initialize with default values if loading fails
                    Model = GetDefaultSettings();
                    UpdateSelectedCountries();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading settings");
                ErrorMessage = "An unexpected error occurred while loading settings";
                Model = GetDefaultSettings();
                UpdateSelectedCountries();
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private void UpdateSelectedCountries()
        {
            if (countries != null)
            {
                if (!string.IsNullOrEmpty(Model.PhoneCode))
                {
                    selectedCountry = countries.FirstOrDefault(c => c.PhoneCode == Model.PhoneCode);
                    phoneFormatExample = selectedCountry?.Format?.Replace('#', '0') ?? "";
                }

                if (!string.IsNullOrEmpty(Model.WhatsAppCode))
                {
                    selectedWhatsAppCountry = countries.FirstOrDefault(c => c.PhoneCode == Model.WhatsAppCode);
                    whatsAppFormatExample = selectedWhatsAppCountry?.Format?.Replace('#', '0') ?? "";
                }
            }
        }

        // Enhanced Save settings with comprehensive validation
        protected async Task SaveSettings()
        {
            try
            {
                IsSaving = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;
                StateHasChanged();

                // Perform comprehensive validation before saving
                var isValid = await ValidateAllTabs();

                if (!isValid)
                {
                    await NavigateToFirstErrorTab();
                    return;
                }

                var result = await SettingService.UpdateSettingsAsync(Model);

                if (result.Success)
                {
                    SuccessMessage = "Settings saved successfully!";
                    await ShowSuccessNotification("Success", SuccessMessage);
                }
                else
                {
                    ErrorMessage = result.Message ?? "Failed to save settings";
                    await ShowErrorNotification("Error", ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving settings");
                ErrorMessage = "An unexpected error occurred while saving settings";
                await ShowErrorNotification("Error", ErrorMessage);
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        // Reset to defaults
        protected async Task ResetToDefaults()
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                "Are you sure you want to reset all settings to their default values? This action cannot be undone.");

            if (confirmed)
            {
                Model = GetDefaultSettings();
                UpdateSelectedCountries();
                SuccessMessage = "Settings have been reset to defaults. Don't forget to save!";
                ErrorMessage = string.Empty;

                // Reset validation states
                ResetPhoneValidation();
                ResetWhatsAppValidation();
                UpdateTabValidationState();

                StateHasChanged();
            }
        }

        private async Task<bool> ValidateAllTabs()
        {
            var isValid = true;

            // Validate Contact Information
            if (string.IsNullOrWhiteSpace(Model.Email))
            {
                tabValidationErrors["contact"] = true;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Model.Address))
            {
                tabValidationErrors["contact"] = true;
                isValid = false;
            }

            // Validate phone numbers if provided
            if (!string.IsNullOrEmpty(Model.Phone))
            {
                ValidatePhoneNumber();
                if (!isPhoneValid)
                {
                    tabValidationErrors["contact"] = true;
                    await ShowErrorNotification("Validation Failed", "Invalid phone number format");
                    isValid = false;
                }
            }

            if (!string.IsNullOrEmpty(Model.WhatsAppNumber))
            {
                ValidateWhatsAppNumber();
                if (!isWhatsAppValid)
                {
                    tabValidationErrors["contact"] = true;
                    await ShowErrorNotification("Validation Failed", "Invalid WhatsApp number format");
                    isValid = false;
                }
            }

            // Validate Social Media URLs
            if (!string.IsNullOrEmpty(Model.FacebookUrl) && HasInvalidUrl(Model.FacebookUrl))
            {
                tabValidationErrors["social"] = true;
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Model.InstagramUrl) && HasInvalidUrl(Model.InstagramUrl))
            {
                tabValidationErrors["social"] = true;
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Model.TwitterUrl) && HasInvalidUrl(Model.TwitterUrl))
            {
                tabValidationErrors["social"] = true;
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Model.LinkedInUrl) && HasInvalidUrl(Model.LinkedInUrl))
            {
                tabValidationErrors["social"] = true;
                isValid = false;
            }

            //// Validate Order Settings
            //if (Model.ShippingAmount < 0)
            //{
            //    tabValidationErrors["order"] = true;
            //    isValid = false;
            //}

            //if (Model.OrderTaxPercentage < 0 || Model.OrderTaxPercentage > 100)
            //{
            //    tabValidationErrors["order"] = true;
            //    isValid = false;
            //}

            //if (Model.OrderExtraCost < 0)
            //{
            //    tabValidationErrors["order"] = true;
            //    isValid = false;
            //}

            // Validate SEO Settings
            if (string.IsNullOrWhiteSpace(Model.SEOTitle))
            {
                tabValidationErrors["seo"] = true;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Model.SEODescription))
            {
                tabValidationErrors["seo"] = true;
                isValid = false;
            }

            // Also run DataAnnotations validation
            if (editContext != null && !editContext.Validate())
            {
                UpdateTabValidationState();
                isValid = false;
            }

            return isValid;
        }

        private async Task NavigateToFirstErrorTab()
        {
            var tabsWithErrors = new[] { "contact", "social", "business", "order", "withdrawal", "seo" }
                .Where(tab => tabValidationErrors[tab])
                .ToList();

            if (tabsWithErrors.Any())
            {
                var firstErrorTab = tabsWithErrors.First();

                // Show notification about validation errors
                var errorMessage = tabsWithErrors.Count == 1
                    ? $"Please fix the validation errors in the {GetTabDisplayName(firstErrorTab)} tab."
                    : $"Please fix the validation errors in {tabsWithErrors.Count} tabs: {string.Join(", ", tabsWithErrors.Select(GetTabDisplayName))}.";

                await ShowErrorNotification("Validation Errors", errorMessage);

                // Navigate to first tab with errors
                SetActiveTab(firstErrorTab);
            }
        }

        private string GetTabDisplayName(string tabKey)
        {
            return tabKey switch
            {
                "contact" => "Contact Information",
                "social" => "Social Media",
                "business" => "Business Settings",
                "order" => "Order Settings",
                "withdrawal" => "Withdrawal Settings",
                "seo" => "SEO Settings",
                "banner" => "Banner Settings",
                _ => tabKey
            };
        }

        // Phone number handling methods
        private void OnPhoneCodeChanged()
        {
            UpdateSelectedCountries();
            ResetPhoneValidation();
            UpdateTabValidationState();
            StateHasChanged();
        }

        private void OnWhatsAppCodeChanged()
        {
            UpdateSelectedCountries();
            ResetWhatsAppValidation();
            UpdateTabValidationState();
            StateHasChanged();
        }

        private string GetPhoneInputClass(bool isWhatsApp = false)
        {
            if (isWhatsApp)
            {
                if (isWhatsAppValid)
                    return "form-control phone-number-input is-valid";
                if (hasValidatedWhatsApp && !isWhatsAppValid && !string.IsNullOrEmpty(Model.WhatsAppNumber))
                    return "form-control phone-number-input is-invalid";
            }
            else
            {
                if (isPhoneValid)
                    return "form-control phone-number-input is-valid";
                if (hasValidatedPhone && !isPhoneValid && !string.IsNullOrEmpty(Model.Phone))
                    return "form-control phone-number-input is-invalid";
            }

            return "form-control phone-number-input";
        }

        private string GetPhoneNumberPlaceholder(bool isWhatsApp = false)
        {
            var country = isWhatsApp ? selectedWhatsAppCountry : selectedCountry;
            if (country != null && !string.IsNullOrEmpty(country.Format))
            {
                return country.Format.Replace('#', '0');
            }
            return isWhatsApp ? "WhatsApp Number" : "Phone Number";
        }

        private void OnPhoneNumberInput(ChangeEventArgs e, bool isWhatsApp = false)
        {
            var input = e.Value?.ToString() ?? "";

            // Remove leading zero if present
            input = RemoveLeadingZero(input);

            if (isWhatsApp)
            {
                Model.WhatsAppNumber = input;
                ResetWhatsAppValidation();
            }
            else
            {
                Model.Phone = input;
                ResetPhoneValidation();
            }

            // Update validation state after any input change
            UpdateTabValidationState();
        }

        // Add input change handlers for other fields
        private void OnEmailInput(ChangeEventArgs e)
        {
            Model.Email = e.Value?.ToString() ?? "";
            UpdateTabValidationState();
        }

        private void OnAddressInput(ChangeEventArgs e)
        {
            Model.Address = e.Value?.ToString() ?? "";
            UpdateTabValidationState();
        }

        private void OnSocialUrlInput(ChangeEventArgs e, string fieldName)
        {
            var value = e.Value?.ToString() ?? "";
            switch (fieldName.ToLower())
            {
                case "facebook":
                    Model.FacebookUrl = value;
                    break;
                case "instagram":
                    Model.InstagramUrl = value;
                    break;
                case "twitter":
                    Model.TwitterUrl = value;
                    break;
                case "linkedin":
                    Model.LinkedInUrl = value;
                    break;
            }
            UpdateTabValidationState();
        }

        private void OnSEOInput(ChangeEventArgs e, string fieldName)
        {
            var value = e.Value?.ToString() ?? "";
            switch (fieldName.ToLower())
            {
                case "title":
                    Model.SEOTitle = value;
                    break;
                case "description":
                    Model.SEODescription = value;
                    break;
                case "metatags":
                    Model.SEOMetaTags = value;
                    break;
            }
            UpdateTabValidationState();
        }

        //private void OnOrderInput(ChangeEventArgs e, string fieldName)
        //{
        //    var value = e.Value?.ToString() ?? "";
        //    switch (fieldName.ToLower())
        //    {
        //        case "shipping":
        //            if (decimal.TryParse(value, out var shipping))
        //                Model.ShippingAmount = shipping;
        //            break;
        //        case "tax":
        //            if (decimal.TryParse(value, out var tax))
        //                Model.OrderTaxPercentage = tax;
        //            break;
        //        case "extracost":
        //            if (decimal.TryParse(value, out var extra))
        //                Model.OrderExtraCost = extra;
        //            break;
        //    }
        //    UpdateTabValidationState();
        //}

        private void ValidatePhoneNumber()
        {
            hasValidatedPhone = true;

            if (!string.IsNullOrEmpty(Model.Phone) && selectedCountry != null)
            {
                // Remove leading zero and extract digits only
                var cleanedPhone = RemoveLeadingZero(Model.Phone);
                var digitsOnly = new string(cleanedPhone.Where(char.IsDigit).ToArray());
                isPhoneValid = ValidatePhoneLength(digitsOnly, Model.PhoneCode);

                // Update the model with cleaned phone number
                Model.Phone = cleanedPhone;
            }
            else
            {
                isPhoneValid = false;
            }
            UpdateTabValidationState();
        }

        private void ValidateWhatsAppNumber()
        {
            hasValidatedWhatsApp = true;

            if (!string.IsNullOrEmpty(Model.WhatsAppNumber) && selectedWhatsAppCountry != null)
            {
                // Remove leading zero and extract digits only
                var cleanedWhatsApp = RemoveLeadingZero(Model.WhatsAppNumber);
                var digitsOnly = new string(cleanedWhatsApp.Where(char.IsDigit).ToArray());
                isWhatsAppValid = ValidatePhoneLength(digitsOnly, Model.WhatsAppCode);

                // Update the model with cleaned WhatsApp number
                Model.WhatsAppNumber = cleanedWhatsApp;
            }
            else
            {
                isWhatsAppValid = false;
            }
            UpdateTabValidationState();
        }

        private string RemoveLeadingZero(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            // Trim whitespace first
            phoneNumber = phoneNumber.Trim();

            // If the number starts with 0, remove it
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }

            return phoneNumber;
        }

        private bool ValidatePhoneLength(string digitsOnly, string countryCode)
        {
            if (string.IsNullOrEmpty(digitsOnly)) return false;

            return countryCode switch
            {
                "+1" => digitsOnly.Length == 10, // US/Canada
                "+20" => digitsOnly.Length == 10, // Egypt (without leading 0)
                "+44" => digitsOnly.Length >= 10 && digitsOnly.Length <= 11, // UK
                "+49" => digitsOnly.Length >= 10 && digitsOnly.Length <= 12, // Germany
                "+33" => digitsOnly.Length == 10, // France (without leading 0)
                "+966" => digitsOnly.Length == 9, // Saudi Arabia (without leading 0)
                "+971" => digitsOnly.Length == 9, // UAE (without leading 0)
                "+212" => digitsOnly.Length == 9, // Morocco (without leading 0)
                "+213" => digitsOnly.Length == 9, // Algeria (without leading 0)
                "+216" => digitsOnly.Length == 8, // Tunisia (without leading 0)
                "+218" => digitsOnly.Length == 9, // Libya (without leading 0)
                "+961" => digitsOnly.Length == 8, // Lebanon (without leading 0)
                "+962" => digitsOnly.Length == 9, // Jordan (without leading 0)
                "+963" => digitsOnly.Length == 9, // Syria (without leading 0)
                "+964" => digitsOnly.Length == 10, // Iraq (without leading 0)
                "+965" => digitsOnly.Length == 8, // Kuwait (without leading 0)
                "+967" => digitsOnly.Length == 9, // Yemen (without leading 0)
                "+968" => digitsOnly.Length == 8, // Oman (without leading 0)
                "+970" => digitsOnly.Length == 9, // Palestine (without leading 0)
                "+973" => digitsOnly.Length == 8, // Bahrain (without leading 0)
                "+974" => digitsOnly.Length == 8, // Qatar (without leading 0)
                _ => digitsOnly.Length >= 7 && digitsOnly.Length <= 15 // Generic validation
            };
        }

        private void ResetPhoneValidation()
        {
            isPhoneValid = false;
            hasValidatedPhone = false;
        }

        private void ResetWhatsAppValidation()
        {
            isWhatsAppValid = false;
            hasValidatedWhatsApp = false;
        }

        // Enhanced Tab management with validation awareness
        protected void SetActiveTab(string tabName)
        {
            ActiveTab = tabName;
            StateHasChanged();
        }

        // Method to get tab CSS class with error indicator
        protected string GetTabClass(string tabName)
        {
            var baseClass = ActiveTab == tabName ? "nav-link active" : "nav-link";

            if (tabValidationErrors.ContainsKey(tabName) && tabValidationErrors[tabName])
            {
                baseClass += " tab-with-errors";
            }

            return baseClass;
        }


        // Handle banner image selection and validation
        //protected async Task HandleSelectedBannerImage(InputFileChangeEventArgs e)
        //{
        //    try
        //    {
        //        var file = e.File;

        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        //        var extension = Path.GetExtension(file.Name).ToLower();

        //        if (!allowedExtensions.Contains(extension))
        //        {
        //            await ShowErrorNotification(
        //                "Invalid Format",
        //                "Please select a valid image format (JPG, JPEG, PNG, or WebP)."
        //            );
        //            return;
        //        }

        //        const int maxFileSize = 20 * 1024 * 1024; // 5MB
        //        if (file.Size > maxFileSize)
        //        {
        //            await ShowErrorNotification(
        //                "File Too Large",
        //                "Please select an image smaller than 5MB."
        //            );
        //            return;
        //        }

        //        try
        //        {
        //            using var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);
        //            using var ms = new MemoryStream();
        //            await stream.CopyToAsync(ms);
        //            var bytes = ms.ToArray();
        //            var base64 = Convert.ToBase64String(bytes);

        //            Model.Base64Image = $"data:{file.ContentType};base64,{base64}";
        //            Model.IsBannerDeleted = false; // Reset deletion flag

        //            // Create preview URL using JavaScript (similar to ShippingCompany)
        //            previewBannerUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, file.ContentType);

        //            if (string.IsNullOrEmpty(previewBannerUrl))
        //            {
        //                await ShowErrorNotification(
        //                    "Processing Error",
        //                    "Failed to process the selected image. Please try again."
        //                );
        //                Model.Base64Image = null;
        //            }

        //            StateHasChanged();
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.LogError(ex, "Error processing banner image");
        //            await ShowErrorNotification(
        //                "Processing Error",
        //                "An error occurred while processing the image. Please try again."
        //            );

        //            Model.Base64Image = null;
        //            previewBannerUrl = null;
        //            StateHasChanged();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Error handling banner image selection");
        //        await ShowErrorNotification(
        //            "Selection Error",
        //            "An error occurred while selecting the image. Please try again."
        //        );

        //        Model.Base64Image = null;
        //        previewBannerUrl = null;
        //        StateHasChanged();
        //    }
        //}

        // Remove the newly selected banner image (not yet saved)
        //private void RemoveSelectedBannerImage()
        //{
        //    previewBannerUrl = null;
        //    Model.Base64Image = null;
        //    StateHasChanged();
        //}

        //// Mark existing banner for deletion (will be deleted on save)
        //private async Task DeleteExistingBannerImage()
        //{
        //    if (await DeleteConfirmNotification())
        //    {
        //        Model.IsBannerDeleted = true;
        //        previewBannerUrl = null;
        //        Model.Base64Image = null;
        //        StateHasChanged();
        //    }
        //}

        protected virtual async Task<bool> DeleteConfirmNotification()
        {
            var options = new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = new
                {
                    cancel = new
                    {
                        text = ActionsResources.Cancel,
                        value = false,
                        visible = true,
                        className = "",
                        closeModal = true
                    },
                    confirm = new
                    {
                        text = ActionsResources.Confirm,
                        value = true,
                        visible = true,
                        className = "swal-button--danger",
                        closeModal = true
                    }
                }
            };

            return await JSRuntime.InvokeAsync<bool>("swal", options);
        }

        // Undo banner deletion (restore the existing banner)
        //private void UndoBannerDeletion()
        //{
        //    Model.IsBannerDeleted = false;
        //    StateHasChanged();
        //}

        // Helper methods
        private GeneralSettingsDto GetDefaultSettings()
        {
            return new GeneralSettingsDto
            {
                Id = Guid.NewGuid(),
                Email = "contact@company.com",
                Phone = "",
                Address = "",
                FacebookUrl = "",
                InstagramUrl = "",
                TwitterUrl = "",
                LinkedInUrl = "",
                WhatsAppNumber = "",
                SEOTitle = "Your Company Name",
                SEODescription = "Professional services and solutions for your business needs.",
                SEOMetaTags = "business, services, professional, solutions"
            };
        }

        protected string GetCharacterCountClass(int currentLength, int recommendedMax)
        {
            if (currentLength == 0) return "";
            if (currentLength <= recommendedMax) return "character-count-good";
            if (currentLength <= recommendedMax + 10) return "character-count-warning";
            return "character-count-danger";
        }

        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task ShowSuccessNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "success");
        }

        // Handle Language Change
        private async Task InitializeLanguageFromStorage()
        {
            try
            {
                var lang = await JSRuntime.InvokeAsync<string>("localization.getCurrentLanguage");
                ResourceManager.CurrentLanguage = lang.StartsWith("ar") ? Language.Arabic : Language.English;
            }
            catch
            {
                // Fallback to English if localStorage is not available
                ResourceManager.CurrentLanguage = Language.English;
            }
        }

        private void HandleLanguageChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            if (editContext != null)
            {
                editContext.OnValidationStateChanged -= OnValidationStateChanged;
            }

            LanguageService.OnLanguageChanged -= HandleLanguageChanged;
        }
    }
}