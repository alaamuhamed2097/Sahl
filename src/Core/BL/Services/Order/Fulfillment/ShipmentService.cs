using AutoMapper;
using BL.Contracts.Service.Order.Fulfillment;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.Fulfillment.Shipment;

namespace BL.Services.Order.Fulfillment;

/// <summary>
/// FINAL Shipment Service
/// - New ShipmentStatus enum values
/// - Tax and discount distribution across shipments
/// - COD payment support via TbShipmentPayment
/// - Payment summary updates
/// - No InvoiceId, no AllowSplitPayment flag
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
            var shipmentPaymentRepo = _unitOfWork.TableRepository<TbShipmentPayment>();

            // Calculate total order subtotal for distribution
            var totalOrderSubTotal = order.OrderDetails.Sum(od => od.SubTotal);

            // Check if payment is COD
            var isCOD = order.OrderPayments.Any(p => p.PaymentMethodType == PaymentMethodType.CashOnDelivery);

            foreach (var group in shipmentGroups)
            {
                // Determine fulfillment type
                var fulfillmentType = await DetermineFulfillmentTypeAsync(group.Key.WarehouseId);

                // Calculate shipment subtotal
                var shipmentSubTotal = group.Sum(od => od.SubTotal);

                // Calculate shipment shipping cost
                var shippingCost = await CalculateShipmentShippingAsync(
                    group.Key.VendorId,
                    order.CustomerAddress.CityId,
                    group.Sum(od => od.Quantity)
                );

                // ==================== DISTRIBUTE TAX AND DISCOUNT ====================
                var shipmentRatio = totalOrderSubTotal > 0
                    ? shipmentSubTotal / totalOrderSubTotal
                    : 0m;

                var taxAmount = Math.Round(order.TaxAmount * shipmentRatio, 2);
                var discountAmount = Math.Round(order.DiscountAmount * shipmentRatio, 2);

                // Create shipment
                var shipment = new TbOrderShipment
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    VendorId = group.Key.VendorId,
                    WarehouseId = group.Key.WarehouseId,
                    Number = await GenerateShipmentNumberAsync(),
                    FulfillmentType = fulfillmentType,
                    ShipmentStatus = ShipmentStatus.PendingProcessing,

                    // Pricing breakdown
                    SubTotal = shipmentSubTotal,
                    ShippingCost = shippingCost,
                    TaxAmount = taxAmount,
                    TaxPercentage = order.TaxPercentage,
                    DiscountAmount = discountAmount,
                    TotalAmount = shipmentSubTotal + shippingCost + taxAmount - discountAmount,

                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = Guid.Empty
                };

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

                // ==================== CREATE COD PAYMENT RECORD IF NEEDED ====================
                if (isCOD)
                {
                    var shipmentPayment = new TbShipmentPayment
                    {
                        Id = Guid.NewGuid(),
                        ShipmentId = shipment.Id,
                        OrderId = orderId,
                        Amount = shipment.TotalAmount,
                        PaymentStatus = PaymentStatus.Pending,
                        TransactionId = $"COD-{shipment.Number}",
                        CreatedDateUtc = DateTime.UtcNow
                    };

                    await shipmentPaymentRepo.CreateAsync(shipmentPayment, Guid.Empty);

                    _logger.Information(
                        "Created COD payment record for shipment {ShipmentNumber} - Amount: {Amount}",
                        shipment.Number,
                        shipment.TotalAmount
                    );
                }

                // Create initial status history
                var statusHistory = new TbShipmentStatusHistory
                {
                    Id = Guid.NewGuid(),
                    ShipmentId = shipment.Id,
                    Status = ShipmentStatus.PendingProcessing,
                    StatusDate = DateTime.UtcNow,
                    Notes = "Shipment created and awaiting warehouse processing",
                    CreatedDateUtc = DateTime.UtcNow
                };

                await historyRepo.CreateAsync(statusHistory, Guid.Empty);

                createdShipments.Add(shipment);

                _logger.Information(
                    "Created shipment {ShipmentNumber} | SubTotal: {SubTotal}, Tax: {Tax}, Discount: {Discount}, Shipping: {Shipping}, Total: {Total}",
                    shipment.Number,
                    shipment.SubTotal,
                    shipment.TaxAmount,
                    shipment.DiscountAmount,
                    shipment.ShippingCost,
                    shipment.TotalAmount
                );
            }

            // ==================== FIX ROUNDING ERRORS ====================
            await AdjustShipmentTotalsForRoundingAsync(order, createdShipments);

            // ==================== UPDATE ORDER SHIPPING AMOUNT ====================
            order.ShippingAmount = createdShipments.Sum(s => s.ShippingCost);

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            await orderRepo.UpdateAsync(order, Guid.Empty);

            _logger.Information(
                "Successfully split order {OrderId} into {Count} shipments | Total Shipping: {ShippingAmount}",
                orderId,
                createdShipments.Count,
                order.ShippingAmount
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
            var shipment = await shipmentRepo.FindAsync(s => s.Number == shipmentNumber);

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

            if (!Enum.TryParse<ShipmentStatus>(newStatus, out var status))
            {
                throw new ArgumentException($"Invalid shipment status: {newStatus}");
            }

            var oldStatus = shipment.ShipmentStatus;
            shipment.ShipmentStatus = status;

            // Update specific fields based on status
            switch (status)
            {
                case ShipmentStatus.PickedUpByCarrier:
                    if (!shipment.EstimatedDeliveryDate.HasValue)
                    {
                        shipment.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(4);
                    }
                    break;

                case ShipmentStatus.DeliveredToCustomer:
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
            if (status == ShipmentStatus.DeliveredToCustomer)
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
            var shipment = await _shipmentRepository
                .GetShipmentByTrackingNumberAsync(trackingNumber);

            if (shipment == null)
            {
                throw new InvalidOperationException(
                    $"Shipment with tracking number {trackingNumber} not found"
                );
            }

            var history = await _shipmentRepository
                .GetShipmentStatusHistoryAsync(shipment.Id);

            return new ShipmentTrackingDto
            {
                ShipmentNumber = shipment.Number,
                TrackingNumber = trackingNumber,
                CurrentStatus = shipment.ShipmentStatus.ToString(),
                EstimatedDeliveryDate = shipment.EstimatedDeliveryDate,
                ActualDeliveryDate = shipment.ActualDeliveryDate,
                ShippingCompanyName = shipment.ShippingCompany?.Name,
                DeliveryAddress = new DeliveryAddressDto
                {
                    Address = shipment.Order.CustomerAddress.Address,
                    CityNameAr = shipment.Order.CustomerAddress.City.TitleAr,
                    CityNameEn = shipment.Order.CustomerAddress.City.TitleEn,
                    StateNameAr = shipment.Order.CustomerAddress.City.State.TitleAr,
                    StateNameEn = shipment.Order.CustomerAddress.City.State.TitleEn,
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

    /// <summary>
    /// Collect COD payment when shipment is delivered
    /// Updates both TbShipmentPayment and Order payment summary
    /// </summary>
    public async Task<ShipmentDto> CollectCODPaymentAsync(
        Guid shipmentId,
        string? notes = null)
    {
        try
        {
            _logger.Information("Collecting COD payment for shipment {ShipmentId}", shipmentId);

            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var shipmentPaymentRepo = _unitOfWork.TableRepository<TbShipmentPayment>();

            var shipment = await shipmentRepo.FindByIdAsync(shipmentId);
            if (shipment == null)
            {
                throw new InvalidOperationException($"Shipment {shipmentId} not found");
            }

            // Get the shipment payment record
            var shipmentPayments = await shipmentPaymentRepo.GetAsync(
                sp => sp.ShipmentId == shipmentId
            );
            var shipmentPayment = shipmentPayments.FirstOrDefault();

            if (shipmentPayment == null)
            {
                throw new InvalidOperationException(
                    $"No COD payment record found for shipment {shipmentId}"
                );
            }

            // Update shipment payment
            shipmentPayment.PaymentStatus = PaymentStatus.Completed;
            shipmentPayment.PaidAt = DateTime.UtcNow;
            shipmentPayment.Notes = notes;
            shipmentPayment.UpdatedDateUtc = DateTime.UtcNow;

            await shipmentPaymentRepo.UpdateAsync(shipmentPayment, Guid.Empty);

            // Update shipment status to delivered
            shipment.ShipmentStatus = ShipmentStatus.DeliveredToCustomer;
            shipment.ActualDeliveryDate = DateTime.UtcNow;
            shipment.UpdatedDateUtc = DateTime.UtcNow;

            await shipmentRepo.UpdateAsync(shipment, Guid.Empty);

            // Update order payment summary
            await UpdateOrderPaymentSummaryAsync(shipment.OrderId);

            _logger.Information(
                "COD payment collected for shipment {ShipmentNumber} - Amount: {Amount}",
                shipment.Number,
                shipmentPayment.Amount
            );

            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error collecting COD payment for shipment {ShipmentId}", shipmentId);
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

            // Check if it's platform warehouse (FBA) or vendor warehouse (FBM)
            return warehouse?.IsDefaultPlatformWarehouse == true
                ? FulfillmentType.Marketplace
                : FulfillmentType.Vendor;
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

    private async Task AdjustShipmentTotalsForRoundingAsync(
        TbOrder order,
        List<TbOrderShipment> shipments)
    {
        if (shipments.Count == 0) return;

        var calculatedTotal = shipments.Sum(s => s.TotalAmount);
        var difference = order.Price - calculatedTotal;

        if (difference != 0)
        {
            _logger.Information(
                "Adjusting shipment totals for rounding - Difference: {Difference}",
                difference
            );

            var lastShipment = shipments.Last();
            lastShipment.TotalAmount += difference;
            lastShipment.TaxAmount += difference;

            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            await shipmentRepo.UpdateAsync(lastShipment, Guid.Empty);
        }
    }

    private async Task UpdateOrderPaymentSummaryAsync(Guid orderId)
    {
        try
        {
            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var orderPaymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
            var shipmentPaymentRepo = _unitOfWork.TableRepository<TbShipmentPayment>();

            var order = await orderRepo.FindByIdAsync(orderId);
            if (order == null) return;

            var orderPayments = await orderPaymentRepo.GetAsync(op => op.OrderId == orderId);
            var shipmentPayments = await shipmentPaymentRepo.GetAsync(sp => sp.OrderId == orderId);

            // Calculate wallet payments
            order.WalletPaidAmount = orderPayments
                .Where(p => p.PaymentMethodType == PaymentMethodType.Wallet
                         && p.PaymentStatus == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            // Calculate card payments
            order.CardPaidAmount = orderPayments
                .Where(p => p.PaymentMethodType == PaymentMethodType.Card
                         && p.PaymentStatus == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            // Calculate cash (COD) payments
            order.CashPaidAmount = shipmentPayments
                .Where(p => p.PaymentStatus == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            // Calculate totals
            order.TotalPaidAmount = order.WalletPaidAmount
                                  + order.CardPaidAmount
                                  + order.CashPaidAmount;

            // Update overall payment status
            if (order.TotalPaidAmount >= order.Price)
            {
                order.PaymentStatus = PaymentStatus.Completed;

                // Set PaidAt to latest payment date
                var latestPaymentDate = new[]
                {
                    orderPayments.Where(p => p.PaymentStatus == PaymentStatus.Completed)
                                .OrderByDescending(p => p.PaidAt)
                                .FirstOrDefault()?.PaidAt,
                    shipmentPayments.Where(p => p.PaymentStatus == PaymentStatus.Completed)
                                   .OrderByDescending(p => p.PaidAt)
                                   .FirstOrDefault()?.PaidAt
                }
                .Where(d => d.HasValue)
                .OrderByDescending(d => d)
                .FirstOrDefault();

                order.PaidAt = latestPaymentDate;
            }
            else if (order.TotalPaidAmount > 0)
            {
                order.PaymentStatus = PaymentStatus.PartiallyPaid;
            }

            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Empty);

            _logger.Information(
                "Updated payment summary for order {OrderId} - Wallet: {Wallet}, Card: {Card}, Cash: {Cash}, Total: {Total}",
                orderId,
                order.WalletPaidAmount,
                order.CardPaidAmount,
                order.CashPaidAmount,
                order.TotalPaidAmount
            );
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating payment summary for order {OrderId}", orderId);
        }
    }

    private async Task CheckOrderCompletionAsync(Guid orderId)
    {
        try
        {
            var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
            var shipments = await shipmentRepo.GetAsync(s => s.OrderId == orderId);

            var allDelivered = shipments.All(s => s.ShipmentStatus == ShipmentStatus.DeliveredToCustomer);

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