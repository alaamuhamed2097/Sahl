using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Location
{
    public interface ICountryService
    {
        /// <summary>
        /// Get all Countrys.
        /// </summary>
        Task<ResponseModel<IEnumerable<CountryDto>>> GetAllAsync();

        /// <summary>
        /// Get Country by ID.
        /// </summary>
        Task<ResponseModel<CountryDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a Country.
        /// </summary>
        Task<ResponseModel<CountryDto>> SaveAsync(CountryDto Country);

        /// <summary>
        /// Delete a Country by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
