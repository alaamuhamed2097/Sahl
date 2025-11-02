using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.User.Admin;

namespace Dashboard.Pages.UserManagement.Administrators
{
    public partial class Create
    {
        protected AdminRegistrationDto Model { get; set; } = new();
        private bool isSaving { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IAdminService adminService { get; set; } = null!;

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner

                var result = await adminService.CreateAsync(Model);

                isSaving = false;
                if (result.Success)
                {
                    await CloseModal();
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
            finally
            {
                await CloseModal();
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/admin");
        }
    }
}
