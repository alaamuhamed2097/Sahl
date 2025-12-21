using Shared.DTOs.Base;

namespace Shared.DTOs.Order
{
    /// <summary>
    /// Stage 1: Add to Cart Response
    /// </summary>
    public class AddToCartResponse : BaseDto
    {
        public Guid CartItemId { get; set; }
        public Guid ItemId { get; set; }
        public Guid OfferId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ItemName { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string Message { get; set; } = null!;
    }
}
