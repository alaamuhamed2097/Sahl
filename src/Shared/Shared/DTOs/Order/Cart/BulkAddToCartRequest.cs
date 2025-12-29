namespace Shared.DTOs.Order.Cart
{
    public class BulkAddToCartRequest
    {
        public List<AddToCartRequest> Items { get; set; } = new();
    }
}