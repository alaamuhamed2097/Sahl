using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;

namespace Dashboard.Pages.Orders.Orders
{
    public partial class Details : LocalizedComponentBase
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
        [Inject] private IResourceLoaderService ResourceLoaderService { get; set; } = null!;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ResourceLoaderService.LoadStyleSheet("css/order-details.css");
            }
            return Task.CompletedTask;
        }

        // ============================================
        // UI STATE FOR TABS
        // ============================================
        protected string ActiveTab { get; set; } = "customer";

        protected void SetActiveTab(string tab)
        {
            ActiveTab = tab;
            StateHasChanged();
        }

        protected Task OnTabChangedAsync(string tab)
        {
            SetActiveTab(tab);
            return Task.CompletedTask;
        }

        protected Task ToggleDropdownAsync(Guid shipmentId)
        {
            ToggleDropdown(shipmentId);
            return Task.CompletedTask;
        }

        // ============================================
        // DROPDOWN STATE
        // ============================================
        protected Guid? OpenDropdownId { get; set; }

        protected void ToggleDropdown(Guid shipmentId)
        {
            OpenDropdownId = OpenDropdownId == shipmentId ? null : shipmentId;
            StateHasChanged();
        }

        protected void CloseDropdown()
        {
            OpenDropdownId = null;
            StateHasChanged();
        }

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

        /// <param name="orderId">Order ID to load</param>
        /// <param name="silentRefresh">If true, refresh order data without showing loading spinner (e.g. after shipment update)</param>
        protected async Task LoadOrderAsync(Guid orderId, bool silentRefresh = false)
        {
            try
            {
                if (!silentRefresh)
                {
                    IsLoading = true;
                    StateHasChanged();
                }

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
                if (!silentRefresh)
                {
                    IsLoading = false;
                }
                StateHasChanged();
            }
        }

        // ============================================
        // PAYMENT METHOD DISPLAY - FIX
        // ============================================

        /// <summary>
        /// Get payment method display based on paid amounts
        /// </summary>
        protected string GetPaymentMethodDisplay()
        {
            var methods = new List<string>();

            if (Order.WalletPaidAmount > 0)
                methods.Add(OrderResources.Wallet);

            if (Order.CardPaidAmount > 0)
                methods.Add(OrderResources.Card);

            if (Order.CashPaidAmount > 0)
                methods.Add(OrderResources.Cash);

            return methods.Any()
                ? string.Join(" + ", methods)
                : "-";
        }

        // ============================================
        // SHIPMENT WORKFLOW - Action Buttons
        // ============================================

        /// <summary>
        /// Get next possible action for shipment.
        /// Workflow starts from PickedUpByCarrier: Pending/Preparing jump to PickedUpByCarrier, then InTransit → Delivered.
        /// </summary>
        protected string GetShipmentNextAction(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => OrderResources.PickedUpByCarrier,
                ShipmentStatus.PreparingForShipment => OrderResources.PickedUpByCarrier,
                ShipmentStatus.PickedUpByCarrier => OrderResources.MarkInTransit,
                ShipmentStatus.InTransitToCustomer => OrderResources.AttemptDelivery,
                ShipmentStatus.DeliveryAttemptFailed => OrderResources.RetryDelivery,
                ShipmentStatus.DeliveredToCustomer => OrderResources.Completed,
                _ => ""
            };
        }

        /// <summary>
        /// Get next status for shipment.
        /// Pending/Preparing jump directly to PickedUpByCarrier; then PickedUp → InTransit → Delivered.
        /// </summary>
        protected ShipmentStatus? GetShipmentNextStatus(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => ShipmentStatus.PickedUpByCarrier,
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
                    Notes = $"Updated to {nextStatus.Value} by admin"
                };

                var result = await ShipmentService.UpdateShipmentStatusAsync(Order.OrderId, request);

                if (result.Success && result.Data != null)
                {
                    shipment.Status = result.Data.ShipmentStatus;

                    // Backend syncs order status when shipment changes; refresh order so UI shows it
                    await LoadOrderAsync(Order.OrderId, silentRefresh: true);

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        OrderResources.ShipmentStatusUpdatedSuccessfully,
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
                CloseDropdown();
                StateHasChanged();
            }
        }

        /// <summary>
        /// Cancel a shipment
        /// </summary>
        protected async Task CancelShipmentAsync(Guid shipmentId)
        {
            try
            {
                ShipmentUpdatingId = shipmentId;
                StateHasChanged();

                var request = new UpdateShipmentStatusRequest
                {
                    OrderId = Order.OrderId,
                    ShipmentId = shipmentId,
                    NewStatus = ShipmentStatus.CancelledByMarketplace.ToString(),
                    Notes = "Shipment cancelled by admin"
                };

                var result = await ShipmentService.UpdateShipmentStatusAsync(Order.OrderId, request);

                if (result.Success && result.Data != null)
                {
                    var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
                    if (shipment != null)
                    {
                        shipment.Status = result.Data.ShipmentStatus;
                    }

                    // Backend syncs order status when shipment changes; refresh order so UI shows it
                    await LoadOrderAsync(Order.OrderId, silentRefresh: true);

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        NotifiAndAlertsResources.OperationCompletedSuccessfully,
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
        /// Mark shipment delivery as failed
        /// </summary>
        protected async Task MarkShipmentDeliveryFailedAsync(Guid shipmentId)
        {
            try
            {
                ShipmentUpdatingId = shipmentId;
                StateHasChanged();

                var request = new UpdateShipmentStatusRequest
                {
                    OrderId = Order.OrderId,
                    ShipmentId = shipmentId,
                    NewStatus = ShipmentStatus.DeliveryAttemptFailed.ToString(),
                    Notes = "Delivery attempt failed - marked by admin"
                };

                var result = await ShipmentService.UpdateShipmentStatusAsync(Order.OrderId, request);

                if (result.Success && result.Data != null)
                {
                    var shipment = Order.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
                    if (shipment != null)
                    {
                        shipment.Status = result.Data.ShipmentStatus;
                    }

                    // Backend syncs order status when shipment changes; refresh order so UI shows it
                    await LoadOrderAsync(Order.OrderId, silentRefresh: true);

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        NotifiAndAlertsResources.OperationCompletedSuccessfully,
                        "warning");

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
                CloseDropdown();
                StateHasChanged();
            }
        }

        // ============================================
        // ORDER STATUS SYNC
        // ============================================

        /// <summary>
        /// Sync order status based on all shipment statuses
        /// </summary>
        protected async Task SyncOrderStatusWithShipmentsAsync()
        {
            if (!Order.Shipments.Any()) return;

            OrderProgressStatus newOrderStatus;

            // All delivered
            if (Order.Shipments.All(s => s.Status == ShipmentStatus.DeliveredToCustomer))
            {
                newOrderStatus = OrderProgressStatus.Delivered;
            }
            // All cancelled
            else if (Order.Shipments.All(s =>
                s.Status == ShipmentStatus.CancelledByCustomer ||
                s.Status == ShipmentStatus.CancelledByMarketplace))
            {
                newOrderStatus = OrderProgressStatus.Cancelled;
            }
            // Any in transit
            else if (Order.Shipments.Any(s =>
                s.Status == ShipmentStatus.InTransitToCustomer ||
                s.Status == ShipmentStatus.PickedUpByCarrier))
            {
                newOrderStatus = OrderProgressStatus.Shipped;
            }
            // Any being prepared
            else if (Order.Shipments.Any(s =>
                s.Status == ShipmentStatus.PreparingForShipment ||
                s.Status == ShipmentStatus.PendingProcessing))
            {
                newOrderStatus = OrderProgressStatus.Processing;
            }
            else
            {
                return; // No change needed
            }

            // Only update if status changed
            if (newOrderStatus != Order.OrderStatus)
            {
                await ChangeOrderStatusAsync(newOrderStatus);
            }
        }

        // ============================================
        // ORDER STATUS CHANGE
        // ============================================

        protected async Task ChangeOrderStatusAsync(OrderProgressStatus newStatus)
        {
            try
            {
                IsSaving = true;
                StateHasChanged();

                // Use ChangeOrderStatusAsync instead of UpdateOrderStatusAsync
                var result = await OrderService.ChangeOrderStatusAsync(
                    new Shared.DTOs.Order.OrderProcessing.ChangeOrderStatusRequest
                    {
                        OrderId = Order.OrderId,
                        NewStatus = newStatus,
                        Notes = $"Status changed to {newStatus} by admin"
                    });

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
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        protected async Task CancelOrderAsync()
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                OrderResources.ConfirmCancelOrder);

            if (!confirmed) return;

            try
            {
                IsSaving = true;
                StateHasChanged();

                // Use ChangeOrderStatusAsync
                var result = await OrderService.ChangeOrderStatusAsync(
                    new Shared.DTOs.Order.OrderProcessing.ChangeOrderStatusRequest
                    {
                        OrderId = Order.OrderId,
                        NewStatus = OrderProgressStatus.Cancelled,
                        Notes = $"Status changed to {OrderProgressStatus.Cancelled} by admin"
                    });

                if (result.Success)
                {
                    Order.OrderStatus = OrderProgressStatus.Cancelled;

                    await JSRuntime.InvokeVoidAsync("swal",
                        NotifiAndAlertsResources.Success,
                        NotifiAndAlertsResources.OperationCompletedSuccessfully,
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
                IsSaving = false;
                StateHasChanged();
            }
        }

        protected string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "--";
        }

        protected string FormatDateTime(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd MMM yyyy, hh:mm tt", System.Globalization.CultureInfo.InvariantCulture) : "--";
        }

        protected string FormatCurrency(decimal value)
        {
            return "$" + value.ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
        }

        // ============================================
        // UI HELPERS
        // ============================================

        protected string GetShipmentActionButtonClass(ShipmentInfoDto shipment)
        {
            return shipment.Status switch
            {
                ShipmentStatus.PendingProcessing => "btn-info",
                ShipmentStatus.PreparingForShipment => "btn-info",
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
                ShipmentStatus.PendingProcessing => "fas fa-truck",
                ShipmentStatus.PreparingForShipment => "fas fa-truck",
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
        /// Admin can manually change order status only for Processing→Shipped and Shipped→Delivered.
        /// Confirmed, Processing (as target), and Cancelled are set by system (payment/shipments).
        /// </summary>
        protected bool CanMoveToNextStatus()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Processing => true,  // → Shipped
                OrderProgressStatus.Shipped => true,    // → Delivered
                _ => false
            };
        }

        /// <summary>
        /// Get next status button text (only for manually allowed transitions).
        /// </summary>
        protected string GetNextStatusButtonText()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Processing => OrderResources.MarkAsShipped,
                OrderProgressStatus.Shipped => OrderResources.MarkAsDelivered,
                _ => ""
            };
        }

        /// <summary>
        /// Get next status button icon.
        /// </summary>
        protected string GetNextStatusButtonIcon()
        {
            return Order.OrderStatus switch
            {
                OrderProgressStatus.Processing => "fas fa-truck",
                OrderProgressStatus.Shipped => "fas fa-box-open",
                _ => "fas fa-check"
            };
        }

        /// <summary>
        /// Move order to next status (manual admin action).
        /// Only Processing→Shipped and Shipped→Delivered are allowed.
        /// </summary>
        protected async Task MoveToNextStatusAsync()
        {
            OrderProgressStatus? nextStatus = Order.OrderStatus switch
            {
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
            Navigation.NavigateTo("/order/orders");
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

        protected string GetOrderStatusText(OrderProgressStatus status) => status switch
        {
            OrderProgressStatus.Pending => OrderResources.Pending,
            OrderProgressStatus.Confirmed => OrderResources.Confirmed,
            OrderProgressStatus.Processing => OrderResources.Processing,
            OrderProgressStatus.Shipped => OrderResources.Shipped,
            OrderProgressStatus.Delivered => OrderResources.Delivered,
            OrderProgressStatus.Completed => OrderResources.Completed,
            OrderProgressStatus.Cancelled => OrderResources.Cancelled,
            OrderProgressStatus.PaymentFailed => OrderResources.PaymentFailed,
            OrderProgressStatus.RefundRequested => OrderResources.RefundRequested,
            OrderProgressStatus.Refunded => OrderResources.Refunded,
            OrderProgressStatus.Returned => OrderResources.Returned
        };

        protected string GetPaymentStatusClass(PaymentStatus status) => status switch
        {
            PaymentStatus.Pending => "warning",
            PaymentStatus.Processing => "info",
            PaymentStatus.Completed => "success",
            PaymentStatus.Failed => "danger",
            PaymentStatus.Cancelled => "secondary",
            PaymentStatus.Refunded or PaymentStatus.PartiallyRefunded => "danger",
            PaymentStatus.PartiallyPaid => "warning",
            _ => "secondary"
        };

        protected string GetPaymentStatusText(PaymentStatus status) => status switch
        {
            PaymentStatus.Pending => OrderResources.Pending,
            PaymentStatus.Processing => OrderResources.Processing,
            PaymentStatus.Completed => OrderResources.Completed,
            PaymentStatus.Failed => OrderResources.Failed,
            PaymentStatus.Cancelled => OrderResources.Cancelled,
            PaymentStatus.Refunded => OrderResources.Refunded,
            PaymentStatus.PartiallyRefunded => OrderResources.PartiallyRefunded,
            PaymentStatus.PartiallyPaid => OrderResources.PartiallyPaid,
        };

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
            ShipmentStatus.PendingProcessing => OrderResources.Pending,
            ShipmentStatus.PreparingForShipment => OrderResources.Preparing,
            ShipmentStatus.PickedUpByCarrier => OrderResources.PickedUp,
            ShipmentStatus.InTransitToCustomer => OrderResources.InTransit,
            ShipmentStatus.DeliveryAttemptFailed => OrderResources.DeliveryFailed,
            ShipmentStatus.DeliveredToCustomer => OrderResources.Delivered,
            ShipmentStatus.ReturnedToSender => OrderResources.Returned,
            ShipmentStatus.CancelledByCustomer => OrderResources.CancelledCustomer,
            ShipmentStatus.CancelledByMarketplace => OrderResources.CancelledAdmin,
            _ => status.ToString()
        };
    }
}