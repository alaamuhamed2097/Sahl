using DAL.Models;
using Domains.Entities.Customer;

namespace DAL.Contracts.Repositories.Customer
{
    /// <summary>
    /// Repository interface for Wishlist operations
    /// All operations are customer-scoped for security
    /// </summary>
    public interface IWishlistRepository
    {
        /// <summary>
        /// Get or create wishlist for customer
        /// Ensures one wishlist per customer
        /// </summary>
        Task<TbWishlist> GetOrCreateWishlistAsync(string customerId, Guid createdBy);

        /// <summary>
        /// Get customer's wishlist
        /// Returns null if not found
        /// </summary>
        Task<TbWishlist?> GetWishlistByCustomerIdAsync(string customerId);

        /// <summary>
        /// Get wishlist item by combination ID for specific customer
        /// Returns null if not found or doesn't belong to customer
        /// </summary>
        Task<TbWishlistItem?> GetWishlistItemAsync(string customerId, Guid itemCombinationId);

        /// <summary>
        /// Get paginated wishlist items for customer
        /// </summary>
        Task<AdvancedPagedResult<TbWishlistItem>> GetWishlistItemsPagedAsync(
            string customerId,
            int page,
            int pageSize);

        /// <summary>
        /// Add item to customer's wishlist
        /// Creates wishlist if doesn't exist
        /// Returns null if item already exists
        /// </summary>
        Task<TbWishlistItem?> AddToWishlistAsync(
            string customerId,
            Guid itemCombinationId,
            Guid createdBy);

        /// <summary>
        /// Remove item from customer's wishlist
        /// Returns false if item not found or doesn't belong to customer
        /// </summary>
        Task<bool> RemoveFromWishlistAsync(
            string customerId,
            Guid itemCombinationId,
            Guid updatedBy);

        /// <summary>
        /// Clear all items from customer's wishlist
        /// Returns number of items removed
        /// </summary>
        Task<int> ClearWishlistAsync(string customerId, Guid updatedBy);

        /// <summary>
        /// Get count of items in customer's wishlist
        /// </summary>
        Task<int> GetWishlistCountAsync(string customerId);

        /// <summary>
        /// Check if item combination is in customer's wishlist
        /// </summary>
        Task<bool> IsInWishlistAsync(string customerId, Guid itemCombinationId);

        /// <summary>
        /// Check if customer owns a specific wishlist item
        /// Used for authorization checks
        /// </summary>
        Task<bool> ValidateWishlistItemOwnershipAsync(
            string customerId,
            Guid wishlistItemId);
    }
}