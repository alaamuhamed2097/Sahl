using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Catalog.Item
{
    public class ItemCombinationRepository : TableRepository<TbItemCombination>, IItemCombinationRepository
    {
        public ItemCombinationRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
        {
        }

        #region Combination Queries

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
                    .Include(c => c.Item.Brand)
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

        #region Dynamic Block Queries with Offset

        public async Task<List<Guid>> GetBestSellersAsync(int limit, int offset = 0)
        {
            try
            {
                return await _dbContext.TbOrderDetails
                    .Where(od => !od.IsDeleted)
                    .GroupBy(od => od.ItemId)
                    .Select(g => new { ItemId = g.Key, TotalSold = g.Sum(od => od.Quantity) })
                    .OrderByDescending(x => x.TotalSold)
                    .Skip(offset)
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

        public async Task<List<Guid>> GetNewArrivalsAsync(int limit, int offset = 0)
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted)
                    .OrderByDescending(i => i.CreatedDateUtc)
                    .Skip(offset)
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

        public async Task<List<Guid>> GetTopRatedAsync(int limit, int offset = 0)
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive &&
                               !i.IsDeleted &&
                               i.AverageRating.HasValue &&
                               i.AverageRating >= 4.0m)
                    .OrderByDescending(i => i.AverageRating)
                    .Skip(offset)
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

        public async Task<List<Guid>> GetTrendingAsync(int limit, int offset = 0)
        {
            try
            {
                // Use recent items as trending (can be enhanced with view tracking later)
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted)
                    .OrderByDescending(i => i.CreatedDateUtc)
                    .Skip(offset)
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

        public async Task<List<Guid>> GetMostWishlistedAsync(int limit, int offset = 0)
        {
            try
            {
                return await _dbContext.TbWishlistItems
                    .Where(w => !w.IsDeleted)
                    .GroupBy(w => w.ItemCombination.ItemId)
                    .Select(g => new { ItemId = g.Key, WishlistCount = g.Count() })
                    .OrderByDescending(x => x.WishlistCount)
                    .Skip(offset)
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

        #region Count Methods

        public async Task<int> GetBestSellersCountAsync()
        {
            try
            {
                return await _dbContext.TbOrderDetails
                    .Where(od => !od.IsDeleted)
                    .Select(od => od.ItemId)
                    .Distinct()
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting best sellers count");
                throw;
            }
        }

        public async Task<int> GetNewArrivalsCountAsync()
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting new arrivals count");
                throw;
            }
        }

        public async Task<int> GetTopRatedCountAsync()
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted &&
                               i.AverageRating.HasValue && i.AverageRating >= 4.0m)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting top rated count");
                throw;
            }
        }

        public async Task<int> GetTrendingCountAsync()
        {
            try
            {
                return await _dbContext.TbItems
                    .Where(i => i.IsActive && !i.IsDeleted)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trending count");
                throw;
            }
        }

        public async Task<int> GetMostWishlistedCountAsync()
        {
            try
            {
                return await _dbContext.TbWishlistItems
                    .Where(w => !w.IsDeleted)
                    .Select(w => w.ItemCombination.ItemId)
                    .Distinct()
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting most wishlisted count");
                throw;
            }
        }

        #endregion
    }
}