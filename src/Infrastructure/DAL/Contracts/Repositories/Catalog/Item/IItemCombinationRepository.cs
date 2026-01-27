using Domains.Entities.Catalog.Item.ItemAttributes;

namespace DAL.Contracts.Repositories.Catalog.Item
{
    /// <summary>
    /// Item Combination Repository Interface
    /// (Add these methods to your existing interface)
    /// </summary>
    public interface IItemCombinationRepository : ITableRepository<TbItemCombination>
    {
        Task<List<TbItemCombination>> GetDefaultCombinationsAsync(List<Guid> itemIds);

        // Dynamic queries with offset
        Task<List<Guid>> GetBestSellersAsync(int limit, int offset = 0);
        Task<List<Guid>> GetNewArrivalsAsync(int limit, int offset = 0);
        Task<List<Guid>> GetTopRatedAsync(int limit, int offset = 0);
        Task<List<Guid>> GetTrendingAsync(int limit, int offset = 0);
        Task<List<Guid>> GetMostWishlistedAsync(int limit, int offset = 0);

        // Count methods
        Task<int> GetBestSellersCountAsync();
        Task<int> GetMostWishlistedCountAsync();

        Task<int> GetNewArrivalsCountAsync();
        Task<int> GetTopRatedCountAsync();
        Task<int> GetTrendingCountAsync();
    }
}