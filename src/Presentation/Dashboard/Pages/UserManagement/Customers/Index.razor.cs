using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Customer;
using Dashboard.Models.pagintion;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Customer;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Pages.UserManagement.Customers
{
    public partial class Index : BaseListPage<CustomerDto>
    {
        protected override string EntityName { get; } = ECommerceResources.Customers;
        protected override string AddRoute { get; } = "/users/customer/create";
        protected override string EditRouteTemplate { get; } = "/users/customer/edit/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Customer.Search;
		//private BaseSearchCriteriaModel searchModel = new BaseSearchCriteriaModel
		//{
		//	PageNumber = 1,
		//	PageSize = 10,
		//};
		protected override Dictionary<string, Func<CustomerDto, object>> ExportColumns { get; }
         = new Dictionary<string, Func<CustomerDto, object>>
         {
             [ECommerceResources.Email] = x => x.Email,
             [ECommerceResources.Name] = x => $"{x.FirstName} {x.LastName}",
             ["Status"] = x => x.UserStatus.ToString(),
             ["Orders"] = x => x.OrderCount,
             ["Wallet Balance"] = x => x.WalletBalance,
         };

        [Inject] protected ICustomerService _custumerService { get; set; } = null!;
		protected override async Task OnInitializedAsync()
		{
			baseUrl = ApiOptions.Value.BaseUrl;
			await Search();
			
		}

        protected override async Task<ResponseModel<IEnumerable<CustomerDto>>> GetAllItemsAsync()
        {
            var result = await _custumerService.GetAllAsync();
            if (result.Success)
            {
                return result;
            }
            else
            {
                return result;
            }
        }
		protected override async Task<string> GetItemId(CustomerDto item)
        {
            var result = await _custumerService.GetByIdAsync(item.Id);
            if (result.Success)
            {
                return result.Data?.Id.ToString() ?? string.Empty;
            }
            else
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, NotifiAndAlertsResources.NoDataFound);
                return string.Empty;
            }
        }
        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await _custumerService.DeleteAsync(id);
        }


    }
}
