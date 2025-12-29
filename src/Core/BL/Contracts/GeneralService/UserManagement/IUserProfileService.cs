using Common.Enumerations.User;
using DAL.Models;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.GeneralService.UserManagement;

public interface IUserProfileService
{
    Task<bool> DeleteAccount(Guid id, Guid updatorId);
    Task<UserStateType> GetUserStateAsync(Guid Id);

    Task<PagedResult<AdminProfileDto>> GetAdminsPage(BaseSearchCriteriaModel criteriaModel);
    Task<IEnumerable<AdminProfileDto>> GetAllAdminsAsync();
    Task<AdminRegistrationDto> FindAdminDtoByIdAsync(string userId);
    Task<AdminProfileDto> GetAdminProfileAsync(string userId);
    Task<ResponseModel<AdminProfileDto>> UpdateAdminProfileAsync(string userId, AdminProfileUpdateDto userProfileUpdateDto, Guid updatorId);
}
