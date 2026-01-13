using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Vendor;
using BL.Services.Base;
using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Microsoft.AspNetCore.Identity;
using Resources;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.ResultModels;
using System.ComponentModel.DataAnnotations;

namespace BL.Services.Vendor;

public class VendorManagementService : BaseService<TbVendor, VendorDto>, IVendorManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVendorManagementRepository _vendorRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IBaseMapper _mapper;
    public VendorManagementService(
        UserManager<ApplicationUser> userManager,
        IVendorManagementRepository vendorRepository,
        IFileUploadService fileUploadService,
        IImageProcessingService imageProcessingService,
        IBaseMapper mapper)
        : base(vendorRepository, mapper)
    {
        _userManager = userManager;
        _vendorRepository = vendorRepository;
        _fileUploadService = fileUploadService;
        _imageProcessingService = imageProcessingService;
        _mapper = mapper;
    }

    public override async Task<VendorDto> FindByIdAsync(Guid Id)
    {
        return _mapper.MapModel<TbVendor, VendorDto>(await _vendorRepository.FindByVendorIdAsync(Id, new CancellationToken()));
    }

    public async Task<PagedResult<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        var entitiesList = await _vendorRepository.GetPageAsync(
            criteriaModel, new CancellationToken());

        var dtoList = _mapper.MapList<TbVendor, VendorDto>(entitiesList.Items);

        return new PagedResult<VendorDto>(dtoList, entitiesList.TotalRecords);
    }

    public async Task<TbVendor> GetByUserIdAsync(string userId)
    {
        return await _vendorRepository.FindAsync(v => v.UserId == userId);
    }

    public async Task<bool> UpdateVendorStatusAsync(Guid vendorId, VendorStatus status)
    {
        var vendor = await _vendorRepository.FindByIdAsync(vendorId);
        if (vendor == null) return false;

        vendor.Status = status;
        await _vendorRepository.UpdateAsync(vendor, Guid.Empty);
        return true;
    }

    public async Task<bool> UpdateUserStatusAsync(Guid vendorId, UserStateType newType)
    {
        return await _vendorRepository.UpdateUserStateAsync(vendorId, newType, new CancellationToken());
    }

    public async Task<ServiceResult<VendorUpdateResponseDto>> UpdateVendorAsync(VendorUpdateRequestDto request, string updaterId)
    {
        try
        {
            // Get the user
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new ServiceResult<VendorUpdateResponseDto>
                {
                    Success = false,
                    Message = "User not found",
                    Errors = new List<string> { "User does not exist" }
                };
            }

            // Get the vendor profile
            var vendor = await GetByUserIdAsync(request.UserId);
            if (vendor == null)
            {
                return new ServiceResult<VendorUpdateResponseDto>
                {
                    Success = false,
                    Message = "Vendor profile not found",
                    Errors = new List<string> { "Vendor profile does not exist" }
                };
            }

            // Validate BirthDate
            if (request.BirthDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return new ServiceResult<VendorUpdateResponseDto>
                {
                    Success = false,
                    Message = string.Format(UserResources.InValidDate, request.BirthDate),
                    Errors = new List<string> { string.Format(UserResources.InValidDate, request.BirthDate) }
                };
            }

            // Store old image paths for deletion after successful update
            string? oldFrontImagePath = vendor.IdentificationImageFrontPath;
            string? oldBackImagePath = vendor.IdentificationImageBackPath;

            // Handle Image Updates (only if new images provided)
            if (!string.IsNullOrWhiteSpace(request.IdentificationImageFrontPath) && request.IdentificationImageFrontPath != oldFrontImagePath)
            {
                try
                {
                    vendor.IdentificationImageFrontPath = await SaveImage(request.IdentificationImageFrontPath);
                }
                catch (Exception ex)
                {
                    return new ServiceResult<VendorUpdateResponseDto>
                    {
                        Success = false,
                        Message = "Failed to process front identification image",
                        Errors = new List<string> { ex.Message }
                    };
                }
            }

            if (!string.IsNullOrWhiteSpace(request.IdentificationImageBackPath) && request.IdentificationImageBackPath != oldBackImagePath)
            {
                try
                {
                    vendor.IdentificationImageBackPath = await SaveImage(request.IdentificationImageBackPath);
                }
                catch (Exception ex)
                {
                    return new ServiceResult<VendorUpdateResponseDto>
                    {
                        Success = false,
                        Message = "Failed to process back identification image",
                        Errors = new List<string> { ex.Message }
                    };
                }
            }

            // Update ApplicationUser info (except email and phone)
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UpdatedBy = Guid.Parse(updaterId);
            user.UpdatedDateUtc = DateTime.UtcNow;

            // Update Vendor Profile
            vendor.BirthDate = request.BirthDate;
            vendor.IdentificationType = request.IdentificationType;
            vendor.IdentificationNumber = request.IdentificationNumber;
            vendor.VendorType = request.VendorType;
            vendor.StoreName = request.StoreName;
            vendor.IsRealEstateRegistered = request.IsRealEstateRegistered;
            vendor.Address = request.Address;
            vendor.PostalCode = request.PostalCode;
            vendor.CityId = request.CityId;
            vendor.Notes = request.Notes;
            vendor.UpdatedBy = Guid.Parse(updaterId);
            vendor.UpdatedDateUtc = DateTime.UtcNow;

            // Execute update in transaction
            var updateResult = await _vendorRepository.UpdateVendorWithUserAsync(
                user, vendor, oldFrontImagePath, oldBackImagePath);

            if (!updateResult.Success)
            {
                var friendlyErrors = updateResult.Errors
                    .Select(e => GetUserFriendlyErrorMessage(e))
                    .ToList();

                return new ServiceResult<VendorUpdateResponseDto>
                {
                    Success = false,
                    Message = "Failed to update vendor profile",
                    Errors = friendlyErrors
                };
            }

            return new ServiceResult<VendorUpdateResponseDto>
            {
                Success = true,
                Message = "Vendor profile updated successfully",
                Data = new VendorUpdateResponseDto
                {
                    VendorId = vendor.Id,
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    StoreName = vendor.StoreName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PhoneCode = user.PhoneCode,
                    ProfileImagePath = user.ProfileImagePath,
                    Notes = vendor.Notes,
                    UpdatedDate = DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<VendorUpdateResponseDto>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrong,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    #region Helper functions
    private async Task<string> SaveImage(string image)
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
            var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/IdAndPassports");

            // Return the path of the uploaded image
            return imagePath;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
        }
    }

    private string GetUserFriendlyErrorMessage(string errorCode)
    {
        return errorCode switch
        {
            "DuplicateEmail" => UserResources.Email_Duplicate,
            "DuplicateUserName" => UserResources.UserName_Duplicate,
            "InvalidEmail" => UserResources.Invalid_Email,
            "UserNameAlreadyTaken" => UserResources.UserName_Duplicate,
            "PasswordTooShort" => UserResources.Password_Too_Short,
            _ => NotifiAndAlertsResources.SaveFailed
        };
    }
    #endregion
}
