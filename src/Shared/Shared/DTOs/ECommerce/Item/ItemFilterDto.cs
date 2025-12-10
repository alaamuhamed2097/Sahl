using Common.Enumerations.Offer;

namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Advanced filter model for customer website item search
    /// Supports filtering by price, rating, availability, vendor info, and more
    /// </summary>
    public class ItemFilterDto
    {
        // === Item-level Filters ===
        public string SearchTerm { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public List<Guid> BrandIds { get; set; }

        // === Price Filters (across all offers) ===
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // === Rating Filters ===
        public decimal? MinItemRating { get; set; }
        public decimal? MinVendorRating { get; set; }

        // === Availability Filters ===
        public bool? InStockOnly { get; set; }
        public int? MinAvailableQuantity { get; set; }

        // === Shipping Filters ===
        public bool? FreeShippingOnly { get; set; }
        public int? MaxDeliveryDays { get; set; }
        public List<StorgeLocation> StorageLocations { get; set; }

        // === Vendor Filters ===
        public List<string> VendorIds { get; set; }
        public bool? VerifiedVendorsOnly { get; set; }
        public bool? PrimeVendorsOnly { get; set; }

        // === Offer Filters ===
        public bool? OnSaleOnly { get; set; }
        public bool? BuyBoxWinnersOnly { get; set; }

        // === Condition and Warranty Filters ===
        public List<Guid> ConditionIds { get; set; }
        public bool? WithWarrantyOnly { get; set; }

        // === Attribute Filters (Color, Size, etc.) ===
        public Dictionary<Guid, List<Guid>> AttributeValues { get; set; }

        // === Sorting ===
        public string SortBy { get; set; }
        // Options: "price_asc", "price_desc", "rating", "vendor_rating", "fastest_delivery", "most_sold", "newest"

        // === Pagination ===
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        // === Display Mode ===
        public bool ShowAllOffers { get; set; } = false;
    }
}
