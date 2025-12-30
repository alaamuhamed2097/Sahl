using AutoMapper;
using BL.Contracts.Service.Order.Fulfillment;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.Fulfillment.Shipment;

namespace BL.Services.Order.Fulfillment;

/// <summary>
/// FINAL FIXED VERSION - Uses custom repositories
/// </summary>
public class ShipmentService : IShipmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ShipmentService(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IShipmentRepository shipmentRepository,
        IMapper mapper,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _shipmentRepository = shipmentRepository ?? throw new ArgumentNullException(nameof(shipmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<ShipmentDto>> SplitOrderIntoShipmentsAsync(Guid orderId)
    {
        try
        {
            _logger.Information("Splitting order {OrderId} into shipments", orderId);

            // ✅ Use custom repository
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderId} not found");
            }

            // Group order details by Vendor + Warehouse
            var shipmentGroups = order.OrderDetails
                .GroupBy(od => new { od.VendorId, od.WarehouseId })
                .ToList();

            var createdShipments = new List<TbOrderShipment>();
            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var shipmentItemRepo = _unitOfWork.TableRepository<TbOrderShipmentItem>();
            var historyRepo = _unitOfWork.TableRepository<TbShipmentStatusHistory>();

            foreach (var group in shipmentGroups)
            {
                // Determine fulfillment type
                var fulfillmentType = await DetermineFulfillmentTypeAsync(group.Key.WarehouseId);

                // Calculate shipment shipping cost
                var shippingCost = await CalculateShipmentShippingAsync(
                    group.Key.VendorId,
                    order.CustomerAddress.CityId,
                    group.Sum(od => od.Quantity)
                );

                // Create shipment
                var shipment = new TbOrderShipment
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    VendorId = group.Key.VendorId,
                    WarehouseId = group.Key.WarehouseId,
                    ShipmentNumber = await GenerateShipmentNumberAsync(),
                    FulfillmentType = fulfillmentType,
                    ShipmentStatus = ShipmentStatus.Pending,
                    ShippingCost = shippingCost,
                    SubTotal = group.Sum(od => od.SubTotal),
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = Guid.Empty // System
                };

                shipment.TotalAmount = shipment.SubTotal + shipment.ShippingCost;

                await shipmentRepo.CreateAsync(shipment, Guid.Empty);

                // Create shipment items
                foreach (var orderDetail in group)
                {
                    var shipmentItem = new TbOrderShipmentItem
                    {
                        Id = Guid.NewGuid(),
                        ShipmentId = shipment.Id,
                        OrderDetailId = orderDetail.Id,
                        ItemId = orderDetail.ItemId,
                        ItemCombinationId = orderDetail.OfferCombinationPricingId,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice,
                        SubTotal = orderDetail.SubTotal,
                        CreatedDateUtc = DateTime.UtcNow
                    };

                    await shipmentItemRepo.CreateAsync(shipmentItem, Guid.Empty);
                }

                // Create initial status history
                var statusHistory = new TbShipmentStatusHistory
                {
                    Id = Guid.NewGuid(),
                    ShipmentId = shipment.Id,
                    Status = ShipmentStatus.Pending,
                    StatusDate = DateTime.UtcNow,
                    Notes = "Shipment created and awaiting processing",
                    CreatedDateUtc = DateTime.UtcNow
                };

                await historyRepo.CreateAsync(statusHistory, Guid.Empty);

                createdShipments.Add(shipment);

                _logger.Information(
                    "Created shipment {ShipmentNumber} for vendor {VendorId}",
                    shipment.ShipmentNumber,
                    group.Key.VendorId
                );
            }

            _logger.Information(
                "Successfully split order {OrderId} into {Count} shipments",
                orderId,
                createdShipments.Count
            );

            return _mapper.Map<List<ShipmentDto>>(createdShipments);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error splitting order {OrderId} into shipments", orderId);
            throw;
        }
    }

    public async Task<ShipmentDto> GetShipmentByIdAsync(Guid shipmentId)
    {
        try
        {
            // ✅ Use custom repository
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(shipmentId);

            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting shipment {ShipmentId}", shipmentId);
            throw;
        }
    }

    public async Task<ShipmentDto> GetShipmentByNumberAsync(string shipmentNumber)
    {
        try
        {
            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var shipment = await shipmentRepo.FindAsync(s => s.ShipmentNumber == shipmentNumber);

            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentNumber} not found");
            }

            return await GetShipmentByIdAsync(shipment.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting shipment by number {ShipmentNumber}", shipmentNumber);
            throw;
        }
    }

    public async Task<List<ShipmentDto>> GetOrderShipmentsAsync(Guid orderId)
    {
        try
        {
            // ✅ Use custom repository
            var shipments = await _shipmentRepository.GetOrderShipmentsAsync(orderId);
            return _mapper.Map<List<ShipmentDto>>(shipments);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting shipments for order {OrderId}", orderId);
            throw;
        }
    }

    public async Task<ShipmentDto> UpdateShipmentStatusAsync(
        Guid shipmentId,
        string newStatus,
        string? location = null,
        string? notes = null)
    {
        try
        {
            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var historyRepo = _unitOfWork.TableRepository<TbShipmentStatusHistory>();

            var shipment = await shipmentRepo.FindByIdAsync(shipmentId);
            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            // Parse and validate status
            if (!Enum.TryParse<ShipmentStatus>(newStatus, out var status))
            {
                throw new ArgumentException($"Invalid shipment status: {newStatus}");
            }

            var oldStatus = shipment.ShipmentStatus;
            shipment.ShipmentStatus = status;

            // Update specific fields based on status
            switch (status)
            {
                case ShipmentStatus.Shipped:
                    if (!shipment.EstimatedDeliveryDate.HasValue)
                    {
                        shipment.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(4);
                    }
                    break;

                case ShipmentStatus.Delivered:
                    shipment.ActualDeliveryDate = DateTime.UtcNow;
                    break;
            }

            shipment.UpdatedDateUtc = DateTime.UtcNow;
            await shipmentRepo.UpdateAsync(shipment, Guid.Empty);

            // Create status history entry
            var history = new TbShipmentStatusHistory
            {
                Id = Guid.NewGuid(),
                ShipmentId = shipmentId,
                Status = status,
                StatusDate = DateTime.UtcNow,
                Location = location,
                Notes = notes ?? $"Status changed from {oldStatus} to {status}",
                CreatedDateUtc = DateTime.UtcNow
            };

            await historyRepo.CreateAsync(history, Guid.Empty);

            _logger.Information(
                "Shipment {ShipmentId} status updated from {OldStatus} to {NewStatus}",
                shipmentId,
                oldStatus,
                status
            );

            // Check if all shipments for this order are delivered
            if (status == ShipmentStatus.Delivered)
            {
                await CheckOrderCompletionAsync(shipment.OrderId);
            }

            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating shipment {ShipmentId} status", shipmentId);
            throw;
        }
    }

    public async Task<ShipmentDto> AssignTrackingNumberAsync(
        Guid shipmentId,
        string trackingNumber,
        DateTime? estimatedDeliveryDate = null)
    {
        try
        {
            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var shipment = await shipmentRepo.FindByIdAsync(shipmentId);

            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            shipment.TrackingNumber = trackingNumber;

            if (estimatedDeliveryDate.HasValue)
            {
                shipment.EstimatedDeliveryDate = estimatedDeliveryDate;
            }

            shipment.UpdatedDateUtc = DateTime.UtcNow;
            await shipmentRepo.UpdateAsync(shipment, Guid.Empty);

            _logger.Information(
                "Tracking number {TrackingNumber} assigned to shipment {ShipmentId}",
                trackingNumber,
                shipmentId
            );

            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error assigning tracking number to shipment {ShipmentId}", shipmentId);
            throw;
        }
    }

    public async Task<ShipmentTrackingDto> TrackShipmentAsync(string trackingNumber)
    {
        try
        {
            // ✅ Use custom repository
            var shipment = await _shipmentRepository
                .GetShipmentByTrackingNumberAsync(trackingNumber);

            if (shipment == null)
            {
                throw new InvalidOperationException(
                    $"Shipment with tracking number {trackingNumber} not found"
                );
            }

            // Get status history
            var history = await _shipmentRepository
                .GetShipmentStatusHistoryAsync(shipment.Id);

            return new ShipmentTrackingDto
            {
                ShipmentNumber = shipment.ShipmentNumber,
                TrackingNumber = trackingNumber,
                CurrentStatus = shipment.ShipmentStatus.ToString(),
                EstimatedDeliveryDate = shipment.EstimatedDeliveryDate,
                ActualDeliveryDate = shipment.ActualDeliveryDate,
                ShippingCompanyName = shipment.ShippingCompany?.Name,
                DeliveryAddress = new DeliveryAddressDto
                {
                    Address = shipment.Order.CustomerAddress.Address,
                    CityName = shipment.Order.CustomerAddress.City.TitleEn,
                    StateName = shipment.Order.CustomerAddress.City.State.TitleEn, // ✅ State not Governorate
                    PhoneCode = shipment.Order.CustomerAddress.PhoneCode,
                    PhoneNumber = shipment.Order.CustomerAddress.PhoneNumber,
                    RecipientName = shipment.Order.CustomerAddress.RecipientName
                },
                StatusHistory = _mapper.Map<List<StatusHistoryDto>>(history)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error tracking shipment {TrackingNumber}", trackingNumber);
            throw;
        }
    }

    public async Task<List<ShipmentDto>> GetVendorShipmentsAsync(
        Guid vendorId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            // ✅ Use custom repository
            var pagedResult = await _shipmentRepository
                .GetVendorShipmentsPagedAsync(vendorId, pageNumber, pageSize);

            return _mapper.Map<List<ShipmentDto>>(pagedResult.Items);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting shipments for vendor {VendorId}", vendorId);
            throw;
        }
    }

    // ==================== PRIVATE HELPER METHODS ====================

    private async Task<string> GenerateShipmentNumberAsync()
    {
        var date = DateTime.UtcNow;
        var datePrefix = date.ToString("yyyyMMdd");

        var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
        var todayCount = await shipmentRepo.CountAsync(s => s.CreatedDateUtc.Date == date.Date);

        return $"SHP-{datePrefix}-{(todayCount + 1).ToString("D6")}";
    }

    private async Task<FulfillmentType> DetermineFulfillmentTypeAsync(Guid? warehouseId)
    {
        if (!warehouseId.HasValue)
        {
            return FulfillmentType.Marketplace;
        }

        try
        {
            var warehouseRepo = _unitOfWork.TableRepository<Domains.Entities.Warehouse.TbWarehouse>();
            var warehouse = await warehouseRepo.FindByIdAsync(warehouseId.Value);

            // ✅ TODO: Check warehouse type property when added to entity
            // For now, default to Marketplace
            return FulfillmentType.Marketplace;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error determining fulfillment type for warehouse {WarehouseId}", warehouseId);
            return FulfillmentType.Marketplace;
        }
    }

    private async Task<decimal> CalculateShipmentShippingAsync(
        Guid vendorId,
        Guid cityId,
        int totalQuantity)
    {
        try
        {
            var shippingDetailRepo = _unitOfWork.TableRepository<TbShippingDetail>();
            var shippingDetail = await shippingDetailRepo.FindAsync(sd =>
                sd.Offer.VendorId == vendorId &&
                sd.CityId == cityId
            );

            if (shippingDetail != null)
            {
                return shippingDetail.ShippingCost;
            }

            const decimal baseRate = 30m;
            const decimal perItemRate = 5m;
            return baseRate + (perItemRate * Math.Min(totalQuantity, 10));
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error calculating shipping for vendor {VendorId}, city {CityId}",
                vendorId,
                cityId
            );
            return 50m;
        }
    }

    private async Task CheckOrderCompletionAsync(Guid orderId)
    {
        try
        {
            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var shipments = await shipmentRepo.GetAsync(s => s.OrderId == orderId);

            var allDelivered = shipments.All(s => s.ShipmentStatus == ShipmentStatus.Delivered);

            if (allDelivered)
            {
                var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                var order = await orderRepo.FindByIdAsync(orderId);

                if (order != null && order.OrderStatus != Common.Enumerations.Order.OrderProgressStatus.Completed)
                {
                    order.OrderStatus = Common.Enumerations.Order.OrderProgressStatus.Completed;
                    order.OrderDeliveryDate = DateTime.UtcNow;
                    order.UpdatedDateUtc = DateTime.UtcNow;

                    await orderRepo.UpdateAsync(order, Guid.Empty);

                    _logger.Information(
                        "Order {OrderId} marked as completed - all shipments delivered",
                        orderId
                    );
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error checking order completion for {OrderId}", orderId);
        }
    }
}