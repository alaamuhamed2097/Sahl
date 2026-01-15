using Common.Enumerations.User;
using Common.Filters;
using Dashboard.Models.pagintion;
using Shared.DTOs.Brand;
using Shared.DTOs.Customer;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Customer
{
	public interface ICustomerService
	{
		/// <summary>
		/// Get all Customers.
		/// </summary>
		Task<ResponseModel<IEnumerable<CustomerDto>>> GetAllAsync();

		/// <summary>
		/// Get Customers by ID.
		/// </summary>
		Task<ResponseModel<CustomerDto>> GetByIdAsync(Guid id);

		/// <summary>
		/// Save or update a Customers.
		/// </summary>
		Task<ResponseModel<CustomerDto>> SaveAsync(CustomerDto Customers);

		/// <summary>
		/// Delete a Customers by ID.
		/// </summary>
		Task<ResponseModel<bool>> DeleteAsync(Guid id);
		
		/// <summary>
		/// Searching for customers by criteria.
		/// </summary>
		Task<ResponseModel<PaginatedDataModel<CustomerDto>>> SearchAsync(BaseSearchCriteriaModel criteria);

		/// <summary>
		/// Change customer account status (Lock, Suspend, Activate, etc).
		/// </summary>
		Task<ResponseModel<bool>> ChangeStatusAsync(Guid customerId, UserStateType newStatus);

		/// <summary>
		/// Get customer account status.
		/// </summary>
		Task<ResponseModel<UserStateType>> GetStatusAsync(Guid customerId);

		/// <summary>
		/// Get customer wallet balance.
		/// </summary>
		Task<ResponseModel<decimal>> GetWalletBalanceAsync(Guid customerId);

		/// <summary>
		/// Get customer order history with pagination.
		/// </summary>
		Task<ResponseModel<PaginatedDataModel<object>>> GetOrderHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria);

		/// <summary>
		/// Get customer wallet transaction history with pagination.
		/// </summary>
		Task<ResponseModel<PaginatedDataModel<object>>> GetWalletHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria);
	}
}
