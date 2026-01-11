using AutoMapper;
using BL.Contracts.GeneralService.UserManagement;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.User.Base;

namespace BL.GeneralService.UserManagement;

public abstract class UserService<TBaseDto, TBaseCreateDto, TBaseUpdateDto> : IUserService<TBaseDto, TBaseCreateDto, TBaseUpdateDto>
    where TBaseDto : BaseUserDto
    where TBaseCreateDto : BaseUserCreateDto
    where TBaseUpdateDto : BaseUserUpdateDto
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    protected abstract UserType _role { get; set; }

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<PagedResult<TBaseDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");

        IEnumerable<ApplicationUser> users = await _userManager.GetUsersInRoleAsync(_role.ToString());

        // Apply search term if provided
        if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
        {
            string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
            users = users.Where(u => (u.UserName.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm)) ||
                                     ((u.FirstName + ' ' + u.LastName) != null && (u.FirstName + ' ' + u.LastName).ToLower().Contains(searchTerm)));
        }

        var totalRecords = users.Count();
        users = users.Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize).Take(criteriaModel.PageSize);
        return new PagedResult<TBaseDto>(_mapper.Map<IEnumerable<TBaseDto>>(users), totalRecords);
    }

    public async Task<IEnumerable<TBaseDto>> GetAllAsync()
    {
        var users = await _userManager.GetUsersInRoleAsync(_role.ToString());
        return _mapper.Map<IEnumerable<TBaseDto>>(users);
    }

    public async Task<TBaseDto> FindByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null || !await _userManager.IsInRoleAsync(user, _role.ToString()))
            throw new Exception("User not found or invalid role.");

        return _mapper.Map<TBaseDto>(user);
    }

    public async Task<TBaseDto> CreateAsync(TBaseCreateDto createDto, Guid creatorId)
    {
        var user = _mapper.Map<ApplicationUser>(createDto);

        user.UserName = user.Email;
        user.CreatedBy = creatorId;
        user.UserState = UserStateType.Inactive;
        user.CreatedDateUtc = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, createDto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, _role.ToString());
        return _mapper.Map<TBaseDto>(user);
    }

    public async Task<TBaseDto> UpdateAsync(Guid id, TBaseUpdateDto updateDto, Guid updatorId)
    {
        if (!_userManager.Users.Any(u => u.Id == updatorId.ToString()))
            throw new Exception("Updator not found");

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null || !await _userManager.IsInRoleAsync(user, _role.ToString()))
            throw new Exception("User not found or invalid role.");

        _mapper.Map(updateDto, user);
        user.UpdatedBy = updatorId;
        user.UpdatedDateUtc = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return _mapper.Map<TBaseDto>(user);
    }

    private void DeleteAlgorithm(ApplicationUser user)
    {
        // Set current user state = 0
        user.UserState = UserStateType.Deleted;

        // Change UserName - Email
        var guid = Guid.NewGuid();
        user.UserName = user.UserName + "-" + guid;
        user.Email = user.Email + "-" + guid;
    }

    public async Task<bool> CheckUserRoleAsync(Guid id, string role)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null || !await _userManager.IsInRoleAsync(user, _role.ToString()))
            throw new Exception("User not found or invalid role.");

        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> Delete(Guid id, Guid updatorId)
    {
        if (!_userManager.Users.Any(u => u.Id == updatorId.ToString()))
            throw new Exception("updator not found");

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null || !await _userManager.IsInRoleAsync(user, _role.ToString()))
            throw new Exception("User not found or invalid role.");
        if (user.UserState == UserStateType.Deleted)
            throw new Exception("The account was already deleted.");

        DeleteAlgorithm(user);
        user.UpdatedBy = updatorId;
        user.UpdatedDateUtc = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return true;
    }
}
