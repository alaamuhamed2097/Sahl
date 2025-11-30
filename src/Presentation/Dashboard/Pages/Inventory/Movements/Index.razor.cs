using Dashboard.Contracts.Inventory;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Inventory;
using Shared.GeneralModels;

namespace Dashboard.Pages.Inventory.Movements
{
    public partial class Index : BaseListPage<MoitemDto>
    {
        [Inject] private IInventoryMovementService InventoryMovementService { get; set; } = null!;

        protected override string EntityName => "Inventory Movements";
        protected override string AddRoute => "/inventory/movement";
        protected override string EditRouteTemplate => "/inventory/movement/{id}";
        protected override string SearchEndpoint => "api/InventoryMovement/search";

        protected override Dictionary<string, Func<MoitemDto, object>> ExportColumns => new()
        {
            { "Document Number", item => item.DocumentNumber ?? "-" },
            { "Date", item => item.DocumentDate.ToString("dd/MM/yyyy") },
            { "Type", item => item.MovementType ?? "-" },
            { "Total Amount", item => item.TotalAmount },
            { "Notes", item => item.Notes ?? "-" }
        };

        protected override async Task<ResponseModel<IEnumerable<MoitemDto>>> GetAllItemsAsync()
        {
            return await InventoryMovementService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            var result = await InventoryMovementService.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = result.Success,
                Message = result.Message,
                Data = result.Success,
                Errors = result.Errors
            };
        }

        protected override async Task<string> GetItemId(MoitemDto item)
        {
            return await Task.FromResult(item.Id.ToString());
        }

        protected async Task OnFilterChanged(ChangeEventArgs e)
        {
            var filterValue = e.Value?.ToString();
            await Search();

            if (!string.IsNullOrEmpty(filterValue) && items != null)
            {
                items = items.Where(m => m.MovementType == filterValue);
                totalRecords = items.Count();
                totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                currentPage = 1;
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
