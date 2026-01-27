using Shared.DTOs.Merchandising.Homepage;

namespace BL.Contracts.Service.Merchandising;

/// <summary>
/// Admin Block Service Interface
/// Handles all admin operations for homepage blocks
/// </summary>
public interface IAdminBlockService
{
    // === Block Management ===

    Task<AdminBlockCreateDto> CreateBlockAsync(AdminBlockCreateDto blockDto, Guid userId);
    Task<AdminBlockCreateDto> UpdateBlockAsync(Guid blockId, AdminBlockCreateDto blockDto, Guid userId);
    Task<bool> DeleteBlockAsync(Guid blockId, Guid userId);
    Task<AdminBlockCreateDto?> GetBlockByIdAsync(Guid blockId);
    Task<List<AdminBlockListDto>> GetAllBlocksAsync();
    Task<bool> UpdateDisplayOrderAsync(Guid blockId, int newOrder, Guid userId);

    // === Product Management ===

    Task<AdminBlockItemDto> AddProductToBlockAsync(Guid blockId, Guid itemId, int displayOrder, Guid userId);
    Task<bool> RemoveProductFromBlockAsync(Guid blockProductId, Guid userId);
    Task<List<AdminBlockItemDto>> GetBlockProductsAsync(Guid blockId);
    Task<bool> UpdateProductDisplayOrderAsync(Guid blockProductId, int newOrder, Guid userId);

    // === Category Management ===

    Task<AdminBlockCategoryDto> AddCategoryToBlockAsync(Guid blockId, Guid categoryId, int displayOrder, Guid userId);
    Task<bool> RemoveCategoryFromBlockAsync(Guid blockCategoryId, Guid userId);
    Task<List<AdminBlockCategoryDto>> GetBlockCategoriesAsync(Guid blockId);
}