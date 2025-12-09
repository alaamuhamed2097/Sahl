using DAL.ResultModels;
using Domains.Entities.ECommerceSystem.Cart;

namespace DAL.Contracts.Repositories;

/// <summary>
/// Specialized cart repository support
/// Handles complex cart operations with atomicity
/// </summary>
public interface ICartRepository : ITableRepository<TbShoppingCart>
{
    /// <summary>
    /// Add item to cart with full transaction support
    /// </summary>
    Task<CartTransactionResult> AddItemToCartAsync(
        string customerId,
        Guid itemId,
        Guid offerId,
        int quantity,
        decimal unitPrice,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update cart item quantity
    /// </summary>
    Task<CartTransactionResult> UpdateCartItemAsync(
        Guid cartItemId,
        string customerId,
        int newQuantity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove item from cart
    /// </summary>
    Task<CartTransactionResult> RemoveItemFromCartAsync(
        Guid cartItemId,
        string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear entire cart
    /// </summary>
    Task<CartTransactionResult> ClearCartAsync(
        string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get cart with all items and related data in single query
    /// </summary>
    Task<TbShoppingCart> GetCartWithItemsAsync(
        string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Merge carts (useful for guest to logged-in user conversion)
    /// </summary>
    Task<CartTransactionResult> MergeCartsAsync(
        string sourceCustomerId,
        string targetCustomerId,
        CancellationToken cancellationToken = default);
}