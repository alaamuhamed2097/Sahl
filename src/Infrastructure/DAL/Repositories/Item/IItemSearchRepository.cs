using Domains.Views.Item;

namespace DAL.Repositories.Item
{
    /// <summary>
    /// Repository for advanced multi-vendor item search operations
    /// Uses stored procedures and views for optimal performance
    /// Supports filtering, sorting, pagination, and price/vendor/stock analysis
    /// </summary>
    public interface IItemSearchRepository
    {
        /// <summary>
        /// Search items using the SpSearchItemsMultiVendor stored procedure
        /// Supports full-text search, multi-filter, and complex sorting
        /// </summary>
        /// <param name="filter">Search filter parameters including pagination</param>
        /// <returns>Paginated list of items with best offers from vendors</returns>
        Task<PagedResult<ItemSearchResultDto>> SearchItemsAsync(ItemSearchFilterDto filter);

        /// <summary>
        /// Get best prices for multiple items from the VwItemBestPrices denormalized view
        /// Optimized for quick catalog price displays
        /// </summary>
        /// <param name="itemIds">List of item IDs to get prices for</param>
        /// <returns>List of best prices per item</returns>
        Task<List<VwItemBestPrice>> GetItemBestPricesAsync(List<Guid> itemIds);

        /// <summary>
        /// Get available filter options based on current search results
        /// Returns counts for categories, brands, and price range
        /// </summary>
        /// <param name="currentFilter">Current search filter to apply</param>
        /// <returns>Available filter options with counts</returns>
        Task<AvailableFiltersDto> GetAvailableFiltersAsync(ItemSearchFilterDto currentFilter);
    }
}
