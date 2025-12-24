using AutoMapper;
using BL.Contracts.Service.Catalog.Pricing;
using BL.Contracts.Service.Merchandising;
using Common.Enumerations.Merchandising;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Merchandising;
using Shared.DTOs.Merchandising.Homepage;

namespace BL.Service.Merchandising
{
    /// <summary>
    /// Homepage Service - manages homepage blocks and content
    /// </summary>
    public class HomepageService : IHomepageService
    {
        private readonly IHomepageBlockRepository _blockRepository;
        private readonly IItemCombinationRepository _combinationRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;

        public HomepageService(
            IHomepageBlockRepository blockRepository,
            IItemCombinationRepository combinationRepository,
            ICampaignRepository campaignRepository,
            IPricingService pricingService,
            IMapper mapper)
        {
            _blockRepository = blockRepository;
            _combinationRepository = combinationRepository;
            _campaignRepository = campaignRepository;
            _pricingService = pricingService;
            _mapper = mapper;
        }

        #region Public Methods

        /// <summary>
        /// Get complete homepage with all blocks
        /// </summary>
        public async Task<GetHomepageResponse> GetHomepageAsync(GetHomepageRequest request)
        {
            // Get all active blocks
            var blocks = await _blockRepository.GetActiveBlocksAsync();

            // Map to DTOs
            var blockDtos = new List<HomepageBlockDto>();

            foreach (var block in blocks)
            {
                var blockDto = await MapBlockToDtoAsync(block, request);

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

            return await MapBlockToDtoAsync(block, new GetHomepageRequest());
        }

        #endregion

        #region Block Mapping

        /// <summary>
        /// Map block entity to DTO using AutoMapper
        /// </summary>
        private async Task<HomepageBlockDto> MapBlockToDtoAsync(
            Domains.Entities.Merchandising.TbHomepageBlock block,
            GetHomepageRequest request)
        {
            // Map basic properties using AutoMapper
            var dto = _mapper.Map<HomepageBlockDto>(block);

            // Load products based on block type
            dto.Products = await GetBlockProductsAsync(block, request);

            // Load categories (for CategoryShowcase blocks)
            if (block.Type == HomepageBlockType.ManualCategories && block.BlockCategories != null)
            {
                dto.Categories = _mapper.Map<List<CategoryCardDto>>(
                    block.BlockCategories.Where(bc => !bc.IsDeleted).OrderBy(bc => bc.DisplayOrder)
                );
            }

            return dto;
        }

        #endregion

        #region Product Loading

        /// <summary>
        /// Get products for a block based on its type
        /// </summary>
        private async Task<List<ItemCardDto>> GetBlockProductsAsync(
            Domains.Entities.Merchandising.TbHomepageBlock block,
            GetHomepageRequest request)
        {
            return block.Type switch
            {
                HomepageBlockType.ManualItems => await GetManualProductsAsync(block),
                HomepageBlockType.Campaign => await GetCampaignProductsAsync(block),
                HomepageBlockType.Dynamic => await GetDynamicProductsAsync(block),
                HomepageBlockType.Personalized => await GetPersonalizedProductsAsync(block, request),
                _ => new List<ItemCardDto>()
            };
        }

        /// <summary>
        /// Get manually selected products
        /// </summary>
        private async Task<List<ItemCardDto>> GetManualProductsAsync(
            Domains.Entities.Merchandising.TbHomepageBlock block)
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
            Domains.Entities.Merchandising.TbHomepageBlock block)
        {
            if (!block.CampaignId.HasValue)
                return new List<ItemCardDto>();

            var campaignItems = await _campaignRepository.GetCampaignItemsAsync(block.CampaignId.Value);

            var activeItems = campaignItems
                .Where(ci => ci.IsActive && !ci.IsDeleted)
                .OrderBy(ci => ci.DisplayOrder)
                .ToList();

            // Use AutoMapper for mapping
            var products = _mapper.Map<List<ItemCardDto>>(activeItems);

            // Calculate prices and discounts for each product
            for (int i = 0; i < products.Count; i++)
            {
                var campaignItem = activeItems[i];
                var product = products[i];
                var defaultCombination = campaignItem.Item.ItemCombinations?.FirstOrDefault(c => c.IsDefault);

                if (defaultCombination != null)
                {
                    // Get original price from pricing service
                    var context = new PricingContext
                    {
                        ItemCombination = defaultCombination,
                        RequestedQuantity = 1,
                        Strategy = defaultCombination.Item.Category.PricingSystemType,
                        CalculationDate = DateTime.UtcNow
                    };

                    var pricingResult = _pricingService.CalculatePrice(context);

                    product.Price = pricingResult.Price;
                    // SalePrice is already set from CampaignPrice by AutoMapper

                    // Calculate discount percentage
                    if (pricingResult.Price > 0 && campaignItem.CampaignPrice < pricingResult.Price)
                    {
                        var discount = ((pricingResult.Price - campaignItem.CampaignPrice) / pricingResult.Price) * 100;
                        product.DiscountPercentage = (int)discount;
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Get dynamic products (Best Sellers, New Arrivals, etc.)
        /// </summary>
        private async Task<List<ItemCardDto>> GetDynamicProductsAsync(
            Domains.Entities.Merchandising.TbHomepageBlock block)
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
            Domains.Entities.Merchandising.TbHomepageBlock block,
            GetHomepageRequest request)
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
            var products = _mapper.Map<List<ItemCardDto>>(combinations);

            // Apply pricing using PricingService
            for (int i = 0; i < products.Count; i++)
            {
                var combination = combinations[i];
                var product = products[i];

                // Create PricingContext
                var context = new PricingContext
                {
                    ItemCombination = combination,
                    RequestedQuantity = 1,
                    Strategy = combination.Item.Category.PricingSystemType,
                    CalculationDate = DateTime.UtcNow
                };

                var pricingResult = _pricingService.CalculatePrice(context);

                product.Price = pricingResult.Price;
                product.SalePrice = pricingResult.SalesPrice;

                // Calculate discount percentage
                if (pricingResult.Price > 0)
                {
                    var discount = ((pricingResult.Price - pricingResult.SalesPrice) / pricingResult.Price) * 100;
                    product.DiscountPercentage = (int)discount;
                }
            }

            return products.Where(p => p.IsAvailable).ToList();
        }

        #endregion
    }
}