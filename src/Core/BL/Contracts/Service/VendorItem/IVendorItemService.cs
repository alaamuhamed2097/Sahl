using DAL.Models;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.VendorItem
{
    public interface IVendorItemService
    {
        Task<ItemDto> FindByIdAsync(Guid Id);
        Task<IEnumerable<VendorItemDetailsDto>> FindByItemCombinationIdAsync(Guid itemCombinationId, CancellationToken token = default);
        Task<PagedResult<OfferDto>> GetPage(ItemSearchCriteriaModel criteriaModel);
        Task<bool> Save(ItemDto dto, Guid userId);
    }
}