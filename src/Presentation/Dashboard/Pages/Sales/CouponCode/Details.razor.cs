using Common.Enumerations.Order;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.CouponCode;

namespace Dashboard.Pages.Sales.CouponCode
{
    public partial class Details : ComponentBase, IDisposable
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
        [Inject] private IResourceLoaderService? ResourceLoaderService { get; set; }
        [Inject] private ICouponCodeService CouponCodeService { get; set; } = null!;

        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                _ = Edit(Id);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Initialize default values
                if (Model.Id == Guid.Empty)
                {
                    Model.IsActive = true;
                    Model.DiscountType = DiscountType.Percentage;
                    Model.PromoType = CouponCodeType.General;
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ValidationResources.Error, ex);
            }
        }

        protected async Task Save()
        {
            try
            {
                _isSaving = true;

                // Validate model
                if (!ValidateModel())
                {
                    _isSaving = false;
                    return;
                }

                // Trim and uppercase code
                if (!string.IsNullOrWhiteSpace(Model.Code))
                {
                    Model.Code = Model.Code.Trim().ToUpper();
                }

                var result = await CouponCodeService.SaveAsync(Model);

                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
                    GoBack();
                }
                else
                {
                    var errorMessage = result.Message ?? NotifiAndAlertsResources.SaveFailed;
                    await ShowErrorNotification(ValidationResources.Failed, errorMessage);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ValidationResources.Error, ex);
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
                await HandleErrorAsync(ValidationResources.Error, ex);
            }
        }

        private bool ValidateModel()
        {
            // Validate percentage
            if (Model.DiscountType == DiscountType.Percentage
                && Model.DiscountValue > 100)
            {
                ShowWarningNotificationSync(
                    ValidationResources.Failed,
                    CouponCodeResources.PercentageValueInvalid);
                return false;
            }

            // Validate dates
            if (Model.ExpiryDate.HasValue
                && Model.StartDate > Model.ExpiryDate)
            {
                ShowWarningNotificationSync(
                    ValidationResources.Failed,
                    CouponCodeResources.StartDateMustBeBeforeEndDate);
                return false;
            }

            // Validate co-funded coupon
            if (Model.PlatformSharePercentage.HasValue && Model.PlatformSharePercentage.Value > 0)
            {
                if (!Model.VendorId.HasValue)
                {
                    ShowWarningNotificationSync(
                        ValidationResources.Failed,
                        CouponCodeResources.VendorRequiredForCoFunded);
                    return false;
                }
            }

            // Validate scope for category/vendor based
            if (Model.PromoType == CouponCodeType.CategoryBased
                || Model.PromoType == CouponCodeType.VendorBased)
            {
                if (Model.ScopeItems == null || !Model.ScopeItems.Any())
                {
                    ShowWarningNotificationSync(
                        ValidationResources.Failed,
                        CouponCodeResources.ScopeItemsRequired);
                    return false;
                }
            }

            return true;
        }

        private void GoBack()
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