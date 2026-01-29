using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.HomePageSlider;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;

namespace Dashboard.Pages.HomePageSlider
{
	public partial class Index : BaseListPage<HomePageSliderDto>
	{
		private bool isSaving { get; set; }
		private bool isImageProcessing { get; set; }
		private string baseUrl = string.Empty;
		private IBrowserFile? selectedImage;
		private string? previewImageUrl;
		private string? oldImagePath;
		protected const long MaxFileSize = 5 * 1024 * 1024; // 5MB
		protected List<HomePageSliderDto> AllSliders { get; set; } = new();
		protected HomePageSliderDto Model { get; set; } = new();

		[Parameter] public Guid Id { get; set; }

		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
		[Inject] protected NavigationManager Navigation { get; set; } = null!;
		[Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
		[Inject] protected IHomePageSliderService SliderService { get; set; } = null!;
		[Inject] IOptions<ApiSettings> ApiOptions { get; set; } = null!;

		// ========== Abstract Members Implementation ==========
		protected override string EntityName => "HomePageSlider";
		protected override string AddRoute => "/HomePageSlider/Add";
		protected override string EditRouteTemplate => "/HomePageSlider/Edit/{0}";
		protected override string SearchEndpoint => "api/v1/HomePageSlider/Search";

		protected override Dictionary<string, Func<HomePageSliderDto, object>> ExportColumns => new()
		{
			{ FormResources.Title, item => item.TitleAr ?? string.Empty },
			{ "Display Order", item => item.DisplayOrder }
		};

		protected override Task<string> GetItemId(HomePageSliderDto item)
		{
			return Task.FromResult(item.Id.ToString());
		}

		protected override async Task<ResponseModel<IEnumerable<HomePageSliderDto>>> GetAllItemsAsync()
		{
			var result = await SliderService.GetAllAsync();

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

		protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
		{
			try
			{
				var result = await SliderService.DeleteAsync(id);

				if (result.Success)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Done,
						NotifiAndAlertsResources.DeletedSuccessfully,
						"success");

					await LoadAllSliders();

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
						result.Message ?? NotifiAndAlertsResources.DeleteFailed,
						"error");

					return new ResponseModel<bool>
					{
						Success = false,
						Data = false,
						Message = result.Message ?? NotifiAndAlertsResources.DeleteFailed
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

		// ========== Lifecycle Methods ==========
		protected override async Task OnParametersSetAsync()
		{
			if (Id != Guid.Empty)
			{
				previewImageUrl = null;
				oldImagePath = null;
				await Edit(Id);
			}
		}

		protected override async Task OnInitializedAsync()
		{
			baseUrl = ApiOptions.Value.BaseUrl;
			await LoadAllSliders();
			ResetModel();
		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				ResourceLoaderService.LoadScript("Common/imageHandler/imageHandler.js");
			}
			return Task.CompletedTask;
		}

		// ========== CRUD Operations ==========
		protected async Task Save()
		{
			if (isSaving || isImageProcessing) return;

			try
			{
				isSaving = true;
				StateHasChanged();

				// Validate required fields
				if (string.IsNullOrWhiteSpace(Model.ImageUrl))
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						ValidationResources.ImageRequired,
						"error");
					return;
				}

				if (string.IsNullOrWhiteSpace(Model.TitleAr))
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						"Title is required",
						"error");
					return;
				}

				// Validate and adjust display order
				await ValidateAndAdjustDisplayOrder();

				var result = await SliderService.SaveAsync(Model);

				if (result.Success)
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Done,
						NotifiAndAlertsResources.SavedSuccessfully,
						"success");

					await LoadAllSliders();

					ResetModel();
					previewImageUrl = null;
					oldImagePath = null;
					selectedImage = null;

					StateHasChanged();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("swal",
						ValidationResources.Failed,
						result.Message ?? NotifiAndAlertsResources.SaveFailed,
						"error");
				}
			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("swal",
					ValidationResources.Error,
					ex.Message,
					"error");
			}
			finally
			{
				isSaving = false;
				await CloseModal();
			}
		}

		protected async Task Edit(Guid id)
		{
			//try
			//{
				//var result = await SliderService.GetByIdAsync(id);
				Navigation.NavigateTo($"/HomePageSlider/Edit/{id}");

			//	if (!result.Success || result.Data == null)
			//	{
			//		await JSRuntime.InvokeVoidAsync("swal",
			//			ValidationResources.Failed,
			//			NotifiAndAlertsResources.FailedToRetrieveData,
			//			"error");
			//		return;
			//	}

			//	Model = result.Data;

			//	oldImagePath = Model.ImageUrl;

			//	if (!string.IsNullOrEmpty(Model.ImageUrl))
			//	{
			//		previewImageUrl = $"{baseUrl}/{Model.ImageUrl}";
			//	}

			//	StateHasChanged();
			//}
			//catch (Exception ex)
			//{
			//	await JSRuntime.InvokeVoidAsync("swal",
			//		ValidationResources.Error,
			//		ex.Message,
			//		"error");
			//}
		}

		// ========== Image Handling ==========
		private async Task HandleSelectedImage(InputFileChangeEventArgs e)
		{
			if (isSaving || isImageProcessing) return;

			isImageProcessing = true;
			StateHasChanged();

			try
			{
				var success = await HandleFileUploadWithResult(e.File);
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
				isImageProcessing = false;
				StateHasChanged();
			}
		}

		private async Task<bool> HandleFileUploadWithResult(IBrowserFile file)
		{
			try
			{
				if (!IsValidImageFile(file))
				{
					await ShowErrorNotification(NotifiAndAlertsResources.Error, ValidationResources.InvalidImageFormat);
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
				Console.WriteLine($"Error processing image: {ex.Message}");
				return false;
			}
		}

		private bool IsValidImageFile(IBrowserFile file)
		{
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
			var extension = Path.GetExtension(file.Name).ToLower();

			if (!allowedExtensions.Contains(extension))
			{
				return false;
			}

			var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
			if (!allowedMimeTypes.Contains(file.ContentType))
			{
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

		private async Task RemoveImage()
		{
			if (isSaving || isImageProcessing) return;

			try
			{
				var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to remove this image?");

				if (confirmed)
				{
					previewImageUrl = null;
					Model.ImageUrl = string.Empty;
					selectedImage = null;

					StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error removing image: {ex.Message}");
			}
		}

		// ========== Display Order Management ==========
		private void OnDisplayOrderChanged()
		{
			var maxOrder = AllSliders.Any() ? AllSliders.Max(c => c.DisplayOrder) : 0;

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
			return AllSliders.Any() ? AllSliders.Max(c => c.DisplayOrder) + 1 : 1;
		}

		private async Task ValidateAndAdjustDisplayOrder()
		{
			var maxOrder = AllSliders.Any() ? AllSliders.Max(c => c.DisplayOrder) : 0;

			if (Model.DisplayOrder > maxOrder + 1)
			{
				Model.DisplayOrder = maxOrder + 1;
			}

			if (Model.DisplayOrder <= 0)
			{
				Model.DisplayOrder = 1;
			}
		}
		protected override async Task Delete(Guid id)
		{
			var confirmed = await DeleteConfirmNotification();

			if (confirmed)
			{
				var result = await SliderService.DeleteAsync(id);
				if (result.Success)
				{
					await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
					await Search();
					await OnAfterDeleteAsync(id);
					StateHasChanged();
				}
				else
				{
					if (result.Errors.Any())
						await ShowErrorNotification(ValidationResources.Failed, string.Join(",", result.Errors));
					else
						await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.DeleteFailed);
				}
			}
		}
		// ========== Helper Methods ==========
		protected async Task LoadAllSliders()
		{
			try
			{
				var result = await SliderService.GetAllAsync();
				AllSliders = result.Data?.OrderBy(x => x.DisplayOrder).ToList() ?? new List<HomePageSliderDto>();
				items = AllSliders;
				StateHasChanged(); // تحديث الواجهة
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading sliders: {ex.Message}");
				AllSliders = new List<HomePageSliderDto>();
			}
		}

		protected void Add()
		{
			ResetModel();
			Navigation.NavigateTo("/HomePageSlider/Add");
		}

		protected async Task CloseModal()
		{
			Navigation.NavigateTo("/HomePageSlider");
		}

		private void ResetModel()
		{
			Model = new HomePageSliderDto
			{
				DisplayOrder = GetNextDisplayOrder(),
			};
		}

		private async Task ShowErrorNotification(string title, string message)
		{
			await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
		}
	}
}