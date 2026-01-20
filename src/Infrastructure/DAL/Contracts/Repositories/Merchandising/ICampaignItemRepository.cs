using Domains.Entities.Campaign;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories.Merchandising
{
	public interface ICampaignItemRepository : ITableRepository<TbCampaignItem>
	{
		Task<IEnumerable<TbCampaignItem>> GetCampaignItemsAsync(Guid campaignId);
		Task<TbCampaignItem> AddItemToCampaignAsync(TbCampaignItem campaignItem);
		Task<TbCampaignItem> UpdateCampaignItemAsync(TbCampaignItem campaignItem);
		Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId,Guid userId);
		Task<bool> IncrementSoldCountAsync(Guid campaignItemId, int quantity);

		#region
		Task<IEnumerable<TbCampaignItem>> GetCampaignItemsForVendorAsync(Guid campaignId, Guid userId);


		#endregion


	}
}
