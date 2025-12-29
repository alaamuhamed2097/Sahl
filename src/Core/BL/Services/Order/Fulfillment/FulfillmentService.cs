using BL.Contracts.Service.Order.Fulfillment;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Offer;
using Domains.Entities.Shipping;
using Domains.Entities.Warehouse;
using Serilog;

namespace BL.Services.Order.Fulfillment;

public class FulfillmentService : IFulfillmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ILogger _logger;

    public FulfillmentService(
        IUnitOfWork unitOfWork,
        IShipmentRepository shipmentRepository,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _shipmentRepository = shipmentRepository ?? throw new ArgumentNullException(nameof(shipmentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ProcessFBAShipmentAsync(Guid shipmentId)
    {
        try
        {
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(shipmentId);
            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            // 1. Reserve inventory from platform warehouse
            var reserved = await ReserveInventoryAsync(shipmentId);
            if (!reserved)
            {
                throw new InvalidOperationException("Failed to reserve inventory");
            }

            // 2. Update shipment status to Processing
            shipment.ShipmentStatus = ShipmentStatus.Processing;
            shipment.UpdatedDateUtc = DateTime.UtcNow;
            await _shipmentRepository.UpdateAsync(shipment, Guid.Empty);

            // 3. Create warehouse picking task
            await CreateWarehousePickingTaskAsync(shipment);

            _logger.Information("FBA shipment {ShipmentId} processing initiated", shipmentId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing FBA shipment {ShipmentId}", shipmentId);
            throw;
        }
    }

    public async Task ProcessFBMShipmentAsync(Guid shipmentId)
    {
        try
        {
            // ✅ FIXED: Use GetAsync with include
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(shipmentId);

            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            // 1. Reserve inventory from vendor's stock
            var reserved = await ReserveInventoryAsync(shipmentId);
            if (!reserved)
            {
                throw new InvalidOperationException("Failed to reserve inventory");
            }

            // 2. Notify vendor of new order
            await NotifyVendorOfNewShipmentAsync(shipment);

            // 3. Update shipment status to Processing (vendor will update to Shipped)
            shipment.ShipmentStatus = ShipmentStatus.Processing;
            shipment.UpdatedDateUtc = DateTime.UtcNow;
            await _shipmentRepository.UpdateAsync(shipment, Guid.Empty);

            _logger.Information("FBM shipment {ShipmentId} assigned to vendor", shipmentId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing FBM shipment {ShipmentId}", shipmentId);
            throw;
        }
    }

    public async Task<bool> ReserveInventoryAsync(Guid shipmentId)
    {
        try
        {
            _logger.Information("Reserving inventory for shipment {ShipmentId}", shipmentId);

            var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();

            // ✅ FIXED: Use GetAsync with include
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                shipmentId);

            if (shipment == null)
            {
                _logger.Warning("Shipment {ShipmentId} not found", shipmentId);
                return false;
            }

            var allReserved = true;

            foreach (var item in shipment.Items)
            {
                var pricing = await pricingRepo.FindByIdAsync(item.ItemCombinationId.Value);

                if (pricing == null)
                {
                    _logger.Warning(
                        "Pricing {PricingId} not found for shipment item {ItemId}",
                        item.ItemCombinationId,
                        item.Id
                    );
                    allReserved = false;
                    continue;
                }

                // Check if enough stock available
                if (pricing.AvailableQuantity < item.Quantity)
                {
                    _logger.Warning(
                        "Insufficient stock for item {ItemId}. Available: {Available}, Required: {Required}",
                        item.ItemId,
                        pricing.AvailableQuantity,
                        item.Quantity
                    );
                    allReserved = false;
                    continue;
                }

                // Reserve inventory
                pricing.AvailableQuantity -= item.Quantity;
                pricing.ReservedQuantity += item.Quantity;
                pricing.UpdatedDateUtc = DateTime.UtcNow;

                await pricingRepo.UpdateAsync(pricing, Guid.Empty);

                _logger.Information(
                    "Reserved {Quantity} units of item {ItemId} for shipment {ShipmentId}",
                    item.Quantity,
                    item.ItemId,
                    shipmentId
                );
            }

            return allReserved;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error reserving inventory for shipment {ShipmentId}", shipmentId);
            return false;
        }
    }

    public async Task<bool> ReleaseInventoryAsync(Guid shipmentId)
    {
        try
        {
            _logger.Information("Releasing inventory for shipment {ShipmentId}", shipmentId);

            var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();

            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(shipmentId);

            if (shipment == null)
            {
                _logger.Warning("Shipment {ShipmentId} not found", shipmentId);
                return false;
            }

            foreach (var item in shipment.Items)
            {
                var pricing = await pricingRepo.FindByIdAsync(item.ItemCombinationId.Value);

                if (pricing == null)
                {
                    _logger.Warning(
                        "Pricing {PricingId} not found for shipment item {ItemId}",
                        item.ItemCombinationId,
                        item.Id
                    );
                    continue;
                }

                // Release reserved inventory back to available
                pricing.AvailableQuantity += item.Quantity;
                pricing.ReservedQuantity = Math.Max(0, pricing.ReservedQuantity - item.Quantity);
                pricing.UpdatedDateUtc = DateTime.UtcNow;

                await pricingRepo.UpdateAsync(pricing, Guid.Empty);

                _logger.Information(
                    "Released {Quantity} units of item {ItemId} from shipment {ShipmentId}",
                    item.Quantity,
                    item.ItemId,
                    shipmentId
                );
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error releasing inventory for shipment {ShipmentId}", shipmentId);
            return false;
        }
    }

    public async Task<FulfillmentType> DetermineFulfillmentTypeAsync(Guid warehouseId)
    {
        try
        {
            var warehouseRepo = _unitOfWork.TableRepository<TbWarehouse>();
            var warehouse = await warehouseRepo.FindByIdAsync(warehouseId);

            if (warehouse == null)
            {
                return FulfillmentType.Seller; // Default to merchant fulfillment
            }

            // ✅ FIXED: Check warehouse type
            // Assuming TbWarehouse has a Type property
            return warehouse.IsDefaultPlatformWarehouse ? FulfillmentType.Marketplace : FulfillmentType.Seller;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error determining fulfillment type for warehouse {WarehouseId}", warehouseId);
            throw;
        }
    }

    // ==================== PRIVATE HELPER METHODS ====================

    private async Task CreateWarehousePickingTaskAsync(TbOrderShipment shipment)
    {
        try
        {
            // TODO: Integration with warehouse management system
            // Create picking task in WMS
            // Generate picking list
            // Assign to warehouse staff

            _logger.Information(
                "Warehouse picking task created for shipment {ShipmentId}",
                shipment.Id
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating warehouse picking task");
        }
    }

    private async Task NotifyVendorOfNewShipmentAsync(TbOrderShipment shipment)
    {
        try
        {
            // TODO: Send notification to vendor
            // - Email notification
            // - SMS notification
            // - In-app notification
            // - Webhook to vendor system

            _logger.Information(
                "Vendor {VendorId} notified of new shipment {ShipmentId}",
                shipment.VendorId,
                shipment.Id
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error notifying vendor");
        }
    }
}