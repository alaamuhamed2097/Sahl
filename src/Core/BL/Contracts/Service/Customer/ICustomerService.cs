using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Customer;
using Shared.DTOs.Customer;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts.Service.Customer
{
	public interface ICustomerService : IBaseService<TbCustomer, CustomerDto>
	{
		PaginatedDataModel<CustomerDto> GetPage(BaseSearchCriteriaModel criteriaModel);
		Task<PaginatedDataModel<CustomerDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
	}
}
