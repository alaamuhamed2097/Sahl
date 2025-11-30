using Dashboard.Contracts.Warehouse;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;

namespace Dashboard.Pages.Warehouse
{
    public partial class Index : BaseListPage<WarehouseDto>
    {
        [Inject] private IWarehouseService WarehouseService { get; set; } = null!;

        protected override string EntityName => "Warehouse Management";
        protected override string AddRoute => "/warehouse";
        protected override string EditRouteTemplate => "/warehouse/{id}";
        protected override string SearchEndpoint => "api/Warehouse/search";

        protected override Dictionary<string, Func<WarehouseDto, object>> ExportColumns => new()
        {
            { "English Name", item => item.TitleEn ?? "-" },
            { "Arabic Name", item => item.TitleAr ?? "-" },
            { "Address", item => item.Address ?? "-" },
            { "Phone", item => !string.IsNullOrEmpty(item.PhoneNumber) ? $"{item.PhoneCode} {item.PhoneNumber}" : "-" },
            { "Status", item => item.IsActive ? "Active" : "Inactive" }
        };

        protected override async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllItemsAsync()
        {
            return await WarehouseService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            var result = await WarehouseService.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = result.Success,
                Message = result.Message,
                Data = result.Success,
                Errors = result.Errors
            };
        }

        protected override async Task<string> GetItemId(WarehouseDto item)
        {
            return await Task.FromResult(item.Id.ToString());
        }

        protected async Task ToggleStatus(WarehouseDto warehouse)
        {
            try
            {
                var result = await WarehouseService.ToggleStatusAsync(warehouse.Id);
                if (result.Success)
                {
                    await ShowSuccessNotification("Status updated successfully");
                    await Search();
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Error, result.Message ?? "Failed to update status");
                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(ValidationResources.Error, "Error updating warehouse status");
            }
        }

        protected async Task OnFilterChanged(ChangeEventArgs e)
        {
            var filterValue = e.Value?.ToString();

            await Search();

            if (!string.IsNullOrEmpty(filterValue) && items != null)
            {
                items = filterValue switch
                {
                    "active" => items.Where(w => w.IsActive),
                    "inactive" => items.Where(w => !w.IsActive),
                    _ => items
                };

                totalRecords = items.Count();
                totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                currentPage = 1;
                searchModel.PageNumber = 1;

                StateHasChanged();
            }
        }

        protected override async Task OnCustomInitializeAsync()
        {
            searchModel.PageSize = 10;
            await base.OnCustomInitializeAsync();
        }
    }
}
