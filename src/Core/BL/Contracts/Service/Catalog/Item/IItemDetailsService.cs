using Shared.DTOs.Catalog.Item;

namespace BL.Contracts.Service.Catalog.Item
{
    /// <summary>
    /// Service interface for item details operations
    /// </summary>
    public interface IItemDetailsService
    {
        /// <summary>
        /// Get item details by combination ID
        /// </summary>
        /// <param name="itemCombinationId">The combination ID</param>
        /// <param name="viewerId">Optional viewer user ID for tracking</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Complete item details</returns>
        /// <exception cref="ArgumentException">If itemCombinationId is empty</exception>
        /// <exception cref="KeyNotFoundException">If item not found</exception>
        Task<ItemDetailsDto> GetItemDetailsAsync(
            Guid itemCombinationId,
            string? viewerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get combination by selected attributes
        /// </summary>
        /// <param name="request">Combination request with selected attribute values</param>
        /// <param name="viewerId">Optional viewer user ID for tracking</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Item details for the matching combination</returns>
        /// <exception cref="ArgumentNullException">If request is null</exception>
        /// <exception cref="ArgumentException">If no attributes selected</exception>
        /// <exception cref="KeyNotFoundException">If combination not found</exception>
        Task<ItemDetailsDto> GetCombinationByAttributesAsync(
            CombinationRequest request,
            string? viewerId,
            CancellationToken cancellationToken = default);
    }
}