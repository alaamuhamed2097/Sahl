//using Dashboard.Configuration;
//using Dashboard.Contracts.General;
//using Dashboard.Contracts.HomePageSlider;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using Microsoft.Extensions.Options;
//using Microsoft.JSInterop;
//using Resources;
//using Shared.DTOs.ECommerce;
//using Shared.DTOs.HomeSlider;


//namespace Dashboard.Pages.HomePageSlider
//{
//	public partial class Index
//	{
//		private bool isSaving { get; set; }
//		private string baseUrl = string.Empty;
//		private IBrowserFile? selectedImage;
//		private string? previewImageUrl;
//		protected const long MaxFileSize = 5 * 1024 * 1024; // 5MB
//		protected List<HomePageSliderDto> AllMainBanners { get; set; } = new();
//		protected HomePageSliderDto Model { get; set; } = new();

//		[Parameter] public Guid Id { get; set; }

//		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
//		[Inject] protected NavigationManager Navigation { get; set; } = null!;
//		[Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
//		[Inject] protected IHomePageSliderService MainBannerService { get; set; } = null!;
//		[Inject] IOptions<ApiSettings> ApiOptions { get; set; } = null!;

//		protected override async Task OnParametersSetAsync()
//		{
//			if (Id != Guid.Empty)
//			{
//				previewImageUrl = null;
//				await Edit(Id);
//			}
//		}

//		protected override async Task OnInitializedAsync()
//		{
//			baseUrl = ApiOptions.Value.BaseUrl;
//			await LoadAllMainBanners();
//			Model = new HomePageSliderDto
//			{
//				DisplayOrder = GetNextDisplayOrder(),
//				//StartDate = DateTime.UtcNow.Date, // UTC date
//				//EndDate = DateTime.UtcNow.Date.AddDays(30) // UTC date
//			};
//		}

//		protected override Task OnAfterRenderAsync(bool firstRender)
//		{
//			if (firstRender)
//			{
//				ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
//			}
//			return Task.CompletedTask;
//		}

//		// ========== Date Validation ==========
//		protected async Task OnStartDateChanged()
//		{
//			//var todayUtc = DateTime.UtcNow.Date;

//			//// Ensure start date is not in the past (UTC comparison)
//			//if (Model.StartDate < todayUtc)
//			//{
//			//	Model.StartDate = todayUtc;
//			//	//await ShowErrorNotification(ValidationResources.Error, ValidationResources.StartDateNotInPast);
//			//}

//			//// Ensure end date is after start date
//			//if (Model.EndDate <= Model.StartDate)
//			//{
//			//	Model.EndDate = Model.StartDate.AddDays(1);
//			//}

//			StateHasChanged();
//		}

//		protected async Task OnEndDateChanged()
//		{
//			//// Ensure end date is after start date
//			//if (Model.EndDate <= Model.StartDate)
//			//{
//			//	Model.EndDate = Model.StartDate.AddDays(1);
//			//	//await ShowErrorNotification(ValidationResources.Error, ValidationResources.EndDateAfterStartDate);
//			//}

//			StateHasChanged();
//		}

//		//private bool IsValidDateRange()
//		//{
//		//	var todayUtc = DateTime.UtcNow.Date;
//		//	return Model.StartDate >= todayUtc && Model.EndDate > Model.StartDate;
//		//}

//		protected async Task Save()
//		{
//			try
//			{
//				//// Validate date range before saving
//				//if (!IsValidDateRange())
//				//{
//				//	await JSRuntime.InvokeVoidAsync("swal",
//				//		ValidationResources.Failed,
//				//		"Please ensure start date is today or future and end date is after start date",
//				//		"error");
//				//	return;
//				//}

//				isSaving = true;
//				StateHasChanged(); // Force UI update to show spinner

//				var result = await MainBannerService.SaveAsync(Model);

//				isSaving = false;
//				if (result.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully, "success");
//				}
//				else
//				{
//					await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
//				}
//			}
//			catch (Exception ex)
//			{
//				await JSRuntime.InvokeVoidAsync("swal",
//					NotifiAndAlertsResources.FailedAlert,
//					"error");
//			}
//			finally
//			{
//				await CloseModal();
//			}
//		}

