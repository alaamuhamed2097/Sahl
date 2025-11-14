using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Service.Base;
using Common.Enumerations.User;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Identity;
using Domins.Entities.Customer;
using Domins.Entities.Vendor;
using Domins.Views.Unit;
using Microsoft.AspNetCore.Identity;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.ECommerce.Unit;
using Shared.DTOs.User.Admin;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Service.Customer
{
	public class CustomerService : BaseService<TbCustomer,CustomerDto> , ICustomerService
	{
		private readonly ITableRepository<TbCustomer> _baseRepository;
		private readonly IBaseMapper _mapper;
		public CustomerService(ITableRepository<TbCustomer> baseRepository, IBaseMapper mapper) : base
			(baseRepository, mapper)
		{ 
			_baseRepository = baseRepository;
			_mapper = mapper;
		}

		

		public Task<PaginatedDataModel<CustomerDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
		{
			throw new NotImplementedException();
		}

		//public async Task<PaginatedDataModel<CustomerDto>> GetCustomerPage(BaseSearchCriteriaModel criteriaModel)
		//{
		//	if (criteriaModel == null)
		//		throw new ArgumentNullException(nameof(criteriaModel));

		//	if (criteriaModel.PageNumber < 1)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

		//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");

		//	//IEnumerable<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("Customer");
		//	//users = users.Where(u => u.UserState != UserStateType.Deleted);

		//	// Apply search term if provided
		//	//if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
		//	//{
		//	//	string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
		//	//	users = users.Where(u => (u.UserName.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm)) ||
		//	//							 ((u.FirstName + ' ' + u.LastName) != null && (u.FirstName + ' ' + u.LastName).ToLower().Contains(searchTerm)));
		//	//}

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
		//	return new PaginatedDataModel<CustomerDto>(_mapper.MapList<ApplicationUser, CustomerDto>(users), totalRecords);
		//}



	}
}
