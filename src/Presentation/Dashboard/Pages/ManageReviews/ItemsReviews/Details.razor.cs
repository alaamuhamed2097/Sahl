//// Dashboard/Pages/ManageReviews/ReviewReports/Details.razor.cs
//using Common.Enumerations.Review;
//using Dashboard.Contracts.Review;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Shared.DTOs.Review;

//namespace Dashboard.Pages.ManageReviews.ReviewReports
//{
//	public partial class Details
//	{
//		[Parameter] public Guid Id { get; set; }

//		[Inject] protected IReportReviewService ReviewReportService { get; set; } = null!;
//		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
//		[Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

//		private ReviewReportDto? report;
//		private IEnumerable<ReviewReportDto> relatedReports = new List<ReviewReportDto>();
//		private bool isLoading = true;

//		protected override async Task OnInitializedAsync()
//		{

//			if (Id != Guid.Empty)
//			{
//				searchModel.ItemId = Id;
//			}

//			await LoadData();
//		}

//		private async Task LoadData()
//		{
//			isLoading = true;
//			try
//			{
//				// Load main report
//				var response = await ReviewReportService.GetReportByIdAsync(Id);
//				if (response.Success && response.Data != null)
//				{
//					report = response.Data;

//					// Load related reports for the same review
//					var relatedResponse = await ReviewReportService.GetReportsByReviewIdAsync(report.ItemReviewId);
//					if (relatedResponse.Success && relatedResponse.Data != null)
//					{
//						relatedReports = relatedResponse.Data;
//					}
//				}
//				else
//				{
//					await JSRuntime.InvokeVoidAsync("alert", response.Message);
//				}
//			}
//			catch (Exception ex)
//			{
//				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
//			}
//			finally
//			{
//				isLoading = false;
//			}
//		}

//		private async Task ResolveReport()
//		{
//			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
//				"Are you sure you want to resolve this report?");

//			if (!confirmed) return;

//			try
//			{
//				var response = await ReviewReportService.ResolveReportAsync(Id);

//				if (response.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("alert", "Report resolved successfully!");
//					await LoadData();
//				}
//				else
//				{
//					await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
//				}
//			}
//			catch (Exception ex)
//			{
//				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
//			}
//		}

//		private async Task MarkReviewAsFlagged()
//		{
//			if (report == null) return;

//			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
//				"Are you sure you want to flag this review?");

//			if (!confirmed) return;

//			try
//			{
//				var response = await ReviewReportService.MarkReviewAsFlaggedAsync(report.ItemReviewId);

//				if (response.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("alert", "Review flagged successfully!");
//					await LoadData();
//				}
//				else
//				{
//					await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
//				}
//			}
//			catch (Exception ex)
//			{
//				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
//			}
//		}

//		private void ViewReport(Guid reportId)
//		{
//			NavigationManager.NavigateTo($"/reviewReport/{reportId}");
//		}

//		private void GoBack()
//		{
//			NavigationManager.NavigateTo("/reviewReports");
//		}

//		private string GetStatusClass(ReviewReportStatus status)
//		{
//			return status switch
//			{
//				ReviewReportStatus.Pending => "bg-warning text-dark",
//				ReviewReportStatus.Reviewed => "bg-info text-white",
//				ReviewReportStatus.Resolved => "bg-success text-white",
//				_ => "bg-secondary text-white"
//			};
//		}

//		private string GetStatusText(ReviewReportStatus status)
//		{
//			return status switch
//			{
//				ReviewReportStatus.Pending => "Pending",
//				ReviewReportStatus.Reviewed => "Reviewed",
//				ReviewReportStatus.Resolved => "Resolved",
//				_ => status.ToString()
//			};
//		}

//		private string GetReasonText(ReviewReportReason reason)
//		{
//			return reason switch
//			{
//				ReviewReportReason.Spam => "Spam",
//				ReviewReportReason.OffensiveLanguage => "Offensive Language",
//				ReviewReportReason.HateSpeech => "Hate Speech",
//				ReviewReportReason.FakeReview => "Fake Review",
//				ReviewReportReason.IrrelevantContent => "Irrelevant Content",
//				ReviewReportReason.Duplicate => "Duplicate",
//				ReviewReportReason.Other => "Other",
//				_ => reason.ToString()
//			};
//		}
//	}
//}



// Dashboard/Pages/ManageReviews/ReviewReports/Details.razor.cs
using Common.Enumerations.Review;
using Dashboard.Contracts.Review;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Review;

namespace Dashboard.Pages.ManageReviews.ItemsReviews
{
	public partial class Details
	{
		[Parameter] public Guid Id { get; set; }

