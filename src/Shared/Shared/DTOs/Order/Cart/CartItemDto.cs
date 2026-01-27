using Shared.DTOs.Catalog.Item;

namespace Shared.DTOs.Order.Cart
{
    /// <summary>
    /// Cart item DTO - UPDATED with VendorId and WarehouseId
    /// </summary>
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public required string ItemNameAr { get; set; } = string.Empty;
        public required string ItemNameEn { get; set; } = string.Empty;
        public Guid OfferCombinationPricingId { get; set; }
		public Guid ItemCombinationId { get; set; }
		public string SellerName { get; set; } = string.Empty;

        public decimal UnitOriginalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
		public List<AttributeFilterDto> Attributes { get; set; } = new();

	}
}