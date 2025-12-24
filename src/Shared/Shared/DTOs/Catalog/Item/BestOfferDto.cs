namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Best offer details for a product (Buy Box winner)
    /// </summary>
    public class BestOfferDto
    {
        /// <summary>
        /// Offer ID
        /// </summary>
        public Guid OfferId { get; set; }

        /// <summary>
        /// Vendor ID (GUID)
        /// </summary>
        public Guid VendorId { get; set; }

        /// <summary>
        /// Vendor name (optional, should be fetched separately)
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Current selling price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Original price (before discount)
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Discount percentage (if on sale)
        /// </summary>
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Available quantity
        /// </summary>
        public int AvailableQuantity { get; set; }

        /// <summary>
        /// Whether this offer includes free shipping
        /// </summary>
        public bool IsFreeShipping { get; set; }

        /// <summary>
        /// Estimated delivery time in days
        /// </summary>
        public int EstimatedDeliveryDays { get; set; }
    }
}
