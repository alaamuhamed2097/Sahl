using System;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using Dashboard.Configuration;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;

namespace Dashboard.Pages.Sales.Orders
{
    /// <summary>
    /// Order Details Page - FINAL VERSION
    /// - New ShipmentStatus enum values
    /// - Payment summary display (Wallet/Card/COD only)
    /// - No InvoiceId, no AllowSplitPayment, no OtherPaidAmount
    /// </summary>
    public partial class Details : ComponentBase
    {
        // ============================================
        // PROPERTIES
        // ============================================

        protected string BaseUrl { get; set; } = string.Empty;
        protected bool IsSaving { get; set; }
        protected bool IsLoading { get; set; }
        protected Guid? ShipmentUpdatingId { get; set; }

        protected AdminOrderDetailsDto Order { get; set; } = new();
        protected Dictionary<Guid, ShipmentStatus> ShipmentStatusSelections { get; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOrderService OrderService { get; set; } = null!;
        [Inject] protected IShipmentService ShipmentService { get; set; } = null!;
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

        protected async Task LoadOrderAsync(Guid orderId)
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                var result = await OrderService.GetOrderByIdAsync(orderId);

                if (result.Success && result.Data != null)
                {
                    Order = result.Data;
                    InitializeShipmentSelections();
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
        // SHIPMENT ACTIONS
        // ============================================

        private void InitializeShipmentSelections()
        {
            ShipmentStatusSelections.Clear();

            foreach (var shipment in Order.Shipments)
            {
                ShipmentStatusSelections[shipment.ShipmentId] = shipment.Status;
            }
        }

        protected ShipmentStatus GetSelectedShipmentStatus(Guid shipmentId)
        {
            if (ShipmentStatusSelections.TryGetValue(shipmentId, out var status))
            {
                return status;
            }

            var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            return shipment?.Status ?? ShipmentStatus.PendingProcessing;
        }

        protected IEnumerable<ShipmentStatus> GetShipmentStatusOptions(ShipmentStatus current)
        {
            _ = current; // current status included in the returned sequence
            return new List<ShipmentStatus>
            {
                ShipmentStatus.PendingProcessing,
                ShipmentStatus.PreparingForShipment,
                ShipmentStatus.PickedUpByCarrier,
                ShipmentStatus.InTransitToCustomer,
                ShipmentStatus.DeliveryAttemptFailed,
                ShipmentStatus.DeliveredToCustomer,
                ShipmentStatus.ReturnedToSender,
                ShipmentStatus.CancelledByCustomer,
                ShipmentStatus.CancelledByMarketplace
            };
        }

        protected void OnShipmentStatusChanged(Guid shipmentId, ChangeEventArgs args)
        {
            if (Enum.TryParse<ShipmentStatus>(args.Value?.ToString(), out var status))
            {
                ShipmentStatusSelections[shipmentId] = status;
            }
        }

        protected async Task UpdateShipmentStatusAsync(Guid shipmentId)
        {
            var selectedStatus = GetSelectedShipmentStatus(shipmentId);
            var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);

            if (shipment == null)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    NotifiAndAlertsResources.SomethingWentWrong,
                    "error");
                return;
            }

