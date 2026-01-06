using BL.Contracts.Service.Order.Fulfillment;
using DAL.Contracts.Repositories;
using Serilog;
using Shared.DTOs.Order.Checkout;
using Shared.DTOs.Order.Fulfillment.Shipment;

namespace BL.Services.Order.Fulfillment;

/// <summary>
/// Service responsible for calculating shipping costs and delivery estimates during checkout phase.
/// This service handles pre-order shipping calculations only.
/// For post-order operations (tracking, status updates, delivery), use:
/// - IShipmentService: Shipment creation and management
/// - IDeliveryService: Delivery completion and returns
/// - IFulfillmentService: Inventory and fulfillment operations
/// </summary>
public class ShippingCalculationService : IShippingCalculationService
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger _logger;

    public ShippingCalculationService(
        IOfferRepository offerRepository,
        ILogger logger)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ShippingCalculationResult> CalculateShippingAsync(
        IEnumerable<CartItemForShipping> cartItems,
        Guid deliveryAddressId,
        Guid deliveryCityId)
    {
        // Group items by warehouse to determine separate shipments
        var shipmentGroups = cartItems
            .GroupBy(i => i.Offer.WarehouseId)
            .ToList();

        var shipmentPreviews = new List<ShipmentPreviewDto>();
        decimal totalShipping = 0;

        foreach (var group in shipmentGroups)
        {
            var warehouseId = group.Key;

            // TODO: Calculate shipping cost based on shipping company integration
            // For now, returning zero until shipping integration is ready
            var shippingCost = await CalculateShippingCostAsync(
                warehouseId,
                deliveryCityId,
                group.Sum(i => i.Quantity)
            );

            totalShipping += shippingCost;

            // Get estimated delivery time based on offers in this shipment
            var estimatedDays = await GetEstimatedDeliveryDaysForShipmentAsync(
                warehouseId,
                group.Select(i => i.Offer.Id).ToList(),
                deliveryCityId
            );

            shipmentPreviews.Add(new ShipmentPreviewDto
            {
                ItemCount = group.Sum(i => i.Quantity),
                ItemsList = group.Select(i => i.ItemName).ToList(),
                SubTotal = group.Sum(i => i.SubTotal),
                ShippingCost = shippingCost,
                EstimatedDeliveryDays = estimatedDays
            });
        }

        return new ShippingCalculationResult
        {
            ShipmentPreviews = shipmentPreviews,
            TotalShippingCost = totalShipping
        };
    }

    public async Task<decimal> CalculateShippingCostAsync(
        Guid warehouseId,
        Guid cityId,
        int totalItems)
    {
        if (totalItems <= 0)
        {
            throw new ArgumentException("Total items must be greater than zero", nameof(totalItems));
        }

        // TODO: Implement actual shipping cost calculation when shipping company integration is ready
        // Will integrate with shipping companies APIs to get real-time rates based on:
        // - Vendor location/warehouse
        // - Destination city
        // - Total weight/volume of items
        // - Shipping method (standard/express)

        return 0m; // Placeholder until shipping integration
    }

    public async Task<int> GetEstimatedDeliveryDaysAsync(Guid vendorId, Guid cityId)
    {
        // Get all vendor offers to calculate average delivery time
        var offers = await _offerRepository.GetOffersByVendorIdAsync(vendorId);

        if (offers != null && offers.Any())
        {
            // Calculate average estimated delivery days from all vendor's offers
            var avgDays = (int)offers.Average(o => o.EstimatedDeliveryDays);

            return avgDays;
        }

        // Default fallback
        _logger.Warning(
            "No offers found for vendor {VendorId}. Using default delivery estimate",
            vendorId
        );

        return 3; // Default: 3 business days
    }

    public async Task<int> GetEstimatedDeliveryDaysByOfferAsync(Guid offerId)
    {
        var offer = await _offerRepository.FindByIdAsync(offerId);

        if (offer == null)
        {
            throw new KeyNotFoundException($"Offer with ID {offerId} not found");
        }

        _logger.Information(
            "Retrieved estimated delivery days for offer {OfferId}: {Days} days",
            offerId,
            offer.EstimatedDeliveryDays
        );

        return offer.EstimatedDeliveryDays;
    }

    /// <summary>
    /// Gets estimated delivery days for a shipment containing multiple offers
    /// Returns the maximum delivery time to ensure all items can be delivered together
    /// </summary>
    private async Task<int> GetEstimatedDeliveryDaysForShipmentAsync(
        Guid vendorId,
        List<Guid> offerIds,
        Guid cityId)
    {
        if (!offerIds.Any())
        {
            return await GetEstimatedDeliveryDaysAsync(vendorId, cityId);
        }

        // Get all offers in this shipment
        var deliveryTimes = new List<int>();

        foreach (var offerId in offerIds.Distinct())
        {
            try
            {
                var days = await GetEstimatedDeliveryDaysByOfferAsync(offerId);
                deliveryTimes.Add(days);
            }
            catch (KeyNotFoundException)
            {
                _logger.Warning("Offer {OfferId} not found, skipping", offerId);
            }
        }

        if (deliveryTimes.Any())
        {
            // Return the maximum delivery time to ensure all items arrive together
            var maxDays = deliveryTimes.Max();

            _logger.Information(
                "Estimated delivery for shipment with {OfferCount} offers: {Days} days (max)",
                offerIds.Count,
                maxDays
            );

            return maxDays;
        }

        // Fallback to vendor average
        return await GetEstimatedDeliveryDaysAsync(vendorId, cityId);
    }
}