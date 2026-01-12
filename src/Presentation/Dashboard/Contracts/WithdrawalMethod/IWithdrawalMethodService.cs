using Shared.DTOs.WithdrawalMethod;
using Shared.GeneralModels;

namespace Dashboard.Contracts.WithdrawalMethod
{
    public interface IWithdrawalMethodService
    {
        /// <summary>
        /// Get all WithdrawalMethods.
        /// </summary>
        Task<ResponseModel<IEnumerable<WithdrawalMethodDto>>> GetAllAsync();

        /// <summary>
        /// Get WithdrawalMethod by ID.
        /// </summary>
        Task<ResponseModel<WithdrawalMethodDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Save or update a WithdrawalMethod.
        /// </summary>
        Task<ResponseModel<WithdrawalMethodDto>> SaveAsync(WithdrawalMethodDto WithdrawalMethod);

        /// <summary>
        /// Delete a WithdrawalMethod by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
