namespace DAL.Repositories.Item
{
    /// <summary>
    /// Single item search result with best offer information
    /// </summary>
    public class ItemSearchResultDto
    {
        /// <summary>
        /// Product ID
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
        /// Category ID
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Brand ID (optional)
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Product thumbnail image URL
        /// </summary>
        public string ThumbnailImage { get; set; }

        /// <summary>
        /// Item creation date
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
        /// Total number of vendor offers for this item
        /// </summary>
        public int OffersCount { get; set; }

        /// <summary>
        /// Fastest delivery time in days among all vendors
        /// </summary>
        public int FastestDelivery { get; set; }

        /// <summary>
        /// Best offer details (usually lowest price with fast delivery)
        /// </summary>
        public BestOfferDto BestOffer { get; set; }
    }

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

    /// <summary>
    /// Available filter options for search interface
    /// </summary>
    public class AvailableFiltersDto
    {
        /// <summary>
        /// Available categories with item count
        /// </summary>
        public List<FilterOptionDto> Categories { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Available brands with item count
        /// </summary>
        public List<FilterOptionDto> Brands { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Price range statistics
        /// </summary>
        public PriceRangeDto PriceRange { get; set; }
    }

    /// <summary>
    /// Single filter option (category or brand)
    /// </summary>
    public class FilterOptionDto
    {
        /// <summary>
        /// Category/Brand ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Category/Brand name in Arabic
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// Category/Brand name in English
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Number of items matching this filter
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// Price range statistics
    /// </summary>
    public class PriceRangeDto
    {
        /// <summary>
        /// Minimum price in current results
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price in current results
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Average price in current results (optional)
        /// </summary>
        public decimal? AvgPrice { get; set; }
    }


}
