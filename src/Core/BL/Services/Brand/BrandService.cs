using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Brand;
using BL.Extensions;
using BL.Services.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Catalog.Brand;
using Microsoft.AspNetCore.Hosting;
using Resources;
using Serilog;
using Shared.DTOs.Brand;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace BL.Services.Brand;

public class BrandService : BaseService<TbBrand, BrandDto>, IBrandService
{
    private readonly ITableRepository<TbBrand> _brandRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger _logger;
    private readonly IBaseMapper _mapper;

    public BrandService(
        ITableRepository<TbBrand> brandRepository,
        IFileUploadService fileUploadService,
        IImageProcessingService imageProcessingService,
        IWebHostEnvironment webHostEnvironment,
        ILogger logger,
        IBaseMapper mapper)
        : base(brandRepository, mapper)
    {
        _brandRepository = brandRepository;
        _fileUploadService = fileUploadService;
        _imageProcessingService = imageProcessingService;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BrandDto>> GetAllAsync()
    {
        var brands = await _brandRepository
            .GetAsync(x => !x.IsDeleted, orderBy: q => q.OrderBy(x => x.DisplayOrder));

        var brandDtos = _mapper.MapList<TbBrand, BrandDto>(brands).ToList();

        return brandDtos;
    }

    public async Task<BrandDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        var brand = await _brandRepository
            .FindByIdAsync(id);

        if (brand == null) return null;

        var brandDto = _mapper.MapModel<TbBrand, BrandDto>(brand);

        return brandDto;
    }

    public async Task<PagedResult<BrandDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        // Base filter
        Expression<Func<TbBrand, bool>> filter = x => !x.IsDeleted;

        // Search term filter
        var searchTerm = criteriaModel.SearchTerm?.Trim();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // SQL Server is usually case-insensitive by default. 
            // Using .Contains() directly allows for SARGable queries if the database collation is Case-Insensitive.
            // Using .ToLower() on the column expression prevents index usage.
            filter = filter.And(x =>
                x.NameEn.Contains(searchTerm) ||
                x.NameAr.Contains(searchTerm)
            );
        }

        var brands = await _brandRepository.GetPageAsync(
            criteriaModel.PageNumber,
            criteriaModel.PageSize,
            filter,
            orderBy: q => q.OrderBy(x => x.DisplayOrder));

        var itemsDto = _mapper.MapList<TbBrand, BrandDto>(brands.Items).ToList();

        return new PagedResult<BrandDto>(itemsDto, brands.TotalRecords);
    }

    public async Task<bool> SaveAsync(BrandDto dto, Guid userId)
    {
        bool isUpdate = dto.Id != null && dto.Id != Guid.Empty;
        var oldImagePathInDb = (await _brandRepository.GetAsync(s => s.Id == dto.Id)).Select(s => s.LogoPath).FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(dto.Base64Image))
        {
            var match = Regex.Match(dto.Base64Image, @"data:image/(?<type>.+?);base64,(?<data>.+)");
            if (!match.Success)
                throw new Exception("Invalid image format.");

            dto.LogoPath = await _saveImageSync(match.Groups["data"].Value);

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
            dto.LogoPath = isUpdate ? oldImagePathInDb : null;
            if (!isUpdate && dto.LogoPath == null)
                throw new Exception(ValidationResources.ImageRequired);
        }

        return (await base.SaveAsync(dto, userId)).Success;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var success = await _brandRepository
            .UpdateIsDeletedAsync(id, userId, true);

        return success;
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
            var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/Brands");

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