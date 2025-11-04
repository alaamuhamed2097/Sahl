//using Microsoft.AspNetCore.Components;
//using Resources;
//using Shared.DTOs.User;
//using UI.Constants;
//using Shared.GeneralModels;
//using UI.Contracts.Location;

//namespace UI.Pages.Location.State
//{
//    public partial class States :BaseListPage<StateDto>
//    {
//        protected override string EntityName { get; } = ECommerceResources.States;
//        protected override string AddRoute { get; } = "/state";
//        protected override string EditRouteTemplate { get; } = $"/state/{{id}}";
//        protected override string SearchEndpoint { get; } = ApiEndpoints.State.Search;
//        protected override Dictionary<string, Func<StateDto, object>> ExportColumns { get; }
//        = new Dictionary<string, Func<StateDto, object>>
//        {
//            [ECommerceResources.Title] = x => x.Title,
//        };

//        [Inject] protected IStateService StateService { get; set; } = null!;
//        protected override async Task<ResponseModel<IEnumerable<StateDto>>> GetAllItemsAsync()
//        {
//            var result = await StateService.GetAllAsync();
//            if (result.Success)
//            {
//                return result;
//            }
//            else
//            {
//                return result;
//            }
//        }
//        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
//        {
//            return await StateService.DeleteAsync(id);
//        }
//        protected override async Task<string> GetItemId(StateDto item)
//        {
//            var result = await StateService.GetByIdAsync(item.Id);
//            if (result.Success)
//            {
//                return result.Data?.Id.ToString() ?? string.Empty;
//            }
//            else
//            {
//                await ShowErrorNotification(NotifiAndAlertsResources.Error, NotifiAndAlertsResources.NoDataFound);
//                return string.Empty;
//            }
//        }
//    }
//}


