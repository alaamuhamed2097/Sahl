using Common.Enumerations.Review;
using Common.Enumerations.VendorStatus;
using Dashboard.Contracts.Review;
using Dashboard.Services.Review;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Review;

namespace Dashboard.Pages.ManageReviews.VendorsReviews
{
	public partial class Details
	{
		[Parameter] public Guid Id { get; set; }

		[Inject] protected IVendorReviewService VendorReviewService { get; set; } = null!;
		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
		[Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

		private VendorReviewDto? reviewItem;
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
				var reviewResponse = await VendorReviewService.GetReviewByIdAsync(Id);
				if (reviewResponse.Success && reviewResponse.Data != null)
				{
					reviewItem = reviewResponse.Data;
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

		private async Task ApproveReview()
		{

			bool confirmed = await ApproveConfirmNotification();



			if (confirmed)
			{
				var result =  await VendorReviewService.ApproveReviewAsync(reviewItem.Id);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.Successful);
					//await Search();
					await OnAfterDeleteAsync(reviewItem.Id);
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


		private async Task RejectReview()
		{
			bool confirmed = await RejectConfirmNotification();

			if (confirmed)
			{
				var result = await VendorReviewService.RejectReviewAsync(reviewItem.Id);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.Successful);
					//await Search();
					await OnAfterDeleteAsync(reviewItem.Id);
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

		private async Task DeleteReview()
		{
			bool confirmed = await DeleteConfirmNotification();


			if (confirmed)
			{
				var result = await VendorReviewService.DeleteReviewAsync(reviewItem.Id);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
					//await Search();
					await OnAfterDeleteAsync(reviewItem.Id);
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
		protected virtual async Task OnAfterDeleteAsync(Guid id)
		{
			await Task.CompletedTask;
		}
		protected virtual async Task ShowSuccessNotification(string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
		}
		//private async Task ApproveReview()
		//{
		//	if (reviewItem == null) return;

		//	bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
		//		ReviewResources.ConfirmApproveReview);

		//	if (!confirmed) return;

		//	try
		//	{
		//		var response = 

		//		if (response.Success)
		//		{
		//			await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewApprovedSuccessfully);
		//			await LoadData();
		//		}
		//		else
		//		{
		//			await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
		//	}
		//}

		//private async Task RejectReview()
		//{
		//	if (reviewItem == null) return;

		//	bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
		//		ReviewResources.ConfirmRejectReview);

		//	if (!confirmed) return;

		//	try
		//	{
		//		var response = await 

		//		if (response.Success)
		//		{
		//			await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewRejectedSuccessfully);
		//			await LoadData();
		//		}
		//		else
		//		{
		//			await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
		//	}
		//}

		//private async Task DeleteReview()
		//{
		//	if (reviewItem == null) return;

		//	bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
		//		"Are you sure you want to delete this review? This action cannot be undone.");

		//	if (!confirmed) return;

		//	try
		//	{
		//		var response = await 

		//		if (response.Success)
		//		{
		//			await JSRuntime.InvokeVoidAsync("alert", "Review deleted successfully!");
		//			GoBack();
		//		}
		//		else
		//		{
		//			await JSRuntime.InvokeVoidAsync("alert", $"Error: {response.Message}");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		await JSRuntime.InvokeVoidAsync("alert", $"Error: {ex.Message}");
		//	}
		//}

		private void GoBack()
		{
			if (reviewItem?.VendorId != null && reviewItem.VendorId != Guid.Empty)
			{
				NavigationManager.NavigateTo($"/ManageReviews/VendorsReviews/{reviewItem.VendorId}");
			}
			else
			{
				NavigationManager.NavigateTo("/ManageReviews/VendorsReviews");
			}
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

		// Methods for Vendor Status
		private string GetVendorStatusClass(VendorStatus status)
		{
			return status switch
			{
				VendorStatus.Approved => "bg-success",
				VendorStatus.Pending => "bg-warning text-dark",
				//VendorStatus.Suspended => "bg-danger",
				VendorStatus.Rejected => "bg-dark",
				_ => "bg-secondary"
			};
		}

		private string GetVendorStatusText(VendorStatus status)
		{
			return status switch
			{
				VendorStatus.Approved => "نشط",
				VendorStatus.Pending => "قيد المراجعة",
				//VendorStatus.Suspended => "موقوف",
				VendorStatus.Rejected => "مرفوض",
				_ => status.ToString()
			};
		}
	}
}

//using Common.Enumerations.Review;
//using Dashboard.Contracts.Review;
//using Dashboard.Services.Review;
//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using Resources;
//using Shared.DTOs.Review;

//namespace Dashboard.Pages.ManageReviews.VendorsReviews
//{
//	public partial class Details
//	{
//		[Parameter] public Guid Id { get; set; }

//		[Inject] protected IVendorReviewService VendorReviewService { get; set; } = null!;
//		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
//		[Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

//		private VendorReviewDto? reviewItem;
//		private bool isLoading = true;

//		protected override async Task OnInitializedAsync()
//		{
//			await LoadData();
//		}

//		private async Task LoadData()
//		{
//			isLoading = true;
//			try
//			{
//				// Load review details
//				var reviewResponse = await VendorReviewService.GetReviewByIdAsync(Id);
//				if (reviewResponse.Success && reviewResponse.Data != null)
//				{
//					reviewItem = reviewResponse.Data;
//				}
//				else
//				{
//					await JSRuntime.InvokeVoidAsync("alert", reviewResponse.Message);
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

//		private async Task ApproveReview()
//		{
//			if (reviewItem == null) return;

//			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
//				ReviewResources.ConfirmApproveReview);

//			if (!confirmed) return;

//			try
//			{
//				var response = await VendorReviewService.ApproveReviewAsync(reviewItem.Id);

//				if (response.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewApprovedSuccessfully);
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

//		private async Task RejectReview()
//		{
//			if (reviewItem == null) return;

//			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
//				ReviewResources.ConfirmRejectReview);

//			if (!confirmed) return;

//			try
//			{
//				var response = await VendorReviewService.RejectReviewAsync(reviewItem.Id);

//				if (response.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("alert", ReviewResources.ReviewRejectedSuccessfully);
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

//		private async Task DeleteReview()
//		{
//			if (reviewItem == null) return;

//			bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
//				"Are you sure you want to delete this review? This action cannot be undone.");

//			if (!confirmed) return;

//			try
//			{
//				var response = await VendorReviewService.DeleteReviewAsync(reviewItem.Id);

//				if (response.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("alert", "Review deleted successfully!");
//					GoBack();
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

//		private void GoBack()
//		{
//			if (reviewItem?.VendorId != null && reviewItem.VendorId != Guid.Empty)
//			{
//				NavigationManager.NavigateTo($"/ManageReviews/VendorsReviews/{reviewItem.VendorId}");
//			}
//			else
//			{
//				NavigationManager.NavigateTo("/ManageReviews/VendorsReviews");
//			}
//		}

//		private string GetReviewStatusClass(ReviewStatus status)
//		{
//			return status switch
//			{
//				ReviewStatus.Pending => "bg-warning text-dark",
//				ReviewStatus.Approved => "bg-success text-white",
//				ReviewStatus.Rejected => "bg-danger text-white",
//				ReviewStatus.Flagged => "bg-dark text-white",
//				_ => "bg-secondary text-white"
//			};
//		}

//		private string GetReviewStatusText(ReviewStatus status)
//		{
//			return status switch
//			{
//				ReviewStatus.Pending => ReviewResources.Pending,
//				ReviewStatus.Approved => ReviewResources.Approved,
//				ReviewStatus.Rejected => ReviewResources.Rejected,
//				ReviewStatus.Flagged => ReviewResources.Flagged,
//				_ => status.ToString()
//			};
//		}
//	}
//}