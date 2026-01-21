using Bl.Contracts.GeneralService.Notification;
using BL.Contracts.Service.Order.Fulfillment;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Notification;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Offer;
using Domains.Entities.Order.Shipping;
using Domains.Entities.Warehouse;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Services.Order.Fulfillment;

/// <summary>
/// Updated Fulfillment Service with:
/// - New ShipmentStatus enum values (PendingProcessing, PreparingForShipment, etc.)
/// - Enhanced notification system
/// - Better inventory management
/// </summary>
public class FulfillmentService : IFulfillmentService
{
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ITableRepository<TbOfferCombinationPricing> _pricingRepository;
    private readonly ITableRepository<TbWarehouse> _warehouseRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger _logger;

    public FulfillmentService(
        IShipmentRepository shipmentRepository,
        ITableRepository<TbOfferCombinationPricing> pricingRepository,
        ITableRepository<TbWarehouse> warehouseRepository,
        INotificationService notificationService,
        ILogger logger)
    {
        _shipmentRepository = shipmentRepository ?? throw new ArgumentNullException(nameof(shipmentRepository));
        _pricingRepository = pricingRepository ?? throw new ArgumentNullException(nameof(pricingRepository));
        _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Process shipment for Fulfillment by Marketplace (FBA)
    /// Platform handles picking, packing, and shipping
    /// ✅ UPDATED: Uses new ShipmentStatus.PreparingForShipment
    /// </summary>
    public async Task ProcessFulfillmentByMarketplaceShipmentAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Information("Processing FBA shipment {ShipmentId}", shipmentId);

            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                shipmentId,
                cancellationToken);

            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            // Validate shipment is in correct status
            if (shipment.ShipmentStatus != ShipmentStatus.PendingProcessing)
            {
                throw new InvalidOperationException(
                    $"Shipment {shipmentId} is not in PendingProcessing status. Current: {shipment.ShipmentStatus}");
            }

            // Reserve inventory from platform warehouse
            var reserved = await ReserveInventoryAsync(shipmentId, cancellationToken);

            if (!reserved)
            {
                throw new InvalidOperationException(
                    $"Failed to reserve inventory for shipment {shipmentId}");
            }

            // ✅ Update shipment status to PreparingForShipment (new enum value)
            shipment.ShipmentStatus = ShipmentStatus.PreparingForShipment;
            shipment.UpdatedDateUtc = DateTime.UtcNow;

            await _shipmentRepository.UpdateAsync(shipment);

            // Create warehouse picking task
            await CreateWarehousePickingTaskAsync(shipment, cancellationToken);

            // Notify customer that order is being prepared
            await NotifyCustomerOrderPreparationAsync(shipment, cancellationToken);

