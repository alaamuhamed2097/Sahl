namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Individual item in checkout
    /// </summary>
    public class CheckoutItemDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsAvailable { get; set; }
    }
}