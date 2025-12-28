using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.CouponCode;

namespace Dashboard.Pages.Sales.CouponCode
{
    public partial class Details
    {
        private bool _isSaving;
        private bool _disposed;

        // Model
        protected CouponCodeDto Model { get; set; } = new();

        // Parameters
        [Parameter] public Guid Id { get; set; }

        // Services
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] private ICouponCodeService CouponCodeService { get; set; } = null!;

        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                Edit(Id);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Initialize if needed
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Initialization error", ex);
            }
        }

        protected async Task Save()
        {
            try
            {
                _isSaving = true;

                var result = await CouponCodeService.SaveAsync(Model);

                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
                    await CloseModal();
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error saving promo code", ex);
            }
            finally
            {
                _isSaving = false;
                StateHasChanged();
            }
        }

        protected async Task Edit(Guid id)
        {
            try
            {
                var result = await CouponCodeService.GetByIdAsync(id);
                if (!result.Success)
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
                    return;
                }

                Model = result.Data ?? new CouponCodeDto();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error editing promo code", ex);
            }
        }

        private async Task CloseModal()
        {
            Navigation.NavigateTo("/couponCodes", true);
        }

        #region Notification Helpers

        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task ShowWarningNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "warning");
        }

        private void ShowWarningNotificationSync(string title, string message)
        {
            _ = ShowWarningNotification(title, message);
        }

        private async Task ShowSuccessNotification(string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
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

        private async Task HandleErrorAsync(string context, Exception ex)
        {
            Console.WriteLine($"{context}: {ex.Message}");
            await ShowErrorNotification(
                ValidationResources.Error,
                NotifiAndAlertsResources.SomethingWentWrongAlert);
        }

        #endregion

        public void Dispose()
        {
            if (!_disposed)
            {
                ResourceLoaderService?.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
