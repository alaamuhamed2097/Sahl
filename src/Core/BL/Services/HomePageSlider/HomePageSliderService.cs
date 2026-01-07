using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.HomePageSlider;
using BL.Extensions;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Merchandising.HomePage;
using Resources;
using Serilog;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Services.HomeSlider
{
	public class HomePageSliderService : BaseService<TbHomePageSlider, HomePageSliderDto>, IHomePageSliderService
    {
        private readonly ITableRepository<TbHomePageSlider> _baseRepository;
        private readonly IBaseMapper _mapper;
		private readonly ILogger _logger;

		public HomePageSliderService(ITableRepository<TbHomePageSlider> baseRepository, IBaseMapper mapper, ILogger logger)
			: base(baseRepository, mapper)
		{
			_baseRepository = baseRepository;
			_mapper = mapper;
			_logger = logger;
		}

		/// <summary>
		/// Get all active sliders within current date range
		/// </summary>
		public async Task<IEnumerable<HomePageSliderDto>> GetAllSliders()
        {
            var sliders = await _baseRepository
                .GetAsync(x => !x.IsDeleted,
                orderBy: x => x.OrderBy(q => q.DisplayOrder));

            var dtos = _mapper.MapList<TbHomePageSlider, HomePageSliderDto>(sliders);

            return dtos;
        }
		public async Task<PagedResult<HomePageSliderDto>> GetPage(
	BaseSearchCriteriaModel criteriaModel,
	CancellationToken cancellationToken = default)
		{
			try
			{
				if (criteriaModel == null)
					throw new ArgumentNullException(nameof(criteriaModel));

				if (criteriaModel.PageNumber < 1)
					throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

				if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
					throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

				// Build filter expression
				Expression<Func<TbHomePageSlider, bool>> filter = x => !x.IsDeleted;

				// Search term filter
				var searchTerm = criteriaModel.SearchTerm?.Trim();
				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					filter = filter.And(x =>
						(x.TitleAr != null && x.TitleAr.Contains(searchTerm)) ||
						(x.TitleEn != null && x.TitleEn.Contains(searchTerm))
					);
				}

				// Build ordering expression
				Func<IQueryable<TbHomePageSlider>, IOrderedQueryable<TbHomePageSlider>> orderByExpression = null;

				if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
				{
					var isDesc = criteriaModel.SortDirection?.ToLowerInvariant() == "desc";

					switch (criteriaModel.SortBy.ToLowerInvariant())
					{
						case "titlear":
							orderByExpression = isDesc
								? q => q.OrderByDescending(x => x.TitleAr)
								: q => q.OrderBy(x => x.TitleAr);
							break;
						case "titleen":
							orderByExpression = isDesc
								? q => q.OrderByDescending(x => x.TitleEn)
								: q => q.OrderBy(x => x.TitleEn);
							break;
						case "createddateutc":
						default:
							orderByExpression = isDesc
								? q => q.OrderByDescending(x => x.CreatedDateUtc)
								: q => q.OrderBy(x => x.CreatedDateUtc);
							break;
					}
				}
				else
				{
					orderByExpression = q => q.OrderByDescending(x => x.CreatedDateUtc);
				}

				var entitiesList = await _baseRepository.GetPageAsync(
					criteriaModel.PageNumber,
					criteriaModel.PageSize,
					filter,
					orderBy: orderByExpression,
					cancellationToken: cancellationToken
				);

				var dtoList = _mapper.MapList<TbHomePageSlider, HomePageSliderDto>(entitiesList.Items);

				return new PagedResult<HomePageSliderDto>(dtoList, entitiesList.TotalRecords);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetPage)}");
				throw;
			}
		}
		//public async Task<PagedResult<HomePageSliderDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
		//{
		//	if (criteriaModel == null)
		//		throw new ArgumentNullException(nameof(criteriaModel));

		//	if (criteriaModel.PageNumber < 1)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

		//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

		//	// Base filter for active entities
		//	Expression<Func<TbHomePageSlider, bool>> filter = x => !x.IsDeleted;

		//	// Apply search term if provided
		//	if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
		//	{
		//		string searchTerm = criteriaModel.SearchTerm.Trim();
		//		filter = x => !x.IsDeleted &&
		//					  (x.TitleAr != null && EF.Functions.Like(x.TitleAr, $"%{searchTerm}%") ||
		//					  x.TitleEn != null && EF.Functions.Like(x.TitleEn, $"%{searchTerm}%"));
		//	}

		//	var entitiesList = await _baseRepository.GetPageAsync(
		//		criteriaModel.PageNumber,
		//		criteriaModel.PageSize,
		//		filter
		//		, orderBy: x => x.OrderByDescending(b => b.CreatedDateUtc));

		//	var dtoList = _mapper.MapList<TbMainBanner, MainBannerDto>(entitiesList.Items);

		//	return new PaginatedDataModel<MainBannerDto>(dtoList, entitiesList.TotalRecords);
		//}


		//public async Task<bool> Save(HomePageSliderDto dto, Guid userId)
		//{
		//	// Validate inputs
		//	if (dto == null) throw new ArgumentNullException(nameof(dto));
		//	if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

		//	// Get all existing MainBanners to determine max display order
		//	var allMainBanners = _baseRepository.Get(c => c.CurrentState == 1).ToList();
		//	var maxDisplayOrder = allMainBanners.Any() ? allMainBanners.Max(c => c.DisplayOrder) : 0;
		//	int? oldDisplayOrder = null;
		//	if (_fileUploadService.ValidateFile(dto.ImageUrl).isValid)
		//	{
		//		string? oldImage = null;
		//		if (dto.Id != Guid.Empty)
		//		{
		//			oldImage = allMainBanners.Find(b => b.Id == dto.Id).ImageUrl;
		//		}
		//		dto.ImageUrl = await SaveImageSync(dto.ImageUrl);
		//		if (oldImage != null)
		//			// Delete the old image file
		//			DeleteImageFile(oldImage);
		//	}
		//	// Handle existing MainBanner case - get old display order BEFORE saving
		//	if (dto.Id != Guid.Empty)
		//	{
		//		var existingMainBanner = allMainBanners.FirstOrDefault(c => c.Id == dto.Id);
		//		if (existingMainBanner != null)
		//		{
		//			oldDisplayOrder = existingMainBanner.DisplayOrder;
		//		}

		//		// Update other entities' display orders BEFORE saving the target entity
		//		if (oldDisplayOrder.HasValue && oldDisplayOrder.Value != dto.DisplayOrder)
		//		{
		//			await UpdateOtherEntitiesDisplayOrderAsync(allMainBanners.Where(c => c.Id != dto.Id).ToList(),
		//													   dto.DisplayOrder,
		//													   oldDisplayOrder.Value,
		//													   userId,
		//													   maxDisplayOrder);
		//		}
		//	}
		//	else
		//	{
		//		// Handle new MainBanner case
		//		if (dto.DisplayOrder <= 0)
		//		{
		//			dto.DisplayOrder = maxDisplayOrder + 1;
		//		}
		//		else if (dto.DisplayOrder > maxDisplayOrder + 1)
		//		{
		//			dto.DisplayOrder = maxDisplayOrder + 1;
		//		}
		//		else
		//		{
		//			await ShiftDisplayOrderForInsertAsync(allMainBanners.Where(c => c.DisplayOrder >= dto.DisplayOrder).ToList(), userId);
		//		}
		//	}

		//	// Map DTO to entity (this will have the correct display order already)
		//	var entity = _mapper.MapModel<MainBannerDto, TbMainBanner>(dto);

		//	// Save the entity (with correct display order)
		//	var saveResult = _baseRepository.Save(entity, userId, out Guid MainBannerId);

		//	return saveResult;
		//}
		
		
		//public async Task<bool> Delete(Guid id, Guid userId)
		//{
		//	try
		//	{
		//		// Get the entity to be deleted
		//		var entity = _baseRepository.GetAsync(f => f.Id == id);
		//		if (entity == null)
		//			return false;

		//		// Store the display order before deletion
		//		int deletedDisplayOrder = entity.;

		//		// Set display order to 0 and update
		//		var dto = _mapper.MapModel<TbHomePageSlider, HomePageSliderDto>(entity);
		//		dto.DisplayOrder = 0;

		//		var updateSuccess = await UpdateAsync(dto, userId);
		//		if (!updateSuccess.Success)
		//			return false;

		//		// Delete the entity
		//		var deleteSuccess = await base.DeleteAsync(id);
		//		if (!deleteSuccess)
		//			return false;

		//		// Shift display orders of remaining entities
		//		if (deletedDisplayOrder > 0)
		//		{
		//			ShiftDisplayOrderAfterDelete(deletedDisplayOrder, userId);
		//		}
		//		// Delete the image file associated with the entity
		//		await DeleteImageFile(entity.ImageUrl);

		//		return true;
		//	}
		//	catch (Exception)
		//	{
		//		return false;
		//	}
		//}


		// helper methods
		private async Task DeleteImageFile(string imageUrl)
		{
			try
			{
				// Extract filename from URL if it's a full URL
				string fileName = Path.GetFileName(imageUrl);

				// Construct the full file path (adjust path as needed for your setup)
				string filePath = Path.Combine("wwwroot/uploads/Images/banners", fileName);

				// Delete the file if it exists
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}
			catch (Exception ex)
			{
				// Log the exception if you have logging configured
				// _logger.LogError(ex, "Failed to delete image file: {ImageUrl}", imageUrl);
			}
		}
		private async Task ShiftDisplayOrderForInsertAsync(List<TbHomePageSlider> entitiesToShift, Guid userId)
		{
			// Get all entities at or after the insert position
			entitiesToShift = entitiesToShift.OrderByDescending(c => c.DisplayOrder) // Update in descending order to avoid constraint violations
			   .ToList();

			foreach (var entity in entitiesToShift)
			{
				entity.DisplayOrder += 1;
				_baseRepository.UpdateAsync(entity, userId);
			}
		}
		//private void ShiftDisplayOrderAfterDelete(int deletedDisplayOrder, Guid userId)
		//{
		//	// Get all active entities with display order greater than the deleted entity
		//	var entitiesToShift = _baseRepository.GetAsync(c => !c.IsDeleted && c.DisplayOrder > deletedDisplayOrder)
				

		//	foreach (var entity in entitiesToShift)
		//	{
		//		entity.DisplayOrder -= 1;
		//		_baseRepository.Update(entity, userId, out _);
		//	}
		//}
		//private async Task UpdateOtherEntitiesDisplayOrderAsync(List<TbMainBanner> entities, int newDisplayOrder, int oldDisplayOrder, Guid userId, int? maxDisplayOrder = null)
		//{
		//	// Validate inputs
		//	if (newDisplayOrder < 1)
		//		throw new ArgumentOutOfRangeException(nameof(newDisplayOrder), "DisplayOrder must be positive.");
		//	if (userId == Guid.Empty)
		//		throw new ArgumentException("Invalid user ID.", nameof(userId));
		//	if (oldDisplayOrder == newDisplayOrder)
		//	{
		//		// No change needed
		//		return;
		//	}

		//	// Get max display order if not provided
		//	if (!maxDisplayOrder.HasValue)
		//	{
		//		var allMainBanners = _baseRepository.Get(c => c.CurrentState == 1).ToList();
		//		maxDisplayOrder = allMainBanners.Any() ? allMainBanners.Max(c => c.DisplayOrder) : 0;
		//	}

		//	// Clamp newDisplayOrder to valid range
		//	newDisplayOrder = Math.Max(1, Math.Min(newDisplayOrder, maxDisplayOrder.Value));

		//	// Sort entities by display order
		//	entities = entities.OrderBy(c => c.DisplayOrder).ToList();

		//	// Adjust DisplayOrder based on whether newDisplayOrder is higher or lower
		//	if (newDisplayOrder > oldDisplayOrder)
		//	{
		//		// Moving to a higher DisplayOrder: decrement entities in (oldDisplayOrder, newDisplayOrder]
		//		var entitiesToUpdate = entities
		//			.Where(c => c.DisplayOrder > oldDisplayOrder && c.DisplayOrder <= newDisplayOrder)
		//			.OrderByDescending(c => c.DisplayOrder) // Update in descending order to avoid constraint violations
		//			.ToList();

		//		foreach (var entity in entitiesToUpdate)
		//		{
		//			entity.DisplayOrder -= 1;
		//			_baseRepository.Update(entity, userId, out _);
		//		}
		//	}
		//	else
		//	{
		//		// Moving to a lower DisplayOrder: increment entities in [newDisplayOrder, oldDisplayOrder)
		//		var entitiesToUpdate = entities
		//			.Where(c => c.DisplayOrder >= newDisplayOrder && c.DisplayOrder < oldDisplayOrder)
		//			.OrderBy(c => c.DisplayOrder) // Update in ascending order to avoid constraint violations
		//			.ToList();

		//		foreach (var entity in entitiesToUpdate)
		//		{
		//			entity.DisplayOrder += 1;
		//			_baseRepository.Update(entity, userId, out _);
		//		}
		//	}
		//}
		//private async Task<string> SaveImageSync(string image)
		//{
		//	// Check if the file is null or empty
		//	if (image == null || image.Length == 0)
		//	{
		//		throw new ValidationException(ValidationResources.ImageRequired);
		//	}

		//	// Validate the file
		//	var imageValidation = _fileUploadService.ValidateFile(image);
		//	if (!imageValidation.isValid)
		//	{
		//		throw new ValidationException(imageValidation.errorMessage);
		//	}

		//	try
		//	{
		//		// Convert the file to byte array
		//		var imageBytes = await _fileUploadService.GetFileBytesAsync(image);

		//		// Resize the image
		//		var resizedImage = _imageProcessingService.ResizeImage(imageBytes, 1920, 920);

		//		// Convert the resized image to WebP format
		//		var webpImage = _imageProcessingService.ConvertToWebP(imageBytes);

		//		// Upload the WebP image to the specified location
		//		var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images\\banners");

		//		// Return the path of the uploaded image
		//		return imagePath;
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
		//	}
		//}

	}
}