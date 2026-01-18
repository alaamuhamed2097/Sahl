using Common.Enumerations.Visibility;
using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels;
using Shared.Parameters;

namespace Dashboard.Pages.Catalog.ProductRequests
{
    public partial class Index : BaseListPage<ItemDto>
    {

        protected override string EntityName { get; } = ECommerceResources.NewProductRequests;
        protected override string AddRoute { get; } = $"/product";
        protected override string EditRouteTemplate { get; } = "/product/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Item.SearchNewItemRequests;
		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;


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
        protected void GoToMainList()
        {
            Navigation.NavigateTo("/products");
        }
        protected async Task Accept(Guid itemId)
        {
            try 
            { 
                var updateStatusModel = new UpdateItemVisibilityRequest()
                {
                    ItemId = itemId,
                    VisibilityScope = ProductVisibilityStatus.Visible
                };
                var result = await ItemService.UpdateStatusAsync(updateStatusModel);
                if (result.Success)
                {
                    // Reload the current page data to reflect changes
                    await Search();
                    StateHasChanged();
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.RequestAccepted, "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SomethingWentWrong, "error");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.UnexpectedError, "error");
            }
        }
        protected async Task Reject(Guid itemId)
        {
            try 
            { 
            var updateStatusModel = new UpdateItemVisibilityRequest()
            {
                ItemId = itemId,
                VisibilityScope = ProductVisibilityStatus.Hidden
            };
            await ItemService.UpdateStatusAsync(updateStatusModel);
            var result = await ItemService.UpdateStatusAsync(updateStatusModel);
            if (result.Success)
            {
                // Reload the current page data to reflect changes
                await Search();
                StateHasChanged();
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, NotifiAndAlertsResources.RequestRejected, "success");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SomethingWentWrong, "error");
            }
        }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.UnexpectedError, "error");
            }
}
    }
}