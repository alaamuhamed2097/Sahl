using Shared.DTOs.ECommerce.Item;

namespace BL.Contracts.Service.ECommerce.Item
{
    public interface IItemDetailsService
    {
        Task<ItemDetailsDto> GetItemDetailsAsync(Guid itemCombinationId);
        Task<ItemDetailsDto> GetCombinationByAttributesAsync(Guid itemId, CombinationRequest request);
    }
}