using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Microsoft.AspNetCore.Components;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels;

namespace Dashboard.Pages.Catalog.VendorItems
{
    public partial class Index : BaseListPage<ItemDto>
    {
        protected override string EntityName { get; } = ECommerceResources.Products;
        protected override string AddRoute { get; } = $"/product";
        protected override string EditRouteTemplate { get; } = "/product/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Item.Search;

        protected override Dictionary<string, Func<ItemDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<ItemDto, object>>
        {
            [FormResources.Image] = x => $"{baseUrl}/{x.ThumbnailImage}",
            [FormResources.Title] = x => ResourceManager.CurrentLanguage == Language.Arabic ? x.TitleAr : x.TitleEn,
            [FormResources.Price] = x => x.BasePrice != null ? $"{x.BasePrice?.ToString("F2")} EGP" : "N/A",
        };

        [Inject] protected IItemService ItemService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<ItemDto>>> GetAllItemsAsync()
        {
            var result = await ItemService.GetAllAsync();
            return result;
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await ItemService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(ItemDto item)
        {
            return item.Id.ToString();
        }

        protected void GoToImport()
        {
            Navigation.NavigateTo("/products/import");
        }
    }
}