using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using Dashboard.Configuration;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;

namespace Dashboard.Pages.Sales.Orders
{
    /// <summary>
    /// Order Details Page - CLEAN VERSION
    /// Uses API DTOs directly - NO intermediate mapping
    /// Clean business logic
    /// </summary>
    public partial class Details : ComponentBase
    {
        // ============================================
        // PROPERTIES - Using API DTOs directly
        // ============================================

        protected string BaseUrl { get; set; } = string.Empty;
        protected bool IsSaving { get; set; }
        protected bool IsLoading { get; set; }

        /// <summary>
        /// Order details - uses API DTO directly
        /// </summary>
        protected AdminOrderDetailsDto Order { get; set; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOrderService OrderService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        // ============================================
        // LIFECYCLE
        // ============================================

        protected override void OnParametersSet()
        {
            BaseUrl = ApiOptions.Value.BaseUrl;
            if (Id != Guid.Empty)
            {
                _ = LoadOrderAsync(Id);
            }
        }

        // ============================================
        // LOAD ORDER DATA
        // ============================================

        /// <summary>
        /// Load order details from API
        /// Returns AdminOrderDetailsDto directly - NO MAPPING
        /// </summary>
        protected async Task LoadOrderAsync(Guid orderId)
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                var result = await OrderService.GetOrderByIdAsync(orderId);

                if (result.Success && result.Data != null)
                {
                    // ✅ Use API DTO directly - NO MAPPING
                    Order = result.Data;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        result.Message ?? NotifiAndAlertsResources.FailedToRetrieveData,
                        "error");
                }
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

        // ============================================
        // ORDER ACTIONS
        // ============================================

        /// <summary>
        /// Update order delivery date
        /// Uses API DTO properties directly
        /// </summary>
        protected async Task SaveOrderAsync()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();

                var request = new UpdateOrderRequest
                {
                    OrderId = Order.OrderId,              // ✅ API DTO property
                    OrderDeliveryDate = Order.DeliveryDate // ✅ API DTO property
                };

                var result = await OrderService.UpdateOrderAsync(request);

                if (result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        NotifiAndAlertsResources.SavedSuccessfully,
                        "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        result.Message ?? NotifiAndAlertsResources.FailedAlert,
                        "error");
                }
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
                IsSaving = false;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Change order status
        /// </summary>
        protected async Task ChangeOrderStatusAsync(OrderProgressStatus newStatus)
        {
            try
            {
                var request = new ChangeOrderStatusRequest
                {
                    OrderId = Order.OrderId,  // ✅ API DTO property
                    NewStatus = newStatus,
                    Notes = $"Status changed to {newStatus} by admin"
                };

                var result = await OrderService.ChangeOrderStatusAsync(request);

                if (result.Success)
                {
                    // ✅ Update API DTO property directly
                    Order.OrderStatus = newStatus;

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        OrderResources.StatusChangedSuccessfully,
                        "success");

                    StateHasChanged();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        result.Message ?? NotifiAndAlertsResources.SomethingWentWrong,
                        "error");
                }
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
        /// Confirm order (Pending → Confirmed)
        /// </summary>
        protected async Task ConfirmOrderAsync()
        {
            if (Order.OrderStatus != OrderProgressStatus.Pending)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.Failed,
                    OrderResources.OrderMustBePending,
                    "warning");
                return;
            }

            await ChangeOrderStatusAsync(OrderProgressStatus.Confirmed);
        }

        /// <summary>
        /// Move to Processing (Confirmed → Processing)
        /// </summary>
        protected async Task MoveToProcessingAsync()
        {
            if (Order.OrderStatus != OrderProgressStatus.Confirmed)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    OrderResources.OrderMustBeConfirmed,
                    "warning");
                return;
            }

            await ChangeOrderStatusAsync(OrderProgressStatus.Processing);
        }

        /// <summary>
        /// Move to Shipped (Processing → Shipped)
        /// </summary>
        protected async Task MoveToShippedAsync()
        {
            if (Order.OrderStatus != OrderProgressStatus.Processing)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    OrderResources.OrderMustBeProcessing,
                    "warning");
                return;
            }

            await ChangeOrderStatusAsync(OrderProgressStatus.Shipped);
        }

        /// <summary>
        /// Move to Delivered (Shipped → Delivered)
        /// Requires delivery date to be set
        /// </summary>
        protected async Task MoveToDeliveredAsync()
        {
            // Validate delivery date
            if (!Order.DeliveryDate.HasValue || Order.DeliveryDate < DateTime.Now)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    OrderResources.SelectValidDeliveryDate,
                    "warning");
                return;
            }

            if (Order.OrderStatus != OrderProgressStatus.Shipped)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    OrderResources.OrderMustBeShipped,
                    "warning");
                return;
            }

            // Save delivery date first
            await SaveOrderAsync();

            // Then change status
            await ChangeOrderStatusAsync(OrderProgressStatus.Delivered);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        protected async Task CancelOrderAsync()
        {
            try
            {
                var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                    OrderResources.ConfirmCancelOrder);

                if (!confirmed) return;

                var result = await OrderService.CancelOrderAsync(Order.OrderId, "Cancelled by admin");

                if (result.Success)
                {
                    Order.OrderStatus = OrderProgressStatus.Cancelled;
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        NotifiAndAlertsResources.OrderCancelledSuccessfully,
                        "success");
                    StateHasChanged();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        result.Message ?? NotifiAndAlertsResources.FailedAlert,
                        "error");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
        }

        // ============================================
        // UI HELPERS - Work with API DTOs directly
        // ============================================

        /// <summary>
        /// Get CSS class for order status badge
        /// </summary>
        protected string GetOrderStatusBadgeClass(OrderProgressStatus status)
        {
            return status switch
            {
                OrderProgressStatus.Pending => "warning",
                OrderProgressStatus.Confirmed => "info",
                OrderProgressStatus.Processing => "primary",
                OrderProgressStatus.Shipped => "primary",
                OrderProgressStatus.Delivered => "success",
                OrderProgressStatus.Completed => "success",
                OrderProgressStatus.Cancelled => "danger",
                OrderProgressStatus.PaymentFailed => "danger",
                OrderProgressStatus.RefundRequested => "warning",
                OrderProgressStatus.Refunded => "info",
                OrderProgressStatus.Returned => "secondary",
                _ => "secondary"
            };
        }

        /// <summary>
        /// Get localized order status text
        /// </summary>
        protected string GetOrderStatusText(OrderProgressStatus status)
        {
            return status switch
            {
                OrderProgressStatus.Pending => OrderResources.Pending,
                OrderProgressStatus.Confirmed => OrderResources.Accepted,
                OrderProgressStatus.Processing => OrderResources.InProgress,
                OrderProgressStatus.Shipped => OrderResources.Shipping,
                OrderProgressStatus.Delivered => OrderResources.Delivered,
                OrderProgressStatus.Completed => OrderResources.Completed,
                OrderProgressStatus.Cancelled => OrderResources.Canceled,
                OrderProgressStatus.PaymentFailed => OrderResources.PaymentFailed,
                OrderProgressStatus.RefundRequested => OrderResources.RefundRequested,
                OrderProgressStatus.Refunded => OrderResources.Refunded,
                OrderProgressStatus.Returned => OrderResources.Returned,
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Get CSS class for payment status
        /// </summary>
        protected string GetPaymentStatusClass(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Completed => "success",
                PaymentStatus.Pending => "warning",
                PaymentStatus.Processing => "info",
                PaymentStatus.Failed => "danger",
                PaymentStatus.Cancelled => "secondary",
                PaymentStatus.Refunded => "info",
                PaymentStatus.PartiallyRefunded => "warning",
                PaymentStatus.PartiallyPaid => "warning",
                _ => "secondary"
            };
        }

        /// <summary>
        /// Get localized payment status text
        /// </summary>
        protected string GetPaymentStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Completed => OrderResources.Paid,
                PaymentStatus.Pending => OrderResources.Pending,
                PaymentStatus.Processing => OrderResources.Processing,
                PaymentStatus.Failed => OrderResources.Failed,
                PaymentStatus.Cancelled => OrderResources.Cancelled,
                PaymentStatus.Refunded => OrderResources.Refunded,
                PaymentStatus.PartiallyRefunded => OrderResources.PartiallyRefunded,
                PaymentStatus.PartiallyPaid => OrderResources.PartiallyPaid,
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Get CSS class for shipment status badge
        /// </summary>
        protected string GetShipmentStatusBadgeClass(ShipmentStatus status)
        {
            return status switch
            {
                ShipmentStatus.Pending => "badge bg-warning",
                ShipmentStatus.Processing => "badge bg-info",
                ShipmentStatus.Shipped => "badge bg-primary",
                ShipmentStatus.InTransit => "badge bg-primary",
                ShipmentStatus.OutForDelivery => "badge bg-primary",
                ShipmentStatus.Delivered => "badge bg-success",
                ShipmentStatus.Returned => "badge bg-danger",
                ShipmentStatus.Cancelled => "badge bg-dark",
                _ => "badge bg-secondary"
            };
        }

        /// <summary>
        /// Get localized shipment status text
        /// </summary>
        protected string GetShipmentStatusText(ShipmentStatus status)
        {
            return status switch
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

        /// <summary>
        /// Check if order can move to next status
        /// Uses API DTO properties
        /// </summary>
        protected bool CanMoveToNextStatus()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Pending => true,
                OrderProgressStatus.Confirmed => true,
                OrderProgressStatus.Processing => true,
                OrderProgressStatus.Shipped => Order.DeliveryDate.HasValue,
                OrderProgressStatus.Delivered => false,
                OrderProgressStatus.Completed => false,
                OrderProgressStatus.Cancelled => false,
                _ => false
            };
        }

        /// <summary>
        /// Get next status button text
        /// </summary>
        protected string GetNextStatusButtonText()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Pending => OrderResources.ConfirmOrder,
                OrderProgressStatus.Confirmed => OrderResources.MoveToProcessing,
                OrderProgressStatus.Processing => OrderResources.MoveToShipping,
                OrderProgressStatus.Shipped => OrderResources.MarkAsDelivered,
                _ => OrderResources.NextStatus
            };
        }

        /// <summary>
        /// Get next status button icon
        /// </summary>
        protected string GetNextStatusButtonIcon()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Pending => "fas fa-check",
                OrderProgressStatus.Confirmed => "fas fa-cog",
                OrderProgressStatus.Processing => "fas fa-truck",
                OrderProgressStatus.Shipped => "fas fa-check-circle",
                _ => "fas fa-arrow-right"
            };
        }

        /// <summary>
        /// Execute next status action
        /// </summary>
        protected async Task MoveToNextStatusAsync()
        {
            switch (Order.OrderStatus)
            {
                case OrderProgressStatus.Pending:
                    await ConfirmOrderAsync();
                    break;
                case OrderProgressStatus.Confirmed:
                    await MoveToProcessingAsync();
                    break;
                case OrderProgressStatus.Processing:
                    await MoveToShippedAsync();
                    break;
                case OrderProgressStatus.Shipped:
                    await MoveToDeliveredAsync();
                    break;
            }
        }

        /// <summary>
        /// Navigate back to orders list
        /// </summary>
        protected void NavigateToList()
        {
            Navigation.NavigateTo("/sales/orders");
        }
    }
}