            _logger.Information(
                "FBA shipment {ShipmentId} moved to PreparingForShipment status",
                shipmentId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process FBA shipment {ShipmentId}", shipmentId);
            throw;
        }
    }

    /// <summary>
    /// Process shipment for Fulfillment by Seller (FBM)
    /// Vendor handles picking, packing, and shipping
    /// ✅ UPDATED: Uses new ShipmentStatus.PreparingForShipment
    /// </summary>
    public async Task ProcessFulfillmentBySellerShipmentAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Information("Processing FBM shipment {ShipmentId}", shipmentId);

            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                shipmentId,
                cancellationToken);

            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            // Validate shipment is in correct status
            if (shipment.ShipmentStatus != ShipmentStatus.PendingProcessing)
            {
                throw new InvalidOperationException(
                    $"Shipment {shipmentId} is not in PendingProcessing status. Current: {shipment.ShipmentStatus}");
            }

            // Reserve inventory from vendor's stock
            var reserved = await ReserveInventoryAsync(shipmentId, cancellationToken);

            if (!reserved)
            {
                throw new InvalidOperationException(
                    $"Failed to reserve inventory for shipment {shipmentId}");
            }

            // Notify vendor of new shipment
            await NotifyVendorOfNewShipmentAsync(shipment, cancellationToken);

            // ✅ Update shipment status to PreparingForShipment (vendor will update to PickedUpByCarrier)
            shipment.ShipmentStatus = ShipmentStatus.PreparingForShipment;
            shipment.UpdatedDateUtc = DateTime.UtcNow;

            await _shipmentRepository.UpdateAsync(shipment);

            _logger.Information(
                "FBM shipment {ShipmentId} assigned to vendor and moved to PreparingForShipment",
                shipmentId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process FBM shipment {ShipmentId}", shipmentId);
            throw;
        }
    }

    /// <summary>
    /// Reserve inventory for shipment items
    /// Moves stock from Available to Reserved
    /// </summary>
    public async Task<bool> ReserveInventoryAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                shipmentId,
                cancellationToken);

            if (shipment == null)
            {
                _logger.Error("Shipment {ShipmentId} not found for inventory reservation", shipmentId);
                return false;
            }

            if (shipment.Items == null || !shipment.Items.Any())
            {
                _logger.Error("Shipment {ShipmentId} has no items", shipmentId);
                return false;
            }

            var allReserved = true;

            foreach (var item in shipment.Items)
            {
                if (!item.ItemCombinationId.HasValue)
                {
                    _logger.Error(
                        "Shipment item {ItemId} has no ItemCombinationId",
                        item.Id);
                    allReserved = false;
                    continue;
                }

                var pricing = await _pricingRepository.FindByIdAsync(
                    item.ItemCombinationId.Value,
                    cancellationToken);

                if (pricing == null)
                {
                    _logger.Error(
                        "Pricing {PricingId} not found for shipment item {ItemId}",
                        item.ItemCombinationId.Value,
                        item.Id);
                    allReserved = false;
                    continue;
                }

                // Check if enough stock available
                if (pricing.AvailableQuantity < item.Quantity)
                {
                    _logger.Error(
                        "Insufficient stock for item {ItemId}. Available: {Available}, Required: {Required}",
                        item.ItemId,
                        pricing.AvailableQuantity,
                        item.Quantity);
                    allReserved = false;
                    continue;
                }

                // Reserve inventory
                pricing.AvailableQuantity -= item.Quantity;
                pricing.ReservedQuantity += item.Quantity;
                pricing.UpdatedDateUtc = DateTime.UtcNow;

                await _pricingRepository.UpdateAsync(pricing);

                _logger.Information(
                    "Reserved {Quantity} units of item {ItemId} for shipment {ShipmentId}",
                    item.Quantity,
                    item.ItemId,
                    shipmentId);
            }

            return allReserved;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to reserve inventory for shipment {ShipmentId}", shipmentId);
            return false;
        }
    }

    /// <summary>
    /// Release reserved inventory back to available stock
    /// Used when order is cancelled or shipment fails
    /// ✅ UPDATED: Supports CancelledByCustomer and CancelledByMarketplace
    /// </summary>
    public async Task<bool> ReleaseInventoryAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                shipmentId,
                cancellationToken);

            if (shipment == null)
            {
                _logger.Error("Shipment {ShipmentId} not found for inventory release", shipmentId);
                return false;
            }

            if (shipment.Items == null || !shipment.Items.Any())
            {
                return true; // No items to release
            }

            foreach (var item in shipment.Items)
            {
                if (!item.ItemCombinationId.HasValue)
                {
                    continue;
                }

                var pricing = await _pricingRepository.FindByIdAsync(
                    item.ItemCombinationId.Value,
                    cancellationToken);

                if (pricing == null)
                {
                    _logger.Error(
                        "Pricing {PricingId} not found for shipment item {ItemId}",
                        item.ItemCombinationId.Value,
                        item.Id);
                    continue;
                }

                // Release reserved inventory back to available
                pricing.AvailableQuantity += item.Quantity;
                pricing.ReservedQuantity = Math.Max(0, pricing.ReservedQuantity - item.Quantity);
                pricing.UpdatedDateUtc = DateTime.UtcNow;

                await _pricingRepository.UpdateAsync(pricing);

                _logger.Information(
                    "Released {Quantity} units of item {ItemId} from shipment {ShipmentId}",
                    item.Quantity,
                    item.ItemId,
                    shipmentId);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to release inventory for shipment {ShipmentId}", shipmentId);
            return false;
        }
    }

    /// <summary>
    /// Determine fulfillment type based on warehouse configuration
    /// </summary>
    public async Task<FulfillmentType> DetermineFulfillmentTypeAsync(
        Guid warehouseId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouse = await _warehouseRepository.FindByIdAsync(warehouseId, cancellationToken);

            if (warehouse == null)
            {
                // Default to vendor fulfillment if warehouse not found
                return FulfillmentType.Vendor;
            }

            // Platform/marketplace warehouse = FBA
            // Vendor warehouse = FBM
            return warehouse.IsDefaultPlatformWarehouse
                ? FulfillmentType.Marketplace
                : FulfillmentType.Vendor;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to determine fulfillment type for warehouse {WarehouseId}", warehouseId);
            throw;
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Create warehouse picking task for FBA shipment
    /// Integration point for Warehouse Management System (WMS)
    /// </summary>
    private async Task CreateWarehousePickingTaskAsync(
        TbOrderShipment shipment,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information(
                "Creating warehouse picking task for shipment {ShipmentNumber}",
                shipment.Number);

            // WMS integration point
            // When integrated with actual WMS:
            // 1. Generate picking list with item locations
            // 2. Calculate optimal picking route
            // 3. Assign to available warehouse staff
            // 4. Track picking progress
            // 5. Generate packing list
            // 6. Create shipping label

            // For now, just log the task creation
            _logger.Information(
                "Picking task created for shipment {ShipmentNumber} - {ItemCount} items",
                shipment.Number,
                shipment.Items?.Count ?? 0);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to create warehouse picking task for shipment {ShipmentId}", shipment.Id);
        }
    }

    /// <summary>
    /// Notify vendor of new FBM shipment
    /// Uses existing INotificationService for Email, SMS, and SignalR
    /// </summary>
    private async Task NotifyVendorOfNewShipmentAsync(
        TbOrderShipment shipment,
        CancellationToken cancellationToken)
    {
        try
        {
            if (shipment.Vendor == null)
            {
                _logger.Warning("Cannot notify vendor - shipment {ShipmentId} has no vendor", shipment.Id);
                return;
            }

            var parameters = new Dictionary<string, string>
            {
                { "VendorName", shipment.Vendor.StoreName ?? "" },
                { "ShipmentNumber", shipment.Number },
                { "OrderNumber", shipment.Order?.Number ?? "" },
                { "ItemCount", shipment.Items?.Count.ToString() ?? "0" },
                { "TotalAmount", shipment.TotalAmount.ToString("N2") },
                { "CustomerName", shipment.Order?.User?.FirstName ?? "" },
                { "ShipmentUrl", $"/vendor/shipments/{shipment.Id}" }
            };

            // 1. Email Notification
            if (shipment.Vendor.User != null && !string.IsNullOrEmpty(shipment.Vendor.User.Email))
            {
                await _notificationService.SendNotificationAsync(new NotificationRequest
                {
                    Recipient = shipment.Vendor.User.Email,
                    Channel = NotificationChannel.Email,
                    Type = NotificationType.VendorNewShipment,
                    Subject = $"New Order - Shipment {shipment.Number}",
                    Title = "New Order Received",
                    Parameters = parameters
                });
            }

            // 2. SMS Notification
            if (!string.IsNullOrEmpty(shipment.Vendor.PhoneNumber))
            {
                await _notificationService.SendNotificationAsync(new NotificationRequest
                {
                    Recipient = shipment.Vendor.PhoneNumber,
                    Channel = NotificationChannel.Sms,
                    Type = NotificationType.VendorNewShipment,
                    Title = $"New order #{shipment.Number} - {shipment.Items?.Count ?? 0} items - {shipment.TotalAmount:N2} EGP",
                    Parameters = parameters
                });
            }

            // 3. In-App Notification (SignalR)
            if (!string.IsNullOrEmpty(shipment.Vendor.UserId))
            {
                await _notificationService.SendNotificationAsync(new NotificationRequest
                {
                    Recipient = shipment.Vendor.UserId,
                    Channel = NotificationChannel.SignalR,
                    Type = NotificationType.VendorNewShipment,
                    Title = "New Order Received",
                    ImagePath = "/images/notifications/new-order-vendor.png",
                    CallToActionUrl = $"/vendor/shipments/{shipment.Id}",
                    Parameters = parameters
                });
            }

            _logger.Information(
                "Vendor {VendorId} notified of new shipment {ShipmentNumber}",
                shipment.VendorId,
                shipment.Number);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to notify vendor of shipment {ShipmentId}", shipment.Id);
            // Don't throw - notification failure shouldn't stop the process
        }
    }

    /// <summary>
    /// ✅ NEW: Notify customer that order is being prepared
    /// </summary>
    private async Task NotifyCustomerOrderPreparationAsync(
        TbOrderShipment shipment,
        CancellationToken cancellationToken)
    {
        try
        {
            if (shipment.Order?.User == null)
            {
                return;
            }

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", shipment.Order.Number },
                { "ShipmentNumber", shipment.Number },
                { "CustomerName", shipment.Order.User.FirstName ?? "" },
                { "EstimatedDelivery", shipment.EstimatedDeliveryDate?.ToString("dd/MM/yyyy") ?? "" }
            };

            // SignalR notification
            await _notificationService.SendNotificationAsync(new NotificationRequest
            {
                Recipient = shipment.Order.UserId,
                Channel = NotificationChannel.SignalR,
                Type = NotificationType.OrderProcessing,
                Title = "Your Order is Being Prepared",
                ImagePath = "/images/notifications/order-preparing.png",
                CallToActionUrl = $"/orders/{shipment.OrderId}",
                Parameters = parameters
            });

            _logger.Information(
                "Customer {UserId} notified that order {OrderNumber} is being prepared",
                shipment.Order.UserId,
                shipment.Order.Number);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to notify customer of order preparation for shipment {ShipmentId}", shipment.Id);
        }
    }

    #endregion
}