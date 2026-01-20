using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Merchandising
{
	public class CampaignItemRepository : TableRepository<TbCampaignItem>, ICampaignItemRepository
	{
		public CampaignItemRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
			: base(dbContext, currentUserService, logger)
		{

		}

		#region Item Management

		/// <summary>
		/// Get all items in a campaign
		/// </summary>
		public async Task<IEnumerable<TbCampaignItem>> GetCampaignItemsAsync(Guid campaignId)
		{
			try
			{
				return await _dbContext.TbCampaignItems
					.AsNoTracking()
					.Include(ci => ci.OfferCombinationPricing)
						.ThenInclude(ocp => ocp.ItemCombination)
							.ThenInclude(ic => ic.Item)
								.ThenInclude(i => i.ItemImages)
					.Include(ci => ci.Campaign)
					.Where(ci => ci.CampaignId == campaignId
						&& !ci.IsDeleted
						&& ci.Campaign.IsActive)
					.OrderBy(ci => ci.CreatedDateUtc)
					.ToListAsync()
					?? throw new KeyNotFoundException($"Campaign with id '{campaignId}' was not found.");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error getting campaign items: {CampaignId}", campaignId);
				throw;
			}
		}

		/// <summary>
		/// Add item to campaign
		/// </summary>
		public async Task<TbCampaignItem> AddItemToCampaignAsync(TbCampaignItem campaignItem)
		{
			try
			{
				await _dbContext.TbCampaignItems.AddAsync(campaignItem);
				await _dbContext.SaveChangesAsync();

				return await _dbContext.TbCampaignItems
							.Include(ci => ci.OfferCombinationPricing)
								.ThenInclude(ocp => ocp.ItemCombination)
									.ThenInclude(ic => ic.Item)
										.ThenInclude(i => i.ItemImages)
							.FirstOrDefaultAsync(ci => ci.Id == campaignItem.Id);
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
			if (quantity <= 0)
				return false;

			using var transaction = await _dbContext.Database.BeginTransactionAsync();

			var campaignItem = await _dbContext.TbCampaignItems
				.FirstOrDefaultAsync(ci => ci.Id == campaignItemId && !ci.IsDeleted);

			if (campaignItem == null)
				return false;

			
			campaignItem.UpdatedDateUtc = DateTime.UtcNow;

			await _dbContext.SaveChangesAsync();
			await transaction.CommitAsync();

			return true;
		}

		#endregion
	}
}
