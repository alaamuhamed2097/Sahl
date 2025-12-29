using BL.Contracts.Service.Merchandising;
using Common.Enumerations.Merchandising;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;

namespace BL.Services.Merchandising;

/// <summary>
/// Admin Block Service
/// Handles all admin operations for homepage blocks
/// </summary>
public class AdminBlockService : IAdminBlockService
{
    private readonly IHomepageBlockRepository _blockRepository;

    public AdminBlockService(IHomepageBlockRepository blockRepository)
    {
        _blockRepository = blockRepository;
    }

    #region Block Management

    public async Task<TbHomepageBlock> CreateBlockAsync(TbHomepageBlock block, Guid userId)
    {
        // Validate configuration
        await ValidateBlockConfigurationAsync(block);

        // Set defaults
        block.Id = Guid.NewGuid();
        block.CreatedDateUtc = DateTime.UtcNow;
        block.CreatedBy = userId;
        block.IsDeleted = false;
        block.IsVisible = true;

        // Use TableRepository's CreateAsync method
        var result = await _blockRepository.CreateAsync(block, userId);

        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to create block");
        }

        return await _blockRepository.GetBlockByIdAsync(result.Id)
            ?? throw new InvalidOperationException("Block created but not found");
    }

    public async Task<TbHomepageBlock> UpdateBlockAsync(TbHomepageBlock block, Guid userId)
    {
        // Validate configuration
        await ValidateBlockConfigurationAsync(block);

        // Verify block exists
        var existing = await _blockRepository.GetBlockByIdAsync(block.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Block with ID {block.Id} not found");
        }

        // Update using TableRepository's UpdateAsync
        var result = await _blockRepository.UpdateAsync(block, userId);

        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to update block");
        }

        return await _blockRepository.GetBlockByIdAsync(block.Id)
            ?? throw new InvalidOperationException("Block updated but not found");
    }

    public async Task<bool> DeleteBlockAsync(Guid blockId, Guid userId)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null)
        {
            return false;
        }

        // Soft delete using TableRepository's SoftDeleteAsync
        return await _blockRepository.SoftDeleteAsync(blockId, userId);
    }

    public async Task<TbHomepageBlock?> GetBlockByIdAsync(Guid blockId)
    {
        return await _blockRepository.GetBlockByIdAsync(blockId);
    }

    public async Task<List<TbHomepageBlock>> GetAllBlocksAsync()
    {
        return await _blockRepository.GetActiveBlocksAsync();
    }

    public async Task<bool> UpdateDisplayOrderAsync(Guid blockId, int newOrder, Guid userId)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null)
        {
            return false;
        }

        block.DisplayOrder = newOrder;
        var result = await _blockRepository.UpdateAsync(block, userId);

        return result.Success;
    }

    #endregion

    #region Product Management

    public async Task<TbBlockItem> AddProductToBlockAsync(TbBlockItem blockProduct, Guid userId)
    {
        // Validate
        if (blockProduct.HomepageBlockId == Guid.Empty)
        {
            throw new ArgumentException("HomepageBlockId is required");
        }

        if (blockProduct.ItemId == Guid.Empty)
        {
            throw new ArgumentException("ItemId is required");
        }

        // Check if block exists
        var block = await _blockRepository.GetBlockByIdAsync(blockProduct.HomepageBlockId);
        if (block == null)
        {
            throw new KeyNotFoundException($"Block with ID {blockProduct.HomepageBlockId} not found");
        }

        // Set defaults
        blockProduct.Id = Guid.NewGuid();
        blockProduct.CreatedDateUtc = DateTime.UtcNow;
        blockProduct.CreatedBy = userId;
        blockProduct.IsDeleted = false;

        // Add product using repository method
        return await _blockRepository.AddProductToBlockAsync(blockProduct);
    }

    public async Task<bool> RemoveProductFromBlockAsync(Guid blockProductId, Guid userId)
    {
        return await _blockRepository.RemoveProductFromBlockAsync(blockProductId);
    }

    public async Task<List<TbBlockItem>> GetBlockProductsAsync(Guid blockId)
    {
        return await _blockRepository.GetBlockProductsAsync(blockId);
    }

    public async Task<bool> UpdateProductDisplayOrderAsync(Guid blockProductId, int newOrder, Guid userId)
    {
        // TODO: Implement if needed
        return true;
    }

    #endregion

    #region Category Management

    public async Task<TbBlockCategory> AddCategoryToBlockAsync(TbBlockCategory blockCategory, Guid userId)
    {
        // Validate
        if (blockCategory.HomepageBlockId == Guid.Empty)
        {
            throw new ArgumentException("HomepageBlockId is required");
        }

        if (blockCategory.CategoryId == Guid.Empty)
        {
            throw new ArgumentException("CategoryId is required");
        }

        // Check if block exists
        var block = await _blockRepository.GetBlockByIdAsync(blockCategory.HomepageBlockId);
        if (block == null)
        {
            throw new KeyNotFoundException($"Block with ID {blockCategory.HomepageBlockId} not found");
        }

        // Set defaults
        blockCategory.Id = Guid.NewGuid();
        blockCategory.CreatedDateUtc = DateTime.UtcNow;
        blockCategory.CreatedBy = userId;
        blockCategory.IsDeleted = false;

        // Add category using repository method
        return await _blockRepository.AddCategoryToBlockAsync(blockCategory);
    }

    public async Task<bool> RemoveCategoryFromBlockAsync(Guid blockCategoryId, Guid userId)
    {
        return await _blockRepository.RemoveCategoryFromBlockAsync(blockCategoryId);
    }

    public async Task<List<TbBlockCategory>> GetBlockCategoriesAsync(Guid blockId)
    {
        return await _blockRepository.GetBlockCategoriesAsync(blockId);
    }

    #endregion

    #region Validation

    public Task<bool> ValidateBlockConfigurationAsync(TbHomepageBlock block)
    {
        var errors = new List<string>();

        // Required fields
        if (string.IsNullOrWhiteSpace(block.TitleAr))
        {
            errors.Add("Arabic title is required");
        }

        if (string.IsNullOrWhiteSpace(block.TitleEn))
        {
            errors.Add("English title is required");
        }

        // Type-specific validation
        switch (block.Type)
        {
            case HomepageBlockType.Campaign:
                if (!block.CampaignId.HasValue)
                {
                    errors.Add("Campaign ID is required for Campaign blocks");
                }
                break;

            case HomepageBlockType.Dynamic:
                if (!block.DynamicSource.HasValue)
                {
                    errors.Add("Dynamic source is required for Dynamic blocks");
                }
                break;

            case HomepageBlockType.Personalized:
                if (!block.PersonalizationSource.HasValue)
                {
                    errors.Add("Personalization source is required for Personalized blocks");
                }
                break;
        }

        // Display order
        if (block.DisplayOrder < 0)
        {
            errors.Add("Display order must be non-negative");
        }

        if (errors.Any())
        {
            throw new ArgumentException($"Validation failed: {string.Join(", ", errors)}");
        }

        return Task.FromResult(true);
    }

    #endregion
}