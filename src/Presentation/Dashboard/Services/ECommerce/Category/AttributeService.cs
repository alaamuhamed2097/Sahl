using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels;
using Shared.ResultModels;

namespace Dashboard.Services.ECommerce.Category
{
    public class AttributeService : IAttributeService
    {
        private readonly IApiService _apiService;

        public AttributeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all attributes with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<AttributeDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<AttributeDto>>($"{ApiEndpoints.Attribute.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<AttributeDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get attribute by ID.
        /// </summary>
        public async Task<ResponseModel<AttributeDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<AttributeDto>($"{ApiEndpoints.Attribute.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<AttributeDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all attributes for a specific category.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CategoryAttributeDto>>> GetByCategoryIdAsync(Guid categoryId)
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CategoryAttributeDto>>($"{ApiEndpoints.Attribute.Get}/category/{categoryId}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<CategoryAttributeDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a attribute.
        /// </summary>
        public async Task<ResponseModel<bool>> SaveAsync(AttributeDto attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            try
            {
                return await _apiService.PostAsync<AttributeDto, bool>($"{ApiEndpoints.Attribute.Save}", attribute);
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
        /// Delete a attribute by ID.
        /// </summary>
        public async Task<ResponseModel<DeleteResult>> DeleteAsync(Guid id)
        {
            try
            {
                return await _apiService.PostAsync<Guid, DeleteResult>($"{ApiEndpoints.Attribute.Delete}", id);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<DeleteResult>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }
    }
}
