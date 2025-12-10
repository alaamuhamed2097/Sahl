using Shared.DTOs.ECommerce.Item;

namespace BL.Contracts.Service.ECommerce.Item
{
    /// <summary>
    /// Service for executing SpSearchItemsMultiVendor stored procedure
    /// Optimized for high-performance multi-vendor search operations
    /// </summary>

    public interface IItemSearchService
    {
        /// <summary>
        /// Execute stored procedure search with advanced filtering
        /// </summary>
        /// <param name="filter">Search filter parameters</param>
        /// <returns>Paginated search results</returns>
        Task<PagedSpSearchResultDto> SearchItemsAsync(ItemFilterDto filter);

        /// <summary>
        /// Get dynamic filter options based on current search criteria
        /// </summary>
        /// <param name="filter">Current filter to apply</param>
        /// <returns>Available filter options with counts</returns>
        Task<AvailableSearchFiltersDto> GetAvailableFiltersAsync(ItemFilterDto filter);

        /// <summary>
        /// Get best prices for multiple items
        /// </summary>
        /// <param name="itemIds">List of item IDs</param>
        /// <returns>List of best prices</returns>
        Task<List<object>> GetItemBestPricesAsync(List<Guid> itemIds);
    }

}
