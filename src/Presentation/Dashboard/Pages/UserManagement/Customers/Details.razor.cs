using Common.Enumerations.User;
using Common.Enumerations.Wallet.Customer;
using Dashboard.Configuration;
using Dashboard.Contracts.Customer;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.Wallet.Customer;
using Shared.GeneralModels;

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
		private string ActiveTab { get; set; } = "orders";
		private UserStateType CurrentUserStatus { get; set; } = UserStateType.Active;
		protected string baseUrl { get; set; } = null!;
		private IEnumerable<OrderHistoryDto> OrderHistory { get; set; } = Enumerable.Empty<OrderHistoryDto>();
		private IEnumerable<CustomerWalletTransactionsDto> WalletHistory { get; set; } = Enumerable.Empty<CustomerWalletTransactionsDto>();


		protected override async Task OnInitializedAsync()
		{
			baseUrl = ApiOptions.Value.BaseUrl;
			await LoadData();
			await LoadOrderHistory();
			//await LoadWalletHistory();

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

		private async Task SetActiveTab(string tabName)
		{
			ActiveTab = tabName;
			if (tabName == "orders")
			{
				await LoadOrderHistory();
			}
			else if (tabName == "wallet")
			{
				await LoadWalletHistory();
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
					OrderHistory = response.Data.Items ?? Enumerable.Empty<OrderHistoryDto>();
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
				var response = await CustomerService.GetWalletHistoryAsync(Id,criteria);
				if (response.Success && response.Data != null)
				{
					WalletHistory = response.Data.Items ?? Enumerable.Empty<CustomerWalletTransactionsDto>();
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
		private string GetTransactionTypeBadgeClass(WalletTransactionType type)
		{
			return type switch
			{
				WalletTransactionType.Deposit => "bg-success",
				WalletTransactionType.Refund => "bg-primary",
				WalletTransactionType.Payment => "bg-secondary",
				_ => "bg-secondary"
			};
		}

		private string GetTransactionStatusBadgeClass(WalletTransactionStatus status)
		{
			return status switch
			{
				WalletTransactionStatus.Completed => "bg-success",
				WalletTransactionStatus.Pending => "bg-warning",
				WalletTransactionStatus.Failed => "bg-danger",
				_ => "bg-light"
			};
		}

		private string GetAmountClass(decimal amount)
		{
			return amount >= 0 ? "text-success fw-bold" : "text-danger fw-bold";
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
