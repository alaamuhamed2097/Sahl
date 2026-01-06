namespace Shared.DTOs.Order.Cart
{
    public class BulkAddItemResult
    {
        public Guid OfferCombinationPricingId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}