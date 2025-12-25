using Common.Enumerations.Merchandising;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Merchandising;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Merchandising
{
    /// <summary>
    /// Homepage Block Repository - manages homepage blocks and their content
    /// </summary>
    public class HomepageBlockRepository : TableRepository<TbHomepageBlock>, IHomepageBlockRepository
    {
        public HomepageBlockRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
        {
        }

        #region Query Methods

        /// <summary>
        /// Get all active visible blocks for homepage
        /// </summary>
        public async Task<List<TbHomepageBlock>> GetActiveBlocksAsync()
        {
            try
            {
                var now = DateTime.UtcNow;

                return await _dbContext.TbHomepageBlocks
                    .AsNoTracking()
                    .Include(b => b.Campaign)
                    .Include(b => b.BlockProducts.Where(p => !p.IsDeleted))
                        .ThenInclude(p => p.Item)
                            .ThenInclude(i => i.ItemImages)
                    .Include(b => b.BlockCategories.Where(c => !c.IsDeleted))
                        .ThenInclude(c => c.Category)
                    .Where(b => b.IsVisible && !b.IsDeleted)
                    .Where(b => (b.VisibleFrom == null || b.VisibleFrom <= now) &&
                               (b.VisibleTo == null || b.VisibleTo >= now))
                    .OrderBy(b => b.DisplayOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting active homepage blocks");
                throw;
            }
        }

        /// <summary>
        /// Get single block with all related data
        /// </summary>
        public async Task<TbHomepageBlock?> GetBlockByIdAsync(Guid blockId)
        {
            try
            {
                return await _dbContext.TbHomepageBlocks
                    .AsNoTracking()
                    .Include(b => b.Campaign)
                    .Include(b => b.BlockProducts.Where(p => !p.IsDeleted))
                        .ThenInclude(p => p.Item)
                            .ThenInclude(i => i.ItemImages)
                    .Include(b => b.BlockCategories.Where(c => !c.IsDeleted))
                        .ThenInclude(c => c.Category)
                    .FirstOrDefaultAsync(b => b.Id == blockId && !b.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting block by ID: {BlockId}", blockId);
                throw;
            }
        }

        /// <summary>
        /// Get blocks by type (e.g., all Campaign blocks)
        /// </summary>
        public async Task<List<TbHomepageBlock>> GetBlocksByTypeAsync(HomepageBlockType type)
        {
            try
            {
                return await _dbContext.TbHomepageBlocks
                    .AsNoTracking()
                    .Where(b => b.Type == type && !b.IsDeleted)
                    .OrderBy(b => b.DisplayOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting blocks by type: {Type}", type);
                throw;
            }
        }

        /// <summary>
        /// Get all blocks for a specific campaign
        /// </summary>
        public async Task<List<TbHomepageBlock>> GetBlocksByCampaignAsync(Guid campaignId)
        {
            try
            {
                return await _dbContext.TbHomepageBlocks
                    .AsNoTracking()
                    .Where(b => b.CampaignId == campaignId && !b.IsDeleted)
                    .OrderBy(b => b.DisplayOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting blocks by campaign: {CampaignId}", campaignId);
                throw;
            }
        }

        #endregion

        #region Product Management

        /// <summary>
        /// Add product to a manual block
        /// </summary>
        public async Task<TbBlockItem> AddProductToBlockAsync(TbBlockItem blockProduct)
        {
            try
            {
                await _dbContext.TbBlockItems.AddAsync(blockProduct);
                await _dbContext.SaveChangesAsync();

                return blockProduct;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding product to block");
                throw;
            }
        }

        /// <summary>
        /// Remove product from block (soft delete)
        /// </summary>
        public async Task<bool> RemoveProductFromBlockAsync(Guid blockProductId)
        {
            try
            {
                var blockProduct = await _dbContext.TbBlockItems
                    .FirstOrDefaultAsync(bp => bp.Id == blockProductId && !bp.IsDeleted);

                if (blockProduct == null)
                    return false;

                blockProduct.IsDeleted = true;
                blockProduct.UpdatedDateUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing product from block: {BlockProductId}", blockProductId);
                return false;
            }
        }

        /// <summary>
        /// Get all products in a block
        /// </summary>
        public async Task<List<TbBlockItem>> GetBlockProductsAsync(Guid blockId)
        {
            try
            {
                return await _dbContext.TbBlockItems
                    .AsNoTracking()
                    .Include(bp => bp.Item)
                        .ThenInclude(i => i.ItemImages)
                    .Where(bp => bp.HomepageBlockId == blockId && !bp.IsDeleted)
                    .OrderBy(bp => bp.DisplayOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting block products: {BlockId}", blockId);
                throw;
            }
        }

        /// <summary>
        /// Update display order for products in block
        /// </summary>
        public async Task<bool> ReorderProductsAsync(List<(Guid ProductId, int Order)> productOrders)
        {
            try
            {
                foreach (var (productId, order) in productOrders)
                {
                    var product = await _dbContext.TbBlockItems
                        .FirstOrDefaultAsync(bp => bp.Id == productId);

                    if (product != null)
                    {
                        product.DisplayOrder = order;
                        product.UpdatedDateUtc = DateTime.UtcNow;
                    }
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error reordering products");
                return false;
            }
        }

        #endregion

        #region Category Management

        /// <summary>
        /// Add category to a category showcase block
        /// </summary>
        public async Task<TbBlockCategory> AddCategoryToBlockAsync(TbBlockCategory blockCategory)
        {
            try
            {
                await _dbContext.TbBlockCategories.AddAsync(blockCategory);
                await _dbContext.SaveChangesAsync();

                return blockCategory;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding category to block");
                throw;
            }
        }

        /// <summary>
        /// Remove category from block (soft delete)
        /// </summary>
        public async Task<bool> RemoveCategoryFromBlockAsync(Guid blockCategoryId)
        {
            try
            {
                var blockCategory = await _dbContext.TbBlockCategories
                    .FirstOrDefaultAsync(bc => bc.Id == blockCategoryId && !bc.IsDeleted);

                if (blockCategory == null)
                    return false;

                blockCategory.IsDeleted = true;
                blockCategory.UpdatedDateUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing category from block: {BlockCategoryId}", blockCategoryId);
                return false;
            }
        }

        /// <summary>
        /// Get all categories in a block
        /// </summary>
        public async Task<List<TbBlockCategory>> GetBlockCategoriesAsync(Guid blockId)
        {
            try
            {
                return await _dbContext.TbBlockCategories
                    .AsNoTracking()
                    .Include(bc => bc.Category)
                    .Where(bc => bc.HomepageBlockId == blockId && !bc.IsDeleted)
                    .OrderBy(bc => bc.DisplayOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting block categories: {BlockId}", blockId);
                throw;
            }
        }

        #endregion

        #region Block Management

        /// <summary>
        /// Toggle block visibility
        /// </summary>
        public async Task<bool> ToggleBlockVisibilityAsync(Guid blockId, bool isVisible)
        {
            try
            {
                var block = await _dbContext.TbHomepageBlocks
                    .FirstOrDefaultAsync(b => b.Id == blockId && !b.IsDeleted);

                if (block == null)
                    return false;

                block.IsVisible = isVisible;
                block.UpdatedDateUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error toggling block visibility: {BlockId}", blockId);
                return false;
            }
        }

        /// <summary>
        /// Update display order for multiple blocks
        /// </summary>
        public async Task<bool> ReorderBlocksAsync(List<(Guid BlockId, int Order)> blockOrders)
        {
            try
            {
                foreach (var (blockId, order) in blockOrders)
                {
                    var block = await _dbContext.TbHomepageBlocks
                        .FirstOrDefaultAsync(b => b.Id == blockId);

                    if (block != null)
                    {
                        block.DisplayOrder = order;
                        block.UpdatedDateUtc = DateTime.UtcNow;
                    }
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error reordering blocks");
                return false;
            }
        }

        #endregion
    }
}