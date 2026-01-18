using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Dashboard.Pages.Merchandising.Campaigns
{
    public partial class Index : BaseListPage<CampaignDto>
    {
        [Inject] protected ICampaignService CampaignService { get; set; } = null!;

        protected override string EntityName { get; } = "Campaign";
        protected override string AddRoute { get; } = "/campaign/new";
        protected override string EditRouteTemplate { get; } = "/campaign/{id}";
        protected override string SearchEndpoint { get; } = "api/v1/campaign/search";

        protected override Dictionary<string, Func<CampaignDto, object>> ExportColumns { get; } =
            new()
            {
                ["Campaign Name (EN)"] = x => x.NameEn,
                ["Campaign Name (AR)"] = x => x.NameAr,
                ["Type"] = x => x.IsFlashSale ? "Flash Sale" : "Regular Campaign",
                ["Start Date"] = x => x.StartDate.ToString("yyyy-MM-dd"),
                ["End Date"] = x => x.EndDate.ToString("yyyy-MM-dd"),
                ["Total Items"] = x => x.TotalItems,
                ["Status"] = x => x.IsActive ? "Active" : "Inactive"
            };

        protected override async Task<ResponseModel<IEnumerable<CampaignDto>>> GetAllItemsAsync()
        {
            try
            {
                var response = await CampaignService.GetAllAsync();
                return new ResponseModel<IEnumerable<CampaignDto>>
                {
                    Success = response.Success,
                    Data = response.Data,
                    Message = response.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<CampaignDto>>
                {
                    Success = false,
                    Data = new List<CampaignDto>(),
                    Message = ex.Message
                };
            }
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            try
            {
                var response = await CampaignService.DeleteCampaignAsync(id);
                return new ResponseModel<bool>
                {
                    Success = response.Success,
                    Message = response.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
		protected virtual async Task SortByColumn(string columnName)
		{
			if (searchModel.SortBy == columnName)
			{
				// Toggle sort direction if same column
				searchModel.SortDirection = searchModel.SortDirection == "asc" ? "desc" : "asc";
			}
			else
			{
				// New column, default to ascending
				searchModel.SortBy = columnName;
				searchModel.SortDirection = "asc";
			}

			// Reset to first page when sorting changes
			currentPage = 1;
			searchModel.PageNumber = 1;
			await Search();
		}
		protected override async Task<string> GetItemId(CampaignDto item)
        {
            return item?.Id != Guid.Empty ? item.Id.ToString() : string.Empty;
        }

        protected virtual async Task OnStatusFilterChanged(string status)
        {
            searchModel.PageNumber = 1;
            await Search();
        }

        protected virtual async Task OnTypeFilterChanged(string type)
        {
            searchModel.PageNumber = 1;
            await Search();
        }

        protected virtual async Task OnDeleteConfirm(Guid campaignId)
        {
            await Delete(campaignId);
        }

        protected virtual async Task GoToPageAsync(int pageNumber)
        {
            searchModel.PageNumber = pageNumber;
            await Search();
        }
    }
}
