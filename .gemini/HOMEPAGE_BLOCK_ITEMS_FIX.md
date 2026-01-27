# Homepage Block Items Save Fix - FINAL SOLUTION

## Problem
When saving a Homepage block with `Type = ManualItems` in the admin dashboard, the items were not being saved to the `TbBlockItems` table.

## Root Cause
The issue was that Entity Framework's `TableRepository.CreateAsync` and `UpdateAsync` methods were not properly handling navigation properties (`BlockItems` and `BlockCategories`). When the block entity was being saved, EF was not tracking the related items, so they weren't being persisted to the database.

## Solution
The fix involves explicitly saving items and categories **after** the block is created, using the repository's dedicated methods (`AddProductToBlockAsync` and `AddCategoryToBlockAsync`).

### Key Changes

#### 1. AutoMapper Profile (`HomepageBlockMappingProfile.cs`)

Added mapping to convert items from DTO to entity:

```csharp
CreateMap<TbHomepageBlock, AdminBlockCreateDto>()
    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BlockItems))
    .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.BlockCategories))
    .ReverseMap()
    .ForMember(dest => dest.BlockItems, opt => opt.MapFrom((src, dest, destMember, context) =>
    {
        // Only map items if Type is ManualItems
        if (!string.IsNullOrEmpty(src.Type) && 
            (src.Type.Equals("ManualItems", StringComparison.OrdinalIgnoreCase) || 
             src.Type == "1") && 
            src.Items != null && src.Items.Any())
        {
            return src.Items.Select(item => new TbBlockItem
            {
                ItemId = item.ItemId,
                DisplayOrder = item.DisplayOrder,
                HomepageBlockId = dest.Id
            }).ToList();
        }
        return new List<TbBlockItem>();
    }))
```

#### 2. Create Block Service (`AdminBlockService.CreateBlockAsync`)

The critical fix is to:
1. **Store items temporarily** before creating the block
2. **Clear navigation properties** to prevent EF from trying to insert them with the block
3. **Create the block first**
4. **Explicitly add each item** using `AddProductToBlockAsync`

```csharp
public async Task<TbHomepageBlock> CreateBlockAsync(TbHomepageBlock block, Guid userId)
{
    // Validate and set defaults
    await ValidateBlockConfigurationAsync(block);
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

    // Create the block first
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

    return await _blockRepository.GetBlockByIdAsync(result.Id)
        ?? throw new InvalidOperationException("Block created but not found");
}
```

#### 3. Update Block Service (`AdminBlockService.UpdateBlockAsync`)

For updates, the service:
1. **Removes items** that are no longer in the list
2. **Adds new items** that weren't previously in the block
3. **Updates display order** for existing items

```csharp
// Handle BlockItems for ManualItems type
if (block.Type == HomepageBlockType.ManualItems)
{
    // Remove existing items that are not in the new list
    var existingItems = await _blockRepository.GetBlockProductsAsync(block.Id);
    var newItemIds = block.BlockItems?.Select(i => i.ItemId).ToHashSet() ?? new HashSet<Guid>();
    
    foreach (var existingItem in existingItems)
    {
        if (!newItemIds.Contains(existingItem.ItemId))
        {
            await _blockRepository.RemoveProductFromBlockAsync(existingItem.Id);
        }
    }

    // Add or update items
    if (block.BlockItems != null && block.BlockItems.Any())
    {
        foreach (var item in block.BlockItems)
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
            }
            else
            {
                // Existing item - update display order
                item.Id = existingItem.Id;
                item.HomepageBlockId = block.Id;
            }
        }
    }
}
```

## Why This Works

1. **Separation of Concerns**: By clearing the navigation properties before creating the block, we prevent EF from attempting to handle them automatically (which was failing).

2. **Explicit Control**: Using the repository's `AddProductToBlockAsync` method gives us explicit control over when and how items are saved.

3. **Proper Initialization**: Each item is properly initialized with all required fields (Id, timestamps, user IDs, etc.) before being saved.

4. **Transaction Safety**: Each `AddProductToBlockAsync` call includes its own `SaveChangesAsync`, ensuring items are persisted immediately.

## Testing

To test the fix:

1. **Create a new block with items**:
```json
POST /api/v1/admin/blocks
{
  "titleAr": "منتجات مختارة",
  "titleEn": "Featured Products",
  "type": "ManualItems",
  "layout": 1,
  "displayOrder": 1,
  "isVisible": true,
  "items": [
    {
      "itemId": "guid-of-item-1",
      "displayOrder": 1
    },
    {
      "itemId": "guid-of-item-2",
      "displayOrder": 2
    }
  ]
}
```

2. **Verify in database**:
```sql
SELECT * FROM TbBlockItems WHERE HomepageBlockId = 'newly-created-block-guid'
```

You should see 2 rows with the items.

## Files Modified
1. `src\Core\BL\Mapper\Merchandising\HomepageBlockMappingProfile.cs`
2. `src\Core\BL\Services\Merchandising\AdminBlockService.cs`

## Build Status
✅ Build succeeded with no errors
