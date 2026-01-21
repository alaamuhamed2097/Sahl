using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;
using Shared.GeneralModels;

namespace Dashboard.Services.Order
{
    /// <summary>
    /// Order Service for Admin Dashboard - CLEAN VERSION
    /// Uses API DTOs directly without intermediate mapping
    /// This is the proper business-logic approach
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IApiService _apiService;

        public OrderService(IApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        /// <summary>
        /// Get order by ID
        /// Returns: AdminOrderDetailsDto directly from API
        /// </summary>
        public async Task<ResponseModel<AdminOrderDetailsDto>> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                var endpoint = $"{ApiEndpoints.Order.GetById}/{orderId}";

                // Return API DTO directly - NO MAPPING
                return await _apiService.GetAsync<AdminOrderDetailsDto>(endpoint);
            }
            catch (Exception ex)
            {
                return new ResponseModel<AdminOrderDetailsDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Change order status
        /// </summary>
        public async Task<ResponseModel<bool>> ChangeOrderStatusAsync(ChangeOrderStatusRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var endpoint = $"{ApiEndpoints.Order.ChangeStatus}/{request.OrderId}/change-status";
                return await _apiService.PostAsync<ChangeOrderStatusRequest, bool>(endpoint, request);
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
        /// Update order
        /// </summary>
        public async Task<ResponseModel<bool>> UpdateOrderAsync(UpdateOrderRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var endpoint = $"{ApiEndpoints.Order.Update}/{request.OrderId}";
                return await _apiService.PutAsync<UpdateOrderRequest, bool>(endpoint, request);
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
        /// Cancel order (changes status to Cancelled)
        /// </summary>
        public async Task<ResponseModel<bool>> CancelOrderAsync(Guid orderId, string reason)
        {
            try
            {
                var request = new ChangeOrderStatusRequest
                {
                    OrderId = orderId,
                    NewStatus = Common.Enumerations.Order.OrderProgressStatus.Cancelled,
                    Notes = reason
                };

                return await ChangeOrderStatusAsync(request);
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
        /// Get today's orders count
        /// </summary>
        public async Task<ResponseModel<int>> GetTodayOrdersCountAsync()
        {
            try
            {
                var endpoint = ApiEndpoints.Order.TodayCount;
                return await _apiService.GetAsync<int>(endpoint);
            }
            catch (Exception ex)
            {
                return new ResponseModel<int>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}