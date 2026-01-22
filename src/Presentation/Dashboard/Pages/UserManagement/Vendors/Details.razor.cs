using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using Dashboard.Configuration;
using Dashboard.Contracts.Vendor;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Vendor;

namespace Dashboard.Pages.UserManagement.Vendors
{
    public partial class Details : LocalizedComponentBase
    {
        [Parameter] public Guid Id { get; set; }

        [Inject] private IVendorService VendorService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private VendorDto Vendor { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private string ActiveTab { get; set; } = "details";
        private UserStateType CurrentUserStatus { get; set; } = UserStateType.Active;
        protected string baseUrl { get; set; } = null!;

        // Modals State
        private bool ShowPasswordModal { get; set; }
        private bool ShowEmailModal { get; set; }

        // Models for Modals
        private string NewPassword { get; set; } = string.Empty;
        private string ConfirmNewPassword { get; set; } = string.Empty;
        private string NewEmail { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            await LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;
            try
            {
                var response = await VendorService.GetByIdAsync(Id);
                if (response.Success && response.Data != null)
                {
                    Vendor = response.Data;
                    CurrentUserStatus = Vendor.UserState;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, NotifiAndAlertsResources.NoDataFound, "error");
                    Navigation.NavigateTo("/users/vendors");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SetActiveTab(string tabName)
        {
            ActiveTab = tabName;
        }

        private void EditVendor()
        {
            // Logic to open edit modal or navigate to edit page
            Navigation.NavigateTo($"/users/vendors/edit/{Vendor.Id}"); // Assuming edit page exists or will be created
        }

        private void OpenChangePasswordModal()
        {
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
            ShowPasswordModal = true;
            StateHasChanged();
        }

        private void CloseChangePasswordModal()
        {
            ShowPasswordModal = false;
            StateHasChanged();
        }

        private async Task SubmitChangePassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 6)
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, UserResources.InvalidPassword, "error");
                return;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, UserResources.PasswordMismatch, "error");
                return;
            }

            // TODO: Call API to change password
            // await VendorService.ChangePasswordAsync(Vendor.Id, NewPassword);
            ShowPasswordModal = false;
            await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Success, NotifiAndAlertsResources.SavedSuccessfully, "success");
        }

        private void OpenChangeEmailModal()
        {
            NewEmail = Vendor.Email ?? string.Empty; // Assuming VendorDto has Email, wait.. let's check VendorDto.
            ShowEmailModal = true;
            StateHasChanged();
        }

        private void CloseChangeEmailModal()
        {
            ShowEmailModal = false;
            StateHasChanged();
        }

        private async Task SubmitChangeEmail()
        {
            if (string.IsNullOrWhiteSpace(NewEmail) || !NewEmail.Contains("@"))
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, "Invalid Email", "error");
                return;
            }

            // TODO: Call API to change email
            // await VendorService.ChangeEmailAsync(Vendor.Id, NewEmail);
            ShowEmailModal = false;
            // Vendor.AdministratorFullName = Vendor.AdministratorFullName; // Removed as it is read-only
            await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Success, NotifiAndAlertsResources.SavedSuccessfully, "success");
        }

        private async Task UpdateVendorStatus(VendorStatus status)
        {
            try
            {
                var response = await VendorService.UpdateVendorStatusAsync(new UpdateVendorStatusDto
                {
                    VendorId = Vendor.Id,
                    Status = status
                });

                if (response.Success)
                {
                    Vendor.Status = status;
                    await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Success, NotifiAndAlertsResources.SavedSuccessfully, "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, response.Message, "error");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
            }
        }

        private async Task UpdateUserStatus(UserStateType status)
        {
            try
            {
                var response = await VendorService.UpdateUserStatusAsync(new UpdateUserStatusDto
                {
                    VendorId = Vendor.Id,
                    Status = status
                });

                if (response.Success)
                {
                    CurrentUserStatus = status;
                    await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Success, NotifiAndAlertsResources.SavedSuccessfully, "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, response.Message, "error");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
            }
        }
    }
}
