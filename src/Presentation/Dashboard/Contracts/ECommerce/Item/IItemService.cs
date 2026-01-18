using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels;
using Shared.Parameters;

namespace Dashboard.Contracts.ECommerce.Item
{
    public interface IItemService
    {
        Task<ResponseModel<IEnumerable<ItemDto>>> GetAllAsync();
        Task<ResponseModel<ItemDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<bool>> SaveAsync(ItemDto item);
        Task<ResponseModel<bool>> UpdateStatusAsync(UpdateItemVisibilityRequest updateItemVisibility);
        Task<ResponseModel<bool>> DeleteAsync(Guid itemId);
    }
}
