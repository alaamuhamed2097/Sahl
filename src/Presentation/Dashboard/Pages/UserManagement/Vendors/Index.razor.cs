using Common.Enumerations.VendorType;
using Dashboard.Constants;
using Dashboard.Contracts.Vendor;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;

namespace Dashboard.Pages.UserManagement.Vendors
{
    public partial class Index : BaseListPage<VendorDto>
    {
        protected override string EntityName { get; } = ECommerceResources.Vendors;
        protected override string AddRoute { get; } = "/users/vendors/create";
        protected override string EditRouteTemplate { get; } = "/users/vendors/edit/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Vendor.Search;
        protected override Dictionary<string, Func<VendorDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<VendorDto, object>>
        {
            [ECommerceResources.StoreName] = x => x.StoreName,
            [ECommerceResources.ContactName] = x => x.AdministratorFullName,
            [ECommerceResources.VendorType] = x => x.VendorType switch
            {
                VendorType.Company => UserResources.Company,
                VendorType.Individual => UserResources.Individual,
                _ => x.VendorType.ToString()
            },
            [UserResources.RegistrationDate] = x => x.RegistrationDate.ToString("dd/MM/yyyy"),
            [ECommerceResources.Rating] = x => x.AverageRating.HasValue ? x.AverageRating.Value.ToString("0.0") : ECommerceResources.NoRating
        };

        [Inject] protected IVendorService _vendorService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<VendorDto>>> GetAllItemsAsync()
        {
            var result = await _vendorService.GetAllAsync();
            if (result.Success)
            {
                return result;
            }
            else
            {
                return result;
            }
        }

        protected override async Task<string> GetItemId(VendorDto item)
        {
            var result = await _vendorService.GetByIdAsync(item.Id);
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
        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await _vendorService.DeleteAsync(id);
        }


    }
}
