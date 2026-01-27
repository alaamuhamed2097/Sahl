using AutoMapper;
using BL.Contracts.Service.Merchandising;
using Common.Enumerations.Merchandising;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Shared.DTOs.Merchandising.Homepage;

namespace BL.Services.Merchandising;

/// <summary>
/// Admin Block Service
/// Handles all admin operations for homepage blocks
/// </summary>
public class AdminBlockService : IAdminBlockService
{
    private readonly IHomepageBlockRepository _blockRepository;
    private readonly IMapper _mapper;

    public AdminBlockService(IHomepageBlockRepository blockRepository, IMapper mapper)
    {
        _blockRepository = blockRepository;
        _mapper = mapper;
    }

    #region Block Management

    public async Task<AdminBlockCreateDto> CreateBlockAsync(AdminBlockCreateDto blockDto, Guid userId)
    {
        // Map DTO to entity
        var block = _mapper.Map<TbHomepageBlock>(blockDto);

        // Validate configuration
        await ValidateBlockConfigurationAsync(block);

        // Set defaults
        block.Id = Guid.NewGuid();
        block.CreatedDateUtc = DateTime.UtcNow;
        block.CreatedBy = userId;
        block.IsDeleted = false;
        block.IsVisible = true;

        // Store items and categories temporarily
        var itemsToAdd = block.BlockItems?.ToList() ?? new List<TbBlockItem>();
        var categoriesToAdd = block.BlockCategories?.ToList() ?? new List<TbBlockCategory>();

        // Clear navigation properties before creating the block
        // This prevents EF from trying to insert them with the block
        block.BlockItems = new List<TbBlockItem>();
        block.BlockCategories = new List<TbBlockCategory>();

        // Use TableRepository's CreateAsync method
        var result = await _blockRepository.CreateAsync(block, userId);

        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to create block");
        }

        // Now add items if Type is ManualItems
        if (block.Type == HomepageBlockType.ManualItems && itemsToAdd.Any())
        {
            foreach (var item in itemsToAdd)
            {
                item.Id = Guid.NewGuid();
                item.HomepageBlockId = block.Id;
                item.CreatedDateUtc = DateTime.UtcNow;
                item.CreatedBy = userId;
                item.IsDeleted = false;

                await _blockRepository.AddProductToBlockAsync(item);
            }
        }

        // Add categories if Type is ManualCategories
        if (block.Type == HomepageBlockType.ManualCategories && categoriesToAdd.Any())
        {
            foreach (var category in categoriesToAdd)
            {
                category.Id = Guid.NewGuid();
                category.HomepageBlockId = block.Id;
                category.CreatedDateUtc = DateTime.UtcNow;
                category.CreatedBy = userId;
                category.IsDeleted = false;

                await _blockRepository.AddCategoryToBlockAsync(category);
            }
        }

        // Get the created block with all related data
        var createdBlock = await _blockRepository.GetBlockByIdAsync(result.Id)
            ?? throw new InvalidOperationException("Block created but not found");