//		protected async Task Edit(Guid id)
//		{
//			try
//			{
//				var result = await MainBannerService.GetByIdAsync(id);

//				if (!result.Success)
//				{
//					await JSRuntime.InvokeVoidAsync("swal",
//						ValidationResources.Failed,
//						NotifiAndAlertsResources.FailedToRetrieveData,
//						"error");
//					return;
//				}

//				Model = result.Data ?? new();

//				//// Ensure dates are properly set for existing records using UTC
//				//if (Model.StartDate == default(DateTime))
//				//	Model.StartDate = DateTime.UtcNow.Date;
//				//if (Model.EndDate == default(DateTime))
//				//	Model.EndDate = DateTime.UtcNow.Date.AddDays(30);

//				StateHasChanged();
//			}
//			catch (Exception ex)
//			{
//				await JSRuntime.InvokeVoidAsync("swal",
//					ValidationResources.Error,
//					ex.Message,
//					"error");
//			}
//		}

//		// ========== Display Order Management ==========
//		private int GetCategoryCurrentIndex()
//		{
//			return Model.DisplayOrder;
//		}

//		private void OnDisplayOrderChanged()
//		{
//			// Validate and adjust display order
//			var maxOrder = AllMainBanners.Any() ? AllMainBanners.Max(c => c.DisplayOrder) : 0;

//			if (Model.DisplayOrder <= 0)
//			{
//				Model.DisplayOrder = 1;
//			}
//			else if (Model.DisplayOrder > maxOrder + 1)
//			{
//				Model.DisplayOrder = maxOrder + 1;
//			}

//			StateHasChanged();
//		}

//		private int GetNextDisplayOrder()
//		{
//			return AllMainBanners.Any() ? AllMainBanners.Max(c => c.DisplayOrder) + 1 : 1;
//		}

//		private async Task ValidateAndAdjustDisplayOrder()
//		{
//			// Ensure display order is within valid range
//			var maxOrder = AllMainBanners.Any() ? AllMainBanners.Max(c => c.DisplayOrder) : 0;

//			if (Model.DisplayOrder > maxOrder + 1)
//			{
//				Model.DisplayOrder = maxOrder + 1;
//			}

//			if (Model.DisplayOrder <= 0)
//			{
//				Model.DisplayOrder = 1;
//			}
//		}

//		// ========== Image Handling ==========
//		private async Task HandleSelectedImage(InputFileChangeEventArgs e)
//		{
//			if (isSaving) return; // Prevent multiple simultaneous operations

//			isSaving = true;
//			StateHasChanged(); // Update UI immediately

//			try
//			{
//				var success = await HandleFileUploadWithResult(e.File, "image");
//				if (success)
//				{
//					selectedImage = e.File;
//					// previewImageUrl and Model.ImageUrl are set in HandleFileUploadWithResult
//				}
//			}
//			catch (Exception ex)
//			{
//				await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
//				// Log the exception properly
//				Console.WriteLine($"Error in HandleSelectedImage: {ex.Message}");
//			}
//			finally
//			{
//				isSaving = false;
//				StateHasChanged(); // Ensure UI reflects the final state
//			}
//		}

//		private async Task<bool> HandleFileUploadWithResult(IBrowserFile file, string fileType)
//		{
//			try
//			{
//				if (!IsValidImageFile(file))
//				{
//					await ShowErrorNotification(NotifiAndAlertsResources.Error, "Invalid image file type");
//					return false;
//				}

//				var (isValid, errorMessage) = await ValidateFileSize(file);
//				if (!isValid)
//				{
//					await ShowErrorNotification(NotifiAndAlertsResources.Error, errorMessage);
//					return false;
//				}

//				var (previewUrl, base64Data) = await ProcessImageFile(file);
//				if (!string.IsNullOrEmpty(previewUrl) && !string.IsNullOrEmpty(base64Data))
//				{
//					previewImageUrl = previewUrl;
//					Model.ImageUrl = base64Data;
//					return true;
//				}
//				else
//				{
//					await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
//					return false;
//				}
//			}
//			catch (Exception ex)
//			{
//				await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
//				Console.WriteLine($"Error processing {fileType}: {ex.Message}");
//				return false;
//			}
//		}

//		private bool IsValidImageFile(IBrowserFile file)
//		{
//			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
//			var extension = Path.GetExtension(file.Name).ToLower();

//			if (!allowedExtensions.Contains(extension))
//			{
//				_ = ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.InvalidImageFormat);
//				return false;
//			}

//			return true;
//		}

//		private async Task<(bool isValid, string errorMessage)> ValidateFileSize(IBrowserFile file)
//		{
//			if (file.Size > MaxFileSize)
//			{
//				return (false, string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / (1024 * 1024)));
//			}

//			return (true, string.Empty);
//		}

//		private async Task<(string previewUrl, string base64Data)> ProcessImageFile(IBrowserFile file)
//		{
//			using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize);
//			using var ms = new MemoryStream();
//			await stream.CopyToAsync(ms);

//			var imageBytes = ms.ToArray();
//			var base64 = Convert.ToBase64String(imageBytes);

//			var previewUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, file.ContentType, 1920, 920);
//			var base64Data = previewUrl?.Replace($"data:{file.ContentType};base64,", "") ?? string.Empty;

//			return (previewUrl ?? string.Empty, base64Data);
//		}

//		protected async Task LoadAllMainBanners()
//		{
//			try
//			{
//				var result = await MainBannerService.GetAllAsync();
//				AllMainBanners = result.Data?.OrderBy(x => x.DisplayOrder).ToList() ?? new List<HomePageSliderDto>();
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"Error loading categories: {ex.Message}");
//				AllMainBanners = new List<HomePageSliderDto>();
//			}
//		}

//		protected async Task CloseModal()
//		{
//			Navigation.NavigateTo("/HomePageSlider");
//		}

//		private async Task ShowErrorNotification(string title, string message)
//		{
//			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
//		}
//	}
//}
using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.HomePageSlider;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;

namespace Dashboard.Pages.HomePageSlider
{
	public partial class Index
	{
		private bool isSaving { get; set; }
		private string baseUrl = string.Empty;
		private IBrowserFile? selectedImage;
		private string? previewImageUrl;
		protected const long MaxFileSize = 5 * 1024 * 1024; // 5MB
		protected List<HomePageSliderDto> AllMainBanners { get; set; } = new();
		protected HomePageSliderDto Model { get; set; } = new();

		[Parameter] public Guid Id { get; set; }

		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] protected NavigationManager Navigation { get; set; } = null!;
		[Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
		[Inject] protected IHomePageSliderService MainBannerService { get; set; } = null!;
		[Inject] IOptions<ApiSettings> ApiOptions { get; set; } = null!;

		// ========== Abstract Members Implementation ==========
		protected override string EntityName => ECommerceResources.MainBanners;

		protected override string AddRoute => "/HomePageSlider/Add";

		protected override string EditRouteTemplate => "/HomePageSlider/Edit/{0}";

		protected override string SearchEndpoint => "api/HomePageSlider/Search"; // غير الـ endpoint حسب الـ API بتاعك

		protected override Dictionary<string, Func<HomePageSliderDto, object>> ExportColumns => new()
		{
			{ FormResources.Title, item => item.Title ?? string.Empty },
			{ "Display Order", item => item.DisplayOrder },
			//{ "Created Date", item => item.CreatedDateUtc.ToString("yyyy-MM-dd") }
		};

		protected override Task<string> GetItemId(HomePageSliderDto item)
		{
			return Task.FromResult(item.Id.ToString());
		}

		protected override async Task<ResponseModel<IEnumerable<HomePageSliderDto>>> GetAllItemsAsync()
		{
			var result = await MainBannerService.GetAllAsync();

			if (result.Success && result.Data != null)
			{
				var orderedData = result.Data.OrderBy(x => x.DisplayOrder);
				return new ResponseModel<IEnumerable<HomePageSliderDto>>
				{
					Success = true,
					Data = orderedData
				};
			}

			return new ResponseModel<IEnumerable<HomePageSliderDto>>
			{
				Success = false,
				Data = Enumerable.Empty<HomePageSliderDto>()
			};
		}

		//protected override async Task<bool> DeleteItemAsync(Guid id)
		//{
		//	try
		//	{
		//		var result = await MainBannerService.DeleteAsync(id);

		//		if (result.Success)
		//		{
		//			await JSRuntime.InvokeVoidAsync("swal",
		//				ValidationResources.Done,
		//				NotifiAndAlertsResources.DeletedSuccessfully,
		//				"success");
		//			return true;
		//		}
		//		else
		//		{
		//			await JSRuntime.InvokeVoidAsync("swal",
		//				ValidationResources.Failed,
		//				NotifiAndAlertsResources.DeleteFailed,
		//				"error");
		//			return false;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		await JSRuntime.InvokeVoidAsync("swal",
		//			ValidationResources.Error,
		//			ex.Message,
		//			"error");
		//		return false;
		//	}
		//}

		// ========== Lifecycle Methods ==========
		protected override async Task OnParametersSetAsync()
		{
			if (Id != Guid.Empty)
			{
				previewImageUrl = null;
				await Edit(Id);
			}
		}

		protected override async Task OnInitializedAsync()
		{
			baseUrl = ApiOptions.Value.BaseUrl;
			await LoadAllMainBanners();
			Model = new HomePageSliderDto
			{
				DisplayOrder = GetNextDisplayOrder(),
			};
		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
			}
			return Task.CompletedTask;
		}

