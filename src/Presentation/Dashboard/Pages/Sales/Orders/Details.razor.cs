using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using Dashboard.Configuration;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.Payment.Refund;

namespace Dashboard.Pages.Sales.Orders
{
    public partial class Details
    {
        protected string baseUrl = string.Empty;
        private bool isSaving { get; set; }
        private bool isRefunded { get; set; }
        private bool isWizardInitialized { get; set; }
        private bool orderChanged { get; set; }
        private bool isLoadingShipments { get; set; }
        protected OrderDto Model { get; set; } = new();
        protected RefundDto RefundModel { get; set; } = new();
        protected RefundResponseDto RefundResponseModel { get; set; } = new();
        protected List<ShippingCompanyDto> ShippingCompanies { get; set; } = new();
        protected List<ShipmentDto> Shipments { get; set; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IOrderService OrderService { get; set; } = null!;
        [Inject] protected IRefundService RefundService { get; set; } = null!;
        [Inject] protected IShippingCompanyService ShippingCompanyService { get; set; } = null!;
        [Inject] protected IShipmentService ShipmentService { get; set; } = null!;
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
                isWizardInitialized = false;
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
                StateHasChanged();

                var result = await OrderService.SaveAsync(Model);

                isSaving = false;
                if (!result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.FailedAlert, "error");
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.FailedAlert, "error");
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

