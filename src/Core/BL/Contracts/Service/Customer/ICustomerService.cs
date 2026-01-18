using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Shared.DTOs.Customer;
using Shared.GeneralModels;

namespace BL.Contracts.Service.Customer
{
	public interface ICustomerService
	{
		/// <summary>
		/// Get all customers
		/// </summary>
		Task<ResponseModel<IEnumerable<CustomerDto>>> GetAllAsync();

		/// <summary>
		/// Get customer by ID
		/// </summary>
		Task<ResponseModel<CustomerDto>> GetByIdAsync(Guid id);

		/// <summary>
		/// Search customers with pagination and filtering
		/// </summary>
		//Task<ResponseModel<AdvancedPagedResult<CustomerDto>>> SearchAsync(BaseSearchCriteriaModel criteria);
		Task<ResponseModel<AdvancedPagedResult<CustomerDto>>> SearchAsync(
	BaseSearchCriteriaModel criteriaModel,
	CancellationToken cancellationToken = default);
		/// <summary>
		/// Save or update customer
		/// </summary>
		Task<ResponseModel<CustomerDto>> SaveAsync(CustomerDto dto);

		/// <summary>
		/// Delete customer by ID
		/// </summary>
		Task<ResponseModel<bool>> DeleteAsync(Guid id);

		/// <summary>
		/// Change customer account status
		/// </summary>
		Task<ResponseModel<bool>> ChangeStatusAsync(Guid customerId, UserStateType newStatus);

		/// <summary>
		/// Get customer account status
		/// </summary>
		Task<ResponseModel<UserStateType>> GetStatusAsync(Guid customerId);

		/// <summary>
		/// Get customer order history
		/// </summary>
		Task<ResponseModel<AdvancedPagedResult<OrderHistoryDto>>> GetOrderHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria);

		/// <summary>
		/// Get customer wallet transaction history
		/// </summary>
		Task<ResponseModel<AdvancedPagedResult<object>>> GetWalletHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria);

		/// <summary>
		/// Get customer wallet balance
		/// </summary>
		Task<ResponseModel<decimal>> GetWalletBalanceAsync(Guid customerId);
	}
}
