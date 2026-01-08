using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Merchandising
{
    /// <summary>
    /// Campaign Repository - manages campaigns and their items
    /// </summary>
    public class CampaignRepository : TableRepository<TbCampaign>, ICampaignRepository
    {
        public CampaignRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
        {
        }

        #region Query Methods

        /// <summary>
        /// Get all active campaigns (currently running)
        /// </summary>
        public async Task<List<TbCampaign>> GetActiveCampaignsAsync()
        {
            try
            {
                var now = DateTime.UtcNow;

                return await _dbContext.TbCampaigns
                    .AsNoTracking()
                    .Where(c => !c.IsDeleted &&
                               c.IsActive &&
                               c.StartDate <= now &&
                               c.EndDate >= now)
                    .OrderBy(c => c.StartDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting active campaigns");
                throw;
            }
        }

        /// <summary>
        /// Get campaign by ID with all related data
        /// </summary>
        public async Task<TbCampaign?> GetCampaignByIdAsync(Guid campaignId)
        {
            try
            {
                return await _dbContext.TbCampaigns
                    .AsNoTracking()
                    .Include(c => c.CampaignItems.Where(ci => !ci.IsDeleted))
                        .ThenInclude(ci => ci.Item)
                            .ThenInclude(i => i.ItemImages)
                    .Include(c => c.CampaignItems.Where(ci => !ci.IsDeleted))
                        .ThenInclude(ci => ci.Item)
                            .ThenInclude(i => i.ItemCombinations.Where(ic => ic.IsDefault && !ic.IsDeleted))
                                .ThenInclude(ic => ic.ItemCombinationImages)
                    .Include(c => c.HomepageBlocks)
                    .FirstOrDefaultAsync(c => c.Id == campaignId && !c.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting campaign by ID: {CampaignId}", campaignId);
                throw;
            }
        }

        /// <summary>
        /// Get all items in a campaign
        /// </summary>
        public async Task<List<TbCampaignItem>> GetCampaignItemsAsync(Guid campaignId)
        {
            try
            {
                return await _dbContext.TbCampaignItems
                    .AsNoTracking()
                    .Include(ci => ci.Item)
                        .ThenInclude(i => i.ItemImages)
                    .Include(ci => ci.Item)
                        .ThenInclude(i => i.ItemCombinations.Where(ic => ic.IsDefault && !ic.IsDeleted))
                            .ThenInclude(ic => ic.ItemCombinationImages)
                    .Include(ci => ci.Campaign)
                    .Where(ci => ci.CampaignId == campaignId && !ci.IsDeleted)
                    .OrderBy(ci => ci.DisplayOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting campaign items: {CampaignId}", campaignId);
                throw;
            }
        }

        /// <summary>
        /// Get active flash sales (currently running)
        /// </summary>
        public async Task<List<TbCampaign>> GetActiveFlashSalesAsync()
        {
            try
            {
                var now = DateTime.UtcNow;

                return await _dbContext.TbCampaigns
                    .AsNoTracking()
                    .Where(c => !c.IsDeleted &&
                               c.IsActive &&
                               c.IsFlashSale &&
                               c.StartDate <= now &&
                               c.EndDate >= now &&
                               (!c.FlashSaleEndTime.HasValue || c.FlashSaleEndTime >= now))
                    .OrderBy(c => c.FlashSaleEndTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting active flash sales");
                throw;
            }
        }

        #endregion

        #region Item Management

        /// <summary>
        /// Add item to campaign
        /// </summary>
        public async Task<TbCampaignItem> AddItemToCampaignAsync(TbCampaignItem campaignItem)
        {
            try
            {
                await _dbContext.TbCampaignItems.AddAsync(campaignItem);
                await _dbContext.SaveChangesAsync();

                return campaignItem;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding item to campaign");
                throw;
            }
        }

        /// <summary>
        /// Remove item from campaign (soft delete)
        /// </summary>
        public async Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId)
        {
            try
            {
                var campaignItem = await _dbContext.TbCampaignItems
                    .FirstOrDefaultAsync(ci => ci.Id == campaignItemId && !ci.IsDeleted);

                if (campaignItem == null)
                    return false;

                campaignItem.IsDeleted = true;
                campaignItem.UpdatedDateUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing item from campaign: {CampaignItemId}", campaignItemId);
                return false;
            }
        }

        /// <summary>
        /// Update sold count for campaign item (for flash sales tracking)
        /// </summary>
        public async Task<bool> IncrementSoldCountAsync(Guid campaignItemId, int quantity)
        {
            try
            {
                var campaignItem = await _dbContext.TbCampaignItems
                    .FirstOrDefaultAsync(ci => ci.Id == campaignItemId && !ci.IsDeleted);

                if (campaignItem == null)
                    return false;

                campaignItem.SoldCount += quantity;
                campaignItem.UpdatedDateUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error incrementing sold count: {CampaignItemId}", campaignItemId);
                return false;
            }
        }

        #endregion
    }
}