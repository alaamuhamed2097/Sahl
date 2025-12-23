using DAL.Models;
using Shared.DTOs.Customer.Wishlist;

namespace BL.Contracts.Service.Customer.Wishlist
{
    /// <summary>
    /// Service interface for managing customer wishlists
    /// Based on requirements specification
    /// </summary>
    public interface IWishlistService
    {
        /// <summary>
        /// Add item to wishlist
        /// POST /api/wishlist/add
        /// - Validates customer
        /// - Checks product exists
        /// - Checks product not already added
        /// - Inserts into database
        /// </summary>
        Task<AddToWishlistResponse> AddToWishlistAsync(string customerId, AddToWishlistRequest request);

        /// <summary>
        /// Remove item from wishlist
        /// DELETE /api/wishlist/remove/{itemCombinationId}
        /// - Removes item from wishlist
        /// - Handles item not found
        /// </summary>
        Task<bool> RemoveFromWishlistAsync(string customerId, Guid itemCombinationId);

        /// <summary>
        /// Get wishlist items (paginated)
        /// GET /api/wishlist?customerId=xxx&page=1&pageSize=10
        /// Returns products with:
        /// - Product name
        /// - Images
        /// - Price
        /// - Stock status
        /// - Offer availability
        /// </summary>
        Task<AdvancedPagedResult<WishlistItemDto>> GetWishlistItemsAsync(
            string customerId,
            int page = 1,
            int pageSize = 10);

        /// <summary>
        /// Clear wishlist
        /// DELETE /api/wishlist/clear
        /// - Removes all customer items
        /// </summary>
        Task<bool> ClearWishlistAsync(string customerId);

        /// <summary>
        /// Move wishlist item to cart
        /// POST /api/wishlist/move-to-cart
        /// - Validates stock
        /// - Adds to cart
        /// - Removes from wishlist
        /// Integration needed with Cart Service
        /// </summary>
        Task<MoveToCartResponse> MoveToCartAsync(string customerId, MoveToCartRequest request);

        /// <summary>
        /// Check wishlist count
        /// GET /api/wishlist/count
        /// - Shows number of items in wishlist
        /// </summary>
        Task<int> GetWishlistCountAsync(string customerId);
    }
}