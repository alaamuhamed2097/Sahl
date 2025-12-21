using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Order.Order;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Order
{

    public class OrderService : IOrderService
    {
        private readonly IApiService _apiService;

        public OrderService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all Orders.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<OrderDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<OrderDto>>($"{ApiEndpoints.Order.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<OrderDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get Orders page.
        /// </summary>
        public async Task<ResponseModel<PaginatedDataModel<OrderDto>>> GetPage(OrderSearchCriteriaModel searchModel)
        {
            try
            {
                var queryString = $"UserId={searchModel.UserId}&PageNumber={searchModel.PageNumber}&PageSize={searchModel.PageSize}&SearchTerm={searchModel.SearchTerm}";
                string url = $"{ApiEndpoints.Order.SearchForUserId}?{queryString}";
                return await _apiService.GetAsync<PaginatedDataModel<OrderDto>>(url);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PaginatedDataModel<OrderDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get Order by ID.
        /// </summary>
        public async Task<ResponseModel<OrderDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<OrderDto>($"{ApiEndpoints.Order.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<OrderDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get Order Number.
        /// </summary>
        public async Task<ResponseModel<string>> GetOrderNumber(string Idetifire)
        {
            try
            {
                return await _apiService.GetAsync<string>($"{ApiEndpoints.Order.GetOrderNumber}/{Idetifire}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Change order status.
        /// </summary>
        public async Task<ResponseModel<bool>> ChangeOrderStatusAsync(OrderDto Order)
        {
            if (Order == null) throw new ArgumentNullException(nameof(Order));

            try
            {
                return await _apiService.PostAsync<OrderDto, bool>($"{ApiEndpoints.Order.ChangeOrderStatus}", Order);
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
        /// Save or update a Order.
        /// </summary>
        public async Task<ResponseModel<OrderDto>> SaveAsync(OrderDto Order)
        {
            if (Order == null) throw new ArgumentNullException(nameof(Order));

            try
            {
                return await _apiService.PostAsync<OrderDto, OrderDto>($"{ApiEndpoints.Order.Save}", Order);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<OrderDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a Order by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.Order.Delete}", id);
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
