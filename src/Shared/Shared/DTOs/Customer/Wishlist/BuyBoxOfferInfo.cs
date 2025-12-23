namespace Shared.DTOs.Customer.Wishlist
{
    /// <summary>
    /// Internal DTO for BuyBox offer pricing lookup
    /// </summary>
    public class BuyBoxOfferInfo
    {
        public Guid OfferPricingId { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