		[Inject] protected IReportReviewService ReviewReportService { get; set; } = null!;
		[Inject] protected IItemReviewService ItemReviewService { get; set; } = null!;
		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
		[Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

		private ItemReviewResponseDto? reviewItem;
		private ReviewReportDto? reviewReport;
		private IEnumerable<ReviewReportDto> reports = new List<ReviewReportDto>();
		private bool isLoading = true;

		protected override async Task OnInitializedAsync()
		{
			await LoadData();
		}

		private async Task LoadData()
		{
			isLoading = true;
			try
			{
				// Load review details
				var reviewResponse = await ItemReviewService.GetReviewByIdAsync(Id);
				if (reviewResponse.Success && reviewResponse.Data != null)
				{
					reviewItem = reviewResponse.Data;

					// Load all reports for this review
					var reportsResponse = await ReviewReportService.GetReportsByReviewIdAsync(Id);
					if (reportsResponse.Success && reportsResponse.Data != null)
					{
						reports = reportsResponse.Data;
					}
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("alert", reviewResponse.Message);
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
			}
			finally
			{
				isLoading = false;
			}
		}

		private async Task ResolveReport(Guid reportId)
		{
			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				ReviewResources.ConfirmResolveReport);

			if (!confirmed) return;

			try
			{
				var response = await ReviewReportService.ResolveReportAsync(reportId);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReportResolvedSuccessfully);
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

		private async Task MarkReviewAsFlagged()
		{
			if (reviewItem == null) return;

			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				ReviewResources.ConfirmMarkAsFlagged);

			if (!confirmed) return;

			try
			{
				var response = await ReviewReportService.MarkReviewAsFlaggedAsync(reviewItem.Id);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewFlaggedSuccessfully);
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

		private async Task ApproveReview()
		{
			if (reviewItem == null) return;

			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				ReviewResources.ConfirmApproveReview);

			if (!confirmed) return;

			try
			{
				var response = await ItemReviewService.ChangeReviewStatusAsync(reviewItem.Id, ReviewStatus.Approved);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewApprovedSuccessfully);
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

		private async Task RejectReview()
		{
			if (reviewItem == null) return;

			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
				ReviewResources.ConfirmRejectReview);

			if (!confirmed) return;

			try
			{
				var response = await ItemReviewService.ChangeReviewStatusAsync(reviewItem.Id, ReviewStatus.Rejected);

				if (response.Success)
				{
					await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewRejectedSuccessfully);
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

		private void ViewReportDetails(Guid reportId)
		{
			NavigationManager.NavigateTo($"/reviewReport/{reportId}");
		}

		private void GoBack()
		{
			NavigationManager.NavigateTo($"/ManageReviews/{reviewItem.ItemId}");
		}

		private string GetStatusClass(ReviewReportStatus status)
		{
			return status switch
			{
				ReviewReportStatus.Pending => "bg-warning text-dark",
				ReviewReportStatus.Reviewed => "bg-info text-white",
				ReviewReportStatus.Resolved => "bg-success text-white",
				_ => "bg-secondary text-white"
			};
		}

		private string GetStatusText(ReviewReportStatus status)
		{
			return status switch
			{
				ReviewReportStatus.Pending => ReviewResources.Pending,
				ReviewReportStatus.Reviewed => ReviewResources.Reviewed,
				ReviewReportStatus.Resolved => ReviewResources.Resolved,
				_ => status.ToString()
			};
		}

		private string GetReasonText(ReviewReportReason reason)
		{
			return reason switch
			{
				ReviewReportReason.Spam => ReviewResources.Spam,
				ReviewReportReason.OffensiveLanguage => ReviewResources.OffensiveLanguage,
				ReviewReportReason.HateSpeech => ReviewResources.HateSpeech,
				ReviewReportReason.FakeReview => ReviewResources.FakeReview,
				ReviewReportReason.IrrelevantContent => ReviewResources.IrrelevantContent,
				ReviewReportReason.Duplicate => ReviewResources.Duplicate,
				ReviewReportReason.Other => ReviewResources.Other,
				_ => reason.ToString()
			};
		}

		private string GetReviewStatusClass(ReviewStatus status)
		{
			return status switch
			{
				ReviewStatus.Pending => "bg-warning text-dark",
				ReviewStatus.Approved => "bg-success text-white",
				ReviewStatus.Rejected => "bg-danger text-white",
				ReviewStatus.Flagged => "bg-dark text-white",
				_ => "bg-secondary text-white"
			};
		}

		private string GetReviewStatusText(ReviewStatus status)
		{
			return status switch
			{
				ReviewStatus.Pending => ReviewResources.Pending,
				ReviewStatus.Approved => ReviewResources.Approved,
				ReviewStatus.Rejected => ReviewResources.Rejected,
				ReviewStatus.Flagged => ReviewResources.Flagged,
				_ => status.ToString()
			};
		}
	}
}