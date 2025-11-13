using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels;

namespace Dashboard.Services.ECommerce.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IApiService _apiService;

        public CategoryService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all categories with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CategoryDto>>($"{ApiEndpoints.Category.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<CategoryDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get category by ID.
        /// </summary>
        public async Task<ResponseModel<CategoryDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CategoryDto>($"{ApiEndpoints.Category.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<CategoryDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a category.
        /// </summary>
        public async Task<ResponseModel<bool>> SaveAsync(CategoryDto category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            try
            {
                var response = await _apiService.PostAsync<CategoryDto, bool>($"{ApiEndpoints.Category.Save}", category);
                return response;
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
        /// Change tree view serials.
        /// </summary>
        public async Task<ResponseModel<bool>> ReorderTreeAsync(Dictionary<Guid, string> serialAssignments)
        {
            if (serialAssignments == null) throw new ArgumentNullException(nameof(serialAssignments));

            try
            {
                var response = await _apiService.PostAsync<Dictionary<Guid, string>, bool>($"{ApiEndpoints.Category.ChangeTreeViewSerials}", serialAssignments);
                return response;
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
        /// Delete a category by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Category.Delete}", id);
                return result;
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
