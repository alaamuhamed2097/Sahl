using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.User;
using Dashboard.Contracts.User.Student;
using Dashboard.Services.User;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.User;
using Shared.DTOs.User.Student;

namespace Dashboard.Pages.User.UserDetails.Components
{
    public partial class BasicInfoTab
    {
        [Parameter] public Guid UserId { get; set; }

        private bool showEditPopup = false;
        private bool isSaving = false;
        private UserProfileUpdateDto userForm = new();
        private VwStudentDto? student;
        private string baseImageUrl = string.Empty;
        private bool isLoading = true;
        private string? imagePreview;
        private string? originalImage;
        private bool showChangePasswordPopup = false;
        private bool isChangingPassword = false;
        private AdminResetPasswordDto changePasswordForm = new();
        private bool showSendEmailPopup = false;
        private bool isSendingEmail = false;
        private AdminSendEmailDto sendEmailForm = new();

        [Inject] protected IStudentService StudentService { get; set; } = null!;
        [Inject] protected IUserActionService UserActionService { get; set; } = null!;
        [Inject] protected IUserProfileService UserProfileService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IResourceLoaderService ResourceLoaderService { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            baseImageUrl = ApiOptions.Value.ITLegendProfileImagesUrl;
            await LoadStudentAsync();
        }

        private async Task SendActivationEmail()
        {
            try
            {
                var result = await UserProfileService.SendActivationCode(UserId.ToString());
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SentSuccessfully);
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SendFailed);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error sending activation email", ex);
            }
        }

        private async Task LoadStudentAsync()
        {
            try
            {
                isLoading = true;
                var res = await StudentService.GetById(UserId.ToString());
                if (res != null && res.Success)
                {
                    student = res.Data;
                }
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task SubmitSendEmail()
        {
            if (isSendingEmail) return;

            try
            {
                isSendingEmail = true;
                var result = await UserProfileService.SendEmail(UserId.ToString(), sendEmailForm);
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SentSuccessfully);
                    showSendEmailPopup = false;
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SendFailed);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error sending email", ex);
            }
            finally
            {
                isSendingEmail = false;
                StateHasChanged();
            }
        }

        private void OpenEditPopup()
        {
            if (student != null)
            {
                userForm = new()
                {
                    FristName = student.FristName,
                    LastName = student.LastName,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    CountryCode = student.CountryCode,
                    Image = ProfileImage
                };
                originalImage = student.Image;
                imagePreview = null;
            }
            showEditPopup = true;
        }

        private void CloseEditPopup()
        {
            showEditPopup = false;
        }

        private void OpenChangePasswordPopup()
        {
            changePasswordForm = new AdminResetPasswordDto();
            showChangePasswordPopup = true;
        }

        private void CloseChangePasswordPopup()
        {
            showChangePasswordPopup = false;
        }

        private void OpenSendEmailPopup()
        {
            sendEmailForm = new AdminSendEmailDto
            {
                Title = string.Empty,
                Subject = string.Empty,
                Message = string.Empty
            };
            showSendEmailPopup = true;
        }

        private void CloseSendEmailPopup()
        {
            showSendEmailPopup = false;
        }

        private async Task SaveUserEdits()
        {
            if (isSaving) return;

            try
            {
                isSaving = true;

                // update user profile 
                var result = await UserProfileService.UpdateUserProfile(UserId.ToString(), userForm);
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
                    showEditPopup = false;
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error saving package", ex);
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
            }

            await Task.CompletedTask;
        }

        private async Task SubmitChangePassword()
        {
            if (isChangingPassword) return;

            try
            {
                isChangingPassword = true;
                var result = await UserProfileService.ResetPassword(UserId.ToString(), changePasswordForm);
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
                    showChangePasswordPopup = false;
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, result.Message ?? NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error changing password", ex);
            }
            finally
            {
                isChangingPassword = false;
                StateHasChanged();
            }
        }

        protected virtual async Task Delete(Guid id)
        {
            var confirmed = await ShowDeleteConfirmation();

            if (confirmed)
            {
                var result = await UserActionService.DeleteUser(id.ToString());
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
                    StateHasChanged();
                }
                else
                {
                    if (result.Message == null)
                        await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.DeleteFailed);
                    else
                        await ShowErrorNotification(ValidationResources.Failed, result.Message);
                }
            }
        }

        protected virtual async Task ReActive(Guid id)
        {
            var confirmed = await ShowWarningNotification(NotifiAndAlertsResources.ConfirmReActiveAlert, "");

            if (confirmed)
            {
                var result = await UserActionService.ReActivateUser(id.ToString());
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.UserActivatedSuccessfully);
                    StateHasChanged();
                }
                else
                {
                    if (result.Message == null)
                        await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.UserActivationFailed);
                    else
                        await ShowErrorNotification(ValidationResources.Failed, result.Message);
                }
            }
        }

        protected virtual async Task Block(Guid id)
        {
            var confirmed = await ShowWarningNotification(NotifiAndAlertsResources.ConfirmBlockAlert, "");
            if (confirmed)
            {
                var result = await UserActionService.BlockUser(id.ToString());
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.UserBlockedSuccessfully);
                    StateHasChanged();
                }
                else
                {
                    if (result.Message == null)
                        await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.UserBlockFailed);
                    else
                        await ShowErrorNotification(ValidationResources.Failed, result.Message);
                }
            }
        }

        #region Image

        private string ProfileImage
        {
            get
            {
                if (student == null || string.IsNullOrWhiteSpace(student.Image))
                    return string.Empty;

                return student.Image.StartsWith("data:", StringComparison.OrdinalIgnoreCase)
                    ? student.Image
                    : ($"{baseImageUrl}/{student.Image}");
            }
        }

        private async Task HandleFileSelected(Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
                try
                {
                    const long maxFileSize = 10 * 1024 * 1024; // 10MB
                    if (file.Size > maxFileSize)
                    {
                        await ShowErrorNotification(ValidationResources.Failed, "Image file is too large. Maximum size is 10MB.");
                        return;
                    }

                    using var memoryStream = new MemoryStream();
                    await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    var base64String = Convert.ToBase64String(fileBytes);
                    string mimeType = file.ContentType;
                    imagePreview = $"data:{mimeType};base64,{base64String}";

                    // Set for API submission
                    userForm.Image = base64String;

                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    await ShowErrorNotification(ValidationResources.Failed, $"Error processing image: {ex.Message}");
                }
            }
        }

        private static bool IsAbsoluteUrl(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("data:", StringComparison.OrdinalIgnoreCase);
        }

        private string GetImageSrc(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return string.Empty;

            if (IsAbsoluteUrl(imagePath))
                return imagePath;

            if (string.IsNullOrWhiteSpace(baseImageUrl))
                return imagePath;

            if (baseImageUrl.EndsWith("/"))
                return imagePath.StartsWith("/") ? baseImageUrl + imagePath.TrimStart('/') : baseImageUrl + imagePath;

            return imagePath.StartsWith("/") ? baseImageUrl + imagePath : baseImageUrl + "/" + imagePath;
        }

        #endregion

        #region Notification Helpers

        private async Task ShowSuccessNotification(string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
        }

        private async Task<bool> ShowWarningNotification(string title, string message)
        {
            return await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = title,
                text = message,
                icon = "warning",
                buttons = new
                {
                    cancel = ActionsResources.Cancel,
                    confirm = ActionsResources.Confirm,
                },
                dangerMode = true,
            });
        }

        private async Task<bool> ShowDeleteConfirmation()
        {
            return await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = new
                {
                    cancel = ActionsResources.Cancel,
                    confirm = ActionsResources.Confirm,
                },
                dangerMode = true,
            });
        }

        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task HandleErrorAsync(string context, Exception ex)
        {
            Console.WriteLine($"{context}: {ex.Message}");
            await ShowErrorNotification(
                ValidationResources.Error,
                NotifiAndAlertsResources.SomethingWentWrongAlert);
        }

        #endregion
    }
}
