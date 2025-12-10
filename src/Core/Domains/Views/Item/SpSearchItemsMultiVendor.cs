namespace Domains.Views.Item
{
    /// <summary>
    /// Result model for SpSearchItemsMultiVendor stored procedure
    /// Contains aggregated item data with best offer information
    /// </summary>
    public class SpSearchItemsMultiVendor
    {
        /// <summary>
        /// Unique identifier for the item
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Product title in Arabic
        /// </summary>
        public string TitleAr { get; set; }

        /// <summary>
        /// Product title in English
        /// </summary>
        public string TitleEn { get; set; }

        /// <summary>
        /// Short description in Arabic
        /// </summary>
        public string ShortDescriptionAr { get; set; }

        /// <summary>
        /// Short description in English
        /// </summary>
        public string ShortDescriptionEn { get; set; }

        /// <summary>
        /// Category identifier
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Brand identifier
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Product thumbnail image URL
        /// </summary>
        public string ThumbnailImage { get; set; }

        /// <summary>
        /// Item creation date in UTC
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// Minimum price across all vendors
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price across all vendors
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Total number of offers available for this item
        /// </summary>
        public int OffersCount { get; set; }

        /// <summary>
        /// Fastest delivery time in days among all offers
        /// </summary>
        public int FastestDelivery { get; set; }

        /// <summary>
        /// Best offer data (concatenated string with pipe separators)
        /// Format: OfferId|VendorId|SalesPrice|OriginalPrice|AvailableQuantity|IsFreeShipping|EstimatedDeliveryDays
        /// Example: "guid-id|vendor-id|699.99|999.99|15|1|2"
        /// </summary>
        public string BestOfferData { get; set; }
    }
}
