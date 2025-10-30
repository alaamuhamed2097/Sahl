using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels;

namespace Dashboard.Services.ECommerce.Item
{
    public class ItemService : IItemService
    {
        private readonly IApiService _apiService;

        public ItemService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all items with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<ItemDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<ItemDto>>($"{ApiEndpoints.Item.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<ItemDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<ItemDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<ItemDto>($"{ApiEndpoints.Item.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<ItemDto>
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

        /// <summary>
        /// Save or update an item.
        /// </summary>
        public async Task<ResponseModel<ItemDto>> SaveAsync(ItemDto item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            try
            {
                return await _apiService.PostAsync<ItemDto, ItemDto>($"{ApiEndpoints.Item.Save}", item);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<ItemDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete an item by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid itemId)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Item.Delete}", itemId);
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
