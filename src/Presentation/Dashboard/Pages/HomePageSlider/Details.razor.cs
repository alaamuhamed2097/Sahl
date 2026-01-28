using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.HomePageSlider;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.HomeSlider;

namespace Dashboard.Pages.HomePageSlider
{
    public partial class Details : LocalizedComponentBase
    {
        private bool isSaving { get; set; }
        private string baseUrl = string.Empty;
        private IBrowserFile? selectedImage;
        private string? previewImageUrl;
        protected const long MaxFileSize = 5 * 1024 * 1024; // 5MB
		private bool isImageProcessing { get; set; }
		[Inject] protected IHomePageSliderService SliderService { get; set; } = null!;
		protected List<HomePageSliderDto> AllMainBanners { get; set; } = new();
        protected HomePageSliderDto Model { get; set; } = new();
		private string? oldImagePath;

		[Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IHomePageSliderService MainBannerService { get; set; } = null!;
        [Inject] IOptions<ApiSettings> ApiOptions { get; set; } = null!;

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

            if (Id == Guid.Empty)
            {
                Model = new HomePageSliderDto
                {
                    DisplayOrder = GetNextDisplayOrder(),
                    //StartDate = DateTime.UtcNow.Date,
                    //EndDate = DateTime.UtcNow.Date.AddDays(30)
                };
            }
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
            var todayUtc = DateTime.UtcNow.Date;

            // Ensure start date is not in the past (UTC comparison)
            //if (Model.StartDate < todayUtc)
            //{
            //	Model.StartDate = todayUtc;
            //	await ShowWarningNotification(ValidationResources.Warning, ValidationResources.StartDateNotInPast);
            //}

            // Ensure end date is after start date
            //if (Model.EndDate <= Model.StartDate)
            //{
            //	Model.EndDate = Model.StartDate.AddDays(1);
            //}

            StateHasChanged();
        }

        protected async Task OnEndDateChanged()
        {
            // Ensure end date is after start date
            //if (Model.EndDate <= Model.StartDate)
            //{
            //	Model.EndDate = Model.StartDate.AddDays(1);
            //	await ShowWarningNotification(ValidationResources.Warning, ValidationResources.EndDateAfterStartDate);
            //}

            StateHasChanged();
        }

        private bool IsValidDateRange()
        {
            var todayUtc = DateTime.UtcNow.Date;
            //Model.StartDate >= todayUtc && Model.EndDate > Model.StartDate;
            return true;
        }
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

		//protected async Task Save()
		//{
		//    try
		//    {
		//        // Validate date range before saving
		//        if (!IsValidDateRange())
		//        {
		//            await JSRuntime.InvokeVoidAsync("swal",
		//                ValidationResources.Failed,
		//                //ValidationResources.InvalidDateRange,
		//                "error");
		//            return;
		//        }

		//        // Validate image
		//        if (string.IsNullOrEmpty(Model.ImageUrl))
		//        {
		//            await JSRuntime.InvokeVoidAsync("swal",
		//                ValidationResources.Failed,
		//                ValidationResources.ImageRequired,
		//                "error");
		//            return;
		//        }

		//        isSaving = true;
		//        StateHasChanged();

		//        var result = await MainBannerService.SaveAsync(Model);

		//        isSaving = false;
		//        if (result.Success)
		//        {
		//            await JSRuntime.InvokeVoidAsync("swal",
		//                ValidationResources.Done,
		//                NotifiAndAlertsResources.SavedSuccessfully,
		//                "success");

		//            await Task.Delay(1000);
		//            await CloseModal();
		//        }
		//        else
		//        {
		//            await JSRuntime.InvokeVoidAsync("swal",
		//                ValidationResources.Failed,
		//                result.Message ?? NotifiAndAlertsResources.SaveFailed,
		//                "error");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        await JSRuntime.InvokeVoidAsync("swal",
		//            ValidationResources.Error,
		//            ex.Message,
		//            "error");
		//    }
		//    finally
		//    {
		//        isSaving = false;
		//    }
		//}
		protected async Task LoadAllSliders()
		{
			try
			{
				var result = await SliderService.GetAllAsync();
				AllMainBanners = result.Data?.OrderBy(x => x.DisplayOrder).ToList() ?? new List<HomePageSliderDto>();
				StateHasChanged(); // تحديث الواجهة
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading sliders: {ex.Message}");
			}
		}
		private void ResetModel()
		{
			Model = new HomePageSliderDto
			{
				DisplayOrder = GetNextDisplayOrder(),
			};
		}
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
                    await CloseModal();
                    return;
                }

                Model = result.Data ?? new();

                // Ensure dates are properly set for existing records using UTC
                //if (Model.StartDate == default(DateTime))
                //	Model.StartDate = DateTime.UtcNow.Date;
                //if (Model.EndDate == default(DateTime))
                //	Model.EndDate = DateTime.UtcNow.Date.AddDays(30);

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
            // Validate and adjust display order
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
            // Ensure display order is within valid range
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
                Console.WriteLine($"Error loading sliders: {ex.Message}");
                AllMainBanners = new List<HomePageSliderDto>();
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/HomePageSlider");
        }

        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task ShowWarningNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "warning");
        }
    }
}