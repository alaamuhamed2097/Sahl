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

		private IEnumerable<VendorReviewDto> items = new List<VendorReviewDto>();
		private VendorReviewSearchCriteriaModel searchModel = new VendorReviewSearchCriteriaModel
		{
			PageNumber = 1,
			PageSize = 10,
		};

		private int currentPage = 1;
		private int totalPages = 1;
		private int totalRecords = 0;
		private string currentSortColumn = "";
		private bool isAscending = true;
		private string selectedStatus = "All";

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

		private async Task FilterByStatus(ReviewStatus? status)
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

		private void ViewDetails(Guid reviewId)
		{
			NavigationManager.NavigateTo($"/vendorsReviews/details/{reviewId}");
		}

		private async Task ApproveReview(Guid reviewId)
		{
			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				"Are you sure you want to approve this review?");

			if (!confirmed) return;

			try
			{
				var response = await VendorReviewService.ApproveReviewAsync(reviewId);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", "Review approved successfully!");
					await LoadData();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
			}
		}

		private async Task RejectReview(Guid reviewId)
		{
			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				"Are you sure you want to reject this review?");

			if (!confirmed) return;

			try
			{
				var response = await VendorReviewService.RejectReviewAsync(reviewId);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", "Review rejected successfully!");
					await LoadData();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
			}
		}

		private async Task Delete(Guid reviewId)
		{
			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				"Are you sure you want to delete this review?");

			if (!confirmed) return;

			try
			{
				var response = await VendorReviewService.DeleteReviewAsync(reviewId);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", "Review deleted successfully!");
					await LoadData();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
			}
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