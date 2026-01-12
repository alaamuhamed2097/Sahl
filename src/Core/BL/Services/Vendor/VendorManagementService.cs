using BL.Contracts.IMapper;
using BL.Contracts.Service.Vendor;
using BL.Services.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Resources;
using Shared.DTOs.Vendor;

namespace BL.Services.Vendor;

public class VendorManagementService : BaseService<TbVendor, VendorDto>, IVendorManagementService
{
    private readonly IVendorManagementRepository _vendorRepository;
    private readonly IBaseMapper _mapper;
    public VendorManagementService(
        IVendorManagementRepository vendorRepository,
        IBaseMapper mapper)
        : base(vendorRepository, mapper)
    {
        _vendorRepository = vendorRepository;
        _mapper = mapper;
    }

    //public async Task<PaginatedDataModel<VendorDto>> GetAdminsPage(BaseSearchCriteriaModel criteriaModel)
    //{
    //	if (criteriaModel == null)
    //		throw new ArgumentNullException(nameof(criteriaModel));

    //	if (criteriaModel.PageNumber < 1)
    //		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

    //	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
    //		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");

    //	IEnumerable<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("Vendor");
    //	users = users.Where(u => u.UserState != UserStateType.Deleted);

    //	// Apply search term if provided
    //	if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
    //	{
    //		string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
    //		users = users.Where(u => (u.UserName.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm)) ||
    //								 ((u.FirstName + ' ' + u.LastName) != null && (u.FirstName + ' ' + u.LastName).ToLower().Contains(searchTerm)));
    //	}

    //	// Apply sorting if specified
    //	if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
    //	{
    //		var sortBy = criteriaModel.SortBy.ToLower();
    //		var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

    //		users = sortBy switch
    //		{
    //			"username" => isDescending ? users.OrderByDescending(x => x.UserName) : users.OrderBy(x => x.UserName),
    //			"email" => isDescending ? users.OrderByDescending(x => x.Email) : users.OrderBy(x => x.Email),
    //			"name" => isDescending ? users.OrderByDescending(x => x.FirstName + " " + x.LastName) : users.OrderBy(x => x.FirstName + " " + x.LastName),
    //			"userstate" => isDescending ? users.OrderByDescending(x => x.UserState) : users.OrderBy(x => x.UserState),
    //			_ => users.OrderBy(x => x.UserName) // Default sorting
    //		};
    //	}

    //	var totalRecords = users.Count();
    //	users = users.Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize).Take(criteriaModel.PageSize);
    //	return new PaginatedDataModel<VendorDto>(_mapper.MapList<ApplicationUser, VendorDto>(users), totalRecords);
    //}

    public async Task<PagedResult<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        var entitiesList = await _vendorRepository.GetPageAsync(
            criteriaModel, new CancellationToken());

        var dtoList = _mapper.MapList<TbVendor, VendorDto>(entitiesList.Items);

        return new PagedResult<VendorDto>(dtoList, entitiesList.TotalRecords);
    }

    public async Task<TbVendor> GetByUserIdAsync(string userId)
    {
        return await _vendorRepository.FindAsync(v => v.UserId == userId);
    }
    public Guid GetMarketStoreVendorId()
    {
        return Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479");
    }
}
