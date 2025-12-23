using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Customer.Wishlist
{
    /// <summary>
    /// Request DTO for moving wishlist item to cart
    /// POST /api/wishlist/move-to-cart
    /// </summary>
    public class MoveToCartRequest
    {
        [Required(ErrorMessage = "ItemCombinationId is required")]
        public Guid ItemCombinationId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
