using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.VendorItem;
using BL.Contracts.Service.VendorWarehouse;
using BL.Extensions;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Offer;
using Common.Enumerations.Pricing;
using Common.Enumerations.Visibility;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using DAL.Repositories;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using Domains.Entities.Warehouse;
using Domains.Views.Offer;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace BL.Services.VendorItem
{
	public class VendorItemService : IVendorItemService
    {
        private const int MaxImageCount = 10;
        private readonly IOfferRepository _vendorItemRepository;
        private readonly IDevelopmentSettingsService _developmentSettingsService;
        private readonly IVendorWarehouseService _vendorWarehouseService;
        private readonly ITableRepository<TbCategory> _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;
		private readonly IRepository<VwVendorItem> _VwVendorItemRepository;




        public VendorItemService(IBaseMapper mapper,
            IUnitOfWork unitOfWork,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            ILogger logger,
            ITableRepository<TbCategory> categoryRepository,
            IOfferRepository vendorItemRepository
,
            IRepository<VwVendorItem> vwVendorItemRepository,
            IDevelopmentSettingsService developmentSettingsService,
            IVendorWarehouseService vendorWarehouseService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _vendorItemRepository = vendorItemRepository;
            _VwVendorItemRepository = vwVendorItemRepository;
            _developmentSettingsService = developmentSettingsService;
            _vendorWarehouseService = vendorWarehouseService;
        }

        public async Task<PagedResult<VendorItemDto>> GetPage(ItemSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter
            Expression<Func<TbOffer, bool>> filter = x => !x.IsDeleted;

            // Combine expressions manually
            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            //if (!string.IsNullOrWhiteSpace(searchTerm))
            //{
            //    filter = filter.And(x =>
            //        (x. != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
            //        (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
            //        (x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm)) ||
            //        (x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm))
            //    );
            //}

            //if (criteriaModel.CategoryIds?.Any() == true)
            //{
            //    filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
            //}

            // New Item Flags Filters
            if (criteriaModel.IsNewArrival.HasValue)
            {
                filter = filter.And(x => x.CreatedDateUtc.Date >= DateTime.UtcNow.AddDays(-3).Date);
            }

            // Get paginated data from repository
            var items = await _vendorItemRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
            );

            var itemsDto = _mapper.MapList<TbOffer, VendorItemDto>(items.Items);

            return new PagedResult<VendorItemDto>(itemsDto, items.TotalRecords);
        }

		public async Task<PagedResult<VendorItemDetailsDto>> GetPageVendor(ItemstatusSearchCriteriaModel criteriaModel)
		{
			if (criteriaModel == null)
				throw new ArgumentNullException(nameof(criteriaModel));
			if (criteriaModel.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);
			if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

			// Base filter
			Expression<Func<VwVendorItem, bool>> filter = x => true;

			// Search term filter
			var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				// محاولة parse الـ OfferVisibilityScope من الـ search term
				var matchedScopes = new List<OfferVisibilityScope>();
				foreach (OfferVisibilityScope scope in Enum.GetValues(typeof(OfferVisibilityScope)))
				{
					// البحث بالاسم أو بالقيمة الرقمية
					if (scope.ToString().ToLower().Contains(searchTerm) ||
						((int)scope).ToString().Contains(searchTerm))
					{
						matchedScopes.Add(scope);
					}
				}

				filter = filter.And(x =>
					(x.ItemTitleAr != null && x.ItemTitleAr.ToLower().Contains(searchTerm)) ||
					(x.ItemTitleEn != null && x.ItemTitleEn.ToLower().Contains(searchTerm)) ||
					(x.Barcode != null && x.Barcode.ToLower().Contains(searchTerm)) ||
					(x.SKU != null && x.SKU.ToLower().Contains(searchTerm)) ||
					(matchedScopes.Any() && matchedScopes.Contains(x.VisibilityScope))
				);
			}

			// Category filter
			if (criteriaModel.CategoryIds?.Any() == true)
			{
				filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
			}

			// Stock Status filter
			if (criteriaModel.StockStatuses?.Any() == true)
			{
				var stockStatusNames = criteriaModel.StockStatuses.Select(s => s.ToString()).ToList();
				filter = filter.And(x => stockStatusNames.Contains(x.StockStatus));
			}

			// Visibility Scope filter
			if (criteriaModel.VisibilityScopes?.Any() == true)
			{
				filter = filter.And(x => criteriaModel.VisibilityScopes.Contains(x.VisibilityScope));
			}

			// New Arrival filter
			if (criteriaModel.IsNewArrival.HasValue && criteriaModel.IsNewArrival.Value)
			{
				var newArrivalDate = DateTime.UtcNow.AddDays(-3).Date;
				filter = filter.And(x => x.CreatedDateUtc.Date >= newArrivalDate);
			}

			// Get paginated data from repository
			var items = await _VwVendorItemRepository.GetPageAsync(
				criteriaModel.PageNumber,
				criteriaModel.PageSize,
				filter,
				orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
			);

			var itemsDto = _mapper.MapList<VwVendorItem, VendorItemDetailsDto>(items.Items);
			return new PagedResult<VendorItemDetailsDto>(itemsDto, items.TotalRecords);
		}
		//public async Task<PagedResult<VendorItemDetailsDto>> GetPageVendor(ItemstatusSearchCriteriaModel criteriaModel)
		//{
		//    if (criteriaModel == null)
		//        throw new ArgumentNullException(nameof(criteriaModel));

		//    if (criteriaModel.PageNumber < 1)
		//        throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

		//    if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//        throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

		//    // Base filter
		//    Expression<Func<VwVendorItem, bool>> filter;

		//    // Combine expressions manually
		//    var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();

		//    if (!string.IsNullOrWhiteSpace(searchTerm))
		//    {
		//        filter = filter.And(x =>
		//            (x. != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
		//            (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
		//            (x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm)) ||
		//            (x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm))
		//        );
		//    }

		//    //if (criteriaModel.CategoryIds?.Any() == true)
		//    //{
		//    //    filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
		//    //}

		//    // New Item Flags Filters
		//    if (criteriaModel.IsNewArrival.HasValue)
		//    {
		//        filter = filter.And(x => x.CreatedDateUtc.Date >= DateTime.UtcNow.AddDays(-3).Date);
		//    }

		//    // Get paginated data from repository
		//    var items = await _vendorItemRepository.GetPageAsync(
		//        criteriaModel.PageNumber,
		//        criteriaModel.PageSize,
		//        filter,
		//        orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
		//    );

		//    var itemsDto = _mapper.MapList<TbOffer, OfferDto>(items.Items);

		//    return new PagedResult<OfferDto>(itemsDto, items.TotalRecords);
		//}

		public async Task<IEnumerable<VendorItemDetailsDto>> FindByItemCombinationIdAsync(Guid itemCombinationId, CancellationToken token = default)
        {
            if (itemCombinationId == Guid.Empty)
                throw new ArgumentNullException(nameof(itemCombinationId));

            var vendorItems = await _vendorItemRepository.GetOffersByItemCombinationIdAsync(itemCombinationId, token);
            if (vendorItems == null)
                throw new KeyNotFoundException(ValidationResources.EntityNotFound);

            return _mapper.MapList<VwVendorItem, VendorItemDetailsDto>(vendorItems);
        }
        public new async Task<ItemDto> FindByIdAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));

            var items = await _vendorItemRepository.GetAsync(
                x => x.Id == Id,
                orderBy: i => i.OrderByDescending(x => x.CreatedDateUtc)
            );

            var item = items.FirstOrDefault();
            if (item == null)
                throw new KeyNotFoundException(ValidationResources.EntityNotFound);

            return _mapper.MapModel<TbOffer, ItemDto>(item);
        }
        public async Task<bool> SaveAsync(SaveVendorItemDto dto, string userId)
        {
            // Input validations
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(UserResources.UserNotFound, nameof(userId));
            if (dto.ItemId == Guid.Empty)
                throw new ValidationException("Item is required");
            if (dto.OfferCombinationPricings == null || !dto.OfferCombinationPricings.Any())
                throw new ValidationException("At least one offer combination pricing is required");
            if (dto.WarehouseId == Guid.Empty)
                throw new ValidationException("Warehouse is required");

            // Validate vendor
            var vendor = await _unitOfWork.TableRepository<TbVendor>()
                .FindAsync(v=>v.UserId == userId);
            if (vendor == null)
                throw new ValidationException("Vendor not found for this user");

            // Validate vendor warehouse
            var vendorAvailableWarehouses = await _vendorWarehouseService.GetVendorAvailableWarehousesByVendorIdAsync(vendor.Id);
            var selectedWarehouse = vendorAvailableWarehouses.FirstOrDefault(vw => vw.Id == dto.WarehouseId);
            
            if (selectedWarehouse == null )
                    throw new ValidationException("Selected warehouse is not available for this vendor");

            // Determine fulfillment type based on warehouse
            dto.FulfillmentType = selectedWarehouse.IsDefaultPlatformWarehouse
                ? FulfillmentType.Marketplace 
                : FulfillmentType.Vendor;

            // Fetch item to validate
            var item = await _unitOfWork.TableRepository<TbItem>().FindByIdAsync(dto.ItemId)
                ?? throw new ValidationException("Item not found");

            // Fetch category to determine pricing system
            var category = await _categoryRepository.FindByIdAsync(item.CategoryId)
                ?? throw new ValidationException("Category not found");

            var isMultiVendorMode = await _developmentSettingsService.IsMultiVendorModeEnabledAsync();

            bool isCombinationBasedPricing = category.PricingSystemType == PricingStrategyType.CombinationBased ||
                                             category.PricingSystemType == PricingStrategyType.Hybrid;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                
                bool result;
                
                // ===== SCENARIO 1: Multi-Vendor Mode with Combination-Based Pricing =====
                // Vendors can create their own combinations and offers
                if (isMultiVendorMode && isCombinationBasedPricing)
                {
                    result = await SaveMultiVendorCombinationBasedOfferAsync(dto, item,vendor.Id,Guid.Parse(userId));
                    if (!result)
                        throw new ValidationException("Failed to save vendor item for multi-vendor combination-based pricing");
                }
                // ===== SCENARIO 2: Multi-Vendor Mode with Simple Pricing =====
                // Vendors can only add offers to existing default combination
                else if (isMultiVendorMode && !isCombinationBasedPricing)
                {
                    result = await SaveMultiVendorSimpleOfferAsync(dto, item, vendor.Id, Guid.Parse(userId));
                    if (!result)
                        throw new ValidationException("Failed to save vendor item for multi-vendor simple pricing");
                }
                // ===== SCENARIO 3: Single-Vendor Mode with Combination-Based Pricing =====
                // Admin/Owner creates combinations and offers
                else if (!isMultiVendorMode && isCombinationBasedPricing)
                {
                    result = await SaveSingleVendorCombinationBasedOfferAsync(dto, item, vendor.Id,Guid.Parse(userId));
                    if (!result)
                        throw new ValidationException("Failed to save vendor item for single-vendor combination-based pricing");
                }
                // ===== SCENARIO 4: Single-Vendor Mode with Simple Pricing =====
                // Update existing default offer
                else
                {
                    result = await SaveSingleVendorSimpleOfferAsync(dto, item, Guid.Parse(userId));
                    if (!result)
                        throw new ValidationException("Failed to save vendor item for single-vendor simple pricing");
                }

                // Recalculate Buy Box winner for this item (especially important in multi-vendor mode)
                if (isMultiVendorMode)
                {
                    //await RecalculateBuyBoxWinnerAsync(dto.ItemId);
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.Error(ex, "Error saving vendor item {VendorItemId} for Item {ItemId}", dto.Id, dto.ItemId);
                throw;
            }
        }

        /// <summary>
        /// SCENARIO 1: Multi-Vendor + Combination-Based Pricing
        /// Vendors create their own combinations with attributes and offers
        /// </summary>
        private async Task<bool> SaveMultiVendorCombinationBasedOfferAsync(SaveVendorItemDto dto, TbItem item, Guid vendorId, Guid userId)
        {
            // Save or update the offer
            TbOffer offer;
            if (dto.Id == Guid.Empty)
            {
                // Create new offer
                offer = new TbOffer
                {
                    ItemId = dto.ItemId,
                    VendorId = vendorId,
                    WarehouseId = dto.WarehouseId,
                    FulfillmentType = dto.FulfillmentType,
                    IsFreeShipping = dto.IsFreeShipping,
                    EstimatedDeliveryDays = dto.EstimatedDeliveryDays,
                    VisibilityScope = OfferVisibilityScope.Pending,
                    WarrantyId = dto.WarrantyId
                };
            }
            else
            {
                // Update existing offer
                offer = await _unitOfWork.TableRepository<TbOffer>().FindByIdAsync(dto.Id)
                    ?? throw new ValidationException("Vendor item not found");

                // Ensure vendor can only update their own offers
                if (offer.VendorId != vendorId)
                    throw new ValidationException("You can only update your own offers");

                offer.WarehouseId = dto.WarehouseId;
                offer.FulfillmentType = dto.FulfillmentType;
                offer.IsFreeShipping = dto.IsFreeShipping;
                offer.EstimatedDeliveryDays = dto.EstimatedDeliveryDays;
                offer.VisibilityScope = OfferVisibilityScope.Pending;
                offer.WarrantyId = dto.WarrantyId;
            }

            var offerSaved = await _unitOfWork.TableRepository<TbOffer>().SaveAsync(offer, userId);
            if (!offerSaved.Success)
                return false;

            var offerId = offerSaved.Id;

            // Process each combination pricing
            foreach (var pricingDto in dto.OfferCombinationPricings ?? new List<OfferCombinationPricingDto>())
            {
                Guid combinationId;

                // Check if vendor is creating a new combination or using existing one
                if (pricingDto.ItemCombinationId == Guid.Empty)
                {
                    // Vendor is creating a NEW combination with specific attributes
                    if (pricingDto.ItemCombinationDtos == null || !pricingDto.ItemCombinationDtos.Any())
                        throw new ValidationException("Combination attributes are required when creating new combinations");
                    
                    var combinationDto = pricingDto.ItemCombinationDtos.First();
                    if (combinationDto.CombinationAttributes == null || !combinationDto.CombinationAttributes.Any())
                        throw new ValidationException("At least one combination attribute is required");
                    
                    var newCombination = new TbItemCombination
                    {
                        ItemId = dto.ItemId,
                        BasePrice = pricingDto.Price, // Use the pricing as base
                        IsDefault = false // Vendor combinations are never default
                    };

                    var combinationSaved = await _unitOfWork.TableRepository<TbItemCombination>()
                        .SaveAsync(newCombination, userId);

                    if (!combinationSaved.Success)
                        return false;

                    combinationId = combinationSaved.Id;

                    // Save combination attributes
                    var attributesSaved = await SaveCombinationAttributesAsync(
                        combinationId,
                        pricingDto.ItemCombinationDtos.First().CombinationAttributes,
                        userId);

                    if (!attributesSaved)
                        return false;

                    // Save combination images if provided
                    if (combinationDto.ItemCombinationImages != null && combinationDto.ItemCombinationImages.Any())
                    {
                        var imagesSaved = await SaveCombinationImagesAsync(
                            combinationId,
                            combinationDto.ItemCombinationImages,
                            userId);

                        if (!imagesSaved)
                            return false;
                    }
                }
                else
                {
                    // Vendor is using an EXISTING combination
                    combinationId = pricingDto.ItemCombinationId;

                    // Validate combination exists and belongs to this item
                    var existingCombination = await _unitOfWork.TableRepository<TbItemCombination>()
                        .FindByIdAsync(combinationId);

                    if (existingCombination == null || existingCombination.ItemId != dto.ItemId)
                        throw new ValidationException("Invalid combination for this item");
                }

                // Save or update the offer combination pricing
                var pricingSaved = await SaveOfferCombinationPricingAsync(
                    pricingDto,
                    offerId,
                    combinationId,
                    userId);

                if (!pricingSaved)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// SCENARIO 2: Multi-Vendor + Simple Pricing
        /// Vendors can only add offers to the default combination (no custom combinations allowed)
        /// </summary>
        private async Task<bool> SaveMultiVendorSimpleOfferAsync(SaveVendorItemDto dto, TbItem item, Guid vendorId, Guid userId)
        {
            // Get the default combination (should exist, created when item was saved)
            var defaultCombination = (await _unitOfWork.TableRepository<TbItemCombination>()
                .GetAsync(c => c.ItemId == dto.ItemId && c.IsDefault == true))
                .FirstOrDefault();

            if (defaultCombination == null)
                throw new ValidationException("Default combination not found. Item may not be properly configured.");

            // Ensure vendor is only trying to add ONE pricing (simple pricing = one combination)
            if (dto.OfferCombinationPricings?.Count > 1)
                throw new ValidationException("Only one offer is allowed for simple pricing items");

            var pricingDto = dto.OfferCombinationPricings?.First();

            // Validate that vendor is using the default combination
            if (pricingDto?.ItemCombinationId != Guid.Empty && pricingDto?.ItemCombinationId != defaultCombination.Id)
                throw new ValidationException("Only the default combination can be used in simple pricing mode");

            // Save or update the offer
            TbOffer offer;
            if (dto.Id == Guid.Empty)
            {
                offer = new TbOffer
                {
                    ItemId = dto.ItemId,
                    VendorId = vendorId,
                    WarehouseId = dto.WarehouseId,
                    FulfillmentType = dto.FulfillmentType,
                    IsFreeShipping = dto.IsFreeShipping,
                    EstimatedDeliveryDays = dto.EstimatedDeliveryDays,
                    VisibilityScope = OfferVisibilityScope.Pending,
                    WarrantyId = dto.WarrantyId
                };
            }
            else
            {
                offer = await _unitOfWork.TableRepository<TbOffer>().FindByIdAsync(dto.Id)
                    ?? throw new ValidationException("Vendor item not found");

                // Ensure vendor can only update their own offers
                if (offer.VendorId != vendorId)
                    throw new ValidationException("You can only update your own offers");

                offer.WarehouseId = dto.WarehouseId;
                offer.FulfillmentType = dto.FulfillmentType;
                offer.IsFreeShipping = dto.IsFreeShipping;
                offer.EstimatedDeliveryDays = dto.EstimatedDeliveryDays;
                offer.VisibilityScope = OfferVisibilityScope.Pending;
                offer.WarrantyId = dto.WarrantyId;
            }

            var offerSaved = await _unitOfWork.TableRepository<TbOffer>().SaveAsync(offer, userId);
            if (!offerSaved.Success)
                return false;

            // Save the pricing for the default combination
            var pricingSaved = await SaveOfferCombinationPricingAsync(
                pricingDto,
                offerSaved.Id,
                defaultCombination.Id,
                userId);

            return pricingSaved;
        }

        /// <summary>
        /// SCENARIO 3: Single-Vendor + Combination-Based Pricing
        /// Admin creates multiple combinations with attributes
        /// </summary>
        private async Task<bool> SaveSingleVendorCombinationBasedOfferAsync(SaveVendorItemDto dto, TbItem item, Guid vendorId, Guid userId)
        {
            // Similar to multi-vendor combination-based, but with admin privileges
            // Allow creating/updating combinations and offers
            return await SaveMultiVendorCombinationBasedOfferAsync(dto, item, vendorId, userId);
        }

        /// <summary>
        /// SCENARIO 4: Single-Vendor + Simple Pricing
        /// Update the default offer that was created with the item
        /// </summary>
        private async Task<bool> SaveSingleVendorSimpleOfferAsync(SaveVendorItemDto dto, TbItem item, Guid userId)
        {
            // Get the default combination
            var defaultCombination = (await _unitOfWork.TableRepository<TbItemCombination>()
                .GetAsync(c => c.ItemId == dto.ItemId && c.IsDefault == true))
                .FirstOrDefault();

            if (defaultCombination == null)
                throw new ValidationException("Default combination not found");

            // Get the existing default offer
            var existingOffer = (await _unitOfWork.TableRepository<TbOffer>()
                .GetAsync(o => o.ItemId == dto.ItemId))
                .FirstOrDefault();

            if (existingOffer == null)
                throw new ValidationException("Default offer not found. Item may not be properly configured.");

            // Update the offer
            existingOffer.WarehouseId = dto.WarehouseId;
            existingOffer.FulfillmentType = dto.FulfillmentType;
            existingOffer.IsFreeShipping = dto.IsFreeShipping;
            existingOffer.EstimatedDeliveryDays = dto.EstimatedDeliveryDays;
            existingOffer.VisibilityScope = OfferVisibilityScope.Pending;
            existingOffer.WarrantyId = dto.WarrantyId;

            var offerSaved = await _unitOfWork.TableRepository<TbOffer>().SaveAsync(existingOffer, userId);
            if (!offerSaved.Success)
                return false;

            // Update the pricing
            if (dto.OfferCombinationPricings != null && dto.OfferCombinationPricings.Any())
            {
                var pricingDto = dto.OfferCombinationPricings.First();
                var pricingSaved = await SaveOfferCombinationPricingAsync(
                    pricingDto,
                    existingOffer.Id,
                    defaultCombination.Id,
                    userId);

                return pricingSaved;
            }

            return true;
        }

        /// <summary>
        /// Helper method to save offer combination pricing
        /// </summary>
        private async Task<bool> SaveOfferCombinationPricingAsync(
            OfferCombinationPricingDto pricingDto,
            Guid offerId,
            Guid combinationId,
            Guid userId)
        {
            // Validate required fields
            if (pricingDto.OfferConditionId == Guid.Empty)
                throw new ValidationException("Offer condition is required");
            if (pricingDto.Price <= 0)
                throw new ValidationException("Price must be greater than zero");
            if (pricingDto.SalesPrice < 0)
                throw new ValidationException("Sales price cannot be negative");

            TbOfferCombinationPricing pricing;

            if (pricingDto.Id == Guid.Empty)
            {
                // Check if pricing already exists for this offer and combination
                var existingPricing = (await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                    .GetAsync(p => p.OfferId == offerId && p.ItemCombinationId == combinationId))
                    .FirstOrDefault();

                if (existingPricing != null)
                    throw new ValidationException("An offer already exists for this combination. Please update the existing offer instead.");

                // Create new pricing
                pricing = new TbOfferCombinationPricing
                {
                    OfferId = offerId,
                    ItemCombinationId = combinationId,
                    OfferConditionId = pricingDto.OfferConditionId,
                    Barcode = pricingDto.Barcode ?? "DEFAULT",
                    SKU = pricingDto.SKU ?? "DEFAULT",
                    Price = pricingDto.Price,
                    SalesPrice = pricingDto.SalesPrice,
                    AvailableQuantity = pricingDto.AvailableQuantity,
                    MinOrderQuantity = pricingDto.MinOrderQuantity,
                    MaxOrderQuantity = pricingDto.MaxOrderQuantity,
                    LowStockThreshold = pricingDto.LowStockThreshold,
                    StockStatus = pricingDto.AvailableQuantity > 0 ? StockStatus.InStock : StockStatus.OutOfStock,
                    IsBuyBoxWinner = false // Will be calculated later by buy box algorithm
                };
            }
            else
            {
                // Update existing pricing
                pricing = await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                    .FindByIdAsync(pricingDto.Id)
                    ?? throw new ValidationException("Offer combination pricing not found");

                pricing.OfferConditionId = pricingDto.OfferConditionId;
                pricing.Barcode = pricingDto.Barcode ?? pricing.Barcode;
                pricing.SKU = pricingDto.SKU ?? pricing.SKU;
                pricing.Price = pricingDto.Price;
                pricing.SalesPrice = pricingDto.SalesPrice;
                pricing.AvailableQuantity = pricingDto.AvailableQuantity;
                pricing.MinOrderQuantity = pricingDto.MinOrderQuantity;
                pricing.MaxOrderQuantity = pricingDto.MaxOrderQuantity;
                pricing.LowStockThreshold = pricingDto.LowStockThreshold;
                pricing.StockStatus = pricingDto.AvailableQuantity > 0 ? StockStatus.InStock : StockStatus.OutOfStock;
            }

            var result = await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                .SaveAsync(pricing, userId);

            return result.Success;
        }

        /// <summary>
        /// Helper method to save combination attributes
        /// </summary>
        private async Task<bool> SaveCombinationAttributesAsync(
            Guid combinationId,
            List<CombinationAttributeDto> attributes,
            Guid userId)
        {
            if (attributes == null || !attributes.Any())
                throw new ValidationException("Combination attributes are required");

            // Validate all attribute values exist
            foreach (var attr in attributes)
            {
                var attributeValue = await _unitOfWork.TableRepository<TbCombinationAttributesValue>()
                    .FindByIdAsync(attr.AttributeValueId);

                if (attributeValue == null)
                    throw new ValidationException($"Attribute value {attr.AttributeValueId} not found");
            }

            // Delete existing attributes if updating
            var existingAttributes = await _unitOfWork.TableRepository<TbCombinationAttribute>()
                .GetAsync(ca => ca.ItemCombinationId == combinationId);

            foreach (var attr in existingAttributes)
                await _unitOfWork.TableRepository<TbCombinationAttribute>().HardDeleteAsync(attr.Id);

            // Save new attributes
            var attributeEntities = attributes.Select(a => new TbCombinationAttribute
            {
                ItemCombinationId = combinationId,
                AttributeValueId = a.AttributeValueId
            }).ToList();

            var result = await _unitOfWork.TableRepository<TbCombinationAttribute>()
                .AddRangeAsync(attributeEntities, userId);

            return result;
        }

        // Helper functions
        /// <summary>
        /// Helper method to save combination images
        /// </summary>
        private async Task<bool> SaveCombinationImagesAsync(
            Guid combinationId,
            List<ItemCombinationImageDto> images,
            Guid userId)
        {
            if (images == null || !images.Any())
                return true;

            // Delete existing images if updating
            var existingImages = await _unitOfWork.TableRepository<TbItemCombinationImage>()
                .GetAsync(img => img.ItemCombinationId == combinationId);

            foreach (var img in existingImages)
            {
                // Delete the physical file if it exists
                if (!string.IsNullOrEmpty(img.Path))
                {
                    try
                    {
                        await _fileUploadService.DeleteFileAsync(img.Path);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning(ex, "Failed to delete image file: {Path}", img.Path);
                        // Continue even if file deletion fails
                    }
                }
                await _unitOfWork.TableRepository<TbItemCombinationImage>().HardDeleteAsync(img.Id);
            }

            // Process and save new images
            var imageEntities = new List<TbItemCombinationImage>();

            foreach (var imageDto in images.OrderBy(i => i.Order))
            {
                if (string.IsNullOrEmpty(imageDto.Path))
                    continue;

                string savedImagePath;

                // Check if the path is a new base64 image or existing path
                if (_fileUploadService.ValidateFile(imageDto.Path).isValid)
                {
                    // New image - process and save
                    savedImagePath = await SaveImageSync(imageDto.Path);
                }
                else
                {
                    // Existing image path - keep as is
                    savedImagePath = imageDto.Path;
                }

                imageEntities.Add(new TbItemCombinationImage
                {
                    ItemCombinationId = combinationId,
                    Path = savedImagePath,
                    Order = imageDto.Order
                });
            }

            if (imageEntities.Any())
            {
                var result = await _unitOfWork.TableRepository<TbItemCombinationImage>()
                    .AddRangeAsync(imageEntities, userId);
                return result;
            }

            return true;
        }

        /// <summary>
        ///  Helper method to save and process an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ApplicationException"></exception>
        private async Task<string> SaveImageSync(string image)
        {
            // Check if the file is null or empty
            if (string.IsNullOrEmpty(image))
            {
                throw new ValidationException(ValidationResources.ImageRequired);
            }

            // Validate the file
            var imageValidation = _fileUploadService.ValidateFile(image);
            if (!imageValidation.isValid)
            {
                throw new ValidationException(imageValidation.errorMessage);
            }

            try
            {
                // Convert the file to byte array
                var imageBytes = await _fileUploadService.GetFileBytesAsync(image);

                // Resize the image
                var resizedImage = _imageProcessingService.ResizeImagePreserveAspectRatio(imageBytes, 800, 600);

                // Convert the resized image to WebP format
                var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

                // Upload the WebP image to the specified location
                var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images");

                // Return the path of the uploaded image
                return imagePath;
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it
                _logger.Error(ex, ValidationResources.ErrorProcessingImage);
                throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
            }
        }
       
        /// <summary>
        /// Recalculates the Buy Box winner for all combinations of an item
        /// </summary>
        private async Task RecalculateBuyBoxWinnerAsync(Guid itemId)
        {
            // Get all combinations for this item
            var combinations = await _unitOfWork.TableRepository<TbItemCombination>()
                .GetAsync(c => c.ItemId == itemId);

            foreach (var combination in combinations)
            {
                // Get all offers for this combination
                var offerPricings = await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                    .GetAsync(
                        p => p.ItemCombinationId == combination.Id && p.StockStatus == StockStatus.InStock,
                        includeProperties: "Offer");

                // Reset all to non-winner
                foreach (var pricing in offerPricings)
                {
                    pricing.IsBuyBoxWinner = false;
                }

                if (offerPricings.Any())
                {
                    // Simple Buy Box algorithm: lowest price + fastest delivery wins
                    // You can make this more sophisticated based on your business rules
                    var winner = offerPricings
                        .OrderBy(p => p.Price)
                        .ThenBy(p => p.Offer?.EstimatedDeliveryDays ?? 999)
                        .FirstOrDefault();

                    if (winner != null)
                    {
                        winner.IsBuyBoxWinner = true;
                        await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                            .UpdateAsync(winner);
                    }
                }
            }
        }
    }
}