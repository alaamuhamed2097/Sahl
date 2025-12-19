using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Catalog.Attribute;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

namespace BL.Contracts.Service.ECommerce.Category
{
    public interface IAttributeService : IBaseService<TbAttribute, AttributeDto>
    {
        Task<PagedResult<AttributeDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<DeleteResult> DeleteAsync(Guid id, string UserId);
        Task<IEnumerable<CategoryAttributeDto>> GetByCategoryIdAsync(Guid categoryId);
    }
}
