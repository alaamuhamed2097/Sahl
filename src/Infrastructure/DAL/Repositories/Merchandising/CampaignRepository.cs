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
		public async Task<IEnumerable<TbCampaign>> GetActiveCampaignsAsync()
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
		public async Task<TbCampaign> GetCampaignByIdAsync(Guid campaignId)
		{
			try
			{
				//return await _dbContext.TbCampaigns
				//    .AsNoTracking()
				//    .Include(c => c.CampaignItems.Where(ci => !ci.IsDeleted))
				//        .ThenInclude(ci => ci.OfferCombinationPricing.ItemCombination.Item)
				//            .ThenInclude(i => i.ItemImages)
				//    .Include(c => c.CampaignItems.Where(ci => !ci.IsDeleted))
				//        .ThenInclude(ci => ci.OfferCombinationPricing.ItemCombination.Item)
				//            .ThenInclude(i => i.ItemCombinations.Where(ic => ic.IsDefault && !ic.IsDeleted))
				//                .ThenInclude(ic => ic.ItemCombinationImages)
				//    .Include(c => c.HomepageBlocks)
				//    .FirstOrDefaultAsync(c => c.Id == campaignId && !c.IsDeleted)
				//    ?? throw new KeyNotFoundException($"Campaign with id '{campaignId}' was not found.");
								var campaign = await _dbContext.TbCampaigns
					.Include(c => c.CampaignItems.Where(ci => !ci.IsDeleted))
						.ThenInclude(ci => ci.OfferCombinationPricing.ItemCombination.Item)
							.ThenInclude(i => i.ItemImages)
					.Include(c => c.CampaignItems.Where(ci => !ci.IsDeleted))
						.ThenInclude(ci => ci.OfferCombinationPricing.ItemCombination.Item)
							.ThenInclude(i => i.ItemCombinations.Where(ic => ic.IsDefault && !ic.IsDeleted))
								.ThenInclude(ic => ic.ItemCombinationImages)
					.Include(c => c.HomepageBlocks)
					.FirstOrDefaultAsync(c => c.Id == campaignId && !c.IsDeleted)
					?? throw new KeyNotFoundException($"Campaign with id '{campaignId}' was not found.");

				// Detach بعد التحميل
				_dbContext.Entry(campaign).State = EntityState.Detached;
				return campaign;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error getting campaign by ID: {CampaignId}", campaignId);
				throw;
			}
		}

		/// <summary>
		/// Get active flash sales (currently running)
		/// </summary>
		public async Task<IEnumerable<TbCampaign>> GetActiveFlashSalesAsync()
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
					.ToListAsync()
					?? throw new KeyNotFoundException($"Campaign with id Flash Sales was not found.");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error getting active flash sales");
				throw;
			}
		}

		#endregion


	}
}