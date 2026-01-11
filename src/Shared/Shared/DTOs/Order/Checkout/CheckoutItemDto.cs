using System.Text.Json.Serialization;

namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Individual item in checkout
    /// </summary>
    public class CheckoutItemDto
    {
        // ============================================
        // IDENTIFICATION
        // ============================================

        public Guid ItemId { get; set; }
        public Guid ItemCombinationId { get; set; }

        /// <summary>
        /// Offer combination pricing ID
        /// Required for order details creation
        /// </summary>
        public required Guid OfferCombinationPricingId { get; set; }

        /// <summary>
        /// Vendor ID
        /// Required for order details and fulfillment
        /// </summary>
        public required Guid VendorId { get; set; }

        /// <summary>
        /// Warehouse ID
        /// Required for order details and fulfillment
        /// </summary>
        [JsonIgnore]
        public Guid WarehouseId { get; set; }

        // ============================================
        // DISPLAY INFORMATION
        // ============================================

        public string ItemName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;

        // ============================================
        // PRICING & QUANTITY
        // ============================================

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Discount amount applied to this item
        /// Required for order details
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Tax amount for this item
        /// Required for order details
        /// </summary>
        public decimal TaxAmount { get; set; }

        // ============================================
        // AVAILABILITY & PRICING STRATEGY
        // ============================================

        public bool IsAvailable { get; set; }
    }
}