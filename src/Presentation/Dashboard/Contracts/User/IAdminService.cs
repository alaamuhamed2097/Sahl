using Shared.DTOs.User.Admin;
using Shared.GeneralModels;

namespace Dashboard.Contracts.User
{
    public interface IAdminService
    {
        Task<ResponseModel<IEnumerable<AdminProfileDto>>> GetAllAsync();
        Task<ResponseModel<AdminProfileDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<bool>> CreateAsync(AdminRegistrationDto user);
        Task<ResponseModel<AdminProfileDto>> UpdateAsync(Guid id, AdminProfileUpdateDto user);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}
