using Shared.DTOs.Order.Checkout;

namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    /// <summary>
    /// Result of shipping calculation containing shipment previews and total cost
    /// </summary>
    public class ShippingCalculationResult
    {
        public List<ShipmentPreviewDto> ShipmentPreviews { get; set; } = new();
        public decimal TotalShippingCost { get; set; }
    }
}