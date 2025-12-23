namespace Shared.DTOs.Customer.Wishlist
{
    /// <summary>
    /// DTO for wishlist item with full product details
    /// </summary>
    public class WishlistItemDto
    {
        public Guid WishlistItemId { get; set; }
        public Guid WishlistId { get; set; }
        public Guid ItemCombinationId { get; set; }
        public DateTime DateAdded { get; set; }

        // Product Information
        public Guid ItemId { get; set; }
        public string ItemTitleAr { get; set; } = null!;
        public string ItemTitleEn { get; set; } = null!;
        public string ItemShortDescriptionAr { get; set; } = null!;
        public string ItemShortDescriptionEn { get; set; } = null!;
        public string ThumbnailImage { get; set; } = null!;

        // Pricing Information (from BuyBox offer)
        public Guid? OfferPricingId { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
