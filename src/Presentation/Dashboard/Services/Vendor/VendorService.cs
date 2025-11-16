using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Vendor;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;

namespace Dashboard.Services.Vendor
{
	public class VendorService : IVendorService
	{
		private readonly IApiService _apiService;
		public VendorService(IApiService apiService)
		{
			_apiService = apiService;
		}
		/// <summary>
		/// Get all Vendors with optional filters.
		/// </summary>
		public async Task<ResponseModel<IEnumerable<VendorDto>>> GetAllAsync()
		{
			try
			{
				return await _apiService.GetAsync<IEnumerable<VendorDto>>($"{ApiEndpoints.Vendor.Get}");
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<IEnumerable<VendorDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get Vendor by ID.
		/// </summary>
		public async Task<ResponseModel<VendorDto>> GetByIdAsync(Guid id)
		{
			try
			{
				return await _apiService.GetAsync<VendorDto>($"{ApiEndpoints.Vendor.Get}/{id}");
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<VendorDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Create a Vendor.
		/// </summary>
		public async Task<ResponseModel<bool>> CreateAsync(VendorDto Vendor)
		{
			if (Vendor == null) throw new ArgumentNullException(nameof(Vendor));

			try
			{
				return await _apiService.PostAsync<VendorDto, bool>($"{ApiEndpoints.Vendor.Create}", Vendor);
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// update a Vendor.
		/// </summary>
		public async Task<ResponseModel<VendorDto>> UpdateAsync(Guid id, VendorDto Vendor)
		{
			if (Vendor == null) throw new ArgumentNullException(nameof(Vendor));

			try
			{
				return await _apiService.PostAsync<VendorDto, VendorDto>($"{ApiEndpoints.Vendor.Update}/{id}", Vendor);
			}
			catch (Exception ex)
			{
				// Log error here
				return new ResponseModel<VendorDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Delete a Vendor by ID.
		/// </summary>
		public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
		{
			try
			{
				var result = await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Vendor.Delete}", id);
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
					Message = "Failed to delete Vendor"
				};
			}
		}
	}
}
