using Dashboard.Constants;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels;

namespace Dashboard.Services
{
    /// <summary>
    /// Service for CouponCode operations in Dashboard - UPDATED
    /// </summary>
    public class CouponCodeService : ICouponCodeService
    {
        private readonly IApiService _apiService;

        public CouponCodeService(IApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        /// <summary>
        /// Get all coupon codes
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CouponCodeDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CouponCodeDto>>($"{ApiEndpoints.CouponCode.Get}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<CouponCodeDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get coupon code by ID
        /// </summary>
        public async Task<ResponseModel<CouponCodeDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CouponCodeDto>($"{ApiEndpoints.CouponCode.Get}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                return new ResponseModel<CouponCodeDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a coupon code
        /// </summary>
        public async Task<ResponseModel<bool>> SaveAsync(CouponCodeDto couponCode)
        {
            if (couponCode == null)
                throw new ArgumentNullException(nameof(couponCode));

            try
            {
                var result = await _apiService.PostAsync<CouponCodeDto, CouponCodeDto>(
                    $"{ApiEndpoints.CouponCode.Save}",
                    couponCode);

                return new ResponseModel<bool>
                {
                    Success = result.Success,
                    Data = result.Success,
                    Message = result.Message ?? (result.Success ? NotifiAndAlertsResources.SavedSuccessfully : NotifiAndAlertsResources.SaveFailed),
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveAsync: {ex.Message}");
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                };
            }
        }

        /// <summary>
        /// Delete a coupon code by ID
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, string>(
                    $"{ApiEndpoints.CouponCode.Delete}",
                    id);

                return new ResponseModel<bool>
                {
                    Success = result.Success,
                    Data = result.Success,
                    Message = result.Message ?? (result.Success ? NotifiAndAlertsResources.DeletedSuccessfully : NotifiAndAlertsResources.DeleteFailed),
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }


        /// <summary>
        /// Search coupon codes
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CouponCodeDto>>> SearchAsync(string searchTerm = "")
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CouponCodeDto>>($"{ApiEndpoints.CouponCode.Search}?searchTerm={searchTerm}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<CouponCodeDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}