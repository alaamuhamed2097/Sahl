using Shared.DTOs.Catalog.Item;

namespace BL.Contracts.Service.Catalog.Item
{
    public interface IItemDetailsService
    {
        Task<ItemDetailsDto> GetItemDetailsAsync(Guid itemCombinationId, string? viewerId);
        Task<ItemDetailsDto> GetCombinationByAttributesAsync(CombinationRequest request, string? viewerId);
    }
}