using Common.Enumerations.FieldType;
using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Category;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels;

namespace Dashboard.Pages.Catalog.Attributes
{
    public partial class Index : BaseListPage<AttributeDto>
    {
        [Inject] protected IAttributeService AttributeService { get; set; } = null!;

        protected override string EntityName { get; } = "Attributes";
        protected override string AddRoute { get; } = $"/attribute";
        protected override string EditRouteTemplate { get; } = "/attribute/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Attribute.Search;
        protected override Dictionary<string, Func<AttributeDto, object>> ExportColumns { get; }
     = new Dictionary<string, Func<AttributeDto, object>>
     {
         [ECommerceResources.Title] = x => x.Title,
         [ECommerceResources.FieldType] = x => x.FieldType switch
         {
             FieldType.Text => ECommerceResources.Text,
             FieldType.IntegerNumber => ECommerceResources.IntegerNumber,
             FieldType.DecimalNumber => ECommerceResources.DecimalNumber,
             FieldType.Date => ECommerceResources.Date,
             FieldType.CheckBox => ECommerceResources.CheckBox,
             FieldType.List => ECommerceResources.List,
             FieldType.MultiSelectList => ECommerceResources.MultiSelectList,
             _ => x.FieldType.ToString()
         },
     };

        protected override async Task<ResponseModel<IEnumerable<AttributeDto>>> GetAllItemsAsync()
        {
            return await AttributeService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            var result = await AttributeService.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = result.Success,
                Data = result.Success,
                Errors = result.Data.Errors
            };
        }

        protected override async Task Delete(Guid id)
        {
            var confirmed = await DeleteConfirmNotification();

            if (confirmed)
            {
                var result = await DeleteItemAsync(id);
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
                    await Search();
                    await OnAfterDeleteAsync(id);
                    StateHasChanged();
                }
                else
                {
                    if (result.Errors.Any())
                        await ShowErrorNotification(ValidationResources.Failed, string.Join(",", result.Errors));
                    else
                        await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.DeleteFailed);
                }
            }
        }

        protected override async Task<string> GetItemId(AttributeDto item)
        {
            return item.Id.ToString();
        }

        private string GetFieldTypeDisplayName(FieldType fieldType)
        {
            return fieldType switch
            {
                FieldType.Text => ECommerceResources.Text,
                FieldType.IntegerNumber => ECommerceResources.IntegerNumber,
                FieldType.DecimalNumber => ECommerceResources.DecimalNumber,
                FieldType.Date => ECommerceResources.Date,
                FieldType.CheckBox => ECommerceResources.CheckBox,
                FieldType.List => ECommerceResources.List,
                FieldType.MultiSelectList => ECommerceResources.MultiSelectList,
                _ => fieldType.ToString()
            };
        }

        private string GetFieldTypeBadgeClass(FieldType fieldType)
        {
            return fieldType switch
            {
                FieldType.Text => "bg-primary",
                FieldType.IntegerNumber => "bg-info",
                FieldType.DecimalNumber => "bg-success",
                FieldType.Date => "bg-warning text-dark",
                FieldType.CheckBox => "bg-secondary",
                FieldType.List => "bg-danger",
                FieldType.MultiSelectList => "bg-purple",
                _ => "bg-dark"
            };
        }
    }
}
