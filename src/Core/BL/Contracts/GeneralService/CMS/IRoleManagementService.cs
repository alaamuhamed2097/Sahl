using Domains.Identity;

namespace BL.Contracts.GeneralService.CMS
{
    public interface IRoleManagementService
    {
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
