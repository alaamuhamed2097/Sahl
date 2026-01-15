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
        Task<PagedResult<VendorItemDto>> GetPage(ItemSearchCriteriaModel criteriaModel);
        Task<bool> SaveAsync(SaveVendorItemDto dto, string userId);
        Task<PagedResult<VendorItemDetailsDto>> GetPageVendor(ItemstatusSearchCriteriaModel criteriaModel);

	}
}