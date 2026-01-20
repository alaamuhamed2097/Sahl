using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Order.Payment.Refund;
using Shared.GeneralModels;

namespace Dashboard.Services.Order
{
    public class RefundService : IRefundService
    {
        private readonly IApiService _apiService;

        public RefundService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all refunds.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<RefundRequestDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<RefundRequestDto>>($"{ApiEndpoints.Refund.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<RefundRequestDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get refund by ID.
        /// </summary>
        public async Task<ResponseModel<RefundDetailsDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<RefundDetailsDto>($"{ApiEndpoints.Refund.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                Console.WriteLine($"{ex.Message}");
                return new ResponseModel<RefundDetailsDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get refund by order ID.
        /// </summary>
        public async Task<ResponseModel<RefundDto>> GetByOrderIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<RefundDto>($"{ApiEndpoints.Refund.Get}/order/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<RefundDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Change refund status.
        /// </summary>
        public async Task<ResponseModel<bool>> ChangeRefundStatusAsync(RefundResponseDto refund)
        {
            if (refund == null) throw new ArgumentNullException(nameof(refund));

            try
            {
                return await _apiService.PostAsync<RefundResponseDto, bool>($"{ApiEndpoints.Refund.ChangeRefundStatus}", refund);
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
        public async Task<ResponseModel<PaginatedDataModel<RefundRequestDto>>> SearchAsync(RefundSearchCriteria model, string Endpoint)
        {
            try
            {
                var queryString = $"PageNumber={model.PageNumber}&PageSize={model.PageSize}&SearchTerm={model.SearchTerm}";

                // Add sorting parameters if provided
                if (!string.IsNullOrWhiteSpace(model.SortBy))
                {
                    queryString += $"&SortBy={model.SortBy}&SortDirection={model.SortDirection}";
                }

                // Add sorting parameters if provided
                if (model.Status != null)
                {
                    queryString += $"&Status={model.Status}";
                }

                string url = $"{Endpoint}?{queryString}";
                return await _apiService.GetAsync<PaginatedDataModel<RefundRequestDto>>(url);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PaginatedDataModel<RefundRequestDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        /// <summary>
        /// update a refund.
        /// </summary>
        public async Task<ResponseModel<bool>> UpdateAsync(RefundRequestDto refund)
        {
            if (refund == null) throw new ArgumentNullException(nameof(refund));

            try
            {
                return await _apiService.PostAsync<RefundRequestDto, bool>($"{ApiEndpoints.Refund.Update}", refund);
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
        /// Delete a refund by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.Refund.Delete}", id);
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
