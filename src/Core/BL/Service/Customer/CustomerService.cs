using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.AspNetCore.Identity;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Customer
{
	public class CustomerService : BaseService<TbCustomer,CustomerDto> , ICustomerService
	{
		private readonly ITableRepository<TbCustomer> _customerRepository;
		private readonly IBaseMapper _mapper;
		public CustomerService(ITableRepository<TbCustomer> customerRepository, IBaseMapper mapper) : base
			(customerRepository, mapper)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}

		//public async Task<IEnumerable<CustomerDto>> GetAllAsync()
		//{
		//	var customers = await _customerRepository.GetAllAsync();

		//	var result = new List<CustomerDto>();

		//	foreach (var c in customers)
		//	{
		//		var dto = _mapper.Map<CustomerDto>(c);

		//		// هات بيانات اليوزر من ASP.NET Identity
		//		var user = await _userManager.FindByIdAsync(c.UserId);

		//		if (user != null)
		//		{
		//			dto.FullName = user.FullName;
		//			dto.Email = user.Email;
		//			dto.Phone = user.PhoneNumber;

		//			var roles = await _userManager.GetRolesAsync(user);
		//			dto.Role = roles.FirstOrDefault();
		//		}

		//		result.Add(dto);
		//	}

		//	return result;
		//}



		public PaginatedDataModel<CustomerDto> GetPage(BaseSearchCriteriaModel criteriaModel)
		{
			if (criteriaModel == null)
				throw new ArgumentNullException(nameof(criteriaModel));

			if (criteriaModel.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

			if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

			// Base filter: Active customers based on UserState
			Expression<Func<TbCustomer, bool>> filter = x => x.CurrentState == 1;

			// Apply search term
			if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
			{
				string term = criteriaModel.SearchTerm.Trim().ToLower();

				filter = x =>
					x.CurrentState == 1 &&
					(
						(x.FirstName != null && x.FirstName.ToLower().Contains(term)) ||
						(x.LastName != null && x.LastName.ToLower().Contains(term)) ||
						(x.Email != null && x.Email.ToLower().Contains(term)) ||
						
						(x.Notes != null && x.Notes.ToLower().Contains(term))
					);
			}

			// OrderBy logic
			Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderBy = null;

			if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
			{
				var sortBy = criteriaModel.SortBy.ToLower();
				bool isDesc = criteriaModel.SortDirection?.ToLower() == "desc";

				orderBy = query =>
				{
					return sortBy switch
					{
						"firstname" => isDesc ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName),
						"lastname" => isDesc ? query.OrderByDescending(x => x.LastName) : query.OrderBy(x => x.LastName),
						"email" => isDesc ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email),
						_ => query.OrderBy(x => x.FirstName)
					};
				};
			}

			// Get paged entities
			var entitiesList = _customerRepository.GetPage(
				criteriaModel.PageNumber,
				criteriaModel.PageSize,
				filter,
				orderBy
			);

			// Map to DTO
			var dtoList = _mapper.MapList<TbCustomer, CustomerDto>(entitiesList.Items);

			return new PaginatedDataModel<CustomerDto>(dtoList, entitiesList.TotalRecords);
		}

		public async Task<PaginatedDataModel<CustomerDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
		{
			if (criteriaModel == null)
				throw new ArgumentNullException(nameof(criteriaModel));

			if (criteriaModel.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

			if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

			// Base filter
			Expression<Func<TbCustomer, bool>> filter = x => x.CurrentState == 1;

			// Combine expressions manually
			var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				filter = filter.And(x =>
					x.FirstName != null && x.FirstName.ToLower().Contains(searchTerm) ||
					x.LastName != null && x.LastName.ToLower().Contains(searchTerm) 
				);
			}

			var customers = await _customerRepository.GetPageAsync(
				criteriaModel.PageNumber,
				criteriaModel.PageSize,
				filter,
				orderBy: q => q.OrderBy(x => x.CreatedDateUtc));

			var itemsDto = _mapper.MapList<TbCustomer, CustomerDto>(customers.Items);

			return new PaginatedDataModel<CustomerDto>(itemsDto, customers.TotalRecords);
		}

		
	}
}
