using Common.Enumerations.Order;
using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.Payment.Refund;

namespace Dashboard.Pages.Orders.Orders.Refunds
{
    public partial class Details : LocalizedComponentBase
    {
        protected string baseUrl = string.Empty;
        private bool isSaving { get; set; }
        private bool isWizardInitialized { get; set; }
        private bool statusChanged { get; set; }
        protected bool IsLoading { get; set; }

        protected RefundDetailsDto RefundModel { get; set; } = new();
        protected RefundResponseDto RefundResponseModel { get; set; } = new();

        // Additional fields for status change
        protected string RejectionReasonInput { get; set; } = string.Empty;
        protected string AdminNotesInput { get; set; } = string.Empty;
        protected int? ApprovedItemsCountInput { get; set; }

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IRefundService RefundService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            if (Id != Guid.Empty)
            {
                await View(Id);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;
            try
            {
                await ResourceLoaderService.LoadStyleSheets([
                    "assets/plugins/smart-wizard/css/smart_wizard.min.css",
                    "assets/plugins/smart-wizard/css/smart_wizard_theme_arrows.min.css",
                    "assets/css/wizard-arrow.css"
                ]);

                await ResourceLoaderService.LoadScriptsSequential(
                    "assets/plugins/smart-wizard/js/jquery.smartWizard.min.js",
                    "assets/js/pages/wizard-custom.js",
                    "assets/js/wizard-arrow.js"
                );

                await JSRuntime.InvokeVoidAsync("initializeSmartWizard", (int)RefundModel.RefundStatus);
                isWizardInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing wizard: {ex.Message}");
            }
        }

        protected async Task View(Guid id)
        {
            IsLoading = true;
            StateHasChanged();

            try
            {
                var result = await RefundService.GetByIdAsync(id);
                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }

                RefundModel = result.Data ?? new();
                RefundResponseModel.RefundId = RefundModel.Id;
                RefundResponseModel.RefundAmount = RefundModel.RefundAmount;
                ApprovedItemsCountInput = RefundModel.ApprovedItemsCount;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private int GetWizardStepFromStatus(RefundStatus status)
        {
            return status switch
            {
                RefundStatus.Open => 0,
                RefundStatus.UnderReview => 1,
                RefundStatus.NeedMoreInfo => 1,
                RefundStatus.InfoApproved => 2,
                RefundStatus.ItemShippedBack => 2,
                RefundStatus.ItemReceived => 3,
                RefundStatus.Inspecting => 3,
                RefundStatus.Approved => 4,
                RefundStatus.Refunded => 5,
                RefundStatus.Rejected => 0,
                RefundStatus.Closed => 5,
                _ => 0
            };
        }

        private string GetRefundReason(RefundReason reason)
        {
            return reason switch
            {
                RefundReason.DefectiveProduct => OrderResources.DefectiveProduct,
                RefundReason.WrongItemShipped => OrderResources.WrongItemShipped,
                RefundReason.ItemNotAsDescribed => OrderResources.ItemNotAsDescribed,
                RefundReason.DamagedDuringShipping => OrderResources.DamagedDuringShipping,
                RefundReason.ChangedMind => OrderResources.ChangedMind,
                RefundReason.OrderedByMistake => OrderResources.OrderedByMistake,
                RefundReason.BetterPriceFound => OrderResources.BetterPriceFound,
                RefundReason.LateDelivery => OrderResources.LateDelivery,
                RefundReason.MissingParts => OrderResources.MissingParts,
                RefundReason.QualityNotSatisfactory => OrderResources.QualityNotSatisfactory,
                RefundReason.Other => OrderResources.Other,
                _ => "Something else!"
            };
        }


        private async Task<bool> ChangeRefundStatus(RefundStatus newStatus)
        {
            try
            {
                var changeStatus = new UpdateRefundStatusDto()
                {
                    RefundId = RefundModel.Id,
                    NewStatus = newStatus,
                    RefundAmount = RefundResponseModel.RefundAmount > 0 ? RefundResponseModel.RefundAmount : null,
                    RejectionReason = newStatus == RefundStatus.Rejected && !string.IsNullOrWhiteSpace(RejectionReasonInput)
                        ? RejectionReasonInput
                        : null,
                    TrackingNumber = !string.IsNullOrWhiteSpace(RefundModel.ReturnTrackingNumber)
                        ? RefundModel.ReturnTrackingNumber
                        : null,
                    ApprovedItemsCount = ApprovedItemsCountInput,
                    Notes = !string.IsNullOrWhiteSpace(AdminNotesInput)
                        ? AdminNotesInput
                        : null,
                };

                var result = await RefundService.ChangeRefundStatusAsync(changeStatus);

                if (result.Success)
                {
                    RefundModel.RefundStatus = newStatus;

                    if (isWizardInitialized && newStatus != RefundStatus.Rejected)
                    {
                        //int step = GetWizardStepFromStatus(newStatus);
                        await JSRuntime.InvokeVoidAsync("updateWizardState", newStatus);
                    }

                    // Clear input fields after successful status change
                    RejectionReasonInput = string.Empty;
                    AdminNotesInput = string.Empty;

                    StateHasChanged();
                    return true;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.SomethingWentWrong,
                        "error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
                return false;
            }
        }

        private async Task MoveToUnderReview()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.Open)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                statusChanged = await ChangeRefundStatus(RefundStatus.UnderReview);

                if (isWizardInitialized && statusChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }
        private async Task MoveToNeedMoreInfo()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.UnderReview)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }
                if (string.IsNullOrWhiteSpace(AdminNotesInput))
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.RejectionReasonRequiredForRefund,
                        "warning");
                    return;
                }
                statusChanged = await ChangeRefundStatus(RefundStatus.NeedMoreInfo);

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }
        private async Task MoveToRejected()
        {
            try
            {
                if (RefundModel.RefundStatus > RefundStatus.Inspecting)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }
                if (string.IsNullOrWhiteSpace(RejectionReasonInput))
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        "Rejection reason is required when rejecting a refund",
                        "warning");
                    return;
                }
                statusChanged = await ChangeRefundStatus(RefundStatus.Rejected);

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task MoveToInfoApproved()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.UnderReview &&
                    RefundModel.RefundStatus != RefundStatus.NeedMoreInfo)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                if (ApprovedItemsCountInput <= 0)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.ApprovedItemsRequired,
                        "warning");
                    return;
                }

                statusChanged = await ChangeRefundStatus(RefundStatus.InfoApproved);

                if (isWizardInitialized && statusChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task MoveToItemShippedBack()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.InfoApproved)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                if (string.IsNullOrWhiteSpace(RefundModel.ReturnTrackingNumber))
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.TrackingNumberRequired,
                        "warning");
                    return;
                }

                var saveModel = new RefundRequestDto
                {
                    Id = RefundModel.Id,
                    ReturnTrackingNumber = RefundModel.ReturnTrackingNumber
                };

                statusChanged = await ChangeRefundStatus(RefundStatus.ItemShippedBack);
                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task MoveToItemReceived()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.InfoApproved &&
                    RefundModel.RefundStatus != RefundStatus.ItemShippedBack)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                statusChanged = await ChangeRefundStatus(RefundStatus.ItemReceived);

                if (isWizardInitialized && statusChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task MoveToApproved()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.ItemReceived &&
                    RefundModel.RefundStatus != RefundStatus.Inspecting)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                statusChanged = await ChangeRefundStatus(RefundStatus.Approved);

                if (isWizardInitialized && statusChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task MoveToRefunded()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.Approved)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                if (RefundResponseModel.RefundAmount <= 0)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidRefundAmount,
                        "warning");
                    return;
                }

                statusChanged = await ChangeRefundStatus(RefundStatus.Refunded);

                if (isWizardInitialized && statusChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        private async Task CloseRefund()
        {
            try
            {
                if (RefundModel.RefundStatus != RefundStatus.Refunded)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidStatusTransition,
                        "warning");
                    return;
                }

                statusChanged = await ChangeRefundStatus(RefundStatus.Closed);

                if (statusChanged)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.SaveSuccess,
                        OrderResources.RefundClosedSuccessfully,
                        "success");

                    Navigation.NavigateTo("/refunds");
                }

                statusChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }
        /// <summary>
        /// Navigate back to refunds list
        /// </summary>
        protected void NavigateToList()
        {
            Navigation.NavigateTo("/order/refunds");
        }
    }
}