using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts.Service.Vendor
{
	public interface IVendorService : IBaseService<TbVendor,VendorDto> 
	{
		PaginatedDataModel<VendorDto> GetPage(BaseSearchCriteriaModel criteriaModel);
		Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
		//Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);

	}
}
