using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using Domains.Entities.Campaign;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.Merchandising.Campaign
{
	public interface ICampaignItemService 
	{
		#region Campaign Items
		Task<ResponseModel<PaginatedSearchResult<CampaignItemDto>>> SearchCampaignItemsAsync(BaseSearchCriteriaModel searchCriteria, Guid campaignId);
		//Task<ResponseModel<PaginatedSearchResult<CampaignItemDto>>> SearchCampaignItemsForVendorAsync(BaseSearchCriteriaModel searchCriteria, Guid campaignId, Guid userId);

		/// <summary>
		/// Get all items in a campaign
		/// </summary>
		Task<IEnumerable<CampaignItemDto>> GetCampaignItemsAsync(Guid campaignId);

		/// <summary>
		/// Add item to campaign
		/// </summary>
		Task<CampaignItemDto> AddItemToCampaignAsync(AddCampaignItemDto dto, Guid userId);

		/// <summary>
		/// Remove item from campaign
		/// </summary>
		Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId, Guid userId);

		/// <summary>
		/// Update sold count when item is purchased
		/// </summary>
		Task<bool> UpdateSoldCountAsync(Guid campaignItemId, int quantity);

		#endregion
	}
}
