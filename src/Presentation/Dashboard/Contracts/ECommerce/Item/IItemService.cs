using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels;

namespace Dashboard.Contracts.ECommerce.Item
{
    public interface IItemService
    {
        Task<ResponseModel<IEnumerable<ItemDto>>> GetAllAsync();
        Task<ResponseModel<ItemDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<ItemDto>> SaveAsync(ItemDto item);
        Task<ResponseModel<bool>> DeleteAsync(Guid itemId);
    }
}
