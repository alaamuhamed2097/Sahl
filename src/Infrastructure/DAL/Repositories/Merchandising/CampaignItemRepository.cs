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

		public async Task<TbCampaignItem> UpdateCampaignItemAsync(TbCampaignItem campaignItem)
		{
			try
			{
				_dbContext.TbCampaignItems.Update(campaignItem);
				await _dbContext.SaveChangesAsync();

				return await _dbContext.TbCampaignItems
					.Include(ci => ci.OfferCombinationPricing)
						.ThenInclude(ocp => ocp.ItemCombination)
							.ThenInclude(ic => ic.Item)
								.ThenInclude(i => i.ItemImages)
					.Include(ci => ci.Campaign)
					.FirstOrDefaultAsync(ci => ci.Id == campaignItem.Id);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error updating campaign item: {CampaignItemId}", campaignItem.Id);
				throw;
			}
		}

		/// <summary>
		/// Remove item from campaign (soft delete)
		/// </summary>
		public async Task<bool> RemoveItemFromCampaignAsync(Guid ItemId, Guid userId)
		{
			try
			{	
				var campaignItem = await _dbContext.TbCampaignItems
					.FirstOrDefaultAsync(ci => ci.OfferCombinationPricingId == ItemId && !ci.IsDeleted);

				if (campaignItem == null)
					return false;

				campaignItem.IsDeleted = true;
				campaignItem.UpdatedDateUtc = DateTime.UtcNow;
				campaignItem.UpdatedBy = userId;

				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error removing item from campaign: {CampaignItemId}", ItemId);
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

		#region item for vendor
		/// <summary>
		/// Get all items in a campaign for a specific vendor
		/// </summary>
		public async Task<IEnumerable<TbCampaignItem>> GetCampaignItemsForVendorAsync(Guid campaignId, Guid userId)
		{
			try
			{
				var vendor = await _dbContext.TbVendors
					.AsNoTracking()
					.FirstOrDefaultAsync(v => v.UserId == userId.ToString() && !v.IsDeleted);

				if (vendor == null)
				{
					throw new KeyNotFoundException($"Vendor with userId '{userId}' was not found.");
				}

				return await _dbContext.TbCampaignItems
					.AsNoTracking()
					.Include(ci => ci.OfferCombinationPricing)
						.ThenInclude(ocp => ocp.ItemCombination)
							.ThenInclude(ic => ic.Item)
								.ThenInclude(i => i.ItemImages)
					.Include(ci => ci.Campaign)
					.Where(ci => ci.CampaignId == campaignId
						&& !ci.IsDeleted
						&& ci.Campaign.IsActive
						&& ci.VendorId == vendor.Id) 
					.OrderBy(ci => ci.CreatedDateUtc)
					.ToListAsync()
					?? throw new KeyNotFoundException($"No campaign items found for campaign '{campaignId}' and vendor '{vendor.Id}'.");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error getting campaign items for vendor: CampaignId={CampaignId}, UserId={UserId}", campaignId, userId);
				throw;
			}
		}

		//public async Task<TbCampaignItem> AddItemToCampaignForVendorAsync(TbCampaignItem campaignItem, Guid userId)
		//{
		//	var vendor = await _dbContext.TbVendors
		//		.AsNoTracking()
		//		.FirstOrDefaultAsync(v => v.UserId == userId.ToString() && !v.IsDeleted)
		//		?? throw new KeyNotFoundException($"Vendor with userId '{userId}' not found");

		//	var campaign = await _campaignRepository.GetCampaignByIdAsync(dto.CampaignId)
		//		?? throw new KeyNotFoundException($"Campaign with ID {dto.CampaignId} not found");

		//	var campaignItem = _mapper.Map<TbCampaignItem>(dto);
		//	campaignItem.Id = Guid.NewGuid();
		//	campaignItem.CreatedDateUtc = DateTime.UtcNow;
		//	campaignItem.CreatedBy = userId;
		//	campaignItem.IsDeleted = false;
		//	campaignItem.IsActive = true;
		//	campaignItem.SoldCount = 0;
		//	campaignItem.VendorId = vendor.Id; 

		//	var result = await _campaignItemRepository.AddItemToCampaignAsync(campaignItem);

		//	return _mapper.Map<CampaignItemDto>(result);
		//}
		
		#endregion
	}
}
