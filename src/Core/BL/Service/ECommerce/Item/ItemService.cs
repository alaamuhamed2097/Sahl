using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.Location;
using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Item;
using BL.Extensions;
using BL.Service.Base;
using Common.Enumerations.Pricing;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Views.Item;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Service.ECommerce.Item
{
    public class ItemService : BaseService<TbItem, ItemDto>, IItemService
    {
        private const int MaxImageCount = 10;
        private readonly ITableRepository<TbItem> _tableRepository;
        private readonly IRepository<VwItem> _repository;
        private readonly ITableRepository<TbCategory> _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly ILocationBasedCurrencyService _locationBasedCurrencyService;
        private readonly ILogger _logger;

        public ItemService(IBaseMapper mapper,
            IUnitOfWork unitOfWork,
            ITableRepository<TbItem> tableRepository,
            IRepository<VwItem> repository,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            ILocationBasedCurrencyService locationBasedCurrencyService,
            ILogger logger,
            ITableRepository<TbCategory> categoryRepository)
            : base(tableRepository, mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tableRepository = tableRepository;
            _repository = repository;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _locationBasedCurrencyService = locationBasedCurrencyService;
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<PaginatedDataModel<VwItemDto>> GetPage(ItemSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter
            Expression<Func<VwItem, bool>> filter = x => true;

            // Combine expressions manually
            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
                    (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
                    (x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm)) ||
                    (x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm)) ||
                    (x.CategoryTitleAr != null && x.CategoryTitleAr.ToLower().Contains(searchTerm)) ||
                    (x.CategoryTitleEn != null && x.CategoryTitleEn.ToLower().Contains(searchTerm))
                );
            }

            if (criteriaModel.CategoryIds?.Any() == true)
            {
                filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
            }

            // New Item Flags Filters
            if (criteriaModel.IsNewArrival.HasValue)
            {
                filter = filter.And(x => x.IsNewArrival == criteriaModel.IsNewArrival.Value);
            }

            // Get paginated data from repository
            var items = await _repository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
            );

            var itemsDto = _mapper.MapList<VwItem, VwItemDto>(items.Items);

            return new PaginatedDataModel<VwItemDto>(itemsDto, items.TotalRecords);
        }

        public new async Task<VwItemDto> FindByIdAsync(Guid Id)
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

            return _mapper.MapModel<VwItem, VwItemDto>(item);
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


            // pre-Load related entities
            var category = await _categoryRepository.FindByIdAsync(dto.CategoryId);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Handle thumbnail image
                if (!string.IsNullOrEmpty(dto.ThumbnailImage) && _fileUploadService.ValidateFile(dto.ThumbnailImage).isValid)
                    dto.ThumbnailImage = await SaveImageSync(dto.ThumbnailImage);

                var imageEntities = new List<TbItemImage>();
                if (dto.Images?.Any() == true)
                {
                    foreach (var image in dto.Images)
                    {
                        if (image.IsNew)
                        {
                            if (!string.IsNullOrEmpty(image.Path) && _fileUploadService.ValidateFile(image.Path).isValid)
                            {
                                image.Path = await SaveImageSync(image.Path);
                                var imageEntity = _mapper.MapModel<ItemImageDto, TbItemImage>(image);
                                imageEntities.Add(imageEntity);
                            }
                        }
                    }
                }

                // If this is an update (item has an ID), fetch existing item to get old image paths
                if (dto.Id != Guid.Empty)
                {
                    // Get existing images for this item
                    var existingImages = await _unitOfWork.TableRepository<TbItemImage>().GetAsync(ii => ii.ItemId == dto.Id);

                    var newImagesPaths = dto.Images?.Select(i => i.Path) ?? Enumerable.Empty<string>();

                    var imagesToDelete = existingImages.Where(i => !newImagesPaths.Contains(i.Path));

                    if (imagesToDelete.Any())
                    {
                        foreach (var image in imagesToDelete)
                        {
                            await _unitOfWork
                            .TableRepository<TbItemImage>()
                            .HardDeleteAsync(image.Id);
                        }
                    }

                    // Delete existing attributes
                    var existingAttributes = await _unitOfWork
                        .TableRepository<TbItemAttribute>()
                        .GetAsync(ia => ia.ItemId == dto.Id);

                    if (existingAttributes.Any())
                    {
                        foreach (var attr in existingAttributes)
                        {
                            await _unitOfWork
                                .TableRepository<TbItemAttribute>()
                                .HardDeleteAsync(attr.Id);
                        }
                    }
                    if (category.PricingSystemType == PricingSystemType.CombinationWithQuantity || category.PricingSystemType == PricingSystemType.Combination)
                    {
                        // Delete existing combinations attributes
                        var existingCombinations = await _unitOfWork
                            .TableRepository<TbItemCombination>()
                            .GetAsync(c => c.ItemId == dto.Id, includeProperties: "CombinationAttributes");

                        if (existingCombinations.Any())
                        {
                            foreach (var combo in existingCombinations)
                            {
                                var combinationAttributesIds = combo.CombinationAttributes.Select(c => c.Id).ToList() ?? new List<Guid>();
                                var CombinationAttributeValuesIds = (await _unitOfWork.TableRepository<TbCombinationAttributesValue>().GetAsync(c => combinationAttributesIds.Contains(c.CombinationAttributeId))).Select(v => v.Id);
                                var AttributeValuesPriceModifierIds = (await _unitOfWork.TableRepository<TbAttributeValuePriceModifier>().GetAsync(c => CombinationAttributeValuesIds.Contains(c.CombinationAttributeValueId))).Select(v => v.Id);
                                await _unitOfWork.TableRepository<TbAttributeValuePriceModifier>().BulkHardDeleteByIdsAsync(AttributeValuesPriceModifierIds);
                                await _unitOfWork.TableRepository<TbCombinationAttributesValue>().BulkHardDeleteByIdsAsync(CombinationAttributeValuesIds);
                                await _unitOfWork.TableRepository<TbCombinationAttribute>().BulkHardDeleteByIdsAsync(combinationAttributesIds);
                            }
                        }
                    }
                }

                var entity = _mapper.MapModel<ItemDto, TbItem>(dto);
                entity.Brand = null;
                entity.Category = null;

                var itemSaved = await _unitOfWork.TableRepository<TbItem>().SaveAsync(entity, userId);

                var itemId = itemSaved.Id;
                if (imageEntities.Any())
                {
                    foreach (var imageEntity in imageEntities)
                    {
                        imageEntity.ItemId = itemId;
                    }
                }

                var imagesSaved = true;
                if (dto.Images?.Count(x => x.IsNew) > 0)
                    imagesSaved = await _unitOfWork.TableRepository<TbItemImage>().AddRangeAsync(imageEntities, userId);

                // Save ItemAttributes
                var attributesSaved = true;
                if (dto.ItemAttributes?.Any() == true)
                {
                    var attributeEntities = new List<TbItemAttribute>();
                    foreach (var attr in dto.ItemAttributes)
                    {
                        // Only save attributes with values
                        if (!string.IsNullOrWhiteSpace(attr.Value))
                        {
                            var attributeEntity = _mapper.MapModel<ItemAttributeDto, TbItemAttribute>(attr);
                            attributeEntity.ItemId = itemId;
                            attributeEntities.Add(attributeEntity);
                        }
                    }

                    if (attributeEntities.Any())
                    {
                        attributesSaved = await _unitOfWork.TableRepository<TbItemAttribute>().AddRangeAsync(attributeEntities, userId);
                    }
                }

                // Ensure every item has at least one combination (default combination)
                if (dto.ItemCombinations == null || !dto.ItemCombinations.Any())
                {
                    // Create a default combination with no attributes
                    dto.ItemCombinations = new List<ItemCombinationDto>
                    {
                        new ItemCombinationDto
                        {
                            ItemId = itemId ,
                            Barcode = "111111",
                            SKU = "DEFAULT",
                            IsDefault = true
                        }
                    };
                }

                // Ensure only one combination is marked as default
                var defaultCombinations = dto.ItemCombinations.Where(c => c.IsDefault).ToList();
                if (defaultCombinations.Count == 0)
                {
                    // No default set, mark the first one as default
                    dto.ItemCombinations.First().IsDefault = true;
                }
                else if (defaultCombinations.Count > 1)
                {
                    // Multiple defaults found, keep only the first one
                    foreach (var combo in defaultCombinations.Skip(1))
                    {
                        combo.IsDefault = false;
                    }
                }
                // Save ItemAttributeCombinationPricings and nested combination attributes/values/modifiers
                var combinationsSaved = true;
                if (dto.ItemCombinations?.Any() == true)
                {
                    // Load category attributes to determine which affect pricing
                    var categoryAttributes = (await _unitOfWork.TableRepository<TbCategoryAttribute>().GetAsync(ca => ca.CategoryId == dto.CategoryId)).ToList();

                    var comboEntitiesToCreate = new List<TbItemCombination>();
                    var comboEntitiesToUpdate = new List<TbItemCombination>();
                    // Maintain entity objects in DTO order so we can link nested attributes later
                    var comboEntityPerDto = new List<TbItemCombination>();

                    // If editing existing item, fetch existing combos (we will not delete them)
                    var existingCombinationsDb = new List<TbItemCombination>();
                    if (dto.Id != Guid.Empty)
                    {
                        existingCombinationsDb = (await _unitOfWork.TableRepository<TbItemCombination>().GetAsync(c => c.ItemId == dto.Id)).ToList();
                    }

                    // Handle pricing system types
                    var pricingType = category.PricingSystemType;

                    // For standard-like pricing systems, ensure we only save/update a single combination and no combination attributes
                    if (pricingType == PricingSystemType.Standard || pricingType == PricingSystemType.Quantity || pricingType == PricingSystemType.CustomerSegmentPricing)
                    {
                        // Determine single DTO to use
                        var singleDto = dto.ItemCombinations.FirstOrDefault();
                        if (singleDto == null)
                        {
                            singleDto = new ItemCombinationDto { ItemId = itemId, Barcode = "111111", SKU = "DEFAULT", IsDefault = true };
                            dto.ItemCombinations = new List<ItemCombinationDto> { singleDto };
                        }

                        // If creating new item, create one combination
                        if (dto.Id == Guid.Empty)
                        {
                            var comboToCreate = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(singleDto);
                            comboToCreate.ItemId = itemId;
                            comboEntitiesToCreate.Add(comboToCreate);
                            comboEntityPerDto.Add(comboToCreate);
                        }
                        else
                        {
                            // Editing: try to match DTO id to existing, otherwise update first existing, otherwise create
                            if (singleDto.Id != Guid.Empty)
                            {
                                var comboToUpdate = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(singleDto);
                                comboToUpdate.ItemId = itemId;
                                comboEntitiesToUpdate.Add(comboToUpdate);
                                comboEntityPerDto.Add(comboToUpdate);
                            }
                            else if (existingCombinationsDb.Any())
                            {
                                var firstExisting = existingCombinationsDb.First();
                                var comboToUpdateExisting = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(singleDto);
                                comboToUpdateExisting.Id = firstExisting.Id;
                                comboToUpdateExisting.ItemId = itemId;
                                comboEntitiesToUpdate.Add(comboToUpdateExisting);
                                comboEntityPerDto.Add(comboToUpdateExisting);
                            }
                            else
                            {
                                var comboToCreate2 = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(singleDto);
                                comboToCreate2.ItemId = itemId;
                                comboEntitiesToCreate.Add(comboToCreate2);
                                comboEntityPerDto.Add(comboToCreate2);
                            }
                        }

                        // Persist creates/updates
                        if (comboEntitiesToCreate.Any())
                            combinationsSaved &= await _unitOfWork.TableRepository<TbItemCombination>().AddRangeAsync(comboEntitiesToCreate, userId);

                        if (comboEntitiesToUpdate.Any())
                            combinationsSaved &= await _unitOfWork.TableRepository<TbItemCombination>().UpdateRangeAsync(comboEntitiesToUpdate, userId);
                    }
                    else
                    {
                        // Combination-based systems: create/update combinations and then create combination attributes only for attributes that affect pricing
                        for (int i = 0; i < dto.ItemCombinations.Count; i++)
                        {
                            var comboDto = dto.ItemCombinations[i];
                            var comboEntityModel = _mapper.MapModel<ItemCombinationDto, TbItemCombination>(comboDto);
                            comboEntityModel.ItemId = itemId;

                            if (dto.Id != Guid.Empty && comboDto.Id != Guid.Empty)
                            {
                                // Update existing combination
                                comboEntityModel.Id = comboDto.Id;
                                comboEntitiesToUpdate.Add(comboEntityModel);
                                comboEntityPerDto.Add(comboEntityModel);
                            }
                            else
                            {
                                // Create new combination
                                comboEntitiesToCreate.Add(comboEntityModel);
                                comboEntityPerDto.Add(comboEntityModel);
                            }
                        }

                        if (comboEntitiesToCreate.Any())
                            combinationsSaved &= await _unitOfWork.TableRepository<TbItemCombination>().AddRangeAsync(comboEntitiesToCreate, userId);

                        if (comboEntitiesToUpdate.Any())
                            combinationsSaved &= await _unitOfWork.TableRepository<TbItemCombination>().UpdateRangeAsync(comboEntitiesToUpdate, userId);

                        // Now create combination attributes/values/modifiers, but only for attributes that affect pricing
                        if (combinationsSaved)
                        {
                            var combinationAttributeEntities = new List<TbCombinationAttribute>();
                            var createdAttrEntitiesPerCombo = new List<List<TbCombinationAttribute>>();

                            for (int comboIndex = 0; comboIndex < dto.ItemCombinations.Count; comboIndex++)
                            {
                                var comboDto = dto.ItemCombinations[comboIndex];
                                var comboEntity = comboEntityPerDto[comboIndex];
                                var createdForCombo = new List<TbCombinationAttribute>();

                                if (comboDto.CombinationAttributes?.Any() == true)
                                {
                                    foreach (var attrDto in comboDto.CombinationAttributes)
                                    {
                                        // Determine if any of the values of this attribute affect pricing (based on category attributes)
                                        var hasPricingValues = attrDto.combinationAttributeValueDtos?.Any(v => categoryAttributes.Any(ca => ca.AttributeId == v.AttributeId && ca.AffectsPricing)) == true;
                                        if (!hasPricingValues)
                                            continue; // skip creating combination attribute for non-pricing attribute

                                        var attrEntity = new TbCombinationAttribute { ItemCombinationId = comboEntity.Id };
                                        combinationAttributeEntities.Add(attrEntity);
                                        createdForCombo.Add(attrEntity);
                                    }
                                }

                                createdAttrEntitiesPerCombo.Add(createdForCombo);
                            }

                            var combinationAttributesSaved = true;
                            if (combinationAttributeEntities.Any())
                            {
                                combinationAttributesSaved = await _unitOfWork.TableRepository<TbCombinationAttribute>().AddRangeAsync(combinationAttributeEntities, userId);
                                combinationsSaved &= combinationAttributesSaved;
                            }

                            // Prepare values
                            var combinationAttributeValueEntities = new List<TbCombinationAttributesValue>();
                            var createdValuesPerAttrPerCombo = new List<List<List<TbCombinationAttributesValue>>>();

                            for (int comboIndex = 0; comboIndex < dto.ItemCombinations.Count; comboIndex++)
                            {
                                var comboDto = dto.ItemCombinations[comboIndex];
                                var attrDtos = comboDto.CombinationAttributes;
                                var createdAttrs = createdAttrEntitiesPerCombo.ElementAtOrDefault(comboIndex) ?? new List<TbCombinationAttribute>();
                                var valuesPerAttr = new List<List<TbCombinationAttributesValue>>();

                                if (attrDtos?.Any() == true)
                                {
                                    for (int attrIndex = 0; attrIndex < attrDtos.Count; attrIndex++)
                                    {
                                        var attrDto = attrDtos[attrIndex];
                                        var createdAttrEntity = createdAttrs.ElementAtOrDefault(attrIndex);
                                        var createdValuesForAttr = new List<TbCombinationAttributesValue>();

                                        if (attrDto?.combinationAttributeValueDtos?.Any() == true && createdAttrEntity != null)
                                        {
                                            foreach (var valDto in attrDto.combinationAttributeValueDtos)
                                            {
                                                // Only create values that affect pricing according to category
                                                if (!categoryAttributes.Any(ca => ca.AttributeId == valDto.AttributeId && ca.AffectsPricing))
                                                    continue;

                                                var valEntity = new TbCombinationAttributesValue
                                                {
                                                    CombinationAttributeId = createdAttrEntity.Id,
                                                    AttributeId = valDto.AttributeId,
                                                    Value = valDto.Value
                                                };
                                                combinationAttributeValueEntities.Add(valEntity);
                                                createdValuesForAttr.Add(valEntity);
                                            }
                                        }

                                        valuesPerAttr.Add(createdValuesForAttr);
                                    }
                                }

                                createdValuesPerAttrPerCombo.Add(valuesPerAttr);
                            }

                            var combinationAttributeValuesSaved = true;
                            if (combinationAttributeValueEntities.Any())
                            {
                                combinationAttributeValuesSaved = await _unitOfWork.TableRepository<TbCombinationAttributesValue>().AddRangeAsync(combinationAttributeValueEntities, userId);
                                combinationsSaved &= combinationAttributeValuesSaved;
                            }

                            // Prepare attribute value price modifiers
                            var attributeValuePriceModifierEntities = new List<TbAttributeValuePriceModifier>();

                            for (int comboIndex = 0; comboIndex < dto.ItemCombinations.Count; comboIndex++)
                            {
                                var comboDto = dto.ItemCombinations[comboIndex];
                                var attrDtos = comboDto.CombinationAttributes;
                                var valuesPerAttr = createdValuesPerAttrPerCombo.ElementAtOrDefault(comboIndex) ?? new List<List<TbCombinationAttributesValue>>();

                                if (attrDtos?.Any() == true)
                                {
                                    for (int attrIndex = 0; attrIndex < attrDtos.Count; attrIndex++)
                                    {
                                        var attrDto = attrDtos[attrIndex];
                                        var createdValuesForAttr = valuesPerAttr.ElementAtOrDefault(attrIndex) ?? new List<TbCombinationAttributesValue>();

                                        if (attrDto?.combinationAttributeValueDtos?.Any() == true)
                                        {
                                            for (int valIndex = 0; valIndex < attrDto.combinationAttributeValueDtos.Count; valIndex++)
                                            {
                                                var valDto = attrDto.combinationAttributeValueDtos[valIndex];
                                                var createdValEntity = createdValuesForAttr.ElementAtOrDefault(valIndex);

                                                if (createdValEntity != null && valDto.AttributeValuePriceModifiers?.Any() == true)
                                                {
                                                    foreach (var modDto in valDto.AttributeValuePriceModifiers)
                                                    {
                                                        var modEntity = new TbAttributeValuePriceModifier
                                                        {
                                                            CombinationAttributeValueId = createdValEntity.Id,
                                                            AttributeId = modDto.AttributeId,
                                                            ModifierType = modDto.ModifierType,
                                                            PriceModifierCategory = modDto.PriceModifierCategory,
                                                            ModifierValue = modDto.ModifierValue,
                                                            DisplayOrder = modDto.DisplayOrder
                                                        };
                                                        attributeValuePriceModifierEntities.Add(modEntity);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (attributeValuePriceModifierEntities.Any())
                            {
                                var modifiersSaved = await _unitOfWork.TableRepository<TbAttributeValuePriceModifier>().AddRangeAsync(attributeValuePriceModifierEntities, userId);
                                combinationsSaved &= modifiersSaved;
                            }
                        }
                    }
                }

                await _unitOfWork.CommitAsync();

                return itemSaved.Success && imagesSaved && attributesSaved && combinationsSaved;
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
    }
}
