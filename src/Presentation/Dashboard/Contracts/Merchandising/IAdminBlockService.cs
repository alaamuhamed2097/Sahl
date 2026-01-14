using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Merchandising
{
    /// <summary>
    /// Dashboard contract for admin block service
    /// </summary>
    public interface IAdminBlockService
    {
        // Block CRUD Operations
        Task<ResponseModel<AdminBlockListDto>> CreateBlockAsync(AdminBlockCreateDto blockDto);
        Task<ResponseModel<List<AdminBlockListDto>>> GetAllBlocksAsync();
        Task<ResponseModel<AdminBlockCreateDto>> GetBlockByIdAsync(Guid blockId);
        Task<ResponseModel<AdminBlockListDto>> UpdateBlockAsync(Guid blockId, AdminBlockCreateDto blockDto);
        Task<ResponseModel<object>> DeleteBlockAsync(Guid blockId);

        // Display Order Management
        Task<ResponseModel<object>> UpdateDisplayOrderAsync(Guid blockId, int newOrder);

        // Product/Item Management
        Task<ResponseModel<AdminBlockItemDto>> AddProductToBlockAsync(Guid blockId, Guid itemId, int displayOrder = 0);
        Task<ResponseModel<object>> RemoveProductFromBlockAsync(Guid productId);
        Task<ResponseModel<List<AdminBlockItemDto>>> GetBlockProductsAsync(Guid blockId);

        // Category Management
        Task<ResponseModel<AdminBlockCategoryDto>> AddCategoryToBlockAsync(Guid blockId, Guid categoryId, int displayOrder = 0);
        Task<ResponseModel<object>> RemoveCategoryFromBlockAsync(Guid categoryId);
        Task<ResponseModel<List<AdminBlockCategoryDto>>> GetBlockCategoriesAsync(Guid blockId);
    }
}
