using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Customer.Wishlist
{
    /// <summary>
    /// Request DTO for adding item to wishlist
    /// POST /api/wishlist/add
    /// </summary>
    public class AddToWishlistRequest
    {
        [Required(ErrorMessage = "ItemCombinationId is required")]
        public Guid ItemCombinationId { get; set; }
    }
}