                Model = result.Data ?? new();
                await LoadShipments(id);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, ex.Message, "error");
            }
        }

        private async Task<bool> ChangeOrderStatus(OrderProgressStatus newStatus)
        {
            try
            {
                Model.CurrentState = newStatus;
                var result = await OrderService.ChangeOrderStatusAsync(Model);

                if (result.Success)
                {
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
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.SaveSuccess, NotifiAndAlertsResources.Success, "success");
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

        // Move from Confirmed (1) to Processing (2)
        private async Task MoveToProcessing()
        {
            try
            {
                if (Model.CurrentState != OrderProgressStatus.Confirmed)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, OrderResources.OrderMustBeConfirmed, "warning");
                    return;
                }

                orderChanged = await ChangeOrderStatus(OrderProgressStatus.Processing);

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

        // Move from Processing (2) to Shipped (3)
        private async Task MoveToShipped()
        {
            try
            {
                if (Model.CurrentState != OrderProgressStatus.Processing)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, OrderResources.OrderMustBeProcessing, "warning");
                    return;
                }

                orderChanged = await ChangeOrderStatus(OrderProgressStatus.Shipped);

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

        // Move from Shipped (3) to Delivered (4)
        private async Task MoveToDelivered()
        {
            try
            {
                if (Model.OrderDeliveryDate == default || Model.OrderDeliveryDate < DateTime.Now)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, OrderResources.SelectValidDeliveryDate, "warning");
                    return;
                }

                if (Model.CurrentState != OrderProgressStatus.Shipped)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, OrderResources.OrderMustBeShipped, "warning");
                    return;
                }

                var saveResult = await Save();
                if (!saveResult)
                {
                    return;
                }

                orderChanged = await ChangeOrderStatus(OrderProgressStatus.Delivered);

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

        private async Task LoadShipments(Guid orderId)
        {
            try
            {
                isLoadingShipments = true;
                StateHasChanged();

                var result = await ShipmentService.GetOrderShipmentsAsync(orderId);
                if (result.Success && result.Data != null)
                {
                    Shipments = result.Data;
                }
                else
                {
                    Shipments = new List<ShipmentDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shipments: {ex.Message}");
                Shipments = new List<ShipmentDto>();
            }
            finally
            {
                isLoadingShipments = false;
                StateHasChanged();
            }
        }

        // FIXED: Align with PaymentStatus enum (1-8)
        private string GetPaymentStatusClass(PaymentStatus status) => status switch
        {
            PaymentStatus.Pending => "warning",                   // 1
            PaymentStatus.Processing => "info",                   // 2
            PaymentStatus.Completed => "success",                 // 3
            PaymentStatus.Failed => "danger",                     // 4
            PaymentStatus.Cancelled => "secondary",               // 5
            PaymentStatus.Refunded => "info",                     // 6
            PaymentStatus.PartiallyRefunded => "warning",         // 7
            PaymentStatus.PartiallyPaid => "warning",             // 8
            _ => "secondary"
        };

        // FIXED: Align with PaymentStatus enum using Resources
        private string GetLocalizedPaymentStatus(PaymentStatus status) => status switch
        {
            PaymentStatus.Pending => OrderResources.Pending,
            PaymentStatus.Processing => OrderResources.Processing,
            PaymentStatus.Completed => OrderResources.Paid,
            PaymentStatus.Failed => OrderResources.Failed,
            PaymentStatus.Cancelled => OrderResources.Cancelled,
            PaymentStatus.Refunded => OrderResources.Refunded,
            PaymentStatus.PartiallyRefunded => OrderResources.PartiallyRefunded,
            PaymentStatus.PartiallyPaid => OrderResources.PartiallyPaid,
            _ => status.ToString()
        };

        // FIXED: Align with OrderProgressStatus enum (0-10)
        private string GetOrderStatusClass(OrderProgressStatus status) => status switch
        {
            OrderProgressStatus.Pending => "secondary",           // 0
            OrderProgressStatus.Confirmed => "info",              // 1
            OrderProgressStatus.Processing => "primary",          // 2
            OrderProgressStatus.Shipped => "warning",             // 3
            OrderProgressStatus.Delivered => "success",           // 4
            OrderProgressStatus.Completed => "success",           // 5
            OrderProgressStatus.Cancelled => "danger",            // 6
            OrderProgressStatus.PaymentFailed => "danger",        // 7
            OrderProgressStatus.RefundRequested => "warning",     // 8
            OrderProgressStatus.Refunded => "info",               // 9
            OrderProgressStatus.Returned => "secondary",          // 10
            _ => "secondary"
        };

        // FIXED: Align with OrderProgressStatus enum using Resources
        private string GetLocalizedOrderStatus(OrderProgressStatus status) => status switch
        {
            OrderProgressStatus.Pending => OrderResources.Pending,
            OrderProgressStatus.Confirmed => OrderResources.Confirmed,
            OrderProgressStatus.Processing => OrderResources.Processing,
            OrderProgressStatus.Shipped => OrderResources.Shipping,
            OrderProgressStatus.Delivered => OrderResources.Delivered,
            OrderProgressStatus.Completed => OrderResources.Completed,
            OrderProgressStatus.Cancelled => OrderResources.Cancelled,
            OrderProgressStatus.PaymentFailed => OrderResources.PaymentFailed,
            OrderProgressStatus.RefundRequested => OrderResources.RefundRequested,
            OrderProgressStatus.Refunded => OrderResources.Refunded,
            OrderProgressStatus.Returned => OrderResources.Returned,
            _ => status.ToString()
        };

        private string GetShipmentStatusClass(ShipmentStatus status) => status switch
        {
            ShipmentStatus.Pending => "secondary",
            ShipmentStatus.Processing => "primary",
            ShipmentStatus.Shipped => "warning",
            ShipmentStatus.InTransit => "info",
            ShipmentStatus.OutForDelivery => "info",
            ShipmentStatus.Delivered => "success",
            ShipmentStatus.Returned => "danger",
            ShipmentStatus.Cancelled => "danger",
            _ => "secondary"
        };

        private string GetLocalizedStatus(ShipmentStatus status) => status switch
        {
            ShipmentStatus.Pending => OrderResources.Pending,
            ShipmentStatus.Processing => OrderResources.Processing,
            ShipmentStatus.Shipped => OrderResources.Shipped,
            ShipmentStatus.InTransit => OrderResources.InTransit,
            ShipmentStatus.OutForDelivery => OrderResources.OutForDelivery,
            ShipmentStatus.Delivered => OrderResources.Delivered,
            ShipmentStatus.Returned => OrderResources.Returned,
            ShipmentStatus.Cancelled => OrderResources.Cancelled,
            _ => status.ToString()
        };
    }
}