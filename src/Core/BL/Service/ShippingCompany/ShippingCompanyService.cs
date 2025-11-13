using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.ShippingCompny;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Resources;
using Serilog;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace BL.Service.ShippingCompany
{
    public class ShippingCompanyService : BaseService<TbShippingCompany, ShippingCompanyDto>, IShippingCompanyService
    {
        private readonly ITableRepository<TbShippingCompany> _baseRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public ShippingCompanyService(
            ITableRepository<TbShippingCompany> baseRepository,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            IWebHostEnvironment webHostEnvironment,
            ILogger logger,
            IBaseMapper mapper)
            : base(baseRepository, mapper)
        {
            _baseRepository = baseRepository;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _mapper = mapper;
        }

        public PaginatedDataModel<ShippingCompanyDto> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbShippingCompany, bool>> filter = x => x.CurrentState == 1;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.CurrentState == 1 &&
                             (x.Name != null && x.Name.ToLower().Contains(searchTerm) ||
                             x.PhoneCode + x.PhoneNumber != null && (x.PhoneCode + x.PhoneNumber).ToLower().Contains(searchTerm));
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<TbShippingCompany>, IOrderedQueryable<TbShippingCompany>> orderBy = null;

            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                orderBy = query =>
                {
                    return sortBy switch
                    {
                        "name" => isDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                        "phonenumber" => isDescending ? query.OrderByDescending(x => x.PhoneNumber) : query.OrderBy(x => x.PhoneNumber),
                        "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
                        _ => query.OrderBy(x => x.Name) // Default sorting
                    };
                };
            }

            var entitiesList = _baseRepository.GetPage(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy);

            var dtoList = _mapper.MapList<TbShippingCompany, ShippingCompanyDto>(entitiesList.Items);

            return new PaginatedDataModel<ShippingCompanyDto>(dtoList, entitiesList.TotalRecords);
        }

        public async Task<bool> Save(ShippingCompanyDto dto, Guid userId)
        {
            bool isUpdate = dto.Id != null && dto.Id != Guid.Empty;
            var oldImagePathInDb = _baseRepository.Get(s => s.Id == dto.Id).Select(s => s.LogoImagePath).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(dto.Base64Image))
            {
                var match = Regex.Match(dto.Base64Image, @"data:image/(?<type>.+?);base64,(?<data>.+)");
                if (!match.Success)
                    throw new Exception("Invalid image format.");

                dto.LogoImagePath = await _saveImageSync(match.Groups["data"].Value);

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
                dto.LogoImagePath = isUpdate ? oldImagePathInDb : null;
                if (!isUpdate && dto.LogoImagePath == null)
                    throw new Exception(ValidationResources.ImageRequired);
            }

            return base.Save(dto, userId);
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
                var resizedImage = _imageProcessingService.ResizeImagePreserveAspectRatio(imageBytes, 800, 600);

                // Convert the resized image to WebP format
                var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

                // Upload the WebP image to the specified location
                var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/ShippingCompanies");

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
