using Dashboard.Constants;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels;

namespace Dashboard.Services
{
    public class ShippingCompanyService : IShippingCompanyService
    {
        private readonly IApiService _apiService;

        public ShippingCompanyService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all shipping company with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<ShippingCompanyDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<ShippingCompanyDto>>($"{ApiEndpoints.ShippingCompany.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<ShippingCompanyDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get shipping company by ID.
        /// </summary>
        public async Task<ResponseModel<ShippingCompanyDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<ShippingCompanyDto>($"{ApiEndpoints.ShippingCompany.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<ShippingCompanyDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a shipping company.
        /// </summary>
        public async Task<ResponseModel<ShippingCompanyDto>> SaveAsync(ShippingCompanyDto companyDto)
        {
            if (companyDto == null) throw new ArgumentNullException(nameof(companyDto));

            try
            {
                return await _apiService.PostAsync<ShippingCompanyDto, ShippingCompanyDto>($"{ApiEndpoints.ShippingCompany.Save}", companyDto);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<ShippingCompanyDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a shipping company by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.ShippingCompany.Delete}", id);
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
