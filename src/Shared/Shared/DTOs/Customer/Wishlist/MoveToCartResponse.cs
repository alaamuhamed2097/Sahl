namespace Shared.DTOs.Customer.Wishlist
{
    /// <summary>
    /// Response DTO after moving item to cart
    /// </summary>
    public class MoveToCartResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public bool AddedToCart { get; set; }
        public bool RemovedFromWishlist { get; set; }
        public string? ErrorReason { get; set; }
    }
}
