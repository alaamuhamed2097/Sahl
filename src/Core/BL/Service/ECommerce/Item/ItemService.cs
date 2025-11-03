using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.Location;
using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Item;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domins.Entities.Item;
using Domins.Views.Item;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Services.Items
{
    public class ItemService : BaseService<TbItem, ItemDto>, IItemService
    {
        private const int MaxImageCount = 10;
        private readonly ITableRepository<TbItem> _tableRepository;
        private readonly IRepository<VwItem> _repository;
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
            ILogger logger)
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
        }

        public PaginatedDataModel<VwItemDto> GetPage(ItemSearchCriteriaModel criteriaModel)
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

            if (criteriaModel.UnitIds?.Any() == true)
            {
                filter = filter.And(x => criteriaModel.UnitIds.Contains(x.UnitId));
            }

            if (criteriaModel.IsInStock.HasValue)
            {
                filter = filter.And(x => x.StockStatus == criteriaModel.IsInStock.Value);
            }

            if (criteriaModel.PriceFrom.HasValue)
            {
                filter = filter.And(x => x.Price >= criteriaModel.PriceFrom.Value);
            }

            if (criteriaModel.PriceTo.HasValue)
            {
                filter = filter.And(x => x.Price <= criteriaModel.PriceTo.Value);
            }

            if (criteriaModel.QuantityFrom.HasValue)
            {
                filter = filter.And(x => x.Quantity >= criteriaModel.QuantityFrom.Value);
            }

            if (criteriaModel.QuantityTo.HasValue)
            {
                filter = filter.And(x => x.Quantity <= criteriaModel.QuantityTo.Value);
            }

            // New Item Flags Filters
            if (criteriaModel.IsNewArrival.HasValue)
            {
                filter = filter.And(x => x.IsNewArrival == criteriaModel.IsNewArrival.Value);
            }

            if (criteriaModel.IsBestSeller.HasValue)
            {
                filter = filter.And(x => x.IsBestSeller == criteriaModel.IsBestSeller.Value);
            }

            if (criteriaModel.IsRecommended.HasValue)
            {
                filter = filter.And(x => x.IsRecommended == criteriaModel.IsRecommended.Value);
            }

            // Get paginated data from repository
            var items = _repository.GetPage(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
            );

            var itemsDto = _mapper.MapList<VwItem, VwItemDto>(items.Items);

            return new PaginatedDataModel<VwItemDto>(itemsDto, items.TotalRecords);
        }

        public override ItemDto FindById(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));
            var item = _tableRepository.Get(x => x.Id == Id, includeProperties: "ItemImages,Category").FirstOrDefault();
            if (item == null)
                throw new KeyNotFoundException(ValidationResources.EntityNotFound);
            return _mapper.MapModel<TbItem, ItemDto>(item);
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
            {
                throw new ArgumentException($"{ValidationResources.MaximumOf} {MaxImageCount} {ValidationResources.ImagesAllowed}", nameof(dto.Images));
            }

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
                    var existingImages = _unitOfWork
                        .TableRepository<TbItemImage>()
                        .Get(ii => ii.ItemId == dto.Id);

                    var newImagesPaths = dto.Images?.Select(i => i.Path) ?? Enumerable.Empty<string>();

                    var imagesToDelete = existingImages.Where(i => !newImagesPaths.Contains(i.Path));

                    if (imagesToDelete.Any())
                    {
                        foreach (var image in imagesToDelete)
                        {
                            _unitOfWork
                            .TableRepository<TbItemImage>()
                            .HardDelete(image.Id);
                        }
                    }
                }

                var entity = _mapper.MapModel<ItemDto, TbItem>(dto);
                entity.Brand = null;
                entity.Category = null;

                var itemSaved = _unitOfWork.TableRepository<TbItem>().Save(entity, userId, out Guid itemId);

                if (imageEntities.Any())
                {
                    foreach (var imageEntity in imageEntities)
                    {
                        imageEntity.ItemId = itemId;
                    }
                }

                var imagesSaved = true;
                if (dto.Images?.Count(x => x.IsNew) > 0)
                    imagesSaved = _unitOfWork.TableRepository<TbItemImage>().AddRange(imageEntities, userId);

                await _unitOfWork.CommitAsync();

                return itemSaved && imagesSaved;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.Error(ex, "Error saving item {ItemId}", dto.Id);
                throw;
            }
        }

        // Currency conversion methods
        public async Task<ItemDto?> GetByIdWithCurrencyConversionAsync(Guid id, string clientIp, bool applyConversion = true)
        {
            try
            {
                var item = FindById(id);
                if (item == null) return null;

                var (targetCurrency, baseCurrency) = await _locationBasedCurrencyService.GetCurrencyInfoAsync(applyConversion ? clientIp : "0");
                return await _locationBasedCurrencyService.ApplyCurrencyConversionAsync(item, baseCurrency?.Code, targetCurrency?.Code);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting item by ID with currency conversion. ItemId: {ItemId}, ClientIp: {ClientIp}, ApplyConversion: {ApplyConversion}", id, clientIp, applyConversion);
                throw;
            }
        }

        public async Task<IEnumerable<ItemDto>> GetAllWithCurrencyConversionAsync(string clientIp, bool applyConversion = true)
        {
            try
            {
                var items = GetAll();
                if (items?.Any() != true) return items ?? Enumerable.Empty<ItemDto>();

                var (targetCurrency, baseCurrency) = await _locationBasedCurrencyService.GetCurrencyInfoAsync(applyConversion ? clientIp : "0");
                return await _locationBasedCurrencyService.ApplyCurrencyConversionAsync(items, baseCurrency?.Code, targetCurrency?.Code);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all items with currency conversion. ClientIp: {ClientIp}, ApplyConversion: {ApplyConversion}", clientIp, applyConversion);
                throw;
            }
        }

        public async Task<PaginatedDataModel<VwItemDto>> GetPageWithCurrencyConversionAsync(ItemSearchCriteriaModel criteriaModel, string clientIp, bool applyConversion = true)
        {
            try
            {
                var result = GetPage(criteriaModel);
                if (result?.Items?.Any() != true) return result;

                var (targetCurrency, baseCurrency) = await _locationBasedCurrencyService.GetCurrencyInfoAsync(applyConversion ? clientIp : "0");
                var convertedItems = await _locationBasedCurrencyService.ApplyCurrencyConversionAsync(result.Items, baseCurrency?.Code, targetCurrency?.Code);

                return new PaginatedDataModel<VwItemDto>(convertedItems, result.TotalRecords);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting paginated items with currency conversion. ClientIp: {ClientIp}, ApplyConversion: {ApplyConversion}", clientIp, applyConversion);
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
