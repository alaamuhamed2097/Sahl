using Common.Filters;
using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.General;
using Dashboard.Services.Merchandising;
using Dashboard.Services.Notification;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Campaign;
using Shared.DTOs.Customer;
using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;

namespace Dashboard.Pages.Merchandising.Campaigns
{
    public partial class Index : BaseListPage<CampaignDto>
    {
        [Inject] protected ICampaignService CampaignService { get; set; } = null!;
		protected string currentStatusFilter = string.Empty;
		protected CampaignSearchCriteriaModel searchModel { get; set; } = new();
		private string currentSortColumn = "";
		private string currentSortDirection = "asc";

		protected override string EntityName { get; } = "Campaign";
        protected override string AddRoute { get; } = "/campaigns/new";
        protected override string EditRouteTemplate { get; } = "/campaigns/{id}";
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
	

		protected override async Task OnInitializedAsync()
		{
			searchModel.PageSize = 10;
			searchModel.PageNumber = 1;
			await base.OnInitializedAsync();
		}

		private async Task OnStatusFilterChanged(string statusValue)
		{
			if (string.IsNullOrEmpty(statusValue))
			{
				searchModel.Status = null; // All
			}
			else if (int.TryParse(statusValue, out int status))
			{
				searchModel.Status = status; // 1 = Active, 2 = Inactive
			}

			searchModel.PageNumber = 1;
			await GetAllItemsAsync();
		}

		private async Task OnTypeFilterChanged(string typeValue)
		{
			if (string.IsNullOrEmpty(typeValue))
			{
				searchModel.Type = null; // All
			}
			else if (int.TryParse(typeValue, out int type))
			{
				searchModel.Type = type; // 1 = Flash Sale, 2 = Regular
			}

			searchModel.PageNumber = 1;
			await LoadItems();
		}
		private async Task OnPageSizeChanged(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value?.ToString(), out int newSize))
			{
				searchModel.PageSize = newSize;
				searchModel.PageNumber = 1; // Reset to first page
				await LoadItems();
			}
		}
		private void ViewDetails(Guid campaignId)
		{
			Navigation.NavigateTo($"/campaigns/show/{campaignId}");
		}
		protected override async Task<string> GetItemId(CampaignDto item)
        {
            return item?.Id != Guid.Empty ? item.Id.ToString() : string.Empty;
        }
    }
}
