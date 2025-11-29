using BL.Contracts.IMapper;
using BL.Contracts.Service.Vendor;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Resources;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Vendor
{
    public class VendorService : BaseService<TbVendor, VendorDto>, IVendorService
    {
        private readonly ITableRepository<TbVendor> _vendorRepository;
        private readonly IBaseMapper _mapper;
        public VendorService(ITableRepository<TbVendor> vendorRepository, IBaseMapper mapper)
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

        public async Task<PaginatedDataModel<VendorDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbVendor, bool>> filter = x => x.CurrentState == 1;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.CurrentState == 1 &&
                             (x.CompanyName != null && x.CompanyName.ToLower().Contains(searchTerm) ||
                             x.ContactName != null && x.ContactName.ToLower().Contains(searchTerm));
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<TbVendor>, IOrderedQueryable<TbVendor>> orderBy = null;

            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                orderBy = query =>
                {
                    return sortBy switch
                    {
                        "companyname" => isDescending ? query.OrderByDescending(x => x.CompanyName) : query.OrderBy(x => x.CompanyName),
                        "contactname" => isDescending ? query.OrderByDescending(x => x.ContactName) : query.OrderBy(x => x.ContactName),
                        _ => query.OrderBy(x => x.CompanyName)
                    };
                };
            }

            var entitiesList = await _vendorRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy);

            var dtoList = _mapper.MapList<TbVendor, VendorDto>(entitiesList.Items);

            return new PaginatedDataModel<VendorDto>(dtoList, entitiesList.TotalRecords);
        }

        public async Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter
            Expression<Func<TbVendor, bool>> filter = x => x.CurrentState == 1;

            // Combine expressions manually
            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.CompanyName != null && x.CompanyName.ToLower().Contains(searchTerm) ||
                    x.CommercialRegister != null && x.CommercialRegister.ToLower().Contains(searchTerm) ||
                    x.ContactName != null && x.ContactName.ToLower().Contains(searchTerm)
                );
            }

            var vendors = await _vendorRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderBy(x => x.CreatedDateUtc));

            var itemsDto = _mapper.MapList<TbVendor, VendorDto>(vendors.Items);

            return new PaginatedDataModel<VendorDto>(itemsDto, vendors.TotalRecords);
        }

        public async Task<PaginatedDataModel<VendorDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbVendor, bool>> filter = x => x.CurrentState == 1;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.CurrentState == 1 &&
                         (x.CompanyName != null && x.CompanyName.ToLower().Contains(searchTerm) ||
                         x.ContactName != null && x.ContactName.ToLower().Contains(searchTerm));
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<TbVendor>, IOrderedQueryable<TbVendor>> orderBy = null;

            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                orderBy = query =>
                {
                    return sortBy switch
                    {
                        "companyname" => isDescending ? query.OrderByDescending(x => x.CompanyName) : query.OrderBy(x => x.CompanyName),
                        "contactname" => isDescending ? query.OrderByDescending(x => x.ContactName) : query.OrderBy(x => x.ContactName),
                        _ => query.OrderBy(x => x.CompanyName)
                    };
                };
            }

            var entitiesList = await _vendorRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy);

            var dtoList = _mapper.MapList<TbVendor, VendorDto>(entitiesList.Items);

            return new PaginatedDataModel<VendorDto>(dtoList, entitiesList.TotalRecords);
        }
    }
}