		// ========== Date Validation ==========
		protected async Task OnStartDateChanged()
		{
			StateHasChanged();
		}

		protected async Task OnEndDateChanged()
		{
			StateHasChanged();
		}
		protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
		{
			try
			{
				var result = await MainBannerService.DeleteAsync(id);

				if (result.Success)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Done,
						NotifiAndAlertsResources.DeletedSuccessfully,
						"success");

					return new ResponseModel<bool>
					{
						Success = true,
						Data = true
					};
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						NotifiAndAlertsResources.DeleteFailed,
						"error");

					return new ResponseModel<bool>
					{
						Success = false,
						Data = false,
						Message = NotifiAndAlertsResources.DeleteFailed
					};
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal",
					ValidationResources.Error,
					ex.Message,
					"error");

				return new ResponseModel<bool>
				{
					Success = false,
					Data = false,
					Message = ex.Message
				};
			}
		}

		protected async Task Save()
		{
			try
			{
				isSaving = true;
				StateHasChanged();

				var result = await MainBannerService.SaveAsync(Model);

				isSaving = false;
				if (result.Success)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Done,
						NotifiAndAlertsResources.SavedSuccessfully,
						"success");

					// Refresh the list
					await LoadAllMainBanners();
					StateHasChanged();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						NotifiAndAlertsResources.SaveFailed,
						"error");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal",
					NotifiAndAlertsResources.FailedAlert,
					"error");
			}
			finally
			{
				isSaving = false;
				await CloseModal();
			}
		}
		//protected async Task Save()
		//{
		//	try
		//	{
		//		isSaving = true;
		//		StateHasChanged();

		//		var result = await MainBannerService.SaveAsync(Model);

		//		isSaving = false;
		//		if (result.Success)
		//		{
		//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.SavedSuccessfully, "success");
		//			await LoadData(); // Refresh the list
		//		}
		//		else
		//		{
		//			await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		await JSRuntime.InvokeVoidAsync("swal",
		//			NotifiAndAlertsResources.FailedAlert,
		//			"error");
		//	}
		//	finally
		//	{
		//		await CloseModal();
		//	}
		//}

		protected async Task Edit(Guid id)
		{
			try
			{
				var result = await MainBannerService.GetByIdAsync(id);

				if (!result.Success)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						NotifiAndAlertsResources.FailedToRetrieveData,
						"error");
					return;
				}

				Model = result.Data ?? new();
				StateHasChanged();
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal",
					ValidationResources.Error,
					ex.Message,
					"error");
			}
		}

		// ========== Display Order Management ==========
		private int GetCategoryCurrentIndex()
		{
			return Model.DisplayOrder;
		}

		private void OnDisplayOrderChanged()
		{
			var maxOrder = AllMainBanners.Any() ? AllMainBanners.Max(c => c.DisplayOrder) : 0;

			if (Model.DisplayOrder <= 0)
			{
				Model.DisplayOrder = 1;
			}
			else if (Model.DisplayOrder > maxOrder + 1)
			{
				Model.DisplayOrder = maxOrder + 1;
			}

			StateHasChanged();
		}

		private int GetNextDisplayOrder()
		{
			return AllMainBanners.Any() ? AllMainBanners.Max(c => c.DisplayOrder) + 1 : 1;
		}

		private async Task ValidateAndAdjustDisplayOrder()
		{
			var maxOrder = AllMainBanners.Any() ? AllMainBanners.Max(c => c.DisplayOrder) : 0;

			if (Model.DisplayOrder > maxOrder + 1)
			{
				Model.DisplayOrder = maxOrder + 1;
			}

			if (Model.DisplayOrder <= 0)
			{
				Model.DisplayOrder = 1;
			}
		}

		// ========== Image Handling ==========
		private async Task HandleSelectedImage(InputFileChangeEventArgs e)
		{
			if (isSaving) return;

			isSaving = true;
			StateHasChanged();

			try
			{
				var success = await HandleFileUploadWithResult(e.File, "image");
				if (success)
				{
					selectedImage = e.File;
				}
			}
			catch (Exception ex)
			{
				await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
				Console.WriteLine($"Error in HandleSelectedImage: {ex.Message}");
			}
			finally
			{
				isSaving = false;
				StateHasChanged();
			}
		}

		private async Task<bool> HandleFileUploadWithResult(IBrowserFile file, string fileType)
		{
			try
			{
				if (!IsValidImageFile(file))
				{
					await ShowErrorNotification(NotifiAndAlertsResources.Error, "Invalid image file type");
					return false;
				}

				var (isValid, errorMessage) = await ValidateFileSize(file);
				if (!isValid)
				{
					await ShowErrorNotification(NotifiAndAlertsResources.Error, errorMessage);
					return false;
				}

				var (previewUrl, base64Data) = await ProcessImageFile(file);
				if (!string.IsNullOrEmpty(previewUrl) && !string.IsNullOrEmpty(base64Data))
				{
					previewImageUrl = previewUrl;
					Model.ImageUrl = base64Data;
					return true;
				}
				else
				{
					await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
					return false;
				}
			}
			catch (Exception ex)
			{
				await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.ErrorProcessingImage);
				Console.WriteLine($"Error processing {fileType}: {ex.Message}");
				return false;
			}
		}

		private bool IsValidImageFile(IBrowserFile file)
		{
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
			var extension = Path.GetExtension(file.Name).ToLower();

			if (!allowedExtensions.Contains(extension))
			{
				_ = ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.InvalidImageFormat);
				return false;
			}

			return true;
		}

		private async Task<(bool isValid, string errorMessage)> ValidateFileSize(IBrowserFile file)
		{
			if (file.Size > MaxFileSize)
			{
				return (false, string.Format(ValidationResources.ImageSizeLimitExceeded, MaxFileSize / (1024 * 1024)));
			}

			return (true, string.Empty);
		}

		private async Task<(string previewUrl, string base64Data)> ProcessImageFile(IBrowserFile file)
		{
			using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize);
			using var ms = new MemoryStream();
			await stream.CopyToAsync(ms);

			var imageBytes = ms.ToArray();
			var base64 = Convert.ToBase64String(imageBytes);

			var previewUrl = await JSRuntime.InvokeAsync<string>("resizeImage", base64, file.ContentType, 1920, 920);
			var base64Data = previewUrl?.Replace($"data:{file.ContentType};base64,", "") ?? string.Empty;

			return (previewUrl ?? string.Empty, base64Data);
		}

		protected async Task LoadAllMainBanners()
		{
			try
			{
				var result = await MainBannerService.GetAllAsync();
				AllMainBanners = result.Data?.OrderBy(x => x.DisplayOrder).ToList() ?? new List<HomePageSliderDto>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading categories: {ex.Message}");
				AllMainBanners = new List<HomePageSliderDto>();
			}
		}
		

		protected void Add()
		{
			Navigation.NavigateTo("/HomePageSlider/Add");
		}

		protected async Task CloseModal()
		{
			Navigation.NavigateTo("/HomePageSlider");
		}

		private async Task ShowErrorNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
		}
	}
}