using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Catalog.Item;
using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.Parameters;

namespace BL.Contracts.Service.Catalog.Item;

public interface IItemService : IBaseService<TbItem, ItemDto>
{
    Task<PagedResult<ItemDto>> GetPage(ItemSearchCriteriaModel criteriaModel);
    new Task<ItemDto> FindByIdAsync(Guid Id);
    Task<PagedResult<ItemDto>> GetNewItemRequestsPage(ItemSearchCriteriaModel criteriaModel);
    new Task<bool> SaveAsync(ItemDto dto, Guid userId);
    Task<OperationResult> UpdateVisibilityScope(UpdateItemVisibilityRequest dto, Guid userId);
}