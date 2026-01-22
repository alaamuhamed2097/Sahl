using Common.Enumerations.User;
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
			//baseUrl = SearchEndpoint;
			await Search();
			
		}
		protected override async Task Search()
		{
			try
			{
				

				var result = await _custumerService.SearchAsync(searchModel);

				if (result.Success && result.Data != null)
				{
					items = result.Data.Items ?? new List<CustomerDto>();
					
					

					StateHasChanged();
				}
				else
				{
					await ShowErrorNotification("Error", result.Message ?? "Search failed");
				}
			}
			catch (Exception ex)
			{
				await ShowErrorNotification("Error", ex.Message);
			}
			finally
			{
				
			}
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
		private string GetStatusBadgeClass(UserStateType status)
		{
			return status switch
			{
				UserStateType.Active => "bg-success",
				UserStateType.Inactive => "bg-secondary",
				UserStateType.Auto_Locked => "bg-warning",
				UserStateType.Restricted => "bg-danger",
				UserStateType.Suspended => "bg-danger",
				UserStateType.Deleted => "bg-dark",
				_ => "bg-light"
			};
		}

		private string GetStatusDisplayName(UserStateType status)
		{
			return status switch
			{
				UserStateType.Active => "Active",
				UserStateType.Inactive => "Inactive",
				UserStateType.Auto_Locked => "Locked",
				UserStateType.Restricted => "Restricted",
				UserStateType.Suspended => "Suspended",
				UserStateType.Deleted => "Deleted",
				_ => status.ToString()
			};
		}

		private void ViewDetails(CustomerDto customer)
		{
			Navigation.NavigateTo($"/users/customer/details/{customer.Id}");
		}

		private async Task GoToPage(int pageNumber)
		{
			searchModel.PageNumber = pageNumber;
			await Search();
		}
		protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await _custumerService.DeleteAsync(id);
        }


    }
}
