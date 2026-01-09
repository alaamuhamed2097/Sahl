using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.Page;
using Shared.GeneralModels;

using Resources;
using System.ComponentModel.DataAnnotations;
using Common.Enumerations;
using Dashboard.Contracts.Page;

namespace UI.Pages.StaticPage
{
    public partial class StaticPageModal : ComponentBase
    {
        [Parameter] public Guid PageId { get; set; }

        [Inject] private IPageService PageService { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private ILogger<StaticPageModal> Logger { get; set; } = null!;

        // Component state
        protected PageDto Model { get; set; } = new();
        protected bool IsSaving { get; set; } = false;
        protected string ErrorMessage { get; set; } = string.Empty;
        protected string SuccessMessage { get; set; } = string.Empty;

        // Lifecycle methods
        protected override async Task OnInitializedAsync()
        {
            // Only allow editing existing pages
            if (PageId != Guid.Empty)
            {
                await LoadPage();
            }
            else
            {
                // Redirect to pages list if trying to access without PageId
                ErrorMessage = "Access denied. You can only edit existing pages.";
                Navigation.NavigateTo("/pages");
                return;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && PageId != Guid.Empty)
            {
                // Ensure TinyMCE is loaded before proceeding
                await Task.Delay(300); // Give time for editors to initialize
                StateHasChanged(); // Trigger re-render if needed
            }
        }

        // Data loading methods
        private async Task LoadPage()
        {
            try
            {
                var result = await PageService.GetByIdAsync(PageId);
                if (result.Success && result.Data != null)
                {
                    Model = result.Data;
                }
                else
                {
                    ErrorMessage = result.Message ?? "Failed to load page";
                    // Redirect back to pages list if page not found
                    await Task.Delay(2000);
                    Navigation.NavigateTo("/pages");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading page {PageId}", PageId);
                ErrorMessage = $"Error loading page: {ex.Message}";
                // Redirect back to pages list on error
                await Task.Delay(2000);
                Navigation.NavigateTo("/pages");
            }
        }

        // Form submission - Only updates existing pages
        protected async Task Save()
        {
            try
            {
                // Ensure we're only updating existing pages
                if (PageId == Guid.Empty)
                {
                    ErrorMessage = "Cannot save new pages. Only existing pages can be modified.";
                    return;
                }

                IsSaving = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                // Set update date for existing page
                Model.UpdatedDateUtc = DateTime.UtcNow;

                var result = await PageService.SaveAsync(Model);
                
                if (result.Success)
                {
                    SuccessMessage = "Page updated successfully";
                    await JSRuntime.InvokeVoidAsync("console.log", "Page updated successfully");
                    
                    // Redirect after a short delay
                    await Task.Delay(1500);
                    Navigation.NavigateTo("/pages");
                }
                else
                {
                    ErrorMessage = result.Message ?? "Error updating page";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating page");
                ErrorMessage = $"Error updating page: {ex.Message}";
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        // Helper methods
        protected List<KeyValuePair<PageType, string>> GetPageTypes()
        {
            return Enum.GetValues<PageType>()
                      .Select(pt => new KeyValuePair<PageType, string>(pt, GetPageTypeDisplayName(pt)))
                      .ToList();
        }

        protected string GetPageTypeDisplayName(PageType pageType)
        {
            return pageType switch
            {
                PageType.AboutUs => "About Us",
                PageType.PrivacyPolicy => "Privacy Policy",
                PageType.TermsAndConditions => "Terms & Conditions",
                PageType.ContactUs => "Contact Us",
                PageType.RefundPolicy => "Refund Policy",
                _ => pageType.ToString()
            };
        }

        // Navigation methods
        protected void CloseModal()
        {
            Navigation.NavigateTo("/pages");
        }

        protected void Cancel()
        {
            Navigation.NavigateTo("/pages");
        }

        // Validation helpers
        protected string GetValidationClass(string propertyName)
        {
            return string.Empty;
        }
    }
}