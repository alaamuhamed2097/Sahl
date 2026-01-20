using Domains.Entities.Campaign;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories.Merchandising
{
	public interface ICampaignItemRepository : ITableRepository<TbCampaignItem>
	{
		/// <summary>
		/// Get all items in a campaign
		/// </summary>
		Task<IEnumerable<TbCampaignItem>> GetCampaignItemsAsync(Guid campaignId);

		/// <summary>
		/// Add item to campaign
		/// </summary>
		Task<TbCampaignItem> AddItemToCampaignAsync(TbCampaignItem campaignItem);

		/// <summary>
		/// Remove item from campaign (soft delete)
		/// </summary>
		Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId);

		/// <summary>
		/// Update sold count for campaign item
		/// </summary>
		Task<bool> IncrementSoldCountAsync(Guid campaignItemId, int quantity);

		#region
		Task<IEnumerable<TbCampaignItem>> GetCampaignItemsForVendorAsync(Guid campaignId, Guid userId);

		#endregion


	}
}
