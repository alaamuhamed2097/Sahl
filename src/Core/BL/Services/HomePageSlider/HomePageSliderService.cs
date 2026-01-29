using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.HomePageSlider;
using BL.Extensions;
using BL.Services.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
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
		private readonly IFileUploadService _fileUploadService;
		private readonly IImageProcessingService _imageProcessingService;
		private readonly IUnitOfWork _unitOfWork;

		public HomePageSliderService(
			ITableRepository<TbHomePageSlider> baseRepository,
			IBaseMapper mapper,
			ILogger logger,
			IFileUploadService fileUploadService,
			IImageProcessingService imageProcessingService,
			IUnitOfWork unitOfWork)
			: base(baseRepository, mapper)
		{
			_baseRepository = baseRepository;
			_mapper = mapper;
			_logger = logger;
			_fileUploadService = fileUploadService;
			_imageProcessingService = imageProcessingService;
			_unitOfWork = unitOfWork;
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

		public async Task<bool> Save(HomePageSliderDto dto, Guid userId)
		{
			// Validate inputs
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			if (userId == Guid.Empty)
				throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

			try
			{
				await _unitOfWork.BeginTransactionAsync();

				// Store old image path for deletion later
				string? oldImagePath = null;

				// Get old image if updating
				if (dto.Id != Guid.Empty)
				{
					var existingSlider = await _baseRepository.FindByIdAsync(dto.Id);
					if (existingSlider != null)
					{
						oldImagePath = existingSlider.ImageUrl;
					}
				}

				// Handle image upload
				if (!string.IsNullOrEmpty(dto.ImageUrl) &&
					_fileUploadService.ValidateFile(dto.ImageUrl).isValid)
				{
					// Save the new image
					dto.ImageUrl = await SaveImageAsync(dto.ImageUrl, "Images/HomeSliders", 1920, 762);
				}

				// Get all existing sliders to determine max display order
				var allSliders = (await _baseRepository.GetAsync(c => !c.IsDeleted)).ToList();
				var maxDisplayOrder = allSliders.Any() ? allSliders.Max(c => c.DisplayOrder) : 0;
				int? oldDisplayOrder = null;

				// Handle existing slider case - get old display order BEFORE saving
				if (dto.Id != Guid.Empty)
				{
					var existingSlider = allSliders.FirstOrDefault(c => c.Id == dto.Id);
					if (existingSlider != null)
					{
						oldDisplayOrder = existingSlider.DisplayOrder;
					}

					// Update other entities' display orders BEFORE saving the target entity
					if (oldDisplayOrder.HasValue && oldDisplayOrder.Value != dto.DisplayOrder)
					{
						await UpdateOtherEntitiesDisplayOrderAsync(
							allSliders.Where(c => c.Id != dto.Id).ToList(),
							dto.DisplayOrder,
							oldDisplayOrder.Value,
							userId,
							maxDisplayOrder);
					}
				}
				else
				{
					// Handle new slider case
					if (dto.DisplayOrder <= 0)
					{
						dto.DisplayOrder = maxDisplayOrder + 1;
					}
					else if (dto.DisplayOrder > maxDisplayOrder + 1)
					{
						dto.DisplayOrder = maxDisplayOrder + 1;
					}
					else
					{
						await ShiftDisplayOrderForInsertAsync(
							allSliders.Where(c => c.DisplayOrder >= dto.DisplayOrder).ToList(),
							userId);
					}
				}

				// Map DTO to entity
				var entity = _mapper.MapModel<HomePageSliderDto, TbHomePageSlider>(dto);

				// Save the entity
				var saveResult = await _baseRepository.SaveAsync(entity, userId);

				if (!saveResult.Success)
				{
					await _unitOfWork.RollbackAsync();
					return false;
				}

				// Delete old image if a new one was uploaded
				if (!string.IsNullOrEmpty(oldImagePath) &&
					oldImagePath != dto.ImageUrl)
				{
					await DeleteImageFileAsync(oldImagePath);
				}

				await _unitOfWork.CommitAsync();
				return true;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackAsync();
				_logger.Error(ex, "Error saving home page slider {SliderId}", dto.Id);
				throw;
			}
		}

		public async Task<bool> Delete(Guid id, Guid userId)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();

				// Get the entity to be deleted
				var entity = await _baseRepository.FindByIdAsync(id);
				if (entity == null)
				{
					await _unitOfWork.RollbackAsync();
					return false;
				}

				// Store the display order and image path before deletion
				int deletedDisplayOrder = entity.DisplayOrder;
				string? imageToDelete = entity.ImageUrl;

				// Set display order to 0 and update
				var dto = _mapper.MapModel<TbHomePageSlider, HomePageSliderDto>(entity);
				dto.DisplayOrder = 0;

				var updateEntity = _mapper.MapModel<HomePageSliderDto, TbHomePageSlider>(dto);
				var updateResult = await _baseRepository.SaveAsync(updateEntity, userId);

				if (!updateResult.Success)
				{
					await _unitOfWork.RollbackAsync();
					return false;
				}

				// Delete the entity
				var deleteSuccess = await _baseRepository.SoftDeleteAsync(entity.Id, userId);
				if (!deleteSuccess)
				{
					await _unitOfWork.RollbackAsync();
					return false;
				}

				// Shift display orders of remaining entities
				if (deletedDisplayOrder > 0)
				{
					await ShiftDisplayOrderAfterDeleteAsync(deletedDisplayOrder, userId);
				}

				// Delete the image file
				if (!string.IsNullOrEmpty(imageToDelete))
				{
					await DeleteImageFileAsync(imageToDelete);
				}

				await _unitOfWork.CommitAsync();
				return true;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackAsync();
				_logger.Error(ex, "Error deleting home page slider {SliderId}", id);
				return false;
			}
		}

		public async Task<bool> UpdateDisplayOrderAsync(Guid sliderId, int newOrder, Guid userId)
		{
			var entity = await _baseRepository.FindByIdAsync(sliderId);
			if (entity == null)
				return false;

			entity.DisplayOrder = newOrder;
			var result = await _baseRepository.SaveAsync(entity, userId);
			return result.Success;
		}

		// ===== HELPER METHODS =====

		/// <summary>
		/// Save and process an image with custom dimensions
		/// </summary>
		private async Task<string> SaveImageAsync(
			string image,
			string uploadPath,
			int width = 1920,
			int height = 920)
		{
			if (string.IsNullOrEmpty(image))
			{
				throw new ValidationException(ValidationResources.ImageRequired);
			}

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
				var resizedImage = _imageProcessingService.ResizeImage(imageBytes, width, height);

				// Convert the resized image to WebP format
				var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

				// Upload the WebP image to the specified location
				var imagePath = await _fileUploadService.UploadFileAsync(webpImage, uploadPath);

				return imagePath;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ValidationResources.ErrorProcessingImage);
				throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
			}
		}

		/// <summary>
		/// Delete an image file from the server
		/// </summary>
		private async Task DeleteImageFileAsync(string imageUrl)
		{
			try
			{
				if (string.IsNullOrEmpty(imageUrl))
					return;

				await _fileUploadService.DeleteFileAsync(imageUrl);
				_logger.Information("Successfully deleted image: {ImageUrl}", imageUrl);
			}
			catch (Exception ex)
			{
				_logger.Warning(ex, "Failed to delete image file: {ImageUrl}", imageUrl);
				// Don't throw - continue even if file deletion fails
			}
		}

		/// <summary>
		/// Shift display orders when inserting a new slider
		/// </summary>
		private async Task ShiftDisplayOrderForInsertAsync(List<TbHomePageSlider> entitiesToShift, Guid userId)
		{
			// Update in descending order to avoid constraint violations
			entitiesToShift = entitiesToShift
				.OrderByDescending(c => c.DisplayOrder)
				.ToList();

			foreach (var entity in entitiesToShift)
			{
				entity.DisplayOrder += 1;
				await _baseRepository.SaveAsync(entity, userId);
			}
		}

		/// <summary>
		/// Shift display orders after deleting a slider
		/// </summary>
		private async Task ShiftDisplayOrderAfterDeleteAsync(int deletedDisplayOrder, Guid userId)
		{
			// Get all active entities with display order greater than the deleted entity
			var entitiesToShift = (await _baseRepository.GetAsync(
				c => !c.IsDeleted && c.DisplayOrder > deletedDisplayOrder))
				.ToList();

			foreach (var entity in entitiesToShift)
			{
				entity.DisplayOrder -= 1;
				await _baseRepository.SaveAsync(entity, userId);
			}
		}

		/// <summary>
		/// Update display orders when reordering sliders
		/// </summary>
		private async Task UpdateOtherEntitiesDisplayOrderAsync(
			List<TbHomePageSlider> entities,
			int newDisplayOrder,
			int oldDisplayOrder,
			Guid userId,
			int? maxDisplayOrder = null)
		{
			// Validate inputs
			if (newDisplayOrder < 1)
				throw new ArgumentOutOfRangeException(nameof(newDisplayOrder), "DisplayOrder must be positive.");

			if (userId == Guid.Empty)
				throw new ArgumentException("Invalid user ID.", nameof(userId));

			if (oldDisplayOrder == newDisplayOrder)
			{
				// No change needed
				return;
			}

			// Get max display order if not provided
			if (!maxDisplayOrder.HasValue)
			{
				var allSliders = (await _baseRepository.GetAsync(c => !c.IsDeleted)).ToList();
				maxDisplayOrder = allSliders.Any() ? allSliders.Max(c => c.DisplayOrder) : 0;
			}

			// Clamp newDisplayOrder to valid range
			newDisplayOrder = Math.Max(1, Math.Min(newDisplayOrder, maxDisplayOrder.Value));

			// Sort entities by display order
			entities = entities.OrderBy(c => c.DisplayOrder).ToList();

			// Adjust DisplayOrder based on whether newDisplayOrder is higher or lower
			if (newDisplayOrder > oldDisplayOrder)
			{
				// Moving to a higher DisplayOrder: decrement entities in (oldDisplayOrder, newDisplayOrder]
				var entitiesToUpdate = entities
					.Where(c => c.DisplayOrder > oldDisplayOrder && c.DisplayOrder <= newDisplayOrder)
					.OrderByDescending(c => c.DisplayOrder) // Update in descending order
					.ToList();

				foreach (var entity in entitiesToUpdate)
				{
					entity.DisplayOrder -= 1;
					await _baseRepository.SaveAsync(entity, userId);
				}
			}
			else
			{
				// Moving to a lower DisplayOrder: increment entities in [newDisplayOrder, oldDisplayOrder)
				var entitiesToUpdate = entities
					.Where(c => c.DisplayOrder >= newDisplayOrder && c.DisplayOrder < oldDisplayOrder)
					.OrderBy(c => c.DisplayOrder) // Update in ascending order
					.ToList();

				foreach (var entity in entitiesToUpdate)
				{
					entity.DisplayOrder += 1;
					await _baseRepository.SaveAsync(entity, userId);
				}
			}
		}
	}
}