using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Location
{
    public interface IStateService
    {
        /// <summary>
        /// Get all States.
        /// </summary>
        Task<ResponseModel<IEnumerable<StateDto>>> GetAllAsync();
        /// <summary>
        /// Get States by ID.
        /// </summary>
        Task<ResponseModel<StateDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a States.
        /// </summary>
        Task<ResponseModel<StateDto>> SaveAsync(StateDto state);

        /// <summary>
        /// Delete a States by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);

    }
}
