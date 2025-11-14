using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Microsoft.AspNetCore.Components;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels;

namespace Dashboard.Pages.Catalog.Products
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
            [ECommerceResources.StockStatus] = x => x.StockStatus ? ECommerceResources.InStock : ECommerceResources.OutOfStock,
            // Get Quantity from default combination or first combination
            [ECommerceResources.Quantity] = x => x.ItemAttributeCombinationPricings?.FirstOrDefault(c => c.IsDefault)?.Quantity
                                                  ?? x.ItemAttributeCombinationPricings?.FirstOrDefault()?.Quantity
                                                  ?? 0,
            // Get Price from default combination or first combination using GetPrice() method
            [FormResources.Price] = x => x.GetPrice(),
        };

        [Inject] protected IItemService ItemService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<ItemDto>>> GetAllItemsAsync()
        {
            var result = await ItemService.GetAllAsync();
            if (result.Success)
            {
                return result;
            }
            else
            {
                return result;
            }
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
