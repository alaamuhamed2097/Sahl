using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Order;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.GeneralModels;

namespace Dashboard.Services.Order
{
    public class ShipmentService : IShipmentService
    {
        private readonly IApiService _apiService;

        public ShipmentService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all shipments for a specific order.
        /// </summary>
        public async Task<ResponseModel<List<ShipmentDto>>> GetOrderShipmentsAsync(Guid orderId)
        {
            try
            {
                return await _apiService.GetAsync<List<ShipmentDto>>($"{ApiEndpoints.Shipment.GetOrderShipments}/{orderId}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ShipmentDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new List<ShipmentDto>()
                };
            }
        }

        /// <summary>
        /// Get shipment tracking information by tracking number.
        /// </summary>
        public async Task<ResponseModel<ShipmentTrackingDto>> GetShipmentTrackingAsync(string trackingNumber)
        {
            try
            {
                return await _apiService.GetAsync<ShipmentTrackingDto>($"{ApiEndpoints.Shipment.Track}/{trackingNumber}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<ShipmentTrackingDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Update a shipment status for a specific order (admin-only endpoint).
        /// </summary>
        public async Task<ResponseModel<ShipmentDto>> UpdateShipmentStatusAsync(Guid orderId, UpdateShipmentStatusRequest request)
        {
            try
            {
                request.OrderId = orderId;
                var endpoint = ApiEndpoints.Order.ShipmentStatus(orderId, request.ShipmentId);
                return await _apiService.PutAsync<UpdateShipmentStatusRequest, ShipmentDto>(endpoint, request);
            }
            catch (Exception ex)
            {
                return new ResponseModel<ShipmentDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
