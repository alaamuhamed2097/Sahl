using Shared.DTOs.ECommerce;
using Shared.GeneralModels;

namespace Dashboard.Contracts
{
    public interface IShippingCompanyService
    {
        /// <summary>
        /// Get all shipping companies.
        /// </summary>
        Task<ResponseModel<IEnumerable<ShippingCompanyDto>>> GetAllAsync();

        /// <summary>
        /// Get Shipping Company by ID.
        /// </summary>
        Task<ResponseModel<ShippingCompanyDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a ShippingCompany.
        /// </summary>
        Task<ResponseModel<ShippingCompanyDto>> SaveAsync(ShippingCompanyDto companyDto);

        /// <summary>
        /// Delete a Shipping Company by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
