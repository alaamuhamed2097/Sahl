using Dashboard.Models.pagintion;
using Shared.DTOs.Customer;
using Shared.DTOs.User.Admin;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Vendor
{
	public interface IVendorService
	{

		
		
		Task<ResponseModel<bool>> CreateAsync(VendorDto user);
		Task<ResponseModel<VendorDto>> UpdateAsync(Guid id, VendorDto user);
		

		/// <summary>
		/// Get all Vendors.
		/// </summary>
		Task<ResponseModel<IEnumerable<VendorDto>>> GetAllAsync();

		/// <summary>
		/// Get Vendors by ID.
		/// </summary>
		Task<ResponseModel<VendorDto>> GetByIdAsync(Guid id);



		/// <summary>
		/// Delete a Vendors by ID.
		/// </summary>
		Task<ResponseModel<bool>> DeleteAsync(Guid id);

		/// <summary>
		/// Searching for Vendors by criteria.
		/// </summary>
		//Task<ResponseModel<PaginatedDataModel<VendorDto>>> SearchAsync(BaseSearchCriteriaModel criteria);

		//Task<string> GetVendorStatusAsync(Guid userId);

		//Task<bool> ChangeStatusAsync(Guid id, bool isActive);

		//----
		//Task<VendorInfoDto> GetVendorInfoAsync(Guid vendorId);
		//Task<IEnumerable<SelectListDto>> GetForSelectAsync();
		
		

	}
}
