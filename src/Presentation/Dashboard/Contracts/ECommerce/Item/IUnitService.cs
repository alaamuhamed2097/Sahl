using Shared.DTOs.Catalog.Unit;
using Shared.GeneralModels;

namespace Dashboard.Contracts.ECommerce.Item
{
    public interface IUnitService
    {
        /// <summary>
        /// Get all units.
        /// </summary>
        Task<ResponseModel<IEnumerable<UnitDto>>> GetAllAsync();

        /// <summary>
        /// Get unit by ID.
        /// </summary>
        Task<ResponseModel<UnitDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a unit.
        /// </summary>
        Task<ResponseModel<UnitDto>> SaveAsync(UnitDto unit);

        /// <summary>
        /// Delete a unit by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
