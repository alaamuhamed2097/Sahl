namespace Shared.DTOs.Order.Cart
{
    /// <summary>
    /// Cart item DTO - UPDATED with VendorId and WarehouseId
    /// </summary>
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public Guid OfferCombinationPricingId { get; set; }
        public string SellerName { get; set; } = string.Empty;

        // ✅ ADDED: Required for checkout shipment grouping
       // public Guid VendorId { get; set; }
        //public Guid? WarehouseId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
    }
}