using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.Location;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Setting;
using DAL.Contracts.Repositories;
using Domains.Entities.Setting;
using Microsoft.AspNetCore.Hosting;
using Resources;
using Shared.DTOs.Setting;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BL.Services.Setting;

public class SettingService : ISettingService
{
    private readonly ITableRepository<TbSetting> _settingRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IBaseMapper _mapper;
    private readonly ILocationBasedCurrencyService _locationBasedCurrencyService;

    public SettingService(
        ITableRepository<TbSetting> settingRepository,
        IFileUploadService fileUploadService,
        IImageProcessingService imageProcessingService,
        IWebHostEnvironment webHostEnvironment,
        IBaseMapper mapper,
        ILocationBasedCurrencyService locationBasedCurrencyService)
    {
        _settingRepository = settingRepository;
        _fileUploadService = fileUploadService;
        _imageProcessingService = imageProcessingService;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _locationBasedCurrencyService = locationBasedCurrencyService;
    }

    public async Task<SettingDto> GetSettingsAsync(string clientIp = "0", bool applyConversion = true)
    {
        var setting = (await _settingRepository.GetAllAsync())
            .FirstOrDefault();

        var result = _mapper.MapModel<TbSetting, SettingDto>(setting);

        if (result != null && applyConversion)
        {
            var (targetCurrency, baseCurrency) = await _locationBasedCurrencyService.GetCurrencyInfoAsync(clientIp);
            result = await _locationBasedCurrencyService.ApplyCurrencyConversionAsync(result, baseCurrency?.Code, targetCurrency?.Code);
        }

        return result;
    }

    public async Task<string> GetMainBannerPathAsync()
    {
        var mainBannerPath = (await _settingRepository.GetAsync())
            .Select(s => s.MainBannerPath)
            .FirstOrDefault();

        return mainBannerPath;
    }

    public async Task<decimal> GetShippingAmountAsync(string clientIp = "0", bool applyConversion = true)
    {
        var setting = (await _settingRepository.GetAsync())
            .Select(s => new TbSetting { ShippingAmount = s.ShippingAmount })
            .FirstOrDefault();

        if (setting == null) return 0m;

        if (!applyConversion)
            return setting.ShippingAmount;

        var dto = _mapper.MapModel<TbSetting, SettingDto>(setting);
        var (targetCurrency, baseCurrency) = await _locationBasedCurrencyService.GetCurrencyInfoAsync(clientIp);
        dto = await _locationBasedCurrencyService.ApplyCurrencyConversionAsync(dto, baseCurrency?.Code, targetCurrency?.Code);
        return dto.ShippingAmount;
    }

    public async Task<bool> UpdateSettingsAsync(SettingDto dto, Guid userId)
    {
        await HandleImage(dto);

        // Update existing setting
        var mappingResult = _mapper.MapModel<SettingDto, TbSetting>(dto);

        var result = await _settingRepository.UpdateAsync(mappingResult, userId);
        return result.Success;
    }

    private async Task HandleImage(SettingDto dto)
    {
        var oldIMainBannerInDb = (await _settingRepository.GetAsync(s => s.Id == dto.Id)).Select(s => s.MainBannerPath).FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(dto.Base64Image))
        {
            var match = Regex.Match(dto.Base64Image, @"data:image/(?<type>.+?);base64,(?<data>.+)");
            if (!match.Success)
                throw new Exception("Invalid image format.");

            dto.MainBannerPath = await _saveImageSync(match.Groups["data"].Value);

            // Delete old image if in update mode and a new image is sent
            if (!string.IsNullOrWhiteSpace(oldIMainBannerInDb))
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldIMainBannerInDb.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }
        }
        else
        {
            // No new image uploaded
            dto.MainBannerPath = dto.IsBannerDeleted ? null : oldIMainBannerInDb;

            // Delete old image if in update mode and a new image is sent
            if (!string.IsNullOrWhiteSpace(oldIMainBannerInDb) && dto.IsBannerDeleted)
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldIMainBannerInDb.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }
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
        var imageValidation = _fileUploadService.ValidateFile(image, 20 * 1024 * 1024);
        if (!imageValidation.isValid)
        {
            throw new ValidationException(imageValidation.errorMessage);
        }

        try
        {
            // Convert the file to byte array
            var imageBytes = await _fileUploadService.GetFileBytesAsync(image);

            // Convert the resized image to WebP format
            var webpImage = _imageProcessingService.ConvertToWebP(imageBytes);

            // Upload the WebP image to the specified location
            var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/Settings");

            // Return the path of the uploaded image
            return imagePath;
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow it
            throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
        }
    }
}