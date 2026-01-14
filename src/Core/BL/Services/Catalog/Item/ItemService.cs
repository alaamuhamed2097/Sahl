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
using Common.Enumerations.Visibility;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
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
using Shared.GeneralModels.SearchCriteriaModels;
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
        IVendorWarehouseService vendorWarehouseService)
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

        // Check if Images is null or empty, but allow saving without image count validation for updates
        if (dto.Images == null)
            dto.Images = new List<ItemImageDto>();

        // Only validate image count for new items
        if (dto.Id == Guid.Empty && (!dto.Images.Any() || dto.Images.Count > MaxImageCount))
            throw new ArgumentException($"{ValidationResources.MaximumOf} {MaxImageCount} {ValidationResources.ImagesAllowed}", nameof(dto.Images));

        if (dto.CategoryId == Guid.Empty)
            throw new ValidationException("Category is required !! ");

        // Fetch category to validate pricing system
        var category = await _categoryRepository.FindByIdAsync(dto.CategoryId)
            ?? throw new ValidationException("Category not found!! ");

        // Validate BasePrice for Standard pricing system
        if (category.PricingSystemType == PricingStrategyType.Simple && (dto.BasePrice == null || dto.BasePrice <= 0))
            throw new ValidationException("Base Price is required!! ");

        var categoryAttributes = await _unitOfWork.TableRepository<TbCategoryAttribute>()
            .GetAsync(ca => ca.CategoryId == dto.CategoryId, includeProperties: "Attribute");

        var isMultiVendorMode = await _developmentSettingsService.IsMultiVendorModeEnabledAsync();

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
                var newImagePaths = dto.Images.Select(i => i.Path);
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
            entity.VisibilityScope = (int)ProductVisibilityStatus.PendingApproval;

            var itemSaved = await _unitOfWork.TableRepository<TbItem>().SaveAsync(entity, userId);

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

            // Save item attributes
            var attributesSaved = await SaveItemAttributesAsync(
                itemId,
                dto.ItemAttributes ?? new List<ItemAttributeDto>(),
                categoryAttributes.ToList(),
                userId);

            if (!attributesSaved)
                throw new ValidationException("Failed to save item attributes");

            // Determine if we should create default combination and offer based on business rules
            bool isNonCombinationPricing = category.PricingSystemType != PricingStrategyType.CombinationBased &&
                                           category.PricingSystemType != PricingStrategyType.Hybrid;

            // Handle combinations and offers based on create vs update
            if (dto.Id == Guid.Empty)
            {
                // ===== NEW ITEM =====

                // RULE 1: Multi-vendor mode with combination-based pricing
                // No default combination, no default offer (vendors create their own combinations and offers)
                if (isMultiVendorMode && !isNonCombinationPricing)
                {
                    // Skip creating default combination and offer
                    // Vendors will create their own combinations and offers
                }
                // RULE 2: Multi-vendor mode with simple pricing
                // Create default combination for vendors to use, but NO default offer
                else if (isMultiVendorMode && isNonCombinationPricing)
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
                        throw new ValidationException("Failed to save item combinations");

                    // No default offer created - vendors will add their own offers on this combination
                    // Barcode and SKU will be set by vendors when they create their offers
                }
                // RULE 3: Single-vendor mode with simple pricing
                // Create both default combination AND default offer
                else if (!isMultiVendorMode && isNonCombinationPricing)
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
                        throw new ValidationException("Failed to save item combinations");

                    // Create default offer
                    var defaultWarehouse = await _vendorWarehouseService.GetMarketWarehousesAsync()
                        ?? throw new ValidationException("Market warehouse not found");

                    var defaultVendorItem = new TbOffer()
                    {
                        ItemId = itemId,
                        FulfillmentType = FulfillmentType.Marketplace,
                        IsFreeShipping = true,
                        WarehouseId = defaultWarehouse.Id,
                        VendorId = _vendorService.GetMarketStoreVendorId(),
                    };

                    var vendorItemSaved = await _unitOfWork.TableRepository<TbOffer>()
                        .SaveAsync(defaultVendorItem, userId);

                    if (!vendorItemSaved.Success)
                        throw new ValidationException("Failed to save default vendor item");

                    // Get or create condition
                    var newConditions = await _vendorItemConditionService.GetNewConditions();
                    Guid defaultConditionId = Guid.Empty;

                    if (newConditions?.Any() == true)
                    {
                        defaultConditionId = newConditions.First().Id;
                    }
                    else
                    {
                        var conditionSaved = await _vendorItemConditionService.SaveAsync(
                            new OfferConditionDto()
                            {
                                IsNew = true,
                                NameAr = "جديد",
                                NameEn = "New"
                            },
                            userId);

                        if (conditionSaved?.Success == true)
                            defaultConditionId = conditionSaved.Id;
                    }

                    if (defaultConditionId == Guid.Empty)
                        throw new ValidationException("Failed to get or create default condition");

                    // Create pricing with Barcode and SKU (now in TbOfferCombinationPricing)
                    var defaultVendorItemCombinationPricing = new TbOfferCombinationPricing()
                    {
                        OfferId = vendorItemSaved.Id,
                        ItemCombinationId = combinationsSaved.Id,
                        Barcode = dto.Barcode ?? $"BAR-{Guid.NewGuid().ToString()}",
                        SKU = dto.SKU ?? $"SKU-{Guid.NewGuid().ToString()}",
                        AvailableQuantity = 0,
                        IsBuyBoxWinner = true,
                        Price = dto.BasePrice ?? 0,
                        SalesPrice = dto.BasePrice ?? 0,
                        StockStatus = StockStatus.OutOfStock,
                        OfferConditionId = defaultConditionId
                    };

                    var vendorItemCombinationPricingSaved = await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                        .SaveAsync(defaultVendorItemCombinationPricing, userId);

                    if (!vendorItemCombinationPricingSaved.Success)
                        throw new ValidationException("Failed to save vendor item combination pricing");
                }
                // RULE 4: Single-vendor mode with combination-based pricing
                // No default combination, no default offer (admin will create combinations manually)
                else
                {
                    // Skip - combinations and offers will be created separately
                }
            }
            else
            {
                // ===== UPDATE EXISTING ITEM =====

                // Only update default combination for Simple pricing system
                if (isNonCombinationPricing)
                {
                    var existingCombination = (await _unitOfWork.TableRepository<TbItemCombination>()
                        .GetAsync(c => c.ItemId == itemId && c.IsDefault == true))
                        .FirstOrDefault();

                    if (existingCombination != null)
                    {
                        // Update existing combination (BasePrice only, Barcode/SKU are in pricing now)
                        existingCombination.BasePrice = dto.BasePrice ?? existingCombination.BasePrice;

                        var combinationsSaved = await _unitOfWork.TableRepository<TbItemCombination>()
                            .SaveAsync(existingCombination, userId);

                        if (!combinationsSaved.Success)
                            throw new ValidationException("Failed to update item combination");
                    }
                }

                // Update offer combination pricing only in single-vendor mode (if properties are provided)
                if (!isMultiVendorMode && isNonCombinationPricing)
                {
                    var existingCombinationPricing = (await _unitOfWork.TableRepository<TbOfferCombinationPricing>()
                        .GetAsync(
                            cp => cp.ItemCombination.ItemId == itemId,
                            includeProperties: "ItemCombination"
                        )).FirstOrDefault();

                    if (existingCombinationPricing != null)
                    {
                        // Update Barcode and SKU (now in TbOfferCombinationPricing)
                        existingCombinationPricing.Barcode = dto.Barcode ?? existingCombinationPricing.Barcode;
                        existingCombinationPricing.SKU = dto.SKU ?? existingCombinationPricing.SKU;

                        // Only update price/quantity if values are provided in DTO
                        if (dto.BasePrice.HasValue)
                            existingCombinationPricing.Price = dto.BasePrice.Value;

                        if (dto.BaseSalesPrice.HasValue)
                            existingCombinationPricing.SalesPrice = dto.BaseSalesPrice.Value;

                        if (dto.Quantity.HasValue)
                        {
                            existingCombinationPricing.AvailableQuantity = dto.Quantity.Value;
                            existingCombinationPricing.StockStatus = dto.Quantity.Value > 0
                                ? StockStatus.InStock
                                : StockStatus.OutOfStock;
                        }

                        if (dto.OfferConditionId.HasValue && dto.OfferConditionId.Value != Guid.Empty)
                            existingCombinationPricing.OfferConditionId = dto.OfferConditionId.Value;

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

    // Helper functions
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