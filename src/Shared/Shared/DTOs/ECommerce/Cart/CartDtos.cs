namespace Shared.DTOs.ECommerce.Cart
{
    public class AddToCartRequest
    {
        public Guid ItemId { get; set; }
        public Guid? ItemCombinationId { get; set; }
        public Guid OfferId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public Guid? ItemCombinationId { get; set; }
        public string? CombinationName { get; set; }
        public Guid OfferId { get; set; }
        public string SellerName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsAvailable { get; set; }
    }

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

    public class RemoveFromCartRequest
    {
        public Guid CartItemId { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
