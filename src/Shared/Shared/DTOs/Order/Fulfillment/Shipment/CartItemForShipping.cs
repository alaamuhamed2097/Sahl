using Shared.DTOs.ECommerce.Offer;

namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    /// <summary>
    /// Simplified cart item model for shipping calculations
    /// </summary>
    public class CartItemForShipping
    {
        public Guid ItemId { get; set; }
        public required string ItemNameAr { get; set; } = string.Empty;
        public required string ItemNameEn { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public VendorItemDto Offer { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}