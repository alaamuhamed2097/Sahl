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
using Shared.DTOs.ECommerce.Order;

namespace Dashboard.Pages.Sales.Orders
{
    public partial class Details
    {
        protected string baseUrl = string.Empty;
        private bool isSaving { get; set; }
        private bool isRefunded { get; set; }
        private bool isWizardInitialized { get; set; }
        private bool orderChanged { get; set; }
        protected OrderDto Model { get; set; } = new();
        protected RefundDto RefundModel { get; set; } = new();
        protected RefundResponseDto RefundResponseModel { get; set; } = new();
        protected List<ShippingCompanyDto> ShippingCompanies { get; set; } = new();
        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IOrderService OrderService { get; set; } = null!;
        [Inject] protected IRefundService RefundService { get; set; } = null!;
        [Inject] protected IShippingCompanyService ShippingCompanyService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var task1 = CheckForExistingRefund();
            var task2 = LoadShippingCompanies();

            await Task.WhenAll(task1, task2);
        }
        protected override void OnParametersSet()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            if (Id != Guid.Empty)
            {
                View(Id);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;
            try
            {
                await ResourceLoaderService.LoadStyleSheets(["assets/plugins/smart-wizard/css/smart_wizard.min.css",
                                                            "assets/plugins/smart-wizard/css/smart_wizard_theme_arrows.min.css",
                                                            "assets/css/pages/wizard-arrow.css"]);

                await ResourceLoaderService.LoadScriptsSequential("assets/plugins/smart-wizard/js/jquery.smartWizard.min.js",
                                                                "assets/js/pages/wizard-custom.js",
                                                                "assets/js/pages/wizard-arrow.js");


                await JSRuntime.InvokeVoidAsync("initializeSmartWizard", (int)Model.CurrentState);
                isWizardInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing wizard: {ex.Message}");
            }
        }

        protected async Task<bool> Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged(); // Force UI update to show spinner

                var result = await OrderService.SaveAsync(Model);

                isSaving = false;
                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.FailedAlert, "error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.FailedAlert,
                    "error");
                return false;
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
            }
        }

        protected async Task View(Guid id)
        {
            try
            {
                var result = await OrderService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                    return;
                }
                // Initialize model with proper empty collections if null
                Model = result.Data ?? new();
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

        private async Task<bool> ChangeOrderStatus(OrderStatus newStatus)
        {
            try
            {
                Model.CurrentState = newStatus;
                var result = await OrderService.ChangeOrderStatusAsync(Model);

                if (result.Success)
                {
                    // Update the wizard to reflect the new state (only updates position, doesn't move forward)
                    if (isWizardInitialized)
                    {
                        await JSRuntime.InvokeVoidAsync("updateWizardState", (int)newStatus);
                    }
                    StateHasChanged();
                    return true;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SomethingWentWrong, "error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
                return false;
            }
        }

        private async Task<bool> ChangeRefundStatus(RefundStatus newStatus)
        {
            try
            {
                RefundResponseModel.RefundId = RefundModel.Id;
                RefundResponseModel.CurrentState = newStatus;

                var saved = await RefundService.ChangeRefundStatusAsync(RefundResponseModel);
                if (saved.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.SaveSuccess, NotifiAndAlertsResources.Success, "success"); ;
                    return true;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SomethingWentWrong, "error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
                return false;
            }
        }

        // Fixed: This method now properly moves the wizard forward
        private async Task MoveToInProgress()
        {
            try
            {
                // Check if current state allows moving to InProgress
                if (Model.CurrentState == OrderStatus.Rejected ||
                    (int)Model.CurrentState < 2)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, ECommerceResources.OrderMustBeAccepted, "warning");
                    return;
                }

                // Change the order status
                if ((int)Model.CurrentState == 2)
                    orderChanged = await ChangeOrderStatus(OrderStatus.InProgress);

                // Move the wizard to the next step
                if (isWizardInitialized && orderChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }
                orderChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
            }
        }

        // Fixed: Added method to move from InProgress to Shipping
        private async Task MoveToShipping()
        {
            try
            {
                if (Model.CurrentState < OrderStatus.InProgress)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, ECommerceResources.OrderMustBeInProgress, "warning");
                    return;
                }

                if ((int)Model.CurrentState == 4)
                    orderChanged = await ChangeOrderStatus(OrderStatus.Shipping);

                if (isWizardInitialized && orderChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }

                orderChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
            }
        }

        private async Task MoveToDelivered()
        {
            try
            {
                // Validate shipping information
                //if (string.IsNullOrEmpty(Model.ShippingCompany))
                //{
                //    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, "Please select a shipping company", "warning");
                //    return;
                //}

                if (Model.OrderDeliveryDate == default || Model.OrderDeliveryDate < DateTime.Now)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, ECommerceResources.SelectValidDeliveryDate, "warning");
                    return;
                }

                if (Model.ShippingCompanyId == null || Model.ShippingCompanyId == Guid.Empty)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, ECommerceResources.SelectShippingCompany, "warning");
                    return;
                }

                // Save the model first (includes shipping info)
                if ((int)Model.CurrentState == 5)
                {
                    var saveResult = await Save();
                    if (!saveResult)
                    {
                        return;
                    }
                    // Change order status to Delivered
                    orderChanged = await ChangeOrderStatus(OrderStatus.Delivered);
                }

                // Move wizard to final step
                if (isWizardInitialized && orderChanged)
                {
                    await JSRuntime.InvokeVoidAsync("moveToNextStep");
                }
                orderChanged = false;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
            }
        }

        private async Task CheckForExistingRefund()
        {
            var result = await RefundService.GetByOrderIdAsync(Id);
            if (result.Success)
            {
                if (result.Data != default)
                {
                    isRefunded = true;
                    RefundModel = result.Data;
                }
            }
        }
        private async Task CompleteOrder()
        {
            try
            {
                // navigate back to orders list after completion
                Navigation.NavigateTo("/orders");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
            }
        }

        private async Task<bool> LoadShippingCompanies()
        {
            try
            {
                var result = await ShippingCompanyService.GetAllAsync();
                if (result.Success)
                {
                    ShippingCompanies = result.Data.ToList();
                    return true;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData, "error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
                return false;
            }
        }
    }
}