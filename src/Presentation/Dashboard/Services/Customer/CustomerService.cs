using Common.Enumerations.User;
using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Customer;
using Dashboard.Contracts.General;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.User.Customer;
using Shared.DTOs.Wallet.Customer;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;

namespace Dashboard.Services.Customer
{
	public class CustomerService : ICustomerService
	{
		private readonly IApiService _apiService;
		public CustomerService(IApiService apiService)
		{
			_apiService = apiService;
		}

		/// <summary>
		/// Get all Customers.
		/// </summary>
		public async Task<ResponseModel<IEnumerable<CustomerDto>>> GetAllAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<CustomerDto>>(ApiEndpoints.Customer.Get);
			}
			catch (Exception ex)
			{
				return new ResponseModel<IEnumerable<CustomerDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get Customer by ID.
		/// </summary>
		public async Task<ResponseModel<CustomerDto>> GetByIdAsync(Guid id)
		{
			try
			{
				return await _apiService.GetAsync<CustomerDto>($"{ApiEndpoints.Customer.Get}/{id}");
			}
			catch (Exception ex)
			{
				return new ResponseModel<CustomerDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}


		/// <summary>
		/// Search Customers with pagination and filtering.
		/// </summary>
		public async Task<ResponseModel<PaginatedDataModel<CustomerDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
		{
			if (criteria == null) throw new ArgumentNullException(nameof(criteria));

			try
			{
				return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<CustomerDto>>(
					ApiEndpoints.Customer.Search, criteria);
			}
			catch (Exception ex)
			{
				return new ResponseModel<PaginatedDataModel<CustomerDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Save or update a Customer.
		/// </summary>
		public async Task<ResponseModel<CustomerDto>> SaveAsync(CustomerDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			try
			{
				return await _apiService.PostAsync<CustomerDto, CustomerDto>(ApiEndpoints.Customer.Save, dto);
			}
			catch (Exception ex)
			{
				return new ResponseModel<CustomerDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Delete a Customer by ID.
		/// </summary>
		public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
		{
			try
			{
				var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.Customer.Delete, id);
				if (result.Success)
				{
					return new ResponseModel<bool>
					{
						Success = true,
						Message = result.Message
					};
				}
				return new ResponseModel<bool>
				{
					Success = false,
					Message = result.Message,
					Errors = result.Errors
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = NotifiAndAlertsResources.DeleteFailed
				};
			}
		}

		/// <summary>
		/// Change customer account status (Lock, Suspend, Activate, etc).
		/// </summary>
		public async Task<ResponseModel<bool>> ChangeStatusAsync(Guid customerId, UserStateType newStatus)
		{
			try
			{
				var request = new { customerId, status = newStatus };
				return await _apiService.PostAsync<object, bool>(ApiEndpoints.Customer.ChangeStatus, request);
			}
			catch (Exception ex)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer account status.
		/// </summary>
		public async Task<ResponseModel<UserStateType>> GetStatusAsync(Guid customerId)
		{
			try
			{
				return await _apiService.GetAsync<UserStateType>($"{ApiEndpoints.Customer.GetUserStatus}/{customerId}");
			}
			catch (Exception ex)
			{
				return new ResponseModel<UserStateType>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer wallet balance.
		/// </summary>
		public async Task<ResponseModel<decimal>> GetWalletBalanceAsync(Guid customerId)
		{
			try
			{
				return await _apiService.GetAsync<decimal>($"{ApiEndpoints.Customer.Get}/{customerId}/wallet-balance");
			}
			catch (Exception ex)
			{
				return new ResponseModel<decimal>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer order history with pagination.
		/// </summary>
		public async Task<ResponseModel<PaginatedDataModel<OrderHistoryDto>>> GetOrderHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria)
		{
			try
			{
				return await _apiService.PostAsync<BaseSearchCriteriaModel, PaginatedDataModel<OrderHistoryDto>>(
					$"{ApiEndpoints.Customer.Get}/{customerId}/orders", criteria);
			}
			catch (Exception ex)
			{
				return new ResponseModel<PaginatedDataModel<OrderHistoryDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
		public async Task<ResponseModel<PaginatedDataModel<CustomerWalletTransactionsDto>>> GetWalletHistoryAsync(
			Guid customerId,
			BaseSearchCriteriaModel criteria)
		{
			try
			{
				// Build query string with all parameters
				var queryString = $"?CustomerId={customerId}" +
								 $"&PageNumber={criteria.PageNumber}" +
								 $"&PageSize={criteria.PageSize}";

				if (!string.IsNullOrEmpty(criteria.SearchTerm))
					queryString += $"&SearchTerm={Uri.EscapeDataString(criteria.SearchTerm)}";

				if (!string.IsNullOrEmpty(criteria.SortBy))
					queryString += $"&SortBy={Uri.EscapeDataString(criteria.SortBy)}";

				if (!string.IsNullOrEmpty(criteria.SortDirection))
					queryString += $"&SortDirection={Uri.EscapeDataString(criteria.SortDirection)}";

				var endpoint = $"api/v1/CustomerWalletTransaction/search-by-admin{queryString}";

				return await _apiService.GetAsync<PaginatedDataModel<CustomerWalletTransactionsDto>>(endpoint);
			}
			catch (Exception ex)
			{
				return new ResponseModel<PaginatedDataModel<CustomerWalletTransactionsDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Register a new customer (Admin creates customer account).
		/// </summary>
		public async Task<ResponseModel<CustomerRegistrationResponseDto>> RegisterCustomerAsync(CustomerRegistrationDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			try
			{
				return await _apiService.PostAsync<CustomerRegistrationDto, CustomerRegistrationResponseDto>(
					ApiEndpoints.Customer.Register, dto);
			}
			catch (Exception ex)
			{
				return new ResponseModel<CustomerRegistrationResponseDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Update customer information.
		/// </summary>
		public async Task<ResponseModel<CustomerUpdateByAdminDto>> UpdateByAdminAsync(CustomerUpdateByAdminDto updateDto)
		{
			if (updateDto == null) throw new ArgumentNullException(nameof(updateDto));

			try
			{
				return await _apiService.PutAsync<CustomerUpdateByAdminDto, CustomerUpdateByAdminDto>(
					ApiEndpoints.Customer.Update, updateDto);
			}
			catch (Exception ex)
			{
				
				return new ResponseModel<CustomerUpdateByAdminDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

	}
}
