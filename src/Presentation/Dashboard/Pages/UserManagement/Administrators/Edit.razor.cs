using Dashboard.Contracts.User;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.User.Admin;

namespace Dashboard.Pages.UserManagement.Administrators
{
    public partial class Edit
    {
        [Parameter] public Guid Id { get; set; } = new();
        protected AdminProfileUpdateDto Model { get; set; } = null;
        private bool isSaving { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IAdminService adminService { get; set; } = null!;

        protected override void OnParametersSet()
        {
            InitializeModel(Id);
        }

        protected async Task InitializeModel(Guid id)
        {
            try
            {
                var result = await adminService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }

                // Initialize model with proper empty collections if null
                Model = new AdminProfileUpdateDto();
                if (result.Data is not null)
                {
                    Model.FirstName = result.Data.FirstName;
                    Model.LastName = result.Data.LastName;
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

                var result = await adminService.UpdateAsync(Id, Model);
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
            Navigation.NavigateTo("/users/administrators");
        }
    }
}
