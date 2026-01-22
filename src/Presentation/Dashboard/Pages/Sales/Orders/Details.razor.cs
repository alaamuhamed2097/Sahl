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
    /// FINAL Order Details - Integrated Workflow
    /// ✅ Order status driven by shipment progress
    /// ✅ Action buttons instead of dropdown
    /// ✅ Proper business rules
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
        // SHIPMENT WORKFLOW - Action Buttons
        // ============================================

        /// <summary>
        /// Get next possible action for shipment
        /// </summary>
        protected string GetShipmentNextAction(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => "Start Processing",
                ShipmentStatus.PreparingForShipment => "Ready for Pickup",
                ShipmentStatus.PickedUpByCarrier => "Mark In Transit",
                ShipmentStatus.InTransitToCustomer => "Attempt Delivery",
                ShipmentStatus.DeliveryAttemptFailed => "Retry Delivery",
                ShipmentStatus.DeliveredToCustomer => "Completed",
                _ => ""
            };
        }

        /// <summary>
        /// Get next status for shipment
        /// </summary>
        protected ShipmentStatus? GetShipmentNextStatus(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => ShipmentStatus.PreparingForShipment,
                ShipmentStatus.PreparingForShipment => ShipmentStatus.PickedUpByCarrier,
                ShipmentStatus.PickedUpByCarrier => ShipmentStatus.InTransitToCustomer,
                ShipmentStatus.InTransitToCustomer => ShipmentStatus.DeliveredToCustomer,
                ShipmentStatus.DeliveryAttemptFailed => ShipmentStatus.InTransitToCustomer,
                _ => null
            };
        }

        /// <summary>
        /// Can shipment move to next status?
        /// </summary>
        protected bool CanShipmentMoveNext(ShipmentInfoDto shipment)
        {
            if (shipment.Status == ShipmentStatus.DeliveredToCustomer ||
                shipment.Status == ShipmentStatus.ReturnedToSender ||
                shipment.Status == ShipmentStatus.CancelledByCustomer ||
                shipment.Status == ShipmentStatus.CancelledByMarketplace)
            {
                return false;
            }

            return GetShipmentNextStatus(shipment).HasValue;
        }

        /// <summary>
        /// Move shipment to next status
        /// </summary>
        protected async Task MoveShipmentToNextStatusAsync(Guid shipmentId)
        {
            var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            if (shipment == null) return;

            var nextStatus = GetShipmentNextStatus(shipment);
            if (!nextStatus.HasValue) return;

            try
            {
                ShipmentUpdatingId = shipmentId;
                StateHasChanged();

                var request = new UpdateShipmentStatusRequest
                {
                    OrderId = Order.OrderId,
                    ShipmentId = shipmentId,
                    NewStatus = nextStatus.Value.ToString(),
                    Notes = $"Updated by admin to {nextStatus.Value}"
                };

                var result = await ShipmentService.UpdateShipmentStatusAsync(Order.OrderId, request);

                if (result.Success && result.Data != null)
                {
                    shipment.Status = result.Data.ShipmentStatus;

                    // ✅ Auto-update order status based on shipments
                    await SyncOrderStatusWithShipmentsAsync();

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        "Shipment status updated successfully",
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
            finally
            {
                ShipmentUpdatingId = null;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Mark shipment as delivery failed
        /// </summary>
        protected async Task MarkShipmentDeliveryFailedAsync(Guid shipmentId)
        {
            var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            if (shipment == null) return;

            if (shipment.Status != ShipmentStatus.InTransitToCustomer)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    "Shipment must be in transit to mark as delivery failed",
                    "warning");
                return;
            }

            var reason = await JSRuntime.InvokeAsync<string>("prompt", "Enter reason for delivery failure:");
            if (string.IsNullOrWhiteSpace(reason)) return;

            await UpdateShipmentStatusDirectlyAsync(shipmentId, ShipmentStatus.DeliveryAttemptFailed, reason);
        }

        /// <summary>
        /// Cancel shipment
        /// </summary>
        protected async Task CancelShipmentAsync(Guid shipmentId)
        {
            var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            if (shipment == null) return;

            if (shipment.Status == ShipmentStatus.DeliveredToCustomer)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Failed,
                    "Cannot cancel delivered shipment",
                    "error");
                return;
            }

            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to cancel this shipment?");
            if (!confirmed) return;

            await UpdateShipmentStatusDirectlyAsync(shipmentId, ShipmentStatus.CancelledByMarketplace, "Cancelled by admin");
        }

        /// <summary>
        /// Update shipment status directly
        /// </summary>
        private async Task UpdateShipmentStatusDirectlyAsync(Guid shipmentId, ShipmentStatus newStatus, string notes)
        {
            try
            {
                ShipmentUpdatingId = shipmentId;
                StateHasChanged();

                var request = new UpdateShipmentStatusRequest
                {
                    OrderId = Order.OrderId,
                    ShipmentId = shipmentId,
                    NewStatus = newStatus.ToString(),
                    Notes = notes
                };

                var result = await ShipmentService.UpdateShipmentStatusAsync(Order.OrderId, request);

                if (result.Success && result.Data != null)
                {
                    var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
                    if (shipment != null)
                    {
                        shipment.Status = result.Data.ShipmentStatus;
                    }

                    await SyncOrderStatusWithShipmentsAsync();

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        "Shipment updated successfully",
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

        // ============================================
        // ORDER STATUS - Auto-sync with Shipments
        // ============================================

        /// <summary>
        /// ✅ Sync order status based on shipment statuses
        /// This is the KEY integration point!
        /// </summary>
        private async Task SyncOrderStatusWithShipmentsAsync()
        {
            if (!Order.Shipments.Any()) return;

            var allDelivered = Order.Shipments.All(s => s.Status == ShipmentStatus.DeliveredToCustomer);
            var anyInTransit = Order.Shipments.Any(s =>
                s.Status == ShipmentStatus.InTransitToCustomer ||
                s.Status == ShipmentStatus.PickedUpByCarrier);
            var anyProcessing = Order.Shipments.Any(s =>
                s.Status == ShipmentStatus.PreparingForShipment ||
                s.Status == ShipmentStatus.PendingProcessing);

            OrderProgressStatus targetStatus;

            if (allDelivered)
            {
                targetStatus = OrderProgressStatus.Completed;
            }
            else if (anyInTransit)
            {
                targetStatus = OrderProgressStatus.Shipped;
            }
            else if (anyProcessing)
            {
                targetStatus = OrderProgressStatus.Processing;
            }
            else
            {
                return; // No change needed
            }

            // Only update if different
            if (Order.OrderStatus != targetStatus)
            {
                await ChangeOrderStatusAsync(targetStatus);
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
                    Notes = $"Auto-updated based on shipment progress"
                };

                var result = await OrderService.ChangeOrderStatusAsync(request);

                if (result.Success)
                {
                    Order.OrderStatus = newStatus;
                    StateHasChanged();
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
                IsSaving = false;
                StateHasChanged();
            }
        }

        protected async Task CancelOrderAsync()
        {
            try
            {
                var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                    "Are you sure you want to cancel this order? All shipments will be cancelled.");

                if (!confirmed) return;

                var result = await OrderService.CancelOrderAsync(Order.OrderId, "Cancelled by admin");

                if (result.Success)
                {
                    Order.OrderStatus = OrderProgressStatus.Cancelled;
                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        "Order cancelled successfully",
                        "success");

                    // Reload to get updated shipment statuses
                    await LoadOrderAsync(Order.OrderId);
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

        protected string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "--";
        }

        // ============================================
        // UI HELPERS
        // ============================================

        protected string GetShipmentActionButtonClass(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => "btn-info",
                ShipmentStatus.PreparingForShipment => "btn-primary",
                ShipmentStatus.PickedUpByCarrier => "btn-warning",
                ShipmentStatus.InTransitToCustomer => "btn-success",
                ShipmentStatus.DeliveryAttemptFailed => "btn-danger",
                _ => "btn-secondary"
            };
        }

        protected string GetShipmentActionIcon(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => "fas fa-play",
                ShipmentStatus.PreparingForShipment => "fas fa-box",
                ShipmentStatus.PickedUpByCarrier => "fas fa-truck",
                ShipmentStatus.InTransitToCustomer => "fas fa-shipping-fast",
                ShipmentStatus.DeliveryAttemptFailed => "fas fa-redo",
                _ => "fas fa-check"
            };
        }

        protected bool CanCancelShipment(ShipmentInfoDto shipment)
        {
            return shipment.Status != ShipmentStatus.DeliveredToCustomer &&
                   shipment.Status != ShipmentStatus.CancelledByCustomer &&
                   shipment.Status != ShipmentStatus.CancelledByMarketplace;
        }

        protected bool CanMarkDeliveryFailed(ShipmentInfoDto shipment)
        {
            return shipment.Status == ShipmentStatus.InTransitToCustomer;
        }

        // ============================================
        // ORDER STATUS HELPERS (for old UI compatibility)
        // ============================================

        /// <summary>
        /// Check if order can move to next status
        /// NOTE: This is for backward compatibility with old UI
        /// New approach: Order status auto-syncs with shipments
        /// </summary>
        protected bool CanMoveToNextStatus()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Pending => true,
                OrderProgressStatus.Confirmed => true,
                OrderProgressStatus.Processing => true,
                OrderProgressStatus.Shipped => true,
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
                OrderProgressStatus.Pending => "Confirm Order",
                OrderProgressStatus.Confirmed => "Start Processing",
                OrderProgressStatus.Processing => "Mark as Shipped",
                OrderProgressStatus.Shipped => "Mark as Delivered",
                _ => ""
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
                OrderProgressStatus.Shipped => "fas fa-box-open",
                _ => "fas fa-check"
            };
        }

        /// <summary>
        /// Move order to next status
        /// NOTE: This is for backward compatibility
        /// Better approach: Let shipments drive order status
        /// </summary>
        protected async Task MoveToNextStatusAsync()
        {
            OrderProgressStatus? nextStatus = Order.OrderStatus switch
            {
                OrderProgressStatus.Pending => OrderProgressStatus.Confirmed,
                OrderProgressStatus.Confirmed => OrderProgressStatus.Processing,
                OrderProgressStatus.Processing => OrderProgressStatus.Shipped,
                OrderProgressStatus.Shipped => OrderProgressStatus.Delivered,
                _ => null
            };

            if (!nextStatus.HasValue) return;

            await ChangeOrderStatusAsync(nextStatus.Value);
        }

        protected bool IsFullyPaid()
        {
            return Order.TotalPaidAmount >= Order.TotalAmount;
        }

        protected void NavigateToList()
        {
            Navigation.NavigateTo("/sales/orders");
        }

        // Status badge helpers (existing methods)
        protected string GetOrderStatusBadgeClass(OrderProgressStatus status) => status switch
        {
            OrderProgressStatus.Pending => "warning",
            OrderProgressStatus.Confirmed => "info",
            OrderProgressStatus.Processing => "primary",
            OrderProgressStatus.Shipped => "success",
            OrderProgressStatus.Delivered => "success",
            OrderProgressStatus.Completed => "success",
            OrderProgressStatus.Cancelled => "danger",
            OrderProgressStatus.PaymentFailed => "danger",
            _ => "secondary"
        };

        protected string GetOrderStatusText(OrderProgressStatus status) => status.ToString();

        protected string GetPaymentStatusClass(PaymentStatus status) => status switch
        {
            PaymentStatus.Pending => "warning",
            PaymentStatus.Processing => "info",
            PaymentStatus.Completed => "success",
            PaymentStatus.Failed => "danger",
            PaymentStatus.Cancelled => "secondary",
            PaymentStatus.Refunded => "danger",
            PaymentStatus.PartiallyPaid => "warning",
            _ => "secondary"
        };

        protected string GetPaymentStatusText(PaymentStatus status) => status.ToString();

        protected string GetShipmentStatusBadgeClass(ShipmentStatus status)
        {
            var badgeClass = status switch
            {
                ShipmentStatus.PendingProcessing => "warning",
                ShipmentStatus.PreparingForShipment => "info",
                ShipmentStatus.PickedUpByCarrier => "primary",
                ShipmentStatus.InTransitToCustomer => "primary",
                ShipmentStatus.DeliveryAttemptFailed => "danger",
                ShipmentStatus.DeliveredToCustomer => "success",
                ShipmentStatus.ReturnedToSender => "warning",
                ShipmentStatus.CancelledByCustomer => "secondary",
                ShipmentStatus.CancelledByMarketplace => "secondary",
                _ => "secondary"
            };

            return $"badge bg-{badgeClass}";
        }

        protected string GetShipmentStatusText(ShipmentStatus status) => status switch
        {
            ShipmentStatus.PendingProcessing => "Pending",
            ShipmentStatus.PreparingForShipment => "Preparing",
            ShipmentStatus.PickedUpByCarrier => "Picked Up",
            ShipmentStatus.InTransitToCustomer => "In Transit",
            ShipmentStatus.DeliveryAttemptFailed => "Delivery Failed",
            ShipmentStatus.DeliveredToCustomer => "Delivered",
            ShipmentStatus.ReturnedToSender => "Returned",
            ShipmentStatus.CancelledByCustomer => "Cancelled (Customer)",
            ShipmentStatus.CancelledByMarketplace => "Cancelled (Admin)",
            _ => status.ToString()
        };
    }
}