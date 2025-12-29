namespace Shared.DTOs.Order.Cart
{
    public class BulkAddItemFailure
    {
        public Guid ItemId { get; set; }
        public Guid OfferCombinationPricingId { get; set; }
        public int Quantity { get; set; }
        public string ErrorMessage { get; set; }
    }
}