            if (shipment.Status == selectedStatus)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.Warning,
                    "Shipment status is already set to this value.",
                    "info");
                return;
            }

            try
            {
                ShipmentUpdatingId = shipmentId;
                StateHasChanged();

                var request = new UpdateShipmentStatusRequest
                {
                    OrderId = Order.OrderId,
                    ShipmentId = shipmentId,
                    NewStatus = selectedStatus.ToString(),
                    Notes = $"Updated by admin at {DateTime.UtcNow:g}"
                };

                var result = await ShipmentService.UpdateShipmentStatusAsync(Order.OrderId, request);

                if (result.Success && result.Data != null)
                {
                    shipment.Status = result.Data.ShipmentStatus;
                    ShipmentStatusSelections[shipmentId] = result.Data.ShipmentStatus;

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        OrderResources.StatusChangedSuccessfully,
                        "success");
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
            finally
            {
                ShipmentUpdatingId = null;
                StateHasChanged();
            }
        }

        protected string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "--";
        }

        // ============================================
        // ORDER ACTIONS
        // ============================================

        protected async Task SaveOrderAsync()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();

                var request = new UpdateOrderRequest
                {
                    OrderId = Order.OrderId,
                    OrderDeliveryDate = Order.DeliveryDate
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

        protected async Task ChangeOrderStatusAsync(OrderProgressStatus newStatus)
        {
            try
            {
                var request = new ChangeOrderStatusRequest
                {
                    OrderId = Order.OrderId,
                    NewStatus = newStatus,
                    Notes = $"Status changed to {newStatus} by admin"
                };

                var result = await OrderService.ChangeOrderStatusAsync(request);

                if (result.Success)
                {
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

        protected async Task MoveToDeliveredAsync()
        {
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

            await SaveOrderAsync();
            await ChangeOrderStatusAsync(OrderProgressStatus.Delivered);
        }

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
        // UI HELPERS
        // ============================================

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
        /// FINAL VERSION - Updated with new ShipmentStatus enum
        /// Removed AtLocalHub
        /// </summary>
        protected string GetShipmentStatusBadgeClass(ShipmentStatus status)
        {
            return status switch
            {
                ShipmentStatus.PendingProcessing => "badge bg-warning",
                ShipmentStatus.PreparingForShipment => "badge bg-info",
                ShipmentStatus.PickedUpByCarrier => "badge bg-primary",
                ShipmentStatus.InTransitToCustomer => "badge bg-primary",
                ShipmentStatus.DeliveredToCustomer => "badge bg-success",
                ShipmentStatus.ReturnedToSender => "badge bg-danger",
                ShipmentStatus.CancelledByCustomer => "badge bg-dark",
                ShipmentStatus.CancelledByMarketplace => "badge bg-dark",
                ShipmentStatus.DeliveryAttemptFailed => "badge bg-warning",
                _ => "badge bg-secondary"
            };
        }

        /// <summary>
        /// FINAL VERSION - Updated with new ShipmentStatus enum
        /// </summary>
        protected string GetShipmentStatusText(ShipmentStatus status)
        {
            return status switch
            {
                ShipmentStatus.PendingProcessing => OrderResources.PendingProcessing,
                ShipmentStatus.PreparingForShipment => OrderResources.PreparingForShipment,
                ShipmentStatus.PickedUpByCarrier => OrderResources.PickedUpByCarrier,
                ShipmentStatus.InTransitToCustomer => OrderResources.InTransit,
                ShipmentStatus.DeliveredToCustomer => OrderResources.Delivered,
                ShipmentStatus.ReturnedToSender => OrderResources.Returned,
                ShipmentStatus.CancelledByCustomer => OrderResources.CancelledByCustomer,
                ShipmentStatus.CancelledByMarketplace => OrderResources.CancelledByMarketplace,
                ShipmentStatus.DeliveryAttemptFailed => OrderResources.DeliveryAttemptFailed,
                _ => status.ToString()
            };
        }

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

        // ============================================
        // PAYMENT SUMMARY HELPERS - FINAL VERSION
        // Only 3 payment methods: Wallet, Card, Cash
        // ============================================

        /// <summary>
        /// Get formatted payment breakdown message
        /// FINAL: Only shows Wallet, Card, and Cash (no Other)
        /// </summary>
        protected string GetPaymentBreakdownMessage()
        {
            var parts = new List<string>();

            if (Order.WalletPaidAmount > 0)
            {
                parts.Add($"{OrderResources.Wallet}: {Order.WalletPaidAmount:N2} {OrderResources.Currency}");
            }

            if (Order.CardPaidAmount > 0)
            {
                parts.Add($"{OrderResources.Card}: {Order.CardPaidAmount:N2} {OrderResources.Currency}");
            }

            if (Order.CashPaidAmount > 0)
            {
                parts.Add($"{OrderResources.Cash}: {Order.CashPaidAmount:N2} {OrderResources.Currency}");
            }

            return parts.Count > 0
                ? string.Join(" + ", parts)
                : $"{OrderResources.Total}: {Order.TotalPaidAmount:N2} {OrderResources.Currency}";
        }

        /// <summary>
        /// Check if order has multiple payment methods
        /// FINAL: Only checks 3 methods
        /// </summary>
        protected bool HasMultiplePaymentMethods()
        {
            var methodCount = 0;

            if (Order.WalletPaidAmount > 0) methodCount++;
            if (Order.CardPaidAmount > 0) methodCount++;
            if (Order.CashPaidAmount > 0) methodCount++;

            return methodCount > 1;
        }

        /// <summary>
        /// Get primary payment method name
        /// </summary>
        protected string GetPrimaryPaymentMethodName()
        {
            if (Order.WalletPaidAmount >= Order.CardPaidAmount &&
                Order.WalletPaidAmount >= Order.CashPaidAmount)
            {
                return OrderResources.Wallet;
            }

            if (Order.CardPaidAmount >= Order.CashPaidAmount)
            {
                return OrderResources.Card;
            }

            return OrderResources.Cash;
        }

        /// <summary>
        /// Check if order is Cash on Delivery
        /// FINAL: Simplified logic without AllowSplitPayment
        /// </summary>
        protected bool IsCashOnDelivery()
        {
            // Order is COD if any cash amount was paid
            return Order.CashPaidAmount > 0;
        }

        /// <summary>
        /// Check if payment is fully completed
        /// </summary>
        protected bool IsFullyPaid()
        {
            return Order.TotalPaidAmount >= Order.TotalAmount;
        }

        protected void NavigateToList()
        {
            Navigation.NavigateTo("/sales/orders");
        }
    }
}