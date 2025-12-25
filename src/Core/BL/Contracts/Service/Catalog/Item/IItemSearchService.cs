using Common.Filters;
using Shared.DTOs.Catalog.Item;

namespace BL.Contracts.Service.Catalog.Item
{
    /// <summary>
    /// Service for advanced multi-vendor item search operations
    /// </summary>
    public interface IItemSearchService
    {
        /// <summary>
        /// Search items with comprehensive filtering and scoring
        /// </summary>
        Task<PagedSpSearchResultDto> SearchItemsAsync(
            ItemFilterQuery filter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get available filter options based on current search
        /// </summary>
        Task<AvailableSearchFiltersDto> GetAvailableFiltersAsync(
            AvailableFiltersQuery filtersQuery);

        /// <summary>
        /// Get best prices for multiple items
        /// </summary>
        Task<List<ItemBestPriceDto>> GetItemBestPricesAsync(
            List<Guid> itemIds,
            CancellationToken cancellationToken = default);
    }
}
