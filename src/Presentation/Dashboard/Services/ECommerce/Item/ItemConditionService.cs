using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels;

namespace Dashboard.Services.ECommerce.Item
{
    public class ItemConditionService : IItemConditionService
    {
        private readonly IApiService _apiService;

        public ItemConditionService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all item conditions with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<VendorItemConditionDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<VendorItemConditionDto>>($"{ApiEndpoints.ItemCondition.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<VendorItemConditionDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get item condition by ID.
        /// </summary>
        public async Task<ResponseModel<VendorItemConditionDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<VendorItemConditionDto>($"{ApiEndpoints.ItemCondition.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<VendorItemConditionDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a item condition.
        /// </summary>
        public async Task<ResponseModel<VendorItemConditionDto>> SaveAsync(VendorItemConditionDto itemCondition)
        {
            if (itemCondition == null) throw new ArgumentNullException(nameof(itemCondition));

            try
            {
                return await _apiService.PostAsync<VendorItemConditionDto, VendorItemConditionDto>($"{ApiEndpoints.ItemCondition.Save}", itemCondition); ;
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<VendorItemConditionDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a item condition by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.ItemCondition.Delete}", id);
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
