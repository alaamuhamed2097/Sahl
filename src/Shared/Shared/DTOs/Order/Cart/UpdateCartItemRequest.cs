namespace Shared.DTOs.Order.Cart
{
    /// <summary>
    /// Request to update cart item
    /// </summary>
    public class UpdateCartItemRequest
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}