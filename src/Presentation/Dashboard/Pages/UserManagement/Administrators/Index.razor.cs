using Common.Enumerations.User;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;

namespace Dashboard.Pages.UserManagement.Administrators
{
    public partial class Index : BaseListPage<AdminProfileDto>
    {
        protected override string EntityName { get; } = ECommerceResources.Admins;
        protected override string AddRoute { get; } = "/addAdmin";
        protected override string EditRouteTemplate { get; } = $"/editAdmin/{{id}}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Admin.Search;
        protected override Dictionary<string, Func<AdminProfileDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<AdminProfileDto, object>>
        {
            [ECommerceResources.Email] = x => x.Email,
            [ECommerceResources.Name] = x => $"{x.FirstName} {x.LastName}",
            [ECommerceResources.UserState] = x => x.UserState switch
            {
                UserStateType.Active => UserResources.Active,
                UserStateType.Inactive => UserResources.PendingActivation,
                UserStateType.Auto_Locked => UserResources.Banned,
                UserStateType.Restricted => UserResources.Restricted,
                UserStateType.Deleted => FormResources.Deleted,
                _ => x.UserState.ToString()
            }
        };

        [Inject] protected IAdminService adminService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<AdminProfileDto>>> GetAllItemsAsync()
        {
            var result = await adminService.GetAllAsync();
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
            return await adminService.DeleteAsync(id);
        }
        protected override async Task<string> GetItemId(AdminProfileDto item)
        {
            var result = await adminService.GetByIdAsync(Guid.Parse(item.Id));
            if (result.Success)
            {
                return result.Data?.Id.ToString() ?? string.Empty;
            }
            else
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, NotifiAndAlertsResources.NoDataFound);
                return string.Empty;
            }
        }
    }
}
