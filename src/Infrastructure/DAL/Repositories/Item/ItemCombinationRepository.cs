using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Item
{
    /// <summary>
    /// Item Combination Repository - handles product combinations and queries
    /// </summary>
    public class ItemCombinationRepository : TableRepository<TbItemCombination>, IItemCombinationRepository
    {
        public ItemCombinationRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
        {
        }

        #region Combination Queries

        /// <summary>
        /// Get default combinations for multiple items
        /// </summary>
        public async Task<List<TbItemCombination>> GetDefaultCombinationsAsync(List<Guid> itemIds)
        {
            try
            {
                if (!itemIds.Any())
                    return new List<TbItemCombination>();

                return await _dbContext.TbItemCombinations
                    .AsNoTracking()
                    .Include(c => c.Item)
                        .ThenInclude(i => i.ItemImages)
                    .Include(c => c.ItemCombinationImages)
                    .Where(c => itemIds.Contains(c.ItemId) &&
                               c.IsDefault &&
                               !c.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting default combinations");
                throw;
            }
        }

        #endregion

        #region Dynamic Block Queries

        /// <summary>
        /// Get best sellers item IDs
        /// </summary>
        public async Task<List<Guid>> GetBestSellersAsync(int limit)
        {
            try
            {
                return await _dbContext.TbOrderDetails
                    .Where(od => !od.IsDeleted)
                    .GroupBy(od => od.ItemId)
                    .Select(g => new { ItemId = g.Key, TotalSold = g.Sum(od => od.Quantity) })
                    .OrderByDescending(x => x.TotalSold)
                    .Take(limit)
                    .Select(x => x.ItemId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting best sellers");
                throw;
            }
        }

        /// <summary>
        /// Get new arrivals item IDs
        /// </summary>
        public async Task<List<Guid>> GetNewArrivalsAsync(int limit)
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted)
                    .OrderByDescending(i => i.CreatedDateUtc)
                    .Take(limit)
                    .Select(i => i.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting new arrivals");
                throw;
            }
        }

        /// <summary>
        /// Get top rated item IDs
        /// </summary>
        public async Task<List<Guid>> GetTopRatedAsync(int limit)
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive &&
                               !i.IsDeleted &&
                               i.AverageRating.HasValue &&
                               i.AverageRating >= 4.0m)
                    .OrderByDescending(i => i.AverageRating)
                    .Take(limit)
                    .Select(i => i.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting top rated");
                throw;
            }
        }

        /// <summary>
        /// Get trending item IDs (most viewed recently)
        /// TODO: Implement proper view tracking
        /// For now, returns recent items
        /// </summary>
        public async Task<List<Guid>> GetTrendingAsync(int limit)
        {
            try
            {
                // Temporary implementation - return recent items
                // In production, this should query a views tracking table
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted)
                    .OrderByDescending(i => i.CreatedDateUtc)
                    .Take(limit)
                    .Select(i => i.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trending");
                throw;
            }
        }

        /// <summary>
        /// Get most wishlisted item IDs
        /// </summary>
        public async Task<List<Guid>> GetMostWishlistedAsync(int limit)
        {
            try
            {
                return await _dbContext.TbWishlistItems
                    .Where(w => !w.IsDeleted)
                    .GroupBy(w => w.ItemCombination.ItemId)
                    .Select(g => new { ItemId = g.Key, WishlistCount = g.Count() })
                    .OrderByDescending(x => x.WishlistCount)
                    .Take(limit)
                    .Select(x => x.ItemId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting most wishlisted");
                throw;
            }
        }

        #endregion
    }
}