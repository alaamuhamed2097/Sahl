using Common.Models.Filters;
using DAL.Models.ItemSearch;
using Domains.Views.Item;

namespace DAL.Contracts.Repositories;

/// <summary>
/// Repository for advanced multi-vendor item search operations
/// Optimized for large-scale e-commerce search with stored procedures
/// Returns domain entities, not DTOs. DTOs are the responsibility of the BL layer.
/// </summary>
public interface IItemSearchRepository
{
    /// <summary>
    /// Execute stored procedure search with comprehensive filtering and scoring
    /// Returns domain entities that represent search results
    /// </summary>
    Task<(List<SpSearchItemsMultiVendor> Items, int TotalCount)> SearchItemsAsync(
        ItemFilterDto filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get available filter options based on current search criteria
    /// Returns entities representing available filters
    /// </summary>
    Task<AvailableFiltersData> GetAvailableFiltersAsync(
        ItemFilterDto currentFilter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get best prices for multiple items from VwItemBestPrices view
    /// Returns view entities with price information
    /// </summary>
    Task<List<VwItemBestPrice>> GetItemBestPricesAsync(
        List<Guid> itemIds,
        CancellationToken cancellationToken = default);
}