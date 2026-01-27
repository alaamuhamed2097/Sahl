using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Pricing;
using BL.Contracts.Service.Merchandising;
using Common.Enumerations.Merchandising;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Catalog.Item;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Serilog;
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
    private readonly ITableRepository<TbBlockItem> _blockItemRepository;
    private readonly ITableRepository<TbBlockCategory> _blockCategoryRepository;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public HomepageService(
        IHomepageBlockRepository blockRepository,
        IItemCombinationRepository combinationRepository,
        IPricingService pricingService,
        ITableRepository<TbCategory> categoryRepository,
        IBaseMapper mapper,
        ICampaignItemRepository campaignItemRepository,
        ITableRepository<TbBlockItem> blockItemRepository,
        ITableRepository<TbBlockCategory> blockCategoryRepository,
        ILogger logger)
    {
        _blockRepository = blockRepository;
        _combinationRepository = combinationRepository;
        _pricingService = pricingService;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _campaignItemRepository = campaignItemRepository;
        _blockItemRepository = blockItemRepository;
        _blockCategoryRepository = blockCategoryRepository;
        _logger = logger;
    }

    #region Public Methods

    /// <summary>
    /// Get complete homepage with all blocks (preview with limited items)
    /// Categories blocks will include category details
    /// </summary>
    public async Task<GetHomepageResponse> GetHomepageAsync(string? userId)
    {
        var blocks = await _blockRepository.GetActiveBlocksAsync();
        var blockDtos = new List<HomepageBlockDto>();

        foreach (var block in blocks)
        {
            // For category blocks, load categories
            if (block.Type == HomepageBlockType.ManualCategories)
            {
                var dto = _mapper.MapModel<TbHomepageBlock, HomepageBlockDto>(block);
                dto.Products = new List<ItemCardDto>();
                
                // Load categories for the block
                var blockCategories = await _blockCategoryRepository.GetAsync(
                    predicate: bc => bc.HomepageBlockId == block.Id && !bc.IsDeleted,
                    orderBy: q => q.OrderBy(bc => bc.DisplayOrder),
                    includeProperties: "Category"
                );

                var categoryList = blockCategories.ToList();
                dto.Categories = categoryList
                    .Select(bc => _mapper.MapModel<TbCategory, CategoryCardDto>(bc.Category))
                    .ToList();
                dto.TotalCategoryCount = categoryList.Count;
                
                blockDtos.Add(dto);
            }
            else
            {
                // Get preview (first 10 items) for other blocks
                var blockDto = await MapBlockToDtoAsync(block, userId, pageSize: 10);

                if (blockDto != null && blockDto.Products.Any())
                {
                    blockDtos.Add(blockDto);
                }
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
    /// Get single block by ID with pagination (for items only)
    /// </summary>
    public async Task<HomepageBlockDto?> GetBlockByIdAsync(Guid blockId, int pageNumber = 1, int pageSize = 20)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null) return null;

        // Don't load categories here - use GetBlockCategoriesAsync instead
        if (block.Type == HomepageBlockType.ManualCategories)
        {
            var dto = _mapper.MapModel<TbHomepageBlock, HomepageBlockDto>(block);
            dto.Products = new List<ItemCardDto>();
            dto.Categories = new List<CategoryCardDto>();
            dto.TotalCategoryCount = await GetBlockCategoriesCountAsync(blockId);
            return dto;
        }

        return await MapBlockToDtoAsync(block, null, pageNumber, pageSize);
    }

    /// <summary>
    /// Get block items with pagination for "Show More" functionality
    /// </summary>
    public async Task<BlockItemsPagedResponse> GetBlockItemsAsync(
        Guid blockId,
        string? userId,
        int pageNumber = 1,
        int pageSize = 20)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null)
            throw new ApplicationException("Block not found");

        if (block.Type == HomepageBlockType.ManualCategories)
            throw new InvalidOperationException("Use GetBlockCategoriesAsync for category blocks");

        var (items, totalCount) = await GetBlockItemsPagedAsync(
            block,
            userId,
            pageNumber,
            pageSize
        );

        return new BlockItemsPagedResponse
        {
            BlockId = blockId,
            BlockTitleAr = block.TitleAr,
            BlockTitleEn = block.TitleEn,
            BlockType = block.Type.ToString(), // ✅ Convert enum to string
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            HasNextPage = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize),
            HasPreviousPage = pageNumber > 1
        };
    }

    /// <summary>
    /// Get block categories with pagination - separate API for category blocks
    /// </summary>
    public async Task<BlockCategoriesPagedResponse> GetBlockCategoriesAsync(
        Guid blockId,
        int pageNumber = 1,
        int pageSize = 20)
    {
        var block = await _blockRepository.GetBlockByIdAsync(blockId);
        if (block == null)
            throw new ApplicationException("Block not found");

        if (block.Type != HomepageBlockType.ManualCategories)
            throw new InvalidOperationException("This endpoint is only for category blocks");

        // ✅ Use the base repository methods properly
        var allCategories = await _blockCategoryRepository.GetAsync(
            predicate: bc => bc.HomepageBlockId == blockId && !bc.IsDeleted,
            orderBy: q => q.OrderBy(bc => bc.DisplayOrder),
            includeProperties: "Category"
        );

        var categoryList = allCategories.ToList();
        var totalCount = categoryList.Count;

        var pagedCategories = categoryList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Map categories using AutoMapper
        var categories = pagedCategories
            .Select(bc => _mapper.MapModel<TbCategory, CategoryCardDto>(bc.Category))
            .ToList();

        return new BlockCategoriesPagedResponse
        {
            BlockId = blockId,
            BlockTitleAr = block.TitleAr,
            BlockTitleEn = block.TitleEn,
            BlockType = block.Type.ToString(),
            Categories = categories,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            HasNextPage = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize),
            HasPreviousPage = pageNumber > 1
        };
    }

    #endregion

    #region Block Mapping

    /// <summary>
    /// Map block entity to DTO using AutoMapper (items only, no categories)
    /// </summary>
    private async Task<HomepageBlockDto> MapBlockToDtoAsync(
        TbHomepageBlock block,
        string? userId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var dto = _mapper.MapModel<TbHomepageBlock, HomepageBlockDto>(block);

        // Load products based on block type
        var (products, totalCount) = await GetBlockItemsPagedAsync(block, userId, pageNumber, pageSize);
        dto.Products = products;
        dto.TotalProductCount = totalCount;

        // Don't load categories - use separate API
        dto.Categories = new List<CategoryCardDto>();

        return dto;
    }

    #endregion

    #region Paginated Item Loading

    /// <summary>
    /// Get paginated items based on block type
    /// </summary>
    private async Task<(List<ItemCardDto> items, int totalCount)> GetBlockItemsPagedAsync(
        TbHomepageBlock block,
        string? userId,
        int pageNumber,
        int pageSize)
    {
        return block.Type switch
        {
            HomepageBlockType.ManualItems =>
                await GetManualItemsPagedAsync(block.Id, pageNumber, pageSize),

            HomepageBlockType.Campaign =>
                await GetCampaignItemsPagedAsync(block.CampaignId!.Value, pageNumber, pageSize),

            HomepageBlockType.Dynamic =>
                await GetDynamicItemsPagedAsync(block.DynamicSource!.Value, pageNumber, pageSize),

            HomepageBlockType.Personalized =>
                await GetPersonalizedItemsPagedAsync(userId, pageNumber, pageSize),

            _ => (new List<ItemCardDto>(), 0)
        };
    }

    /// <summary>
    /// Get total count of categories in a block
    /// </summary>
    private async Task<int> GetBlockCategoriesCountAsync(Guid blockId)
    {
        var categories = await _blockCategoryRepository.GetAsync(
            predicate: bc => bc.HomepageBlockId == blockId && !bc.IsDeleted
        );
        return categories.Count();
    }

    #endregion

    #region Manual Items

    /// <summary>
    /// Get manually selected products with pagination
    /// </summary>
    private async Task<(List<ItemCardDto>, int)> GetManualItemsPagedAsync(
        Guid blockId,
        int pageNumber,
        int pageSize)
    {
        // ✅ Use GetAsync properly
        var allBlockItems = await _blockItemRepository.GetAsync(
            predicate: bp => bp.HomepageBlockId == blockId && !bp.IsDeleted,
            orderBy: q => q.OrderBy(bp => bp.DisplayOrder)
        );

        var blockItemsList = allBlockItems.ToList();
        var totalCount = blockItemsList.Count;

        var pagedBlockItems = blockItemsList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(bp => new { bp.ItemId, bp.DisplayOrder })
            .ToList();

        if (!pagedBlockItems.Any())
            return (new List<ItemCardDto>(), totalCount);

        var itemIds = pagedBlockItems.Select(bi => bi.ItemId).ToList();
        var items = await GetProductsByItemIdsAsync(itemIds);

        // Preserve DisplayOrder from block
        var orderedItems = pagedBlockItems
            .Join(items,
                bi => bi.ItemId,
                item => item.ItemId,
                (bi, item) => item)
            .ToList();

        return (orderedItems, totalCount);
    }

    #endregion

    #region Campaign Items

    /// <summary>
    /// Get campaign products with pagination
    /// </summary>
    private async Task<(List<ItemCardDto>, int)> GetCampaignItemsPagedAsync(
        Guid campaignId,
        int pageNumber,
        int pageSize)
    {
        // ✅ Remove DisplayOrder since it doesn't exist in TbCampaignItem
        var allCampaignItems = await _campaignItemRepository.GetAsync(
            predicate: ci => ci.CampaignId == campaignId && ci.IsActive && !ci.IsDeleted,
            orderBy: q => q.OrderBy(ci => ci.CreatedDateUtc), // ✅ Order by CreatedDateUtc only
            includeProperties: "OfferCombinationPricing.ItemCombination.Item.Brand,OfferCombinationPricing.ItemCombination.Item.Category,OfferCombinationPricing.ItemCombination.Item.ItemImages"
        );

        var campaignItemsList = allCampaignItems.ToList();
        var totalCount = campaignItemsList.Count;

        var pagedCampaignItems = campaignItemsList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var products = new List<ItemCardDto>();

        foreach (var campaignItem in pagedCampaignItems)
        {
            var combination = campaignItem.OfferCombinationPricing.ItemCombination;
            var item = combination.Item;

            // Map using AutoMapper
            var product = _mapper.MapModel<TbItem, ItemCardDto>(item);

            // Set basic combination info
            product.ItemCombinationId = combination.Id;
            product.IsDefault = combination.IsDefault;

            // Calculate pricing
            try
            {
                var pricingResult = await _pricingService.CalculatePrice(
                    combination.Id,
                    item.Category.PricingSystemType,
                    1
                );

                product.Price = pricingResult.FinalPrice;
                product.OriginalPrice = pricingResult.OriginalPrice;
                product.DiscountPercentage = pricingResult.DiscountPercentage;
                product.IsBuyBoxWinner = campaignItem.OfferCombinationPricing.IsBuyBoxWinner;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error calculating price for combination {combination.Id}");
                product.Price = item.BasePrice ?? 0;
                product.OriginalPrice = item.BasePrice ?? 0;
                product.DiscountPercentage = 0;
            }

            products.Add(product);
        }

        return (products, totalCount);
    }

    #endregion

    #region Dynamic Items

    /// <summary>
    /// Get dynamic products with pagination based on source
    /// </summary>
    private async Task<(List<ItemCardDto>, int)> GetDynamicItemsPagedAsync(
        DynamicBlockSource source,
        int pageNumber,
        int pageSize)
    {
        // Get total count first
        var totalCount = await GetDynamicSourceCountAsync(source);

        // Get paginated item IDs
        var itemIds = source switch
        {
            DynamicBlockSource.BestSellers =>
                await _combinationRepository.GetBestSellersAsync(pageSize, (pageNumber - 1) * pageSize),

            DynamicBlockSource.NewArrivals =>
                await _combinationRepository.GetNewArrivalsAsync(pageSize, (pageNumber - 1) * pageSize),

            DynamicBlockSource.TopRated =>
                await _combinationRepository.GetTopRatedAsync(pageSize, (pageNumber - 1) * pageSize),

            DynamicBlockSource.Trending =>
                await _combinationRepository.GetTrendingAsync(pageSize, (pageNumber - 1) * pageSize),

            DynamicBlockSource.MostWishlisted =>
                await _combinationRepository.GetMostWishlistedAsync(pageSize, (pageNumber - 1) * pageSize),

            _ => throw new ArgumentException($"Invalid dynamic source: {source}")
        };

        var items = await GetProductsByItemIdsAsync(itemIds);

        return (items, totalCount);
    }

    private async Task<int> GetDynamicSourceCountAsync(DynamicBlockSource source)
    {
        return source switch
        {
            DynamicBlockSource.BestSellers =>
                await _combinationRepository.GetBestSellersCountAsync(),

            DynamicBlockSource.NewArrivals =>
                await _combinationRepository.GetNewArrivalsCountAsync(),

            DynamicBlockSource.TopRated =>
                await _combinationRepository.GetTopRatedCountAsync(),

            DynamicBlockSource.Trending =>
                await _combinationRepository.GetTrendingCountAsync(),

            DynamicBlockSource.MostWishlisted =>
                await _combinationRepository.GetMostWishlistedCountAsync(),

            _ => 0
        };
    }

    #endregion

    #region Personalized Items

    /// <summary>
    /// Get personalized products (View History, Purchase History, etc.)
    /// </summary>
    private async Task<(List<ItemCardDto>, int)> GetPersonalizedItemsPagedAsync(
        string? userId,
        int pageNumber,
        int pageSize)
    {
        // TODO: Implement personalization logic with user history
        return (new List<ItemCardDto>(), 0);
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

        var combinations = await _combinationRepository.GetDefaultCombinationsAsync(itemIds);

        if (!combinations.Any())
            return new List<ItemCardDto>();

        var products = new List<ItemCardDto>();

        foreach (var combination in combinations)
        {
            var item = combination.Item;
            var product = _mapper.MapModel<TbItem, ItemCardDto>(item);

            product.ItemCombinationId = combination.Id;
            product.IsDefault = combination.IsDefault;

            var category = await _categoryRepository.FindByIdAsync(item.CategoryId);
            if (category == null)
            {
                _logger.Warning($"Category with id={item.CategoryId} not found for item {item.Id}");
                continue;
            }

            try
            {
                var pricingResult = await _pricingService.CalculatePrice(
                    combination.Id,
                    category.PricingSystemType,
                    1
                );

                product.Price = pricingResult.FinalPrice;
                product.OriginalPrice = pricingResult.OriginalPrice;
                product.DiscountPercentage = pricingResult.DiscountPercentage;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error calculating price for combination {combination.Id}");
                product.Price = item.BasePrice ?? 0;
                product.OriginalPrice = item.BasePrice ?? 0;
                product.DiscountPercentage = 0;
            }

            products.Add(product);
        }

        // Preserve original order
        var orderedProducts = itemIds
            .Join(products,
                id => id,
                p => p.ItemId,
                (id, p) => p)
            .ToList();

        return orderedProducts;
    }

    #endregion
}