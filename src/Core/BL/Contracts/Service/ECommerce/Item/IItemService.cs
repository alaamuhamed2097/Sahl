using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Catalog.Item;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.ECommerce.Item
{
    public interface IItemService : IBaseService<TbItem, ItemDto>
    {
        Task<PaginatedDataModel<VwItemDto>> GetPage(ItemSearchCriteriaModel criteriaModel);
        new Task<bool> Save(ItemDto dto, Guid userId);

        // Add new currency conversion methods with optional conversion
        Task<ItemDto> GetByIdWithCurrencyConversionAsync(Guid id, string clientIp, bool applyConversion = true);
        Task<IEnumerable<ItemDto>> GetAllWithCurrencyConversionAsync(string clientIp, bool applyConversion = true);
        Task<PaginatedDataModel<VwItemDto>> GetPageWithCurrencyConversionAsync(ItemSearchCriteriaModel criteriaModel, string clientIp, bool applyConversion = true);
    }
}