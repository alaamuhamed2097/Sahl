using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels;
using Shared.ResultModels;

namespace Dashboard.Contracts.ECommerce.Category
{
    public interface IAttributeService
    {
        /// <summary>
        /// Get all attributes.
        /// </summary>
        Task<ResponseModel<IEnumerable<AttributeDto>>> GetAllAsync();

        /// <summary>
        /// Get attribute by ID.
        /// </summary>
        Task<ResponseModel<AttributeDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a attribute.
        /// </summary>
        Task<ResponseModel<bool>> SaveAsync(AttributeDto attribute);

        /// <summary>
        /// Delete a attribute by ID.
        /// </summary>
        Task<ResponseModel<DeleteResult>> DeleteAsync(Guid id);


    }
}
