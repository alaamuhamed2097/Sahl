using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Warehouse;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Warehouse
{
	public class WarehouseService : IWarehouseService
	{
		private readonly IApiService _apiService;

		public WarehouseService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<WarehouseDto>>(ApiEndpoints.Warehouse.Get);
			}
			catch (Exception ex)
			{
				return new ResponseModel<IEnumerable<WarehouseDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		
		public async Task<ResponseModel<IEnumerable<WarehouseDto>>> GetActiveWarehousesAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<WarehouseDto>>(ApiEndpoints.Warehouse.GetActive);
			}
			catch (Exception ex)
			{
				return new ResponseModel<IEnumerable<WarehouseDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<WarehouseDto>> GetByIdAsync(Guid id)
		{
			try
			{
				return await _apiService.GetAsync<WarehouseDto>($"{ApiEndpoints.Warehouse.Get}/{id}");
			}
			catch (Exception ex)
			{
				return new ResponseModel<WarehouseDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
		public async Task<ResponseModel<IEnumerable<VendorDto>>> GetVendorsAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<VendorDto>>(ApiEndpoints.Warehouse.GetVendors);
			}
			catch (Exception ex)
			{
				return new ResponseModel<IEnumerable<VendorDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<bool>> IsMultiVendorEnabledAsync()
		{
			try
			{
				return await _apiService.GetAsync<bool>(ApiEndpoints.Warehouse.IsMultiVendorEnabled);
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
		public async Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchAsync(WarehouseSearchCriteriaModel criteria)
		{
			if (criteria == null) throw new ArgumentNullException(nameof(criteria));

			try
			{
				// ???? ??? query parameters
				var queryParams = new Dictionary<string, string>
		{
			{ nameof(criteria.PageNumber), criteria.PageNumber.ToString() },
			{ nameof(criteria.PageSize), criteria.PageSize.ToString() }
		};

				// ????? SearchTerm
				if (!string.IsNullOrEmpty(criteria.SearchTerm))
					queryParams.Add(nameof(criteria.SearchTerm), criteria.SearchTerm);

				// ????? IsActive
				if (criteria.IsActive.HasValue)
					queryParams.Add(nameof(criteria.IsActive), criteria.IsActive.Value.ToString());

				// ????? IsDefaultPlatformWarehouse
				if (criteria.IsDefaultPlatformWarehouse.HasValue)
					queryParams.Add(nameof(criteria.IsDefaultPlatformWarehouse), criteria.IsDefaultPlatformWarehouse.Value.ToString());

				// ????? VendorId
				if (criteria.VendorId.HasValue)
					queryParams.Add(nameof(criteria.VendorId), criteria.VendorId.Value.ToString());

				// ????? Email
				if (!string.IsNullOrEmpty(criteria.Email))
					queryParams.Add(nameof(criteria.Email), criteria.Email);

				// ????? Address
				if (!string.IsNullOrEmpty(criteria.Address))
					queryParams.Add(nameof(criteria.Address), criteria.Address);

				// ????? SortBy
				if (!string.IsNullOrEmpty(criteria.SortBy))
					queryParams.Add(nameof(criteria.SortBy), criteria.SortBy);

				// ????? SortDirection
				if (!string.IsNullOrEmpty(criteria.SortDirection))
					queryParams.Add(nameof(criteria.SortDirection), criteria.SortDirection);

				// ????? CreatedDateFrom
				if (criteria.CreatedDateFrom.HasValue)
					queryParams.Add(nameof(criteria.CreatedDateFrom), criteria.CreatedDateFrom.Value.ToString("O"));

				// ????? CreatedDateTo
				if (criteria.CreatedDateTo.HasValue)
					queryParams.Add(nameof(criteria.CreatedDateTo), criteria.CreatedDateTo.Value.ToString("O"));

				return await _apiService.GetAsync<PaginatedDataModel<WarehouseDto>>(
					ApiEndpoints.Warehouse.Search, queryParams);
			}
			catch (Exception ex)
			{
				return new ResponseModel<PaginatedDataModel<WarehouseDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchVendorAsync(WarehouseSearchCriteriaModel criteria)
		{
			if (criteria == null) throw new ArgumentNullException(nameof(criteria));

			try
			{
				// ???? ??? query parameters
				var queryParams = new Dictionary<string, string>
		{
			{ nameof(criteria.PageNumber), criteria.PageNumber.ToString() },
			{ nameof(criteria.PageSize), criteria.PageSize.ToString() }
		};

				// ????? SearchTerm
				if (!string.IsNullOrEmpty(criteria.SearchTerm))
					queryParams.Add(nameof(criteria.SearchTerm), criteria.SearchTerm);

				// ????? IsActive
				if (criteria.IsActive.HasValue)
					queryParams.Add(nameof(criteria.IsActive), criteria.IsActive.Value.ToString());

				// ????? IsDefaultPlatformWarehouse
				if (criteria.IsDefaultPlatformWarehouse.HasValue)
					queryParams.Add(nameof(criteria.IsDefaultPlatformWarehouse), criteria.IsDefaultPlatformWarehouse.Value.ToString());

				// ????? VendorId
				if (criteria.VendorId.HasValue)
					queryParams.Add(nameof(criteria.VendorId), criteria.VendorId.Value.ToString());

				// ????? Email
				if (!string.IsNullOrEmpty(criteria.Email))
					queryParams.Add(nameof(criteria.Email), criteria.Email);

				// ????? Address
				if (!string.IsNullOrEmpty(criteria.Address))
					queryParams.Add(nameof(criteria.Address), criteria.Address);

				// ????? SortBy
				if (!string.IsNullOrEmpty(criteria.SortBy))
					queryParams.Add(nameof(criteria.SortBy), criteria.SortBy);

				// ????? SortDirection
				if (!string.IsNullOrEmpty(criteria.SortDirection))
					queryParams.Add(nameof(criteria.SortDirection), criteria.SortDirection);

				// ????? CreatedDateFrom
				if (criteria.CreatedDateFrom.HasValue)
					queryParams.Add(nameof(criteria.CreatedDateFrom), criteria.CreatedDateFrom.Value.ToString("O"));

				// ????? CreatedDateTo
				if (criteria.CreatedDateTo.HasValue)
					queryParams.Add(nameof(criteria.CreatedDateTo), criteria.CreatedDateTo.Value.ToString("O"));

				return await _apiService.GetAsync<PaginatedDataModel<WarehouseDto>>(
					ApiEndpoints.Warehouse.SearchVendor, queryParams);
			}
			catch (Exception ex)
			{
				return new ResponseModel<PaginatedDataModel<WarehouseDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}


		//public async Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchAsync(WarehouseSearchCriteriaModel criteria)
		//{
		//	if (criteria == null) throw new ArgumentNullException(nameof(criteria));

		//	try
		//	{

		//		var queryParams = new Dictionary<string, string>
		//{
		//	{ "PageNumber", criteria.PageNumber.ToString() },
		//	{ "PageSize", criteria.PageSize.ToString() }
		//};


		//		if (!string.IsNullOrEmpty(criteria.SearchTerm))
		//		{
		//			queryParams.Add("SearchTerm", criteria.SearchTerm);
		//		}


		//		if (!string.IsNullOrEmpty(criteria.SortBy))
		//		{
		//			queryParams.Add("SortBy", criteria.SortBy);
		//		}

		//		if (!string.IsNullOrEmpty(criteria.SortDirection))
		//		{
		//			queryParams.Add("SortDirection", criteria.SortDirection);
		//		}

		//		return await _apiService.GetAsync<PaginatedDataModel<WarehouseDto>>(
		//			ApiEndpoints.Warehouse.Search, queryParams);
		//	}
		//	catch (Exception ex)
		//	{
		//		return new ResponseModel<PaginatedDataModel<WarehouseDto>>
		//		{
		//			Success = false,
		//			Message = ex.Message
		//		};
		//	}
		//}

		public async Task<ResponseModel<WarehouseDto>> SaveAsync(WarehouseDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			try
			{
				return await _apiService.PostAsync<WarehouseDto, WarehouseDto>(ApiEndpoints.Warehouse.Save, dto);
			}
			catch (Exception ex)
			{
				return new ResponseModel<WarehouseDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
		{
			try
			{
				var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.Warehouse.Delete, id);
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
			catch (Exception)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = NotifiAndAlertsResources.DeleteFailed
				};
			}
		}

		public async Task<ResponseModel<bool>> ToggleStatusAsync(Guid id)
		{
			try
			{
				var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>(ApiEndpoints.Warehouse.ToggleStatus, id);
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
					Message = ex.Message
				};
			}
		}
		public async Task<ResponseModel<IEnumerable<VendorWithUserDto>>> GetActiveVendorsAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<VendorWithUserDto>>(ApiEndpoints.Warehouse.withUsers);
			}
			catch (Exception ex)
			{
				return new ResponseModel<IEnumerable<VendorWithUserDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
		
	}
}
