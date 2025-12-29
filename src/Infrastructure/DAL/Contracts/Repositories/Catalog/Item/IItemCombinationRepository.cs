using Domains.Entities.Catalog.Item.ItemAttributes;

namespace DAL.Contracts.Repositories.Catalog.Item
{
    /// <summary>
    /// Item Combination Repository Interface
    /// (Add these methods to your existing interface)
    /// </summary>
    public interface IItemCombinationRepository : ITableRepository<TbItemCombination>
    {
        /// <summary>
        /// Get default combinations for multiple items
        /// </summary>
        Task<List<TbItemCombination>> GetDefaultCombinationsAsync(List<Guid> itemIds);

        /// <summary>
        /// Get best sellers item IDs
        /// </summary>
        Task<List<Guid>> GetBestSellersAsync(int limit);

        /// <summary>
        /// Get new arrivals item IDs
        /// </summary>
        Task<List<Guid>> GetNewArrivalsAsync(int limit);

        /// <summary>
        /// Get top rated item IDs
        /// </summary>
        Task<List<Guid>> GetTopRatedAsync(int limit);

        /// <summary>
        /// Get trending item IDs (most viewed)
        /// </summary>
        Task<List<Guid>> GetTrendingAsync(int limit);

        /// <summary>
        /// Get most wishlisted item IDs
        /// </summary>
        Task<List<Guid>> GetMostWishlistedAsync(int limit);
    }
}