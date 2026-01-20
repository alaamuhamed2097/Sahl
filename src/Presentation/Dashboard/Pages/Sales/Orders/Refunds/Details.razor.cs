using Common.Enumerations.Order;
using Dashboard.Configuration;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.DTOs.Order.Payment.Refund;

namespace Dashboard.Pages.Sales.Orders.Refunds
{
    public partial class Details
    {
        protected string baseUrl = string.Empty;
        private bool isSaving { get; set; }
        private bool isWizardInitialized { get; set; }
        private bool statusChanged { get; set; }
        protected RefundDetailsDto RefundModel { get; set; } = new();
        protected RefundResponseDto RefundResponseModel { get; set; } = new();

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

                // Map RefundStatus to wizard step (0-based index)
                int currentStep = GetWizardStepFromStatus(RefundModel.RefundStatus);
                await JSRuntime.InvokeVoidAsync("initializeSmartWizard", currentStep);
                isWizardInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing wizard: {ex.Message}");
            }
        }

        protected async Task View(Guid id)
        {
            try
            {
                var result = await RefundService.GetByIdAsync(id);
                Console.WriteLine($"Result :  success: {result.Success} \n model : {result.Data?.VendorName}");
                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }

                RefundModel = result.Data ?? new();
                Console.WriteLine($"Vendor name :  {RefundModel.VendorName}");
                // Initialize RefundResponseModel with current refund data
                RefundResponseModel.RefundId = RefundModel.Id;
                RefundResponseModel.RefundAmount = RefundModel.RefundAmount;

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

        private int GetWizardStepFromStatus(RefundStatus status)
        {
            return status switch
            {
                RefundStatus.Open => 0,
                RefundStatus.UnderReview => 1,
                RefundStatus.NeedMoreInfo => 1, // Same as UnderReview in wizard
                RefundStatus.InfoApproved => 2,
                RefundStatus.ItemShippedBack => 2, // Same as InfoApproved
                RefundStatus.ItemReceived => 3,
                RefundStatus.Inspecting => 3, // Same as ItemReceived
                RefundStatus.Approved => 4,
                RefundStatus.Refunded => 5,
                RefundStatus.Rejected => 0, // Stay at current position
                RefundStatus.Closed => 5, // Final step
                _ => 0
            };
        }

        private async Task<bool> ChangeRefundStatus(RefundStatus newStatus)
        {
            try
            {
                RefundResponseModel.RefundId = RefundModel.Id;
                RefundResponseModel.CurrentState = newStatus;

                var result = await RefundService.ChangeRefundStatusAsync(RefundResponseModel);

                if (result.Success)
                {
                    RefundModel.RefundStatus = newStatus;

                    if (isWizardInitialized && newStatus != RefundStatus.Rejected)
                    {
                        int step = GetWizardStepFromStatus(newStatus);
                        await JSRuntime.InvokeVoidAsync("updateWizardState", step);
                    }

                    StateHasChanged();

                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.SaveSuccess,
                        NotifiAndAlertsResources.Success,
                        "success");

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

                // Update tracking number first
                var saveModel = new RefundRequestDto
                {
                    Id = RefundModel.Id,
                    ReturnTrackingNumber = RefundModel.ReturnTrackingNumber
                };

                var updateResult = await RefundService.UpdateAsync(saveModel);

                if (!updateResult.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.SaveFailed,
                        "error");
                    return;
                }

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

                // Validate refund amount
                if (RefundResponseModel.RefundAmount <= 0)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.InvalidRefundAmount,
                        "warning");
                    return;
                }

                // Validate transaction ID
                if (string.IsNullOrWhiteSpace(RefundModel.RefundTransactionId))
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        OrderResources.TransactionIdRequired,
                        "warning");
                    return;
                }

                // Update refund details
                var saveModel = new RefundRequestDto
                {
                    Id = RefundModel.Id,
                    RefundTransactionId = RefundModel.RefundTransactionId,
                    RefundAmount = RefundResponseModel.RefundAmount
                };

                var updateResult = await RefundService.UpdateAsync(saveModel);

                if (!updateResult.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.SaveFailed,
                        "error");
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

                    // Navigate back to refunds list
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
    }
}