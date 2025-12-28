using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Extensions;
using BL.Service.Base;
using Common.Enumerations.Pricing;
using Common.Enumerations.Visibility;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Offer;
using Domains.Views.Item;
using Domains.Views.Offer;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.ECommerce.Item;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Service.VendorItem
{
    public class VendorItemService : IVendorItemService
    {
        private const int MaxImageCount = 10;
        private readonly IOfferRepository _vendorItemRepository;
        private readonly ITableRepository<TbCategory> _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

        public VendorItemService(IBaseMapper mapper,
            IUnitOfWork unitOfWork,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            ILogger logger,
            ITableRepository<TbCategory> categoryRepository,
            IOfferRepository vendorItemRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _vendorItemRepository = vendorItemRepository;
        }

        public async Task<PagedResult<OfferDto>> GetPage(ItemSearchCriteriaModel criteriaModel)
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

            var itemsDto = _mapper.MapList<TbOffer, OfferDto>(items.Items);

            return new PagedResult<OfferDto>(itemsDto, items.TotalRecords);
        }

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
        public new async Task<bool> Save(ItemDto dto, Guid userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty)
                throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            // Fix: Check if Images is null or empty, but allow saving without image count validation for updates
            if (dto.Images == null)
                dto.Images = new List<ItemImageDto>();

            // Only validate image count for new items
            if (dto.Id == Guid.Empty && (!dto.Images.Any() || dto.Images.Count > MaxImageCount))
                throw new ArgumentException($"{ValidationResources.MaximumOf} {MaxImageCount} {ValidationResources.ImagesAllowed}", nameof(dto.Images));
            if (dto.CategoryId == Guid.Empty)
                throw new ValidationException("Category is required !! ");

            // Fetch category to validate pricing system
            var category = await _categoryRepository.FindByIdAsync(dto.CategoryId) ?? throw new ValidationException("Category not found!! ");

            // Validate BasePrice for Standard pricing system
            if (category.PricingSystemType == PricingStrategyType.Simple && (dto.BasePrice == null || dto.BasePrice <= 0))
                throw new ValidationException("Base Price is required!! ");

            var categoryAttributes = await _unitOfWork.TableRepository<TbCategoryAttribute>()
                                                      .GetAsync(ca => ca.CategoryId == dto.CategoryId, includeProperties: "Attribute");

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
                    var existingImages = await _unitOfWork.TableRepository<TbItemImage>().GetAsync(ii => ii.ItemId == dto.Id);
                    var newImagePaths = dto.Images.Select(i => i.Path);
                    var imagesToRemove = existingImages.Where(i => !newImagePaths.Contains(i.Path));

                    foreach (var img in imagesToRemove)
                        await _unitOfWork.TableRepository<TbItemImage>().HardDeleteAsync(img.Id);

                    // Delete existing item attributes
                    var existingAttributes = await _unitOfWork.TableRepository<TbItemAttribute>().GetAsync(a => a.ItemId == dto.Id);
                    foreach (var attr in existingAttributes)
                        await _unitOfWork.TableRepository<TbItemAttribute>().HardDeleteAsync(attr.Id);

                    // Delete old combinations (unchanged logic)
                    //if (category.PricingSystemType == PricingSystemType.CombinationWithQuantity ||
                    //    category.PricingSystemType == PricingSystemType.Combination)
                    //{
                    //    var oldCombinations = await _unitOfWork.TableRepository<TbItemCombination>()
                    //                                           .GetAsync(c => c.ItemId == dto.Id, includeProperties: "CombinationAttributes");

                    //    foreach (var combo in oldCombinations)
                    //    {
                    //        var comboAttrIds = combo.CombinationAttributes.Select(c => c.Id).ToList();
                    //        var comboValuesIds = (await _unitOfWork.TableRepository<TbCombinationAttributesValue>()
                    //                                .GetAsync(v => comboAttrIds.Contains(v.CombinationAttributeId)))
                    //                                .Select(v => v.Id);

                    //        var priceModifierIds = (await _unitOfWork.TableRepository<TbAttributeValuePriceModifier>()
                    //                                .GetAsync(v => comboValuesIds.Contains(v.CombinationAttributeValueId)))
                    //                                .Select(v => v.Id);

                    //        await _unitOfWork.TableRepository<TbAttributeValuePriceModifier>().BulkHardDeleteByIdsAsync(priceModifierIds);
                    //        await _unitOfWork.TableRepository<TbCombinationAttributesValue>().BulkHardDeleteByIdsAsync(comboValuesIds);
                    //        await _unitOfWork.TableRepository<TbCombinationAttribute>().BulkHardDeleteByIdsAsync(comboAttrIds);
                    //    }
                    //}
                }

                var entity = _mapper.MapModel<ItemDto, TbItem>(dto);
                // Detach navigation properties to avoid EF Core tracking issues
                entity.Brand = null;
                entity.Category = null;
                entity.ItemAttributes = null;
                entity.ItemCombinations = null;
                entity.ItemImages = null;
                entity.VisibilityScope = (int)ProductVisibilityStatus.PendingApproval;
                entity.ItemAttributes = null;
                entity.ItemCombinations = null;
                entity.ItemImages = null;
                entity.Unit = null;
                var itemSaved = await _unitOfWork.TableRepository<TbItem>().SaveAsync(entity, userId);
                var itemId = itemSaved.Id;

                // Save new images
                if (imageEntities.Any()) foreach (var img in imageEntities) img.ItemId = itemId;

                var imagesSaved = dto.Images?.Count(x => x.IsNew) > 0
                    ? await _unitOfWork.TableRepository<TbItemImage>().AddRangeAsync(imageEntities, userId)
                    : true;

                // Save item attributes
                var attributesSaved = await SaveItemAttributesAsync(itemId, dto.ItemAttributes ?? new List<ItemAttributeDto>(), categoryAttributes.ToList(), userId);

                // Save item combinations
                //var combinationsSaved = await ProcessItemCombinationsAsync(itemId, dto.ItemCombinations ?? new List<ItemCombinationDto>(), category, categoryAttributes.ToList(), userId);

                await _unitOfWork.CommitAsync();
                return itemSaved.Success && imagesSaved && attributesSaved;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
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

        //private async Task<bool> ProcessItemCombinationsAsync(Guid itemId, List<ItemCombinationDto> itemCombinations, TbCategory category, List<TbCategoryAttribute> categoryAttributes, Guid userId)
        //{
        //    if (itemCombinations == null || !itemCombinations.Any())
        //        return true;

        //    var comboEntitiesToCreate = new List<TbItemCombination>();
        //    var comboEntitiesToUpdate = new List<TbItemCombination>();
        //    var comboEntityPerDto = new List<TbItemCombination>();

        //    // Handle pricing system types
        //    var pricingType = category.PricingSystemType;

        //    // For standard-like pricing systems
        //    if (pricingType == PricingSystemType.Standard || pricingType == PricingSystemType.Quantity || pricingType == PricingSystemType.CustomerSegmentPricing)
        //    {
        //        var singleDto = itemCombinations.FirstOrDefault();
        //        if (singleDto == null)
        //        {
        //            singleDto = new ItemCombinationDto { ItemId = itemId, Barcode = "111111", SKU = "DEFAULT", IsDefault = true };
        //            itemCombinations = new List<ItemCombinationDto> { singleDto };
        //        }

        //        if (singleDto.Id != Guid.Empty)
        //        {
        //            var comboToUpdate = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(singleDto);
        //            comboToUpdate.ItemId = itemId;
        //            comboEntitiesToUpdate.Add(comboToUpdate);
        //            comboEntityPerDto.Add(comboToUpdate);
        //        }
        //        else
        //        {
        //            var comboToCreate = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(singleDto);
        //            comboToCreate.ItemId = itemId;
        //            comboEntitiesToCreate.Add(comboToCreate);
        //            comboEntityPerDto.Add(comboToCreate);
        //        }

        //        if (comboEntitiesToCreate.Any())
        //            await _unitOfWork.TableRepository<TbItemCombination>().AddRangeAsync(comboEntitiesToCreate, userId);

        //        if (comboEntitiesToUpdate.Any())
        //            await _unitOfWork.TableRepository<TbItemCombination>().UpdateRangeAsync(comboEntitiesToUpdate, userId);
        //    }
        //    else
        //    {
        //        // Combination-based systems
        //        for (int i = 0; i < itemCombinations.Count; i++)
        //        {
        //            var comboDto = itemCombinations[i];
        //            var comboEntityModel = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(comboDto);
        //            comboEntityModel.ItemId = itemId;

        //            if (comboDto.Id != Guid.Empty)
        //            {
        //                comboEntityModel.Id = comboDto.Id;
        //                comboEntitiesToUpdate.Add(comboEntityModel);
        //                comboEntityPerDto.Add(comboEntityModel);
        //            }
        //            else
        //            {
        //                comboEntitiesToCreate.Add(comboEntityModel);
        //                comboEntityPerDto.Add(comboEntityModel);
        //            }
        //        }

        //        if (comboEntitiesToCreate.Any())
        //            await _unitOfWork.TableRepository<TbItemCombination>().AddRangeAsync(comboEntitiesToCreate, userId);

        //        if (comboEntitiesToUpdate.Any())
        //            await _unitOfWork.TableRepository<TbItemCombination>().UpdateRangeAsync(comboEntitiesToUpdate, userId);

        //        // Create combination attributes/values/modifiers
        //        var combinationAttributeEntities = new List<TbCombinationAttribute>();
        //        var createdAttrEntitiesPerCombo = new List<List<TbCombinationAttribute>>();

        //        for (int comboIndex = 0; comboIndex < itemCombinations.Count; comboIndex++)
        //        {
        //            var comboDto = itemCombinations[comboIndex];
        //            var comboEntity = comboEntityPerDto[comboIndex];
        //            var createdForCombo = new List<TbCombinationAttribute>();

        //            if (comboDto.CombinationAttributes?.Any() == true)
        //            {
        //                foreach (var attrDto in comboDto.CombinationAttributes)
        //                {
        //                    var hasPricingValues = attrDto.combinationAttributeValueDtos?.Any(v => categoryAttributes.Any(ca => ca.AttributeId == v.AttributeId && ca.AffectsPricing)) == true;
        //                    if (!hasPricingValues)
        //                        continue;

        //                    var attrEntity = new TbCombinationAttribute { ItemCombinationId = comboEntity.Id };
        //                    combinationAttributeEntities.Add(attrEntity);
        //                    createdForCombo.Add(attrEntity);
        //                }
        //            }

        //            createdAttrEntitiesPerCombo.Add(createdForCombo);
        //        }

        //        if (combinationAttributeEntities.Any())
        //            await _unitOfWork.TableRepository<TbCombinationAttribute>().AddRangeAsync(combinationAttributeEntities, userId);

        //        var combinationAttributeValueEntities = new List<TbCombinationAttributesValue>();
        //        var createdValuesPerAttrPerCombo = new List<List<List<TbCombinationAttributesValue>>>();

        //        for (int comboIndex = 0; comboIndex < itemCombinations.Count; comboIndex++)
        //        {
        //            var comboDto = itemCombinations[comboIndex];
        //            var attrDtos = comboDto.CombinationAttributes;
        //            var createdAttrs = createdAttrEntitiesPerCombo.ElementAtOrDefault(comboIndex) ?? new List<TbCombinationAttribute>();
        //            var valuesPerAttr = new List<List<TbCombinationAttributesValue>>();

        //            if (attrDtos?.Any() == true)
        //            {
        //                for (int attrIndex = 0; attrIndex < attrDtos.Count; attrIndex++)
        //                {
        //                    var attrDto = attrDtos[attrIndex];
        //                    var createdAttrEntity = createdAttrs.ElementAtOrDefault(attrIndex);
        //                    var createdValuesForAttr = new List<TbCombinationAttributesValue>();

        //                    if (attrDto?.combinationAttributeValueDtos?.Any() == true && createdAttrEntity != null)
        //                    {
        //                        foreach (var valDto in attrDto.combinationAttributeValueDtos)
        //                        {
        //                            if (!categoryAttributes.Any(ca => ca.AttributeId == valDto.AttributeId && ca.AffectsPricing))
        //                                continue;

        //                            var valEntity = new TbCombinationAttributesValue
        //                            {
        //                                CombinationAttributeId = createdAttrEntity.Id,
        //                                AttributeId = valDto.AttributeId,
        //                                Value = valDto.Value
        //                            };
        //                            combinationAttributeValueEntities.Add(valEntity);
        //                            createdValuesForAttr.Add(valEntity);
        //                        }
        //                    }

        //                    valuesPerAttr.Add(createdValuesForAttr);
        //                }
        //            }

        //            createdValuesPerAttrPerCombo.Add(valuesPerAttr);
        //        }

        //        if (combinationAttributeValueEntities.Any())
        //            await _unitOfWork.TableRepository<TbCombinationAttributesValue>().AddRangeAsync(combinationAttributeValueEntities, userId);

        //        var attributeValuePriceModifierEntities = new List<TbAttributeValuePriceModifier>();

        //        for (int comboIndex = 0; comboIndex < itemCombinations.Count; comboIndex++)
        //        {
        //            var comboDto = itemCombinations[comboIndex];
        //            var attrDtos = comboDto.CombinationAttributes;
        //            var valuesPerAttr = createdValuesPerAttrPerCombo.ElementAtOrDefault(comboIndex) ?? new List<List<TbCombinationAttributesValue>>();

        //            if (attrDtos?.Any() == true)
        //            {
        //                for (int attrIndex = 0; attrIndex < attrDtos.Count; attrIndex++)
        //                {
        //                    var attrDto = attrDtos[attrIndex];
        //                    var createdValuesForAttr = valuesPerAttr.ElementAtOrDefault(attrIndex) ?? new List<TbCombinationAttributesValue>();

        //                    if (attrDto?.combinationAttributeValueDtos?.Any() == true)
        //                    {
        //                        for (int valIndex = 0; valIndex < attrDto.combinationAttributeValueDtos.Count; valIndex++)
        //                        {
        //                            var valDto = attrDto.combinationAttributeValueDtos[valIndex];
        //                            var createdValEntity = createdValuesForAttr.ElementAtOrDefault(valIndex);

        //                            if (createdValEntity != null && valDto.AttributeValuePriceModifiers?.Any() == true)
        //                            {
        //                                foreach (var modDto in valDto.AttributeValuePriceModifiers)
        //                                {
        //                                    var modEntity = new TbAttributeValuePriceModifier
        //                                    {
        //                                        CombinationAttributeValueId = createdValEntity.Id,
        //                                        AttributeId = valDto.AttributeId,
        //                                        ModifierType = modDto.ModifierType,
        //                                        PriceModifierCategory = modDto.PriceModifierCategory,
        //                                        ModifierValue = modDto.ModifierValue,
        //                                        DisplayOrder = modDto.DisplayOrder
        //                                    };
        //                                    attributeValuePriceModifierEntities.Add(modEntity);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (attributeValuePriceModifierEntities.Any())
        //            await _unitOfWork.TableRepository<TbAttributeValuePriceModifier>().AddRangeAsync(attributeValuePriceModifierEntities, userId);
        //    }

        //    return true;
        //}

    }
}