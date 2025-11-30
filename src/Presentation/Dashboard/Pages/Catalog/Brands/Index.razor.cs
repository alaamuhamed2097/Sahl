using Dashboard.Contracts.Brand;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Brand;
using Shared.GeneralModels;

namespace Dashboard.Pages.Catalog.Brands
{
    public partial class Index : BaseListPage<BrandDto>
    {
        [Inject] private IBrandService BrandService { get; set; } = null!;

        // Abstract properties implementation
        protected override string EntityName => BrandResources.BrandManagement;
        protected override string AddRoute => $"/brand";
        protected override string EditRouteTemplate => "/brand/{id}";
        protected override string SearchEndpoint => "api/Brand/search";

        // Export columns configuration
        protected override Dictionary<string, Func<BrandDto, object>> ExportColumns => new()
        {
            { BrandResources.BrandName, item => item.Name },
            { $"{FormResources.EnglishName}", item => item.NameEn },
            { $"{FormResources.ArabicName}", item => item.NameAr },
            { BrandResources.WebsiteUrl, item => item.WebsiteUrl ?? "-" },
            { BrandResources.IsFavorite, item => item.IsFavorite ? GeneralResources.Yes : GeneralResources.No },
            { BrandResources.DisplayOrder, item => item.DisplayOrder }
        };

        // Abstract methods implementation
        protected override async Task<ResponseModel<IEnumerable<BrandDto>>> GetAllItemsAsync()
        {
            return await BrandService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            var result = await BrandService.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = result.Success,
                Message = result.Message,
                Data = result.Success,
                Errors = result.Errors
            };
        }

        protected override async Task<string> GetItemId(BrandDto item)
        {
            return await Task.FromResult(item.Id.ToString());
        }

        // Custom methods specific to brands
        protected async Task ToggleFavorite(BrandDto brand)
        {
            try
            {
                var result = await BrandService.MarkAsFavoriteAsync(brand.Id);
                if (result.Success)
                {
                    await ShowSuccessNotification(BrandResources.FavoriteStatusUpdatedSuccessfully);
                    await Search(); // Refresh the list
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Error, result.Message ?? BrandResources.FailedToUpdateFavoriteStatus);
                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(ValidationResources.Error, BrandResources.ErrorUpdatingFavoriteStatus);
            }
        }

        protected async Task OnFilterChanged(ChangeEventArgs e)
        {
            var filterValue = e.Value?.ToString();

            // Reset to show all items first
            await Search();

            if (!string.IsNullOrEmpty(filterValue) && items != null)
            {
                items = filterValue switch
                {
                    "favorites" => items.Where(b => b.IsFavorite),
                    "withWebsite" => items.Where(b => !string.IsNullOrEmpty(b.WebsiteUrl)),
                    "recent" => items.Where(b => b.CreatedDateUtc >= DateTime.UtcNow.AddDays(-30)),
                    _ => items
                };

                // Update pagination info
                totalRecords = items.Count();
                totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                currentPage = 1;
                searchModel.PageNumber = 1;

                StateHasChanged();
            }
        }

        // Override search to handle brand-specific logic if needed
        protected override async Task OnAfterSearchAsync()
        {
            // Add any brand-specific post-search logic here
            await base.OnAfterSearchAsync();
        }

        // Override initialization if needed
        protected override async Task OnCustomInitializeAsync()
        {
            // Add any brand-specific initialization logic here
            searchModel.PageSize = 10;
            await base.OnCustomInitializeAsync();
        }
    }
}