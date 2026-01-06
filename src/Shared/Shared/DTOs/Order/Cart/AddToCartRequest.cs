namespace Shared.DTOs.Order.Cart
{
    /// <summary>
    /// Request to add item to cart
    /// </summary>
    public class AddToCartRequest
    {
        public Guid OfferCombinationPricingId { get; set; }
        public int Quantity { get; set; }
    }
}