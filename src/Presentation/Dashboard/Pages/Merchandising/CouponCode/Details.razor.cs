using Common.Enumerations.Order;
using Common.Filters;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Merchandising;
using Dashboard.Models.pagintion;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Merchandising.PromoCode;
using Shared.DTOs.Order.CouponCode;

namespace Dashboard.Pages.Merchandising.CouponCode
{
    public partial class Details
    {
        private bool _isSaving;
        private bool _disposed;
        private bool _isLoadingParticipation;

        protected PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto> ParticipationRequests { get; set; }
            = new PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>(
                new List<AdminVendorPromoCodeParticipationRequestListDto>(),
                0);

        // Model
        protected CouponCodeDto Model { get; set; } = new();

        // Parameters
        [Parameter] public Guid Id { get; set; }

        // Services
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IResourceLoaderService? ResourceLoaderService { get; set; }
        [Inject] private ICouponCodeService CouponCodeService { get; set; } = null!;
        [Inject] private IVendorPromoCodeParticipationAdminService VendorPromoParticipationAdminService { get; set; } = null!;
        [Inject] private Dashboard.Contracts.ECommerce.Category.ICategoryService CategoryService { get; set; } = null!;
        [Inject] private Dashboard.Contracts.ECommerce.Item.IItemService ItemService { get; set; } = null!;

        // Category Selection
        protected List<Shared.DTOs.Catalog.Category.CategoryDto> SelectedCategories { get; set; } = new();
        
        // Item Selection
        protected List<Shared.DTOs.Catalog.Item.ItemDto> SelectedItems { get; set; } = new();

        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                _ = Edit(Id);
                _ = LoadParticipationRequestsAsync();
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

                await LoadParticipationRequestsAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ValidationResources.Error, ex);
            }
        }

        private async Task LoadParticipationRequestsAsync()
        {
            try
            {
                if (Id == Guid.Empty)
                    return;

                _isLoadingParticipation = true;

                var request = new AdminVendorPromoCodeParticipationListRequestDto
                {
                    PromoCodeId = Id,
                    Criteria = new BaseSearchCriteriaModel
                    {
                        PageNumber = 1,
                        PageSize = 100
                    }
                };

                var result = await VendorPromoParticipationAdminService.ListAsync(request);
                if (result.Success && result.Data != null)
                    ParticipationRequests = result.Data;
                else
                    ParticipationRequests = new PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>(
                        new List<AdminVendorPromoCodeParticipationRequestListDto>(),
                        0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadParticipationRequestsAsync: {ex.Message}");
            }
            finally
            {
                _isLoadingParticipation = false;
                StateHasChanged();
            }
        }

        protected string GetRequestStatusBadge(int status)
        {
            // SellerRequestStatus enum values live in API; keep dashboard mapping lightweight
            return status switch
            {
                2 => "bg-warning text-dark", // Pending
                3 => "bg-info",             // UnderReview
                5 => "bg-success",          // Approved
                6 => "bg-danger",           // Rejected
                8 => "bg-secondary",        // Cancelled
                _ => "bg-secondary"
            };
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

                // Load selected categories if CategoryBased
                if (Model.PromoType == CouponCodeType.CategoryBased && Model.ScopeItems != null && Model.ScopeItems.Any())
                {
                    var scopeIds = Model.ScopeItems.Select(s => s.ScopeId).ToHashSet();
                    // Ideally we would fetch by IDs, but service only has GetAll
                    var response = await CategoryService.GetAllAsync();
                    if (response.Success && response.Data != null)
                    {
                        SelectedCategories = response.Data.Where(c => scopeIds.Contains(c.Id)).ToList();
                    }
                }
                // Load selected items if ItemBased
                else if (Model.PromoType == CouponCodeType.ItemBased && Model.ScopeItems != null && Model.ScopeItems.Any())
                {
                     var scopeIds = Model.ScopeItems.Select(s => s.ScopeId).ToHashSet();
                     // Ideally we would fetch by IDs, but service only has GetAll
                     var response = await ItemService.GetAllAsync();
                     if (response.Success && response.Data != null)
                     {
                         SelectedItems = response.Data.Where(i => scopeIds.Contains(i.Id)).ToList();
                     }
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ValidationResources.Error, ex);
            }
        }

        protected Task OnSelectedCategoriesChanged(List<Shared.DTOs.Catalog.Category.CategoryDto> categories)
        {
            SelectedCategories = categories;

            // Update Model.ScopeItems
            if (Model.PromoType == CouponCodeType.CategoryBased)
            {
                Model.ScopeItems = categories.Select(c => new CouponScopeDto
                {
                    Id = Guid.NewGuid(),
                    ScopeType = CouponCodeScopeType.Category,
                    ScopeId = c.Id
                }).ToList();
            }

            return Task.CompletedTask;
        }

        protected Task OnSelectedItemsChanged(List<Shared.DTOs.Catalog.Item.ItemDto> items)
        {
            SelectedItems = items;

            // Update Model.ScopeItems
            if (Model.PromoType == CouponCodeType.ItemBased)
            {
                Model.ScopeItems = items.Select(i => new CouponScopeDto
                {
                    Id = Guid.NewGuid(),
                    ScopeType = CouponCodeScopeType.Item,
                    ScopeId = i.Id
                }).ToList();
            }

            return Task.CompletedTask;
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

            // Validate scope for category/vendor/item based
            if (Model.PromoType == CouponCodeType.CategoryBased
                || Model.PromoType == CouponCodeType.VendorBased
                || Model.PromoType == CouponCodeType.ItemBased)
            {
                // VendorBased might not need ScopeItems if it applies to all vendor items, 
                // but checking if implementation requires specific scope logic.
                // Assuming VendorBased applies to everything from that vendor, so ScopeItems might be empty.
                // But for Category/Item based, we definitely need selection.

                if (Model.PromoType != CouponCodeType.VendorBased && (Model.ScopeItems == null || !Model.ScopeItems.Any()))
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