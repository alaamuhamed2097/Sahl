using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Pricing;
using BL.Contracts.Service.Merchandising;
using Common.Enumerations.Merchandising;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Catalog.Item;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Campaign;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Shared.DTOs.Merchandising.Homepage;

namespace BL.Services.Merchandising;

/// <summary>
/// Homepage Service - manages homepage blocks and content
/// </summary>
public class HomepageService : IHomepageService
{
    private readonly IHomepageBlockRepository _blockRepository;
    private readonly IItemCombinationRepository _combinationRepository;
    private readonly ICampaignItemRepository _campaignItemRepository;
    private readonly IPricingService _pricingService;
    private readonly ITableRepository<TbCategory> _categoryRepository;
    private readonly IBaseMapper _mapper;

	public HomepageService(
		IHomepageBlockRepository blockRepository,
		IItemCombinationRepository combinationRepository,
		IPricingService pricingService,
		ITableRepository<TbCategory> categoryRepository,
		IBaseMapper mapper,
		ICampaignItemRepository campaignItemRepository)
	{
		_blockRepository = blockRepository;
		_combinationRepository = combinationRepository;
		_pricingService = pricingService;
		_categoryRepository = categoryRepository;
		_mapper = mapper;
		_campaignItemRepository = campaignItemRepository;
	}

	#region Public Methods

	/// <summary>
	/// Get complete homepage with all blocks
	/// </summary>
	public async Task<GetHomepageResponse> GetHomepageAsync(string? userId)
    {
        // Get all active blocks
        var blocks = await _blockRepository.GetActiveBlocksAsync();

        // Map to DTOs
        var blockDtos = new List<HomepageBlockDto>();

        foreach (var block in blocks)
        {
            var blockDto = await MapBlockToDtoAsync(block, userId);

            // Only include blocks that have content
            if (blockDto != null && (blockDto.Products.Any() || blockDto.Categories.Any()))
            {
                blockDtos.Add(blockDto);
            }
        }

        return new GetHomepageResponse
        {
            Blocks = blockDtos,
            TotalBlocks = blockDtos.Count,
            LoadedAt = DateTime.UtcNow
        };
    }


    /// <summary>
    /// Get single block by ID
    /// </summary>
    public async Task<HomepageBlockDto?> GetBlockByIdAsync(Guid blockId)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null) return null;

        return await MapBlockToDtoAsync(block, null);
    }

    #endregion

    #region Block Mapping

    /// <summary>
    /// Map block entity to DTO using AutoMapper
    /// </summary>
    private async Task<HomepageBlockDto> MapBlockToDtoAsync(
        TbHomepageBlock block,
        string? userId)
    {
        // Map basic properties using AutoMapper
        var dto = _mapper.MapModel<TbHomepageBlock, HomepageBlockDto>(block);

        // Load products based on block type
        dto.Products = await GetBlockProductsAsync(block, userId);

        // Load categories (for CategoryShowcase blocks)
        if (block.Type == HomepageBlockType.ManualCategories && block.BlockCategories != null)
        {
            dto.Categories = _mapper.MapList<TbBlockCategory, CategoryCardDto>(
                block.BlockCategories.Where(bc => !bc.IsDeleted).OrderBy(bc => bc.DisplayOrder)
            ).ToList();
        }

        return dto;
    }

    #endregion

    #region Product Loading

    /// <summary>
    /// Get products for a block based on its type
    /// </summary>
    private async Task<List<ItemCardDto>> GetBlockProductsAsync(
        TbHomepageBlock block,
        string? userId)
    {
        return block.Type switch
        {
            HomepageBlockType.ManualItems => await GetManualProductsAsync(block),
            HomepageBlockType.Campaign => await GetCampaignProductsAsync(block),
            HomepageBlockType.Dynamic => await GetDynamicProductsAsync(block),
            HomepageBlockType.Personalized => await GetPersonalizedProductsAsync(block, userId),
            _ => new List<ItemCardDto>()
        };
    }

    /// <summary>
    /// Get manually selected products
    /// </summary>
    private async Task<List<ItemCardDto>> GetManualProductsAsync(
        TbHomepageBlock block)
    {
        if (block.BlockProducts == null || !block.BlockProducts.Any())
            return new List<ItemCardDto>();

        var itemIds = block.BlockProducts
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.DisplayOrder)
            .Select(p => p.ItemId)
            .ToList();

        return await GetProductsByItemIdsAsync(itemIds);
    }

    /// <summary>
    /// Get campaign products
    /// </summary>
    private async Task<List<ItemCardDto>> GetCampaignProductsAsync(
        TbHomepageBlock block)
    {
        if (!block.CampaignId.HasValue)
            return new List<ItemCardDto>();

        var campaignItems = await _campaignItemRepository.GetCampaignItemsAsync(block.CampaignId.Value);

        var activeItems = campaignItems
            .Where(ci => ci.IsActive && !ci.IsDeleted)
            .OrderBy(ci => ci.CreatedDateUtc)
            .ToList();

        if (!activeItems.Any())
            throw new ApplicationException("No active items found for the specified campaign.");

        // Use AutoMapper for mapping
        var products = _mapper.MapList<TbCampaignItem, ItemCardDto>(activeItems ?? new List<TbCampaignItem>()).ToList();

        // Calculate prices and discounts for each product
        for (int i = 0; i < products.Count; i++)
        {
            var campaignItem = activeItems[i];
            var product = products[i];
            var defaultCombination = campaignItem.OfferCombinationPricing.ItemCombination.Item.ItemCombinations?.FirstOrDefault(c => c.IsDefault);

            if (defaultCombination != null)
            {
                var pricingResult = await _pricingService.CalculatePrice(
                    defaultCombination.Id,
                    defaultCombination.Item.Category.PricingSystemType,
                    1);
            }
        }

        return products;
    }

    /// <summary>
    /// Get dynamic products (Best Sellers, New Arrivals, etc.)
    /// </summary>
    private async Task<List<ItemCardDto>> GetDynamicProductsAsync(
        TbHomepageBlock block)
    {
        if (!block.DynamicSource.HasValue)
            return new List<ItemCardDto>();

        var limit = 20; // Default limit for dynamic blocks

        // Get item IDs based on source
        var itemIds = block.DynamicSource.Value switch
        {
            DynamicBlockSource.BestSellers => await _combinationRepository.GetBestSellersAsync(limit),
            DynamicBlockSource.NewArrivals => await _combinationRepository.GetNewArrivalsAsync(limit),
            DynamicBlockSource.TopRated => await _combinationRepository.GetTopRatedAsync(limit),
            DynamicBlockSource.Trending => await _combinationRepository.GetTrendingAsync(limit),
            DynamicBlockSource.MostWishlisted => await _combinationRepository.GetMostWishlistedAsync(limit),
            _ => new List<Guid>()
        };

        return await GetProductsByItemIdsAsync(itemIds);
    }

    /// <summary>
    /// Get personalized products (View History, Purchase History, etc.)
    /// </summary>
    private async Task<List<ItemCardDto>> GetPersonalizedProductsAsync(
        TbHomepageBlock block,
        string? userId)
    {
        // TODO: Implement personalization logic with user history
        // For now, return empty list
        return new List<ItemCardDto>();
    }

    #endregion

    #region Mapping Helpers

    /// <summary>
    /// Get products by item IDs and map to DTOs using AutoMapper
    /// </summary>
    private async Task<List<ItemCardDto>> GetProductsByItemIdsAsync(List<Guid> itemIds)
    {
        if (!itemIds.Any())
            return new List<ItemCardDto>();

        // Get default combinations for items
        var combinations = await _combinationRepository.GetDefaultCombinationsAsync(itemIds);

        // Map using AutoMapper
        var products = _mapper.MapList<TbItemCombination, ItemCardDto>(combinations).ToList();

        // Apply pricing using PricingService
        for (int i = 0; i < products.Count; i++)
        {
            var combination = combinations[i];
            var product = products[i];

            var category = await _categoryRepository.FindByIdAsync(combination.Item.CategoryId);
            if (category == null) throw new ApplicationException($"Category with id={combination.Item.CategoryId} not found");
        }

        return products.ToList();
    }

    #endregion
}