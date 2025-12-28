namespace Shared.DTOs.Order.Cart
{
    /// <summary>
    /// Complete cart summary
    /// </summary>
    public class CartSummaryDto
    {
        public Guid CartId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingEstimate { get; set; }
        public decimal TaxEstimate { get; set; }
        public decimal TotalEstimate { get; set; }
        public int ItemCount { get; set; }
    }
}