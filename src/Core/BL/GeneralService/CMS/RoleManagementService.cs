using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Identity;

namespace BL.GeneralService.CMS;

public class RoleManagementService : IRoleManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBaseMapper _mapper;

    public RoleManagementService(UserManager<ApplicationUser> userManager, IBaseMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role)
    {
        return (await _userManager.GetUsersInRoleAsync(role)).Where(u => u.UserState == UserStateType.Active || u.UserState == UserStateType.Inactive).ToList();
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());
        return (await _userManager.DeleteAsync(applicationUser)).Succeeded;
    }

}
