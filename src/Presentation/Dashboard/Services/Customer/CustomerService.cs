using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Customer;
using Dashboard.Contracts.General;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.Customer;
using Shared.GeneralModels;

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
				// Log error here
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
				// Log error here
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
				// Log error here
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
				// Log error here
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
				// Log error here
				return new ResponseModel<bool>
				{
					Success = false,
					Message = NotifiAndAlertsResources.DeleteFailed
				};
			}
		}

	}
}
