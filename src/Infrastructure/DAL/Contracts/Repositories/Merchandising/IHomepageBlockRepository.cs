using Common.Enumerations.Merchandising;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;

namespace DAL.Contracts.Repositories.Merchandising
{
    /// <summary>
    /// Interface for Homepage Block Repository
    /// </summary>
    public interface IHomepageBlockRepository : ITableRepository<TbHomepageBlock>
    {
        #region Query Methods

        /// <summary>
        /// Get all active visible blocks for homepage
        /// </summary>
        Task<List<TbHomepageBlock>> GetActiveBlocksAsync();

        /// <summary>
        /// Get single block with all related data
        /// </summary>
        Task<TbHomepageBlock?> GetBlockByIdAsync(Guid blockId);

        /// <summary>
        /// Get blocks by type (e.g., all Campaign blocks)
        /// </summary>
        Task<List<TbHomepageBlock>> GetBlocksByTypeAsync(HomepageBlockType type);

        /// <summary>
        /// Get all blocks for a specific campaign
        /// </summary>
        Task<List<TbHomepageBlock>> GetBlocksByCampaignAsync(Guid campaignId);

        #endregion

        #region Product Management

        /// <summary>
        /// Add product to a manual block
        /// </summary>
        Task<TbBlockItem> AddProductToBlockAsync(TbBlockItem blockProduct);

        /// <summary>
        /// Remove product from block (soft delete)
        /// </summary>
        Task<bool> RemoveProductFromBlockAsync(Guid blockProductId);

        /// <summary>
        /// Get all products in a block
        /// </summary>
        Task<List<TbBlockItem>> GetBlockProductsAsync(Guid blockId);

        /// <summary>
        /// Update display order for products in block
        /// </summary>
        Task<bool> ReorderProductsAsync(List<(Guid ProductId, int Order)> productOrders);

        #endregion

        #region Category Management

        /// <summary>
        /// Add category to a category showcase block
        /// </summary>
        Task<TbBlockCategory> AddCategoryToBlockAsync(TbBlockCategory blockCategory);

        /// <summary>
        /// Remove category from block (soft delete)
        /// </summary>
        Task<bool> RemoveCategoryFromBlockAsync(Guid blockCategoryId);

        /// <summary>
        /// Get all categories in a block
        /// </summary>
        Task<List<TbBlockCategory>> GetBlockCategoriesAsync(Guid blockId);

        #endregion

        #region Block Management

        /// <summary>
        /// Toggle block visibility
        /// </summary>
        Task<bool> ToggleBlockVisibilityAsync(Guid blockId, bool isVisible);

        /// <summary>
        /// Update display order for multiple blocks
        /// </summary>
        Task<bool> ReorderBlocksAsync(List<(Guid BlockId, int Order)> blockOrders);

        #endregion
    }
}