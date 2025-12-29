namespace Shared.DTOs.Order.Cart
{
    public class BulkAddItemResult
    {
        public Guid ItemId { get; set; }
        public Guid OfferCombinationPricingId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}