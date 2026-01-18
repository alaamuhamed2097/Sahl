using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Item;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.Vendor;
using BL.Contracts.Service.VendorItem;
using BL.Contracts.Service.VendorWarehouse;
using BL.Contracts.Service.Warehouse;
using BL.Extensions;
using BL.Services.Base;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Offer;
using Common.Enumerations.Pricing;
using Common.Enumerations.User;
using Common.Enumerations.Visibility;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using DAL.Services;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Offer;
using Domains.Views.Item;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;

namespace BL.Services.Catalog.Item;

public class ItemService : BaseService<TbItem, ItemDto>, IItemService
{
    private const int MaxImageCount = 10;
    private readonly ITableRepository<TbItem> _tableRepository;
    private readonly IRepository<VwItem> _repository;
    private readonly ITableRepository<TbCategory> _categoryRepository;
    private readonly IVendorWarehouseService _vendorWarehouseService;
    private readonly IVendorManagementService _vendorService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IVendorItemConditionService _vendorItemConditionService;
    private readonly IDevelopmentSettingsService _developmentSettingsService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public ItemService(IBaseMapper mapper,
        IUnitOfWork unitOfWork,
        ITableRepository<TbItem> tableRepository,
        IRepository<VwItem> repository,
        IFileUploadService fileUploadService,
        IImageProcessingService imageProcessingService,
        ILogger logger,
        ITableRepository<TbCategory> categoryRepository,
        IVendorManagementService vendorService,
        IVendorItemConditionService vendorItemConditionService,
        IDevelopmentSettingsService developmentSettingsService,
        IVendorWarehouseService vendorWarehouseService,
        ICurrentUserService currentUserService)
        : base(tableRepository, mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _tableRepository = tableRepository;
        _repository = repository;
        _fileUploadService = fileUploadService;
        _imageProcessingService = imageProcessingService;
        _logger = logger;
        _categoryRepository = categoryRepository;
        _vendorService = vendorService;
        _vendorItemConditionService = vendorItemConditionService;
        _developmentSettingsService = developmentSettingsService;
        _vendorWarehouseService = vendorWarehouseService;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<ItemDto>> GetPage(ItemSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        // Base filter
        Expression<Func<TbItem, bool>> filter = x => !x.IsDeleted;

        // Combine expressions manually
        var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filter = filter.And(x =>
                (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
                (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
                (x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm)) ||
                (x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm))
            );
        }

        if (criteriaModel.CategoryIds?.Any() == true)
        {
            filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
        }

        // New Item Flags Filters
        if (criteriaModel.IsNewArrival.HasValue)
        {
            filter = filter.And(x => x.CreatedDateUtc.Date >= DateTime.UtcNow.AddDays(-3).Date);
        }

        // Get paginated data from repository
        var items = await _tableRepository.GetPageAsync(
            criteriaModel.PageNumber,
            criteriaModel.PageSize,
            filter,
            orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
        );

        var itemsDto = _mapper.MapList<TbItem, ItemDto>(items.Items);

        return new PagedResult<ItemDto>(itemsDto, items.TotalRecords);
    }
    public async Task<PagedResult<ItemDto>> GetNewItemRequestsPage(ItemSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        // Base filter
        Expression<Func<TbItem, bool>> filter = x => !x.IsDeleted &&
                                                x.VisibilityScope == (int) ProductVisibilityStatus.PendingApproval;

        // Combine expressions manually
        var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filter = filter.And(x =>
                (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
                (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
                (x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm)) ||
                (x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm))
            );
        }

        if (criteriaModel.CategoryIds?.Any() == true)
        {
            filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
        }

        // New Item Flags Filters
        if (criteriaModel.IsNewArrival.HasValue)
        {
            filter = filter.And(x => x.CreatedDateUtc.Date >= DateTime.UtcNow.AddDays(-3).Date);
        }

        // Get paginated data from repository
        var items = await _tableRepository.GetPageAsync(
            criteriaModel.PageNumber,
            criteriaModel.PageSize,
            filter,
            orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
        );

        var itemsDto = _mapper.MapList<TbItem, ItemDto>(items.Items);

        return new PagedResult<ItemDto>(itemsDto, items.TotalRecords);
    }

    public new async Task<ItemDto> FindByIdAsync(Guid Id)
    {
        if (Id == Guid.Empty)
            throw new ArgumentNullException(nameof(Id));

        var items = await _repository.GetAsync(
            x => x.Id == Id,
            orderBy: i => i.OrderByDescending(x => x.CreatedDateUtc)
        );

        var item = items.FirstOrDefault();
        if (item == null)
            throw new KeyNotFoundException(ValidationResources.EntityNotFound);

        return _mapper.MapModel<VwItem, ItemDto>(item);
    }
    public new async Task<bool> SaveAsync(ItemDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        if (userId == Guid.Empty)
            throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

        // Initialize Images if null
        if (dto.Images == null)
            dto.Images = new List<ItemImageDto>();

        // Validate images: Either thumbnail OR at least one image is required
        if (string.IsNullOrEmpty(dto.ThumbnailImage) && !dto.Images.Any())
            throw new ValidationException("Either a thumbnail image or at least one product image is required.");

        // Only validate maximum image count for new items (updates can keep existing images)
        if (dto.Id == Guid.Empty && dto.Images.Count > MaxImageCount)
            throw new ArgumentException($"{ValidationResources.MaximumOf} {MaxImageCount} {ValidationResources.ImagesAllowed}", nameof(dto.Images));

        if (dto.CategoryId == Guid.Empty)
            throw new ValidationException("Category is required!");
        // Fetch category to validate pricing system
        var category = await _categoryRepository.FindByIdAsync(dto.CategoryId)
            ?? throw new ValidationException("Category not found!");

        var categoryAttributes = await _unitOfWork.TableRepository<TbCategoryAttribute>()
            .GetAsync(ca => ca.CategoryId == dto.CategoryId, includeProperties: "Attribute");

        // Check multi-vendor mode and pricing system
        var isMultiVendorMode = await _developmentSettingsService.IsMultiVendorModeEnabledAsync();
        var isCombinationBased = category.PricingSystemType == PricingStrategyType.CombinationBased ||
                                 category.PricingSystemType == PricingStrategyType.Hybrid;

        // === VALIDATION RULES ===

        // RULE 1: Multi-vendor + Combination-based
        // No validation needed - vendors create their own combinations and offers

        // RULE 2: Multi-vendor + Simple pricing
        // Validate BasePrice (default combination will be created, but no offer)
        if (isMultiVendorMode && !isCombinationBased)
        {
            if (dto.BasePrice == null || dto.BasePrice <= 0)
                throw new ValidationException("Base Price is required and must be greater than zero for multi-vendor mode with simple pricing!");
        }

        // RULE 3 & 4: Single-vendor mode
        // Validate defaultOfferData is provided
        if (!isMultiVendorMode)
        {
            if (dto.defaultOfferData == null)
                throw new ValidationException("Default offer data is required for single-vendor mode!");

            if (dto.defaultOfferData.EstimatedDeliveryDays <= 0)
                throw new ValidationException("Estimated delivery days must be greater than zero!");

            if (dto.defaultOfferData.OfferCombinationPricings == null ||
                !dto.defaultOfferData.OfferCombinationPricings.Any())
                throw new ValidationException("At least one offer pricing combination is required!");

            // Validate each combination pricing
            foreach (var pricing in dto.defaultOfferData.OfferCombinationPricings)
            {
                if (pricing.Price <= 0)
                    throw new ValidationException("Each combination must have a price greater than zero!");

                if (pricing.OfferConditionId == Guid.Empty)
                    throw new ValidationException("Each combination must have a valid condition!");

                if (pricing.AvailableQuantity < 0)
                    throw new ValidationException("Available quantity cannot be negative!");

                // For combination-based pricing, validate attributes
                if (isCombinationBased)
                {
                    var pricingAttributes = categoryAttributes.Where(a => a.AffectsPricing).ToList();

                    if (pricingAttributes.Any())
                    {
                        if (pricing.ItemCombinationDtos == null || !pricing.ItemCombinationDtos.Any())
                            throw new ValidationException("Combination attribute data is required for combination-based pricing!");

                        var combination = pricing.ItemCombinationDtos.First();

                        foreach (var attr in pricingAttributes)
                        {
                            var attrValue = combination.CombinationAttributes?
                                .FirstOrDefault(ca => ca.combinationAttributeValue.AttributeId == attr.AttributeId);

                            if (attrValue == null || string.IsNullOrWhiteSpace(attrValue.combinationAttributeValue.Value))
                                throw new ValidationException($"Required pricing attribute '{attr.Attribute?.TitleEn ?? "Unknown"}' must have a value!");
                        }
                    }
                }
            }
        }

        // Validate required non-pricing attributes
        var requiredAttributes = categoryAttributes.Where(ca => ca.IsRequired && !ca.AffectsPricing).ToList();
        foreach (var requiredAttr in requiredAttributes)
        {
            var itemAttr = dto.ItemAttributes?.FirstOrDefault(ia => ia.AttributeId == requiredAttr.AttributeId);
            if (itemAttr == null || string.IsNullOrWhiteSpace(itemAttr.Value))
                throw new ValidationException($"Required attribute '{requiredAttr.Attribute?.TitleEn ?? "Unknown"}' must have a value!");
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // Handle thumbnail image
            if (!string.IsNullOrEmpty(dto.ThumbnailImage) && _fileUploadService.ValidateFile(dto.ThumbnailImage).isValid)
                dto.ThumbnailImage = await SaveImageSync(dto.ThumbnailImage);

            // Process images
            var imageEntities = new List<TbItemImage>();
            if (dto.Images?.Any() == true)
            {
                foreach (var image in dto.Images)
                {
                    if (image.IsNew && !string.IsNullOrEmpty(image.Path) && _fileUploadService.ValidateFile(image.Path).isValid)
                    {
                        image.Path = await SaveImageSync(image.Path);
                        imageEntities.Add(_mapper.MapModel<ItemImageDto, TbItemImage>(image));
                    }
                }
            }

            // Update deletion handling
            if (dto.Id != Guid.Empty)
            {
                // Delete removed images
                var existingImages = await _unitOfWork.TableRepository<TbItemImage>()
                    .GetAsync(ii => ii.ItemId == dto.Id);
                var newImagePaths = dto.Images?.Select(i => i.Path);
                var imagesToRemove = existingImages.Where(i => !newImagePaths.Contains(i.Path));

                foreach (var img in imagesToRemove)
                    await _unitOfWork.TableRepository<TbItemImage>().HardDeleteAsync(img.Id);

                // Delete existing item attributes
                var existingAttributes = await _unitOfWork.TableRepository<TbItemAttribute>()
                    .GetAsync(a => a.ItemId == dto.Id);
                foreach (var attr in existingAttributes)
                    await _unitOfWork.TableRepository<TbItemAttribute>().HardDeleteAsync(attr.Id);
            }

            // Save main item entity
            var entity = _mapper.MapModel<ItemDto, TbItem>(dto);

            // Detach navigation properties to avoid EF Core tracking issues
            entity.Brand = null;
            entity.Category = null;
            entity.ItemAttributes = null;
            entity.ItemCombinations = null;
            entity.ItemImages = null;
            entity.Unit = null;

            // Get current user role
            var userRole = _currentUserService.GetCurrentUserRole();
            if(userRole == nameof(UserType.Vendor) && entity.CreatedBy != userId)
                throw new ValidationException("Vendors can only create or update their own items.");
            // If admin, set visibility to Visible, else PendingApproval
            if (userRole != null && userRole == nameof(UserType.Admin))
                entity.VisibilityScope = (int)ProductVisibilityStatus.Visible;
            else
                entity.VisibilityScope = (int)ProductVisibilityStatus.PendingApproval;

            // Save main item entity
            var itemSaved = await _unitOfWork.TableRepository<TbItem>().SaveAsync(entity, userId);
            // Validate save result
            if (!itemSaved.Success)
                throw new ValidationException("Failed to save item entity");

            var itemId = itemSaved.Id;

            // Save new images
            if (imageEntities.Any())
            {
                foreach (var img in imageEntities)
                    img.ItemId = itemId;
            }

            var imagesSaved = dto.Images?.Count(x => x.IsNew) > 0
                ? await _unitOfWork.TableRepository<TbItemImage>().AddRangeAsync(imageEntities, userId)
                : true;

            if (!imagesSaved)
                throw new ValidationException("Failed to save item images");

            // Save item attributes (non-pricing attributes)
            var attributesSaved = await SaveItemAttributesAsync(
                itemId,
                dto.ItemAttributes ?? new List<ItemAttributeDto>(),
                categoryAttributes.ToList(),
                userId);

            if (!attributesSaved)
                throw new ValidationException("Failed to save item attributes");

            // Handle combinations and offers based on create vs update
            if (dto.Id == Guid.Empty)
            {
                // ===== NEW ITEM =====

                // RULE 1: Multi-vendor mode with combination-based pricing
                // No default combination, no default offer (vendors create their own)
                if (isMultiVendorMode && isCombinationBased)
                {
                    // Skip - vendors will create their own combinations and offers
                }
                // RULE 2: Multi-vendor mode with simple pricing
                // Create default combination for vendors to use, but NO default offer
                else if (isMultiVendorMode && !isCombinationBased)
                {
                    var defaultItemCombination = new TbItemCombination()
                    {
                        ItemId = itemId,
                        BasePrice = dto.BasePrice ?? 0,
                        IsDefault = true
                    };

                    var combinationsSaved = await _unitOfWork.TableRepository<TbItemCombination>()
                        .SaveAsync(defaultItemCombination, userId);

                    if (!combinationsSaved.Success)
                        throw new ValidationException("Failed to save default item combination");

                    // No default offer created - vendors will add their own offers
                }
                // RULE 3 & 4: Single-vendor mode (both simple and combination-based)
                // Create combinations AND offers from defaultOfferData
                else if (!isMultiVendorMode)
                {
                    // Get default warehouse
                    var defaultWarehouse = await _vendorWarehouseService.GetMarketWarehousesAsync()
                        ?? throw new ValidationException("Market warehouse not found");

                    // Create the offer first
                    var defaultVendorItem = new TbOffer()
                    {
                        ItemId = itemId,
                        FulfillmentType = FulfillmentType.Marketplace,
                        IsFreeShipping = dto.defaultOfferData?.IsFreeShipping ?? false,
                        EstimatedDeliveryDays = dto.defaultOfferData?.EstimatedDeliveryDays,
                        WarehouseId = defaultWarehouse.Id,
                        VendorId = _vendorService.GetMarketStoreVendorId(),
                    };

                    var vendorItemSaved = await _unitOfWork.TableRepository<TbOffer>()
                        .SaveAsync(defaultVendorItem, userId);

                    if (!vendorItemSaved.Success)
                        throw new ValidationException("Failed to save default vendor item");

                    // Process each combination pricing
                    foreach (var pricingDto in dto.defaultOfferData?.OfferCombinationPricings ?? new List<OfferCombinationPricingDto>())
                    {
                        // Create the item combination
                        var itemCombination = new TbItemCombination()
                        {
                            ItemId = itemId,
                            BasePrice = pricingDto.Price,
                            IsDefault = false // Will be set later if needed
                        };

                        var combinationSaved = await _unitOfWork.TableRepository<TbItemCombination>()
                            .SaveAsync(itemCombination, userId);

                        if (!combinationSaved.Success)
                            throw new ValidationException("Failed to save item combination");

                        // Save combination attributes if provided
                        if (pricingDto.ItemCombinationDtos?.Any() == true)
                        {
                            var combinationDto = pricingDto.ItemCombinationDtos.First();
                            if (combinationDto.CombinationAttributes?.Any() == true)
                            {
                                foreach (var attrDto in combinationDto.CombinationAttributes)
                                {
                                    var combinationAttrValue = new TbCombinationAttributesValue()
                                    {
                                        AttributeId = attrDto.combinationAttributeValue.AttributeId,
                                        Value = attrDto.combinationAttributeValue.Value
                                    };

                                    var attrValueSaved = await _unitOfWork.TableRepository<TbCombinationAttributesValue>()
                                        .SaveAsync(combinationAttrValue, userId);

                                    if (!attrValueSaved.Success)
                                        throw new ValidationException("Failed to save combination attribute value");

                                    // Link to combination
                                    var combinationAttr = new TbCombinationAttribute()
                                    {
                                        ItemCombinationId = combinationSaved.Id,
                                        AttributeValueId = attrValueSaved.Id
                                    };

                                    await _unitOfWork.TableRepository<TbCombinationAttribute>()
                                        .SaveAsync(combinationAttr, userId);
                                }
                            }
                        }

                        // Create the offer combination pricing
                        var offerPricing = new TbOfferCombinationPricing()
                        {
                            OfferId = vendorItemSaved.Id,
                            ItemCombinationId = combinationSaved.Id,
                            Barcode = pricingDto.Barcode ?? $"BAR-{Guid.NewGuid().ToString().Substring(0, 12)}",
                            SKU = pricingDto.SKU ?? $"SKU-{Guid.NewGuid().ToString().Substring(0, 12)}",
                            AvailableQuantity = pricingDto.AvailableQuantity,
                            IsBuyBoxWinner = true,
                            Price = pricingDto.Price,
                            SalesPrice = pricingDto.SalesPrice > 0 ? pricingDto.SalesPrice : pricingDto.Price,
                            StockStatus = pricingDto.AvailableQuantity > 0 ? StockStatus.InStock : StockStatus.OutOfStock,
                            OfferConditionId = pricingDto.OfferConditionId,
                            MinOrderQuantity = pricingDto.MinOrderQuantity > 0 ? pricingDto.MinOrderQuantity  : 1,
                            MaxOrderQuantity = pricingDto.MaxOrderQuantity > 0 ? pricingDto.MaxOrderQuantity : 10,
                            LowStockThreshold = pricingDto.LowStockThreshold > 0 ? pricingDto.LowStockThreshold : 5
                        };

                        var pricingSaved = await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                            .SaveAsync(offerPricing, userId);

                        if (!pricingSaved.Success)
                            throw new ValidationException("Failed to save offer combination pricing");
                    }
                }
            }
            else
            {
                // ===== UPDATE EXISTING ITEM =====

                // Only update default combination for Simple pricing in multi-vendor mode
                if (isMultiVendorMode && !isCombinationBased)
                {
                    var existingCombination = (await _unitOfWork.TableRepository<TbItemCombination>()
                        .GetAsync(c => c.ItemId == itemId && c.IsDefault == true))
                        .FirstOrDefault();

                    if (existingCombination != null)
                    {
                        existingCombination.BasePrice = dto.BasePrice ?? existingCombination.BasePrice;

                        var combinationsSaved = await _unitOfWork.TableRepository<TbItemCombination>()
                            .SaveAsync(existingCombination, userId);

                        if (!combinationsSaved.Success)
                            throw new ValidationException("Failed to update item combination");
                    }
                }

                // Update offer combination pricing only in single-vendor mode
                if (!isMultiVendorMode && dto.defaultOfferData?.OfferCombinationPricings?.Any() == true)
                {
                    var existingCombinationPricing = (await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                        .GetAsync(
                            cp => cp.ItemCombination.ItemId == itemId,
                            includeProperties: "ItemCombination"
                        )).FirstOrDefault();

                    if (existingCombinationPricing != null)
                    {
                        var pricingDto = dto.defaultOfferData.OfferCombinationPricings.First();

                        // Update fields
                        existingCombinationPricing.Barcode = pricingDto.Barcode ?? existingCombinationPricing.Barcode;
                        existingCombinationPricing.SKU = pricingDto.SKU ?? existingCombinationPricing.SKU;

                        if (pricingDto.Price > 0)
                            existingCombinationPricing.Price = pricingDto.Price;

                        if ( pricingDto.SalesPrice > 0)
                            existingCombinationPricing.SalesPrice = pricingDto.SalesPrice;

                        if (pricingDto.AvailableQuantity >= 0)
                        {
                            existingCombinationPricing.AvailableQuantity = pricingDto.AvailableQuantity;
                            existingCombinationPricing.StockStatus = pricingDto.AvailableQuantity > 0
                                ? StockStatus.InStock
                                : StockStatus.OutOfStock;
                        }

                        if (pricingDto.OfferConditionId != Guid.Empty)
                            existingCombinationPricing.OfferConditionId = pricingDto.OfferConditionId;

                        var combinationPricingSaved = await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                            .SaveAsync(existingCombinationPricing, userId);

                        if (!combinationPricingSaved.Success)
                            throw new ValidationException("Failed to update combination pricing");
                    }
                }
            }

            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.Error(ex, "Error saving item {ItemId}", dto.Id);
            throw;
        }
    }

    public async Task<OperationResult> UpdateVisibilityScope(UpdateItemVisibilityRequest dto, Guid userId)
    {
        try
        {
            var item = await _tableRepository.FindByIdAsync(dto.ItemId);
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!Enum.IsDefined(typeof(ProductVisibilityStatus), dto.VisibilityScope))
                throw new Exception(string.Format(ValidationResources.InvalidFormat, nameof(dto.VisibilityScope)));

            var currentState = (ProductVisibilityStatus)item.VisibilityScope;
            var targetState = dto.VisibilityScope;

            // Short-circuit if nothing changes
            if (currentState == targetState)
                return new OperationResult { Success = true, Message = NotifiAndAlertsResources.ItemUpdated };

            item.VisibilityScope = (int)targetState;

            var updated = await _tableRepository.UpdateAsync(item, userId);

            // Now call central notification logic
            // await SendItemNotificationsAsync(item, targetState, currentState, item.CreatedBy);

            return updated.Success
                ? new OperationResult { Success = true, Message = NotifiAndAlertsResources.ItemUpdated }
                : new OperationResult { Message = NotifiAndAlertsResources.ItemUpdateFailed };
        }
        catch (Exception ex)
        {
            return new OperationResult
            {
                Success = false,
                Message = ValidationResources.Error,
                Errors = new List<string> { ex.Message }
            };
        }
    }
    // helper methods
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
    private async Task<bool> SaveItemAttributesAsync(
Guid itemId,
List<ItemAttributeDto> itemAttributes,
List<TbCategoryAttribute> categoryAttributes,
Guid userId)
    {
        if (itemAttributes == null || !itemAttributes.Any())
            return true; // nothing to save

        var attributeEntities = new List<TbItemAttribute>();

        foreach (var attr in itemAttributes)
        {
            var attribute = categoryAttributes.FirstOrDefault(ca => ca.AttributeId == attr.AttributeId)?.Attribute;

            // Only save attributes with values
            if (!string.IsNullOrWhiteSpace(attr.Value) && attribute != null)
            {
                var attributeEntity = _mapper.MapModel<ItemAttributeDto, TbItemAttribute>(attr);
                attributeEntity.ItemId = itemId;
                attributeEntity.FieldType = attribute.FieldType;
                attributeEntity.IsRangeFieldType = attribute.IsRangeFieldType;
                attributeEntity.MaxLength = attribute.MaxLength ?? 100;
                attributeEntity.TitleAr = attribute.TitleAr;
                attributeEntity.TitleEn = attribute.TitleEn;

                attributeEntities.Add(attributeEntity);
            }
        }

        if (!attributeEntities.Any())
            return true;

        return await _unitOfWork
            .TableRepository<TbItemAttribute>()
            .AddRangeAsync(attributeEntities, userId);
    }
}