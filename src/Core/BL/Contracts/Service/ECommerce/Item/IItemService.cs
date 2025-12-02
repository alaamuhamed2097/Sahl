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
        new Task<VwItemDto> FindByIdAsync(Guid Id);
        new Task<bool> Save(ItemDto dto, Guid userId);
    }
}