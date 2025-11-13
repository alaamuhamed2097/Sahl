using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Testimonial;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Resources;
using Serilog;
using Shared.DTOs.Testimonial;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace BL.Service.Testimonial
{
    public class TestimonialService : BaseService<TbTestimonial, TestimonialDto>, ITestimonialService
    {
        private readonly ITableRepository<TbTestimonial> _testimonialRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public TestimonialService(
            ITableRepository<TbTestimonial> testimonialRepository,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            IWebHostEnvironment webHostEnvironment,
            ILogger logger,
            IBaseMapper mapper) : base(testimonialRepository, mapper)
        {
            _testimonialRepository = testimonialRepository;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TestimonialDto>> GetAllAsync()
        {
            var testimonials = (await _testimonialRepository.GetAllAsync())
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.CreatedDateUtc)
                .ToList();

            return _mapper.MapList<TbTestimonial, TestimonialDto>(testimonials);
        }

        public async Task<TestimonialDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var testimonial = await _testimonialRepository
                .FindAsync(x => x.Id == id && x.CurrentState == 1);

            return _mapper.MapModel<TbTestimonial, TestimonialDto>(testimonial);
        }

        public async Task<PaginatedDataModel<TestimonialDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter
            Expression<Func<TbTestimonial, bool>> filter = x => x.CurrentState == 1;

            // Combine expressions manually
            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.CustomerName != null && x.CustomerName.ToLower().Contains(searchTerm) ||
                    x.CustomerTitle != null && x.CustomerTitle.ToLower().Contains(searchTerm) ||
                    x.TestimonialText != null && x.TestimonialText.ToLower().Contains(searchTerm)
                );
            }

            var testimonials = await _testimonialRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderBy(x => x.DisplayOrder));

            var itemsDto = _mapper.MapList<TbTestimonial, TestimonialDto>(testimonials.Items);

            return new PaginatedDataModel<TestimonialDto>(itemsDto, testimonials.TotalRecords);
        }

        public async Task<bool> SaveAsync(TestimonialDto dto, Guid userId)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            bool isUpdate = dto.Id != null && dto.Id != Guid.Empty;
            var oldImagePathInDb = _testimonialRepository.Get(s => s.Id == dto.Id).Select(s => s.CustomerImagePath).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(dto.Base64Image))
            {
                var match = Regex.Match(dto.Base64Image, @"data:image/(?<type>.+?);base64,(?<data>.+)");
                if (!match.Success)
                    throw new Exception("Invalid image format.");

                dto.CustomerImagePath = await _saveImageSync(match.Groups["data"].Value);

                // Delete old image if in update mode and a new image is sent
                if (isUpdate && !string.IsNullOrWhiteSpace(oldImagePathInDb))
                {
                    var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldImagePathInDb.TrimStart('/'));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }
            }
            else
            {
                // No new image uploaded
                dto.CustomerImagePath = isUpdate ? oldImagePathInDb : null;
                if (!isUpdate && dto.CustomerImagePath == null)
                    throw new Exception(ValidationResources.ImageRequired);
            }

            // Auto-assign display order for new testimonials
            if (dto.Id == Guid.Empty && dto.DisplayOrder <= 0)
            {
                var maxOrder = await GetMaxDisplayOrderAsync();
                dto.DisplayOrder = maxOrder + 1;
            }

            // Create new testimonial
            var newTestimonial = _mapper.MapModel<TestimonialDto, TbTestimonial>(dto);
            var result = await _testimonialRepository.SaveAsync(newTestimonial, userId);

            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var success = await _testimonialRepository
                .UpdateCurrentStateAsync(id, userId, 0);

            return success;
        }

        private async Task<int> GetMaxDisplayOrderAsync()
        {
            try
            {
                var testimonials = await _testimonialRepository.GetAllAsync();
                return testimonials.Any() ? testimonials.Max(t => t.DisplayOrder) : 0;
            }
            catch
            {
                return 0; // Fallback
            }
        }

        private async Task<string> _saveImageSync(string image)
        {
            // Check if the file is null or empty
            if (image == null || image.Length == 0)
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
                var resizedImage = _imageProcessingService.ResizeImagePreserveAspectRatio(imageBytes, 500, 500);

                // Convert the resized image to WebP format
                var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

                // Upload the WebP image to the specified location
                var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/Testimonials");

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