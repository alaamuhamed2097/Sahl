using Common.Filters;
using DAL.Models;
using Shared.DTOs.User.Base;

namespace BL.Contracts.GeneralService.UserManagement;

public interface IUserService<TBaseDto, TBaseCreateDto, TBaseUpdateDto>
    where TBaseDto : BaseUserDto
    where TBaseCreateDto : BaseUserCreateDto
    where TBaseUpdateDto : BaseUserUpdateDto
{
    //Task<PaginatedDataModel<T>> GetPage(BaseSearchCriteriaModel criteriaModel);

    Task<PagedResult<TBaseDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
    Task<IEnumerable<TBaseDto>> GetAllAsync();
    Task<TBaseDto> FindByIdAsync(Guid id);
    Task<TBaseDto> CreateAsync(TBaseCreateDto createDto, Guid creatorId);
    Task<TBaseDto> UpdateAsync(Guid id, TBaseUpdateDto updateDto, Guid updatorId);
    Task<bool> CheckUserRoleAsync(Guid id, string role);
    Task<bool> Delete(Guid id, Guid updatorId);
}