        // Map back to DTO
        return _mapper.Map<AdminBlockCreateDto>(createdBlock);
    }

    public async Task<AdminBlockCreateDto> UpdateBlockAsync(Guid blockId, AdminBlockCreateDto blockDto, Guid userId)
    {
        // Map DTO to entity
        var block = _mapper.Map<TbHomepageBlock>(blockDto);
        block.Id = blockId; // Ensure the correct ID

        // Validate configuration
        await ValidateBlockConfigurationAsync(block);

        // Verify block exists
        var existing = await _blockRepository.GetBlockByIdAsync(block.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Block with ID {block.Id} not found");
        }

        // Store items and categories temporarily
        var itemsToProcess = block.BlockItems?.ToList() ?? new List<TbBlockItem>();
        var categoriesToProcess = block.BlockCategories?.ToList() ?? new List<TbBlockCategory>();

        // Clear navigation properties before updating the block
        block.BlockItems = new List<TbBlockItem>();
        block.BlockCategories = new List<TbBlockCategory>();

        // Handle BlockItems for ManualItems type
        if (block.Type == HomepageBlockType.ManualItems)
        {
            // Get existing items
            var existingItems = await _blockRepository.GetBlockProductsAsync(block.Id);
            var newItemIds = itemsToProcess.Select(i => i.ItemId).ToHashSet();
            
            // Remove items that are no longer in the list
            foreach (var existingItem in existingItems)
            {
                if (!newItemIds.Contains(existingItem.ItemId))
                {
                    await _blockRepository.RemoveProductFromBlockAsync(existingItem.Id);
                }
            }

            // Add new items
            foreach (var item in itemsToProcess)
            {
                var existingItem = existingItems.FirstOrDefault(ei => ei.ItemId == item.ItemId);
                if (existingItem == null)
                {
                    // New item - add it
                    item.Id = Guid.NewGuid();
                    item.HomepageBlockId = block.Id;
                    item.CreatedDateUtc = DateTime.UtcNow;
                    item.CreatedBy = userId;
                    item.IsDeleted = false;

                    await _blockRepository.AddProductToBlockAsync(item);
                }
                // Note: Display order updates would need a separate method
            }
        }

        // Handle BlockCategories for ManualCategories type
        if (block.Type == HomepageBlockType.ManualCategories)
        {
            // Get existing categories
            var existingCategories = await _blockRepository.GetBlockCategoriesAsync(block.Id);
            var newCategoryIds = categoriesToProcess.Select(c => c.CategoryId).ToHashSet();
            
            // Remove categories that are no longer in the list
            foreach (var existingCategory in existingCategories)
            {
                if (!newCategoryIds.Contains(existingCategory.CategoryId))
                {
                    await _blockRepository.RemoveCategoryFromBlockAsync(existingCategory.Id);
                }
            }

            // Add new categories
            foreach (var category in categoriesToProcess)
            {
                var existingCategory = existingCategories.FirstOrDefault(ec => ec.CategoryId == category.CategoryId);
                if (existingCategory == null)
                {
                    // New category - add it
                    category.Id = Guid.NewGuid();
                    category.HomepageBlockId = block.Id;
                    category.CreatedDateUtc = DateTime.UtcNow;
                    category.CreatedBy = userId;
                    category.IsDeleted = false;

                    await _blockRepository.AddCategoryToBlockAsync(category);
                }
            }
        }

        // Update using TableRepository's UpdateAsync
        var result = await _blockRepository.UpdateAsync(block, userId);

        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to update block");
        }

        // Get the updated block with all related data
        var updatedBlock = await _blockRepository.GetBlockByIdAsync(block.Id)
            ?? throw new InvalidOperationException("Block updated but not found");

        // Map back to DTO
        return _mapper.Map<AdminBlockCreateDto>(updatedBlock);
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

    public async Task<AdminBlockCreateDto?> GetBlockByIdAsync(Guid blockId)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        return block == null ? null : _mapper.Map<AdminBlockCreateDto>(block);
    }

    public async Task<List<AdminBlockListDto>> GetAllBlocksAsync()
    {
        var blocks = await _blockRepository.GetActiveBlocksAsync();
        return _mapper.Map<List<AdminBlockListDto>>(blocks);
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

    public async Task<AdminBlockItemDto> AddProductToBlockAsync(Guid blockId, Guid itemId, int displayOrder, Guid userId)
    {
        // Validate
        if (blockId == Guid.Empty)
        {
            throw new ArgumentException("BlockId is required");
        }

        if (itemId == Guid.Empty)
        {
            throw new ArgumentException("ItemId is required");
        }

        // Check if block exists
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null)
        {
            throw new KeyNotFoundException($"Block with ID {blockId} not found");
        }

        // Create block item
        var blockProduct = new TbBlockItem
        {
            Id = Guid.NewGuid(),
            HomepageBlockId = blockId,
            ItemId = itemId,
            DisplayOrder = displayOrder,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedBy = userId,
            IsDeleted = false
        };

        // Add product using repository method
        var created = await _blockRepository.AddProductToBlockAsync(blockProduct);
        return _mapper.Map<AdminBlockItemDto>(created);
    }

    public async Task<bool> RemoveProductFromBlockAsync(Guid blockProductId, Guid userId)
    {
        return await _blockRepository.RemoveProductFromBlockAsync(blockProductId);
    }

    public async Task<List<AdminBlockItemDto>> GetBlockProductsAsync(Guid blockId)
    {
        var items = await _blockRepository.GetBlockProductsAsync(blockId);
        return _mapper.Map<List<AdminBlockItemDto>>(items);
    }

    public async Task<bool> UpdateProductDisplayOrderAsync(Guid blockProductId, int newOrder, Guid userId)
    {
        // TODO: Implement if needed
        return true;
    }

    #endregion

    #region Category Management

    public async Task<AdminBlockCategoryDto> AddCategoryToBlockAsync(Guid blockId, Guid categoryId, int displayOrder, Guid userId)
    {
        // Validate
        if (blockId == Guid.Empty)
        {
            throw new ArgumentException("BlockId is required");
        }

        if (categoryId == Guid.Empty)
        {
            throw new ArgumentException("CategoryId is required");
        }

        // Check if block exists
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null)
        {
            throw new KeyNotFoundException($"Block with ID {blockId} not found");
        }

        // Create block category
        var blockCategory = new TbBlockCategory
        {
            Id = Guid.NewGuid(),
            HomepageBlockId = blockId,
            CategoryId = categoryId,
            DisplayOrder = displayOrder,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedBy = userId,
            IsDeleted = false
        };

        // Add category using repository method
        var created = await _blockRepository.AddCategoryToBlockAsync(blockCategory);
        return _mapper.Map<AdminBlockCategoryDto>(created);
    }

    public async Task<bool> RemoveCategoryFromBlockAsync(Guid blockCategoryId, Guid userId)
    {
        return await _blockRepository.RemoveCategoryFromBlockAsync(blockCategoryId);
    }

    public async Task<List<AdminBlockCategoryDto>> GetBlockCategoriesAsync(Guid blockId)
    {
        var categories = await _blockRepository.GetBlockCategoriesAsync(blockId);
        return _mapper.Map<List<AdminBlockCategoryDto>>(categories);
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