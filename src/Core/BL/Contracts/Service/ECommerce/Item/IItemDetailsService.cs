using Shared.DTOs.ECommerce.Item;

namespace BL.Contracts.Service.ECommerce.Item
{
    public interface IItemDetailsService
    {
        Task<ItemDetailsDto> GetItemDetailsAsync(Guid itemId);
        Task<CombinationDetailsDto> GetCombinationByAttributesAsync(Guid itemId, GetCombinationRequest request);
    }
}