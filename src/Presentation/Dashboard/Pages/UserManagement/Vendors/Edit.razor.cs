using Dashboard.Contracts.Vendor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Vendor;

namespace Dashboard.Pages.UserManagement.Vendors
{
    public partial class Edit
    {
        [Parameter] public Guid Id { get; set; } = new();
        protected VendorDto Model { get; set; } = null;
        private bool isSaving { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IVendorService _vendorService { get; set; } = null!;

        protected override void OnParametersSet()
        {
            InitializeModel(Id);
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

                // Initialize model with proper empty collections if null
                Model = new VendorDto();
                if (result.Data is not null)
                {
                    Model.CompanyName = result.Data.CompanyName;
                    Model.ContactName = result.Data.ContactName;
                    Model.CommercialRegister = result.Data.CommercialRegister;
                    Model.TaxNumber = result.Data.TaxNumber;
                    Model.Address = result.Data.Address;
                    Model.VendorCode = result.Data.VendorCode;
                    Model.IsActive = result.Data.IsActive;
                    Model.Notes = result.Data.Notes;
                    Model.PostalCode = result.Data.PostalCode;
                    Model.Rating = result.Data.Rating;
                    Model.VATRegistered = result.Data.VATRegistered;
                    Model.VendorType = result.Data.VendorType;
				}
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

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner

                var result = await _vendorService.UpdateAsync(Id, Model);
                isSaving = false;

                if (result.Success)
                {
                    CloseModal();
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully, "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.FailedAlert,
                    "error");
            }
        }

        protected void CloseModal()
        {
            Navigation.NavigateTo("/users/vendors");
        }
    }
}
