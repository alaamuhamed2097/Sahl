using Shared.DTOs.User.Admin;
using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IUserRegistrationService
    {
        Task<OperationResult> RegisterAdminAsync(AdminRegistrationDto userDto, Guid CreatorId);
    }
}
