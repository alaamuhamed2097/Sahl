using Domains.Entities.Merchandising;

namespace BL.Contracts.Service.Merchandising
{
    /// <summary>
    /// Admin Block Service Interface
    /// Handles all admin operations for homepage blocks
    /// </summary>
    public interface IAdminBlockService
    {
        // === Block Management ===

        Task<TbHomepageBlock> CreateBlockAsync(TbHomepageBlock block, Guid userId);
        Task<TbHomepageBlock> UpdateBlockAsync(TbHomepageBlock block, Guid userId);
        Task<bool> DeleteBlockAsync(Guid blockId, Guid userId);
        Task<TbHomepageBlock?> GetBlockByIdAsync(Guid blockId);
        Task<List<TbHomepageBlock>> GetAllBlocksAsync();
        Task<bool> UpdateDisplayOrderAsync(Guid blockId, int newOrder, Guid userId);

        // === Product Management ===

        Task<TbBlockItem> AddProductToBlockAsync(TbBlockItem blockProduct, Guid userId);
        Task<bool> RemoveProductFromBlockAsync(Guid blockProductId, Guid userId);
        Task<List<TbBlockItem>> GetBlockProductsAsync(Guid blockId);
        Task<bool> UpdateProductDisplayOrderAsync(Guid blockProductId, int newOrder, Guid userId);

        // === Category Management ===

        Task<TbBlockCategory> AddCategoryToBlockAsync(TbBlockCategory blockCategory, Guid userId);
        Task<bool> RemoveCategoryFromBlockAsync(Guid blockCategoryId, Guid userId);
        Task<List<TbBlockCategory>> GetBlockCategoriesAsync(Guid blockId);

        // === Validation ===

        Task<bool> ValidateBlockConfigurationAsync(TbHomepageBlock block);
    }
}