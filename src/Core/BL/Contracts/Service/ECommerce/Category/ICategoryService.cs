using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Category;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.ECommerce.Category
{
    public interface ICategoryService : IBaseService<TbCategory, CategoryDto>
    {
        Task<PaginatedDataModel<CategoryDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<IEnumerable<MainCategoryDto>> GetMainCategoriesAsync();
        Task<IEnumerable<CategoryPreviewDto>> GetPreviewedCategories(bool isFeaturedCategory, bool isParent);
        Task<IEnumerable<VwCategoryItemsDto>> GetHomeCategories(string userId);
        Task<List<CategoryTreeDto>> BuildCategoryTree();
        Task<bool> Save(CategoryDto dto, Guid userId);
        Task<bool> UpdateSerialsAsync(Dictionary<Guid, string> serialAssignments, Guid userId);
        Task<(bool, List<string>)> Delete(Guid id, Guid userId);
        Task<CategoryTreeDto> GetCategoryWithChildren(Guid categoryId);
    }
}
