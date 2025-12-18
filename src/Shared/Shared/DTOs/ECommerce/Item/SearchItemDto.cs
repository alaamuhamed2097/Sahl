using Common.Enumerations.Fulfillment;

namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Result DTO for SpSearchItemsMultiVendor stored procedure
    /// Optimized for high-performance multi-vendor search
    /// </summary>
    public class SearchItemDto
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
        /// Brand identifier (nullable)
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
        /// Original price (before discount)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Current sales price (the price customer pays)
        /// </summary>
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// Best offer details (aggregated from stored procedure)
        /// Usually the lowest price with fastest delivery
        /// </summary>
        public BestOfferDetailsDto BestOffer { get; set; }

        /// <summary>
        /// Badges for highlighting product characteristics (e.g., "New", "Best Seller")
        /// </summary>
        public List<string> Badges { get; set; } = new List<string>();

        /// <summary>
        /// Whether the item was created within the last 30 days
        /// </summary>
        public bool IsNew => CreatedDateUtc >= DateTime.UtcNow.AddDays(-30);
    }

    /// <summary>
    /// Best offer details extracted from stored procedure result
    /// Represents the most attractive offer (usually lowest price + fastest delivery)
    /// </summary>
    public class BestOfferDetailsDto
    {
        /// <summary>
        /// Offer unique identifier
        /// </summary>
        public Guid OfferId { get; set; }

        /// <summary>
        /// Vendor unique identifier
        /// </summary>
        public Guid VendorId { get; set; }

        /// <summary>
        /// Vendor name (populated from join if needed)
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Current selling price (sales price)
        /// </summary>
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// Original price before discount
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Calculated discount percentage
        /// Only populated if OriginalPrice > SalesPrice
        /// </summary>
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Available quantity for immediate purchase
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

        /// <summary>
        /// Indicates if this is the Buy Box winner
        /// (best overall offer selected by algorithm)
        /// </summary>
        public bool IsBuyBoxWinner { get; set; }

        /// <summary>
        /// Fulfillment type (seller fulfilled or marketplace fulfilled)
        /// </summary>
        public FulfillmentType FulfillmentType { get; set; }
    }

    /// <summary>
    /// Paginated result wrapper for stored procedure search results
    /// </summary>
    public class PagedSpSearchResultDto
    {
        /// <summary>
        /// List of items on current page
        /// </summary>
        public List<SearchItemDto> Items { get; set; } = new List<SearchItemDto>();

        /// <summary>
        /// Total number of items matching search criteria
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Whether there's a previous page
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Whether there's a next page
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;
    }

    /// <summary>
    /// Best price information for an item across all vendors
    /// </summary>
    public class ItemBestPriceDto
    {
        /// <summary>
        /// Item unique identifier
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Best price available for this item
        /// </summary>
        public decimal BestPrice { get; set; }

        /// <summary>
        /// Total available stock across all vendors
        /// </summary>
        public int TotalStock { get; set; }

        /// <summary>
        /// Total number of offers for this item
        /// </summary>
        public int TotalOffers { get; set; }

        /// <summary>
        /// Buy Box winner ratio (0-1)
        /// </summary>
        public double BuyBoxRatio { get; set; }
    }
}
