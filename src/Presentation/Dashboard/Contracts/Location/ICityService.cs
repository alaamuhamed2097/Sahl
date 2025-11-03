using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Location
{
    public interface ICityService
    {
        /// <summary>
        /// Get all Cities.
        /// </summary>
        Task<ResponseModel<IEnumerable<CityDto>>> GetAllAsync();

        /// <summary>
        /// Get Cities by ID.
        /// </summary>
        Task<ResponseModel<CityDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a Cities.
        /// </summary>
        Task<ResponseModel<CityDto>> SaveAsync(CityDto Cities);

        /// <summary>
        /// Delete a Cities by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
