using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels;
using Dashboard.Constants;
using Dashboard.Contracts;

namespace Dashboard.Pages.Shipping.Companies
{
    public partial class Index : BaseListPage<ShippingCompanyDto>
    {
        protected override string EntityName { get; } = "ShippingCompany";
        protected override string AddRoute { get; } = "/ShippingCompany";
        protected override string EditRouteTemplate { get; } = $"/ShippingCompany/{{id}}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.ShippingCompany.Search;
        protected override Dictionary<string, Func<ShippingCompanyDto, object>> ExportColumns { get; }
            = new Dictionary<string, Func<ShippingCompanyDto, object>>
            {
                [ECommerceResources.Name] = x => x.Name,
                [FormResources.PhoneNumber] = x => $"{x.PhoneCode}{x.PhoneNumber}",
                [FormResources.Logo] = x => x.LogoImagePath
            };

        [Inject] protected IShippingCompanyService ShippingCompanyService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<ShippingCompanyDto>>> GetAllItemsAsync()
        {
            var result = await ShippingCompanyService.GetAllAsync();
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
            return await ShippingCompanyService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(ShippingCompanyDto item)
        {
            var result = await ShippingCompanyService.GetByIdAsync(item.Id);
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
