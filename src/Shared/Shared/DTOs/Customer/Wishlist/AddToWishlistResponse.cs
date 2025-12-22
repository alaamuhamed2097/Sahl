namespace Shared.DTOs.Customer.Wishlist
{
    /// <summary>
    /// Response DTO after adding item to wishlist
    /// </summary>
    public class AddToWishlistResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public Guid? WishlistItemId { get; set; }
        public bool WasAlreadyInWishlist { get; set; }
    }
}
