namespace Shared.DTOs.Catalog.Pricing
{
    /// <summary>
    /// Result of pricing calculation
    /// </summary>
    public class PricingResult
    {
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? ActiveOfferId { get; set; }
    }
}
