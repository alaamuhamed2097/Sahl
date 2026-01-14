using Common.Enumerations.User;
using Dashboard.Contracts.Customer;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.GeneralModels;
using Shared.DTOs.Customer;
using Dashboard.Configuration;
using Microsoft.Extensions.Options;

namespace Dashboard.Pages.UserManagement.Customers
{
	public partial class Details
	{
		[Parameter] public Guid Id { get; set; }

		[Inject] private ICustomerService CustomerService { get; set; } = default!;
		[Inject] private NavigationManager Navigation { get; set; } = default!;
		[Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

		private CustomerDto Customer { get; set; } = new();
		private bool IsLoading { get; set; } = true;
		private string ActiveTab { get; set; } = "profile";
		private UserStateType CurrentUserStatus { get; set; } = UserStateType.Active;
		protected string baseUrl { get; set; } = null!;
		private IEnumerable<object> OrderHistory { get; set; } = Enumerable.Empty<object>();
		private IEnumerable<object> WalletHistory { get; set; } = Enumerable.Empty<object>();

		protected override async Task OnInitializedAsync()
		{
			baseUrl = ApiOptions.Value.BaseUrl;
			await LoadData();
		}

		private async Task LoadData()
		{
			IsLoading = true;
			try
			{
				var response = await CustomerService.GetByIdAsync(Id);
				if (response.Success && response.Data != null)
				{
					Customer = response.Data;
					CurrentUserStatus = Customer.UserStatus;
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, NotifiAndAlertsResources.NoDataFound, "error");
					Navigation.NavigateTo("/users/customers");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private void SetActiveTab(string tabName)
		{
			ActiveTab = tabName;
			if (tabName == "orders")
			{
				LoadOrderHistory();
			}
			else if (tabName == "wallet")
			{
				LoadWalletHistory();
			}
		}

		private async Task LoadOrderHistory()
		{
			try
			{
				var criteria = new Common.Filters.BaseSearchCriteriaModel { PageSize = 10, PageNumber = 1 };
				var response = await CustomerService.GetOrderHistoryAsync(Id, criteria);
				if (response.Success && response.Data != null)
				{
					OrderHistory = response.Data.Items ?? Enumerable.Empty<object>();
				}
				StateHasChanged();
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
			}
		}

		private async Task LoadWalletHistory()
		{
			try
			{
				var criteria = new Common.Filters.BaseSearchCriteriaModel { PageSize = 10, PageNumber = 1 };
				var response = await CustomerService.GetWalletHistoryAsync(Id, criteria);
				if (response.Success && response.Data != null)
				{
					WalletHistory = response.Data.Items ?? Enumerable.Empty<object>();
				}
				StateHasChanged();
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
			}
		}

		private async Task ChangeStatus(UserStateType newStatus)
		{
			try
			{
				var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", 
					$"Are you sure you want to change the customer status to {newStatus}?");
				
				if (!confirmed) return;

				var result = await CustomerService.ChangeStatusAsync(Id, newStatus);
				if (result.Success)
				{
					CurrentUserStatus = newStatus;
					Customer.UserStatus = newStatus;
					await JSRuntime.InvokeVoidAsync("swal", "Success", "Account status updated successfully", "success");
					StateHasChanged();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, result.Message ?? "Failed to update status", "error");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal", NotifiAndAlertsResources.Error, ex.Message, "error");
			}
		}

		private async Task LockAccount()
		{
			await ChangeStatus(UserStateType.Auto_Locked);
		}

		private async Task SuspendAccount()
		{
			await ChangeStatus(UserStateType.Suspended);
		}

		private async Task RestrictAccount()
		{
			await ChangeStatus(UserStateType.Restricted);
		}

		private async Task ActivateAccount()
		{
			await ChangeStatus(UserStateType.Active);
		}

		private void GoBack()
		{
			Navigation.NavigateTo("/users/customers");
		}

		private string GetStatusBadgeClass(UserStateType status)
		{
			return status switch
			{
				UserStateType.Active => "badge bg-success",
				UserStateType.Inactive => "badge bg-secondary",
				UserStateType.Auto_Locked => "badge bg-warning",
				UserStateType.Restricted => "badge bg-danger",
				UserStateType.Suspended => "badge bg-danger",
				UserStateType.Deleted => "badge bg-dark",
				_ => "badge bg-light"
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
	}
}
