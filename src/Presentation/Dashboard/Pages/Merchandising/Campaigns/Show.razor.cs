using Common.Enumerations.User;
using Common.Filters;
using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Dashboard.Pages.Merchandising.Campaigns
{
	[Authorize(Roles = nameof(UserRole.Admin))]
	public partial class Show : IDisposable
	{
		#region Parameters & Injects

		[Parameter] public string? CampaignId { get; set; }
		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] protected ICampaignService CampaignService { get; set; } = null!;
		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
		[Inject] protected INotificationService NotificationService { get; set; } = null!;


		#endregion

		#region Properties

		private bool IsLoading { get; set; } = true;
		private bool IsRemoving { get; set; }
		private bool IsSearching { get; set; }

		protected List<CampaignItemDto> CampaignItems { get; set; } = new();

		protected string CampaignName { get; set; } = string.Empty;
		protected string? PreviewImageUrl { get; set; }

		// Search Model
		protected BaseSearchCriteriaModel searchModel { get; set; } = new()
		{
			PageNumber = 1,
			PageSize = 10,
			SearchTerm = string.Empty
		};

		// Pagination
		protected int TotalRecords { get; set; }
		protected int TotalPages => (int)Math.Ceiling((double)TotalRecords / searchModel.PageSize);

		private Guid _campaignGuid;
		private bool? _statusFilter = null;

		#endregion

		#region Lifecycle Methods

		protected override async Task OnInitializedAsync()
		{
			if (string.IsNullOrEmpty(CampaignId) || !Guid.TryParse(CampaignId, out _campaignGuid))
			{
				await NotificationService.ShowErrorAsync("Invalid Campaign ID");
				NavigationManager.NavigateTo("/campaigns");
				return;
			}

			await LoadDataAsync();
		}

		public void Dispose()
		{
			// Clean up if needed
		}

		#endregion

		#region Data Loading

		private async Task LoadDataAsync()
		{
			IsLoading = true;
			try
			{
				// Load campaign details to get name
				await LoadCampaignDetailsAsync();

				// Load campaign items with search
				await SearchCampaignItemsAsync();
			}
			catch (Exception ex)
			{
				await NotificationService.ShowErrorAsync($"Error loading data: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private async Task LoadCampaignDetailsAsync()
		{
			try
			{
				var response = await CampaignService.GetCampaignByIdAsync(_campaignGuid);
				if (response.Success && response.Data != null)
				{
					CampaignName = $"{response.Data.NameEn} / {response.Data.NameAr}";
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading campaign details: {ex.Message}");
			}
		}

		private async Task SearchCampaignItemsAsync()
		{
			IsSearching = true;
			try
			{
				var response = await CampaignService.SearchCampaignItemsAsync(_campaignGuid, searchModel);

				if (response.Success && response.Data != null)
				{
					var allItems = response.Data.Items?.ToList() ?? new List<CampaignItemDto>();

					// Apply status filter if set
					if (_statusFilter.HasValue)
					{
						allItems = allItems.Where(x => x.IsActive == _statusFilter.Value).ToList();
					}

					CampaignItems = allItems;
					TotalRecords = response.Data.TotalRecords;
				}
				else
				{
					CampaignItems = new List<CampaignItemDto>();
					TotalRecords = 0;

					if (!string.IsNullOrEmpty(response.Message))
					{
						await NotificationService.ShowErrorAsync(response.Message);
					}
				}
			}
			catch (Exception ex)
			{
				await NotificationService.ShowErrorAsync($"Error searching items: {ex.Message}");
				Console.WriteLine($"Error searching campaign items: {ex.Message}");
				CampaignItems = new List<CampaignItemDto>();
				TotalRecords = 0;
			}
			finally
			{
				IsSearching = false;
			}
		}

		#endregion

		#region Search & Filter

		private async Task Search()
		{
			searchModel.PageNumber = 1; // Reset to first page on new search
			await SearchCampaignItemsAsync();
		}

		private async Task ClearSearch()
		{
			searchModel.SearchTerm = string.Empty;
			searchModel.PageNumber = 1;
			_statusFilter = null;
			await SearchCampaignItemsAsync();
		}

		private async Task OnPageSizeChanged(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value?.ToString(), out int newPageSize))
			{
				searchModel.PageSize = newPageSize;
				searchModel.PageNumber = 1;
				await SearchCampaignItemsAsync();
			}
		}

		private async Task OnStatusFilterChanged(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				_statusFilter = null;
			}
			else if (value == "1")
			{
				_statusFilter = true;
			}
			else if (value == "0")
			{
				_statusFilter = false;
			}

			searchModel.PageNumber = 1;
			await SearchCampaignItemsAsync();
		}

		#endregion

		#region Pagination

		private async Task GoToPage(int page)
		{
			if (page < 1 || page > TotalPages || page == searchModel.PageNumber)
				return;

			searchModel.PageNumber = page;
			await SearchCampaignItemsAsync();
		}

		private async Task NextPage()
		{
			if (searchModel.PageNumber < TotalPages)
			{
				searchModel.PageNumber++;
				await SearchCampaignItemsAsync();
			}
		}

		private async Task PreviousPage()
		{
			if (searchModel.PageNumber > 1)
			{
				searchModel.PageNumber--;
				await SearchCampaignItemsAsync();
			}
		}

		#endregion

		#region Actions
		protected virtual async Task Delete(Guid id)
		{
			var confirmed = await DeleteConfirmNotification();

			if (confirmed)
			{
				var result = await CampaignService.RemoveItemFromCampaignAsync(id);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
					await SearchCampaignItemsAsync();
					await OnAfterDeleteAsync(id);
					StateHasChanged();
				}
				else
				{
					if (result.Message == null)
						await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.DeleteFailed);
					else
						await ShowErrorNotification(ValidationResources.Failed, result.Message);
				}
			}
		}

		protected virtual async Task OnAfterDeleteAsync(Guid id)
		{
			await Task.CompletedTask;
		}
		protected virtual async Task ShowErrorNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
		}

		protected virtual async Task ShowWarningNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "warning");
		}

		protected virtual async Task ShowSuccessNotification(string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
		}

		protected virtual async Task<bool> DeleteConfirmNotification()
		{
			var options = new
			{
				title = NotifiAndAlertsResources.AreYouSure,
				text = NotifiAndAlertsResources.ConfirmDeleteAlert,
				icon = "warning",
				buttons = new
				{
					cancel = new
					{
						text = ActionsResources.Cancel,
						value = false,
						visible = true,
						className = "",
						closeModal = true
					},
					confirm = new
					{
						text = ActionsResources.Confirm,
						value = true,
						visible = true,
						className = "swal-button--danger",
						closeModal = true
					}
				}
			};

			return (await JSRuntime.InvokeAsync<bool>("swal", options));
		}

		private void AddNewItem()
		{
			NavigationManager.NavigateTo($"/campaigns/{CampaignId}/add-items");
		}

		private void ViewItemDetails(Guid itemId)
		{
			NavigationManager.NavigateTo($"/items/{itemId}");
		}

		private void BackToCampaigns()
		{
			NavigationManager.NavigateTo("/campaigns");
		}

		#endregion

		#region Export

		private async Task ExportToExcel()
		{
			try
			{
				//await NotificationService.ShowInfoAsync("Exporting to Excel...");
				// Implement Excel export logic here
				await Task.Delay(1000); // Placeholder
				await NotificationService.ShowSuccessAsync("Exported successfully");
			}
			catch (Exception ex)
			{
				await NotificationService.ShowErrorAsync($"Error exporting: {ex.Message}");
			}
		}

		private async Task ExportToPrint()
		{
			try
			{
				await JSRuntime.InvokeVoidAsync("print");
			}
			catch (Exception ex)
			{
				await NotificationService.ShowErrorAsync($"Error printing: {ex.Message}");
			}
		}

		#endregion
		
		

		#region Image Preview

		private void ShowImagePreview(string imageUrl)
		{
			PreviewImageUrl = imageUrl;
			StateHasChanged();
		}

		private void CloseImagePreview()
		{
			PreviewImageUrl = null;
			StateHasChanged();
		}

				
		#endregion
	}
}