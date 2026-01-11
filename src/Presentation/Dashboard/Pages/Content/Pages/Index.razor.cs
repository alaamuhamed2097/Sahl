using Dashboard.Contracts.Page;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.Page;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Pages.Content.Pages
{
    public partial class Index : BaseListPage<PageDto>
    {
        [Inject] private IPageService PageService { get; set; } = null!;

        // Abstract properties implementation
        protected override string EntityName => "Pages";
        protected override string AddRoute => $"/content/pages/{Guid.Empty}";
        protected override string EditRouteTemplate => "/content/pages/{id}";
        protected override string SearchEndpoint => "api/Page/search";

        // Export columns configuration
        protected override Dictionary<string, Func<PageDto, object>> ExportColumns => new()
        {
            { "Title (English)", item => item.TitleEn },
            { "Title (Arabic)", item => item.TitleAr },
            { "Short Description", item => item.ShortDescriptionEn },
            { "Created Date", item => item.CreatedDateUtc.ToString("dd-MM-yyyy") },
            { "Modified Date", item => (item.UpdatedDateUtc ?? item.CreatedDateUtc).ToString("dd-MM-yyyy") }
        };

        // Abstract methods implementation
        protected override async Task<ResponseModel<IEnumerable<PageDto>>> GetAllItemsAsync()
        {
            return await PageService.GetAllAsync();
        }


		//protected override async Task<ResponseModel<IEnumerable<PageDto>>> searchItems()
		//{
		//	// استدعاء Search مع Criteria فاضية عشان تجيب كل الداتا
		//	var searchCriteria = new BaseSearchCriteriaModel
		//	{
		//		PageNumber = currentPage,
		//		PageSize = searchModel.PageSize,
		//		SearchTerm = searchModel.SearchTerm,
		//		SortBy = searchModel.SortBy,
		//		SortDirection = searchModel.SortDirection
		//	};

		//	var result = await PageService.SearchAsync(searchCriteria);

		//	if (result.Success && result.Data != null)
		//	{
		//		// تحديث معلومات الـ Pagination
		//		totalRecords = result.Data.TotalCount;
		//		totalPages = result.Data.TotalPages;

		//		return new ResponseModel<IEnumerable<PageDto>>
		//		{
		//			Success = true,
		//			Message = result.Message,
		//			Data = result.Data.Items
		//		};
		//	}

		//	return new ResponseModel<IEnumerable<PageDto>>
		//	{
		//		Success = false,
		//		Message = result.Message ?? "Failed to retrieve data",
		//		Data = Enumerable.Empty<PageDto>()
		//	};
		//}
		
        
        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await PageService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(PageDto item)
        {
            return await Task.FromResult(item.Id.ToString());
        }

        // Custom initialization for Pages
        protected override async Task OnCustomInitializeAsync()
        {
            searchModel.PageSize = 10;
            await base.OnCustomInitializeAsync();
        }

        // Custom actions for pages
        protected async Task Preview(PageDto page)
        {
            try
            {
                // Show only the content based on current language
                var content = page.Content ?? string.Empty;

                await JSRuntime.InvokeVoidAsync("showPreviewModal", page.Title, content);
            }
            catch (Exception)
            {
                await ShowErrorNotification("Error", "Failed to preview page");
            }
        }
		protected void Add()
		{
			Navigation.NavigateTo("/pages/add");
		}
		// Navigation methods
		protected void EditPage(PageDto page)
        {
            Navigation.NavigateTo($"/pages/{page.Id}");
        }

        protected void ViewPage(PageDto page)
        {
            // Navigate to the actual page view - you might need to implement this route
            Navigation.NavigateTo($"/page-view/{page.Id}");
        }

        // Method to get truncated content for display
        protected string GetTruncatedContent(string content, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(content))
                return "-";

            return content.Length <= maxLength
                ? content
                : content.Substring(0, maxLength) + "...";
        }

        // Override delete to add page-specific confirmation
        protected override async Task Delete(Guid id)
        {
            var page = items?.FirstOrDefault(p => p.Id == id);
            if (page == null) return;

            var confirmed = await ShowConfirmDialog(
                "Delete Page",
                $"Are you sure you want to delete the page '{page.Title}'? This action cannot be undone.",
                "Delete",
                "Cancel");

            if (confirmed)
            {
                var result = await DeleteItemAsync(id);
                if (result.Success)
                {
                    await ShowSuccessNotification("Page deleted successfully");
                    await Search();
                }
                else
                {
                    await ShowErrorNotification("Error", result.Message ?? "Failed to delete page");
                }
            }
        }
    }
}