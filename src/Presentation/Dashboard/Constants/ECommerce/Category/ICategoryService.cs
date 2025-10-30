using Shared.DTOs.ECommerce;
using Shared.GeneralModels;

namespace Dashboard.Constants.ECommerce.Category
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all categories.
        /// </summary>
        Task<ResponseModel<IEnumerable<CategoryDto>>> GetAllAsync();

        /// <summary>
        /// Get category by ID.
        /// </summary>
        Task<ResponseModel<CategoryDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a category.
        /// </summary>
        Task<ResponseModel<bool>> SaveAsync(CategoryDto category);

        /// <summary>
        /// Change tree view serials.
        /// </summary>
        Task<ResponseModel<bool>> ReorderTreeAsync(Dictionary<Guid, string> serialAssignments);

        /// <summary>
        /// Delete a category by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
