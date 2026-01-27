namespace Shared.DTOs.Catalog.Pricing
{
    /// <summary>
    /// Result of pricing calculation
    /// </summary>
    public class PricingResult
    {
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? ActiveOfferId { get; set; }
    }
}
