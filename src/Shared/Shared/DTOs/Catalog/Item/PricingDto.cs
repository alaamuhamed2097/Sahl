namespace Shared.DTOs.Catalog.Item
{
    public class PricingDto
    {
        public int VendorCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public BestPriceOfferDto BestOffer { get; set; } = new BestPriceOfferDto();
    }
}
