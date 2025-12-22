using Common.Enumerations.Offer;

namespace Shared.DTOs.ECommerce.Item
{
    public class BestPriceOfferDto
    {
        public Guid OfferId { get; set; }
        public Guid VendorId { get; set; }
        public string? VendorName { get; set; }
        public decimal? VendorRating { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int AvailableQuantity { get; set; }
        public StockStatus StockStatus { get; set; }
        public bool IsFreeShipping { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public bool IsBuyBoxWinner { get; set; }
        public int MinOrderQuantity { get; set; }
        public int MaxOrderQuantity { get; set; }
        public List<QuantityTierDto>? QuantityTiers { get; set; }
    }
}
