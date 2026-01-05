using Shared.DTOs.Order.Fulfillment.Shipment;

namespace BL.Contracts.Service.Order.Fulfillment;

/// <summary>
/// Service responsible for calculating shipping costs and delivery estimates during checkout.
/// Note: This service focuses on pre-checkout calculations only.
/// For post-order shipment operations, use IShipmentService, IDeliveryService, and IFulfillmentService.
/// </summary>
public interface IShippingCalculationService
{
    /// <summary>
    /// Calculates shipping details for all cart items grouped by vendor/warehouse.
    /// Groups items by vendor and warehouse to determine shipment previews.
    /// </summary>
    /// <param name="cartItems">Collection of cart items to calculate shipping for</param>
    /// <param name="deliveryAddressId">The delivery address identifier</param>
    /// <param name="deliveryCityId">The delivery city identifier for shipping rate lookup</param>
    /// <returns>Shipping calculation result with shipment previews and total cost</returns>
    Task<ShippingCalculationResult> CalculateShippingAsync(
        IEnumerable<CartItemForShipping> cartItems,
        Guid deliveryAddressId,
        Guid deliveryCityId);

    /// <summary>
    /// Calculates shipping cost for a specific vendor and city combination.
    /// Looks up shipping rates from the database or applies default rates.
    /// </summary>
    /// <param name="vendorId">The vendor identifier</param>
    /// <param name="cityId">The destination city identifier</param>
    /// <param name="totalItems">Total number of items for bulk discount calculation</param>
    /// <returns>Calculated shipping cost</returns>
    Task<decimal> CalculateShippingCostAsync(
        Guid warehouseId,
        Guid cityId,
        int totalItems);

    /// <summary>
    /// Gets estimated delivery days for a vendor-city combination.
    /// Used for showing delivery estimates during checkout.
    /// </summary>
    /// <param name="vendorId">The vendor identifier</param>
    /// <param name="cityId">The destination city identifier</param>
    /// <returns>Estimated number of delivery days</returns>
    Task<int> GetEstimatedDeliveryDaysAsync(Guid vendorId, Guid cityId);

    /// <summary>
    /// Gets estimated delivery days directly from an offer.
    /// Use this when you already have the offer ID.
    /// </summary>
    /// <param name="offerId">The offer identifier</param>
    /// <returns>Estimated number of delivery days</returns>
    Task<int> GetEstimatedDeliveryDaysByOfferAsync(Guid offerId);
}