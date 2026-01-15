using Common.Enumerations.Review;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Review;
using Dashboard.Services.Review;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Pages.ManageReviews.VendorsReviews
{
	public partial class Index
	{
		[Parameter] public Guid Id { get; set; }

		[Inject] protected IVendorReviewService VendorReviewService { get; set; } = null!;
		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
		[Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
		[Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;

		private string selectedStatus = "All"; 
		private ReviewStatus? currentStatusFilter = null;  

		private IEnumerable<VendorReviewDto> items = new List<VendorReviewDto>();
		private AdminVendorReviewSearchCriteriaModel searchModel = new AdminVendorReviewSearchCriteriaModel
		{
			PageNumber = 1,
			PageSize = 10,
		};

		private int currentPage = 1;
		private int totalPages = 1;
		private int totalRecords = 0;
		private string currentSortColumn = "";
		private bool isAscending = true;
		//private string selectedStatus = "All";

		protected override async Task OnInitializedAsync()
		{
			if (Id != Guid.Empty)
			{
				searchModel.VendorId = Id;
			}

			await LoadData();
		}

		protected override async Task OnParametersSetAsync()
		{
			Console.WriteLine($"Hello from OnParametersSetAsync, your ID is {Id}");

			if (Id != Guid.Empty)
			{
				searchModel.VendorId = Id;
				await LoadData();
			}
		}
		
		private async Task LoadData()
		{
			try
			{
				var response = await VendorReviewService.SearchVendorReviews(searchModel);

				if (response.Success && response.Data != null)
				{
					items = response.Data.Items;
					totalRecords = response.Data.TotalRecords;
					totalPages = (int)Math.Ceiling(totalRecords / (double)searchModel.PageSize);
					currentPage = searchModel.PageNumber;

				}
				else
				{
					items = new List<VendorReviewDto>();
					Console.WriteLine($"Error: {response.Message}");
				}
				StateHasChanged();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
				items = new List<VendorReviewDto>();
			}
		}

		private async Task Search()
		{
			searchModel.PageNumber = 1;

			if (Id != Guid.Empty)
			{
				searchModel.VendorId = Id;
			}

			await LoadData();
		}

		private async Task OnPageSizeChanged(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value?.ToString(), out int newSize))
			{
				searchModel.PageSize = newSize;
				searchModel.PageNumber = 1;

				if (Id != Guid.Empty)
				{
					searchModel.VendorId = Id;
				}

				await LoadData();
			}
		}

		private async Task GoToPage(int pageNumber)
		{
			if (pageNumber >= 1 && pageNumber <= totalPages && pageNumber != currentPage)
			{
				searchModel.PageNumber = pageNumber;

				if (Id != Guid.Empty)
				{
					searchModel.VendorId = Id;
				}

				await LoadData();
			}
		}

		private async Task SortByColumn(string columnName)
		{
			if (currentSortColumn == columnName)
			{
				isAscending = !isAscending;
			}
			else
			{
				currentSortColumn = columnName;
				isAscending = true;
			}

			searchModel.SortDirection = isAscending ? "asc" : "desc";
			searchModel.SortBy = columnName;

			if (Id != Guid.Empty)
			{
				searchModel.VendorId = Id;
			}

			await LoadData();
		}

		private string GetSortIconClass(string columnName)
		{
			if (currentSortColumn != columnName)
				return "fas fa-sort text-muted";

			return isAscending ? "fas fa-sort-up text-primary" : "fas fa-sort-down text-primary";
		}

		private async Task FilterByStatusSearch(ReviewStatus? status)
		{
			selectedStatus = status?.ToString() ?? "All";
			searchModel.PageNumber = 1;

			if (status.HasValue)
			{
				searchModel.Statuses = new List<ReviewStatus> { status.Value };
			}
			else
			{
				searchModel.Statuses = null;
			}

			if (Id != Guid.Empty)
			{
				searchModel.VendorId = Id;
			}


			await LoadData();
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

		protected virtual async Task<bool> RejectConfirmNotification()
		{
			var options = new
			{
				title = NotifiAndAlertsResources.AreYouSure,
				text = NotifiAndAlertsResources.ConfirmRejectAlert,
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

		protected virtual async Task<bool> ApproveConfirmNotification()
		{
			var options = new
			{
				title = NotifiAndAlertsResources.AreYouSure,
				text = NotifiAndAlertsResources.ConfirmRejectAlert,
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

		private void ViewDetails(Guid reviewId)
		{
			NavigationManager.NavigateTo($"/vendorsReviews/details/{reviewId}");
		}

		private async Task ApproveReview(Guid reviewId)
		{

			bool confirmed = await ApproveConfirmNotification();



			if (confirmed)
			{
				var result = await VendorReviewService.ApproveReviewAsync(reviewId);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.Successful);
					await Search();
					await OnAfterDeleteAsync(reviewId);
					StateHasChanged();
				}
				else
				{
					if (result.Message == null)
						await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.Failed);
					else
						await ShowErrorNotification(ValidationResources.Failed, result.Message);
				}
			}
		}


		private async Task RejectReview(Guid reviewId)
		{
			bool confirmed = await RejectConfirmNotification();

			if (confirmed)
			{
				var result = await VendorReviewService.RejectReviewAsync(reviewId);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.Successful);
					await Search();
					await OnAfterDeleteAsync(reviewId);
					StateHasChanged();
				}
				else
				{
					if (result.Message == null)
						await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.Failed);
					else
						await ShowErrorNotification(ValidationResources.Failed, result.Message);
				}
			}
		}
		
		
		
		protected virtual async Task OnAfterDeleteAsync(Guid id)
		{
			await Task.CompletedTask;
		}
		protected virtual async Task ShowSuccessNotification(string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
		}
		private async Task Delete(Guid reviewId)
		{
			bool confirmed = await DeleteConfirmNotification();


			if (confirmed)
			{
				var result = await VendorReviewService.DeleteReviewAsync(reviewId);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
					await Search();
					await OnAfterDeleteAsync(reviewId);
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
		protected virtual async Task ShowErrorNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
		}
		private async Task ExportToExcel()
		{
			await JSRuntime.InvokeVoidAsync("alert", "Excel export will be implemented");
		}

		private async Task ExportToPrint()
		{
			await JSRuntime.InvokeVoidAsync("window.print");
		}
	}
}