using Dashboard.Contracts.General;
using Dashboard.Contracts.Merchandising;
using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;

namespace Dashboard.Services.Merchandising
{
    /// <summary>
    /// Dashboard service for homepage block management
    /// </summary>
    public class AdminBlockService : IAdminBlockService
    {
        private readonly IApiService _apiService;
        private const string BaseEndpoint = "api/v1/admin/blocks";

        public AdminBlockService(IApiService apiService)
        {
            _apiService = apiService;
        }

        #region Block CRUD Operations

        /// <summary>
        /// Create a new homepage block
        /// </summary>
        public async Task<ResponseModel<AdminBlockListDto>> CreateBlockAsync(AdminBlockCreateDto blockDto)
        {
            return await _apiService.PostAsync<AdminBlockCreateDto, AdminBlockListDto>(BaseEndpoint, blockDto);
        }

        /// <summary>
        /// Get all homepage blocks
        /// </summary>
        public async Task<ResponseModel<List<AdminBlockListDto>>> GetAllBlocksAsync()
        {
            return await _apiService.GetAsync<List<AdminBlockListDto>>(BaseEndpoint);
        }

        /// <summary>
        /// Get homepage block by ID
        /// </summary>
        public async Task<ResponseModel<AdminBlockCreateDto>> GetBlockByIdAsync(Guid blockId)
        {
            return await _apiService.GetAsync<AdminBlockCreateDto>($"{BaseEndpoint}/{blockId}");
        }

        /// <summary>
        /// Update an existing homepage block
        /// </summary>
        public async Task<ResponseModel<AdminBlockListDto>> UpdateBlockAsync(Guid blockId, AdminBlockCreateDto blockDto)
        {
            return await _apiService.PutAsync<AdminBlockCreateDto, AdminBlockListDto>($"{BaseEndpoint}/{blockId}", blockDto);
        }

        /// <summary>
        /// Delete a homepage block
        /// </summary>
        public async Task<ResponseModel<object>> DeleteBlockAsync(Guid blockId)
        {
            return await _apiService.DeleteAsync<object>($"{BaseEndpoint}/{blockId}");
        }

        #endregion

        #region Display Order Management

        /// <summary>
        /// Update display order for a block
        /// </summary>
        public async Task<ResponseModel<object>> UpdateDisplayOrderAsync(Guid blockId, int newOrder)
        {
            var request = new { newOrder };
            return await _apiService.PutAsync<object, object>($"{BaseEndpoint}/{blockId}/display-order", request);
        }

        #endregion

        #region Product/Item Management

        /// <summary>
        /// Add product to block
        /// </summary>
        public async Task<ResponseModel<AdminBlockItemDto>> AddProductToBlockAsync(Guid blockId, Guid itemId, int displayOrder = 0)
        {
            var request = new { itemId, displayOrder };
            return await _apiService.PostAsync<object, AdminBlockItemDto>($"{BaseEndpoint}/{blockId}/products", request);
        }

        /// <summary>
        /// Remove product from block
        /// </summary>
        public async Task<ResponseModel<object>> RemoveProductFromBlockAsync(Guid productId)
        {
            return await _apiService.DeleteAsync<object>($"{BaseEndpoint}/products/{productId}");
        }

        /// <summary>
        /// Get all products in a block
        /// </summary>
        public async Task<ResponseModel<List<AdminBlockItemDto>>> GetBlockProductsAsync(Guid blockId)
        {
            return await _apiService.GetAsync<List<AdminBlockItemDto>>($"{BaseEndpoint}/{blockId}/products");
        }

        #endregion

        #region Category Management

        /// <summary>
        /// Add category to block
        /// </summary>
        public async Task<ResponseModel<AdminBlockCategoryDto>> AddCategoryToBlockAsync(Guid blockId, Guid categoryId, int displayOrder = 0)
        {
            var request = new { categoryId, displayOrder };
            return await _apiService.PostAsync<object, AdminBlockCategoryDto>($"{BaseEndpoint}/{blockId}/categories", request);
        }

        /// <summary>
        /// Remove category from block
        /// </summary>
        public async Task<ResponseModel<object>> RemoveCategoryFromBlockAsync(Guid categoryId)
        {
            return await _apiService.DeleteAsync<object>($"{BaseEndpoint}/categories/{categoryId}");
        }

        /// <summary>
        /// Get all categories in a block
        /// </summary>
        public async Task<ResponseModel<List<AdminBlockCategoryDto>>> GetBlockCategoriesAsync(Guid blockId)
        {
            return await _apiService.GetAsync<List<AdminBlockCategoryDto>>($"{BaseEndpoint}/{blockId}/categories");
        }

        #endregion
    }
}
