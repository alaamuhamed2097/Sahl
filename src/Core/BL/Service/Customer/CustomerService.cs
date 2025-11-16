using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Extensions;
using BL.Service.Base;
using Common.Enumerations.User;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Customer;
using Domains.Entities.Vendor;
using Domains.Identity;
using Domains.Views.Unit;
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



		//public PaginatedDataModel<CustomerDto> GetPage(BaseSearchCriteriaModel criteriaModel)
		//{
		//	if (criteriaModel == null)
		//		throw new ArgumentNullException(nameof(criteriaModel));

		//	if (criteriaModel.PageNumber < 1)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

		//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

		//	// Base filter for active entities
		//	Expression<Func<TbVendor, bool>> filter = x => x.CurrentState == 1;

		//	// Apply search term if provided
		//	if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
		//	{
		//		string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
		//		filter = x => x.CurrentState == 1 &&
		//					 (x.CompanyName != null && x.CompanyName.ToLower().Contains(searchTerm) ||
		//					 x.ContactName != null && x.ContactName.ToLower().Contains(searchTerm));
		//	}

		//	// Create ordering function based on SortBy and SortDirection
		//	Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderBy = null;

		//	if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
		//	{
		//		var sortBy = criteriaModel.SortBy.ToLower();
		//		var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

		//		orderBy = query =>
		//		{
		//			return sortBy switch
		//			{
		//				"CompanyName" => isDescending ? query.OrderByDescending(x => x.CompanyName) : query.OrderBy(x => x.CompanyName),
		//				"ContactName" => isDescending ? query.OrderByDescending(x => x.ContactName) : query.OrderBy(x => x.ContactName)
		//			};
		//		};
		//	}

		//	var entitiesList = _vendorRepository.GetPage(
		//		criteriaModel.PageNumber,
		//		criteriaModel.PageSize,
		//		filter,
		//		orderBy);

		//	var dtoList = _mapper.MapList<TbVendor, VendorDto>(entitiesList.Items);

		//	return new PaginatedDataModel<VendorDto>(dtoList, entitiesList.TotalRecords);
		//}

		//public async Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
		//{
		//	if (criteriaModel == null)
		//		throw new ArgumentNullException(nameof(criteriaModel));

		//	if (criteriaModel.PageNumber < 1)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

		//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

		//	// Base filter
		//	Expression<Func<TbVendor, bool>> filter = x => x.CurrentState == 1;

		//	// Combine expressions manually
		//	var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
		//	if (!string.IsNullOrWhiteSpace(searchTerm))
		//	{
		//		filter = filter.And(x =>
		//			x.CompanyName != null && x.CompanyName.ToLower().Contains(searchTerm) ||
		//			x.CommercialRegister != null && x.CommercialRegister.ToLower().Contains(searchTerm) ||
		//			x.ContactName != null && x.ContactName.ToLower().Contains(searchTerm)
		//		);
		//	}

		//	var vendors = await _vendorRepository.GetPageAsync(
		//		criteriaModel.PageNumber,
		//		criteriaModel.PageSize,
		//		filter,
		//		orderBy: q => q.OrderBy(x => x.CreatedDateUtc));

		//	var itemsDto = _mapper.MapList<TbVendor, VendorDto>(vendors.Items);

		//	return new PaginatedDataModel<VendorDto>(itemsDto, vendors.TotalRecords);
		//}



	}
}
