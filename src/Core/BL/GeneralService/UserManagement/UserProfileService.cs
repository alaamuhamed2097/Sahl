using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.IMapper;
using Common.Enumerations.User;
using DAL.Contracts.UnitOfWork;
using DAL.Exceptions;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Resources;
using Serilog;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;

namespace BL.GeneralService.UserManagement
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _userProfileUnitOfWork;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

        public UserProfileService(
            UserManager<ApplicationUser> userManager,
            IBaseMapper mapper,
            IUnitOfWork userProfileUnitOfWork,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            ILogger logger)
        {
            _userManager = userManager;
            _userProfileUnitOfWork = userProfileUnitOfWork;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _logger = logger;
        }

        public async Task<bool> DeleteAccount(Guid id, Guid updatorId)
        {
            if (!_userManager.Users.Any(u => u.Id == updatorId.ToString()))
                throw new Exception("updator not found");

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new Exception("User not found");
            if (user.UserState == UserStateType.Deleted)
                throw new Exception("The account was already deleted.");

            // Deleted algorithm 
            var guid = Guid.NewGuid();
            user.UserState = UserStateType.Deleted;
            user.UserName = user.UserName + "-" + guid;
            user.Email = user.Email + "-" + guid;

            user.UpdatedBy = updatorId;
            user.UpdatedDateUtc = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return true;
        }

        public async Task<UserStateType> GetUserStateAsync(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
                throw new Exception("User not found");

            return user.UserState;
        }

        public async Task<PagedResult<AdminProfileDto>> GetAdminsPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");

            IEnumerable<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("Admin");
            users = users.Where(u => u.UserState != UserStateType.Deleted);

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                users = users.Where(u => (u.UserName.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm)) ||
                                         ((u.FirstName + ' ' + u.LastName) != null && (u.FirstName + ' ' + u.LastName).ToLower().Contains(searchTerm)));
            }

            // Apply sorting if specified
            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                users = sortBy switch
                {
                    "username" => isDescending ? users.OrderByDescending(x => x.UserName) : users.OrderBy(x => x.UserName),
                    "email" => isDescending ? users.OrderByDescending(x => x.Email) : users.OrderBy(x => x.Email),
                    "name" => isDescending ? users.OrderByDescending(x => x.FirstName + " " + x.LastName) : users.OrderBy(x => x.FirstName + " " + x.LastName),
                    "userstate" => isDescending ? users.OrderByDescending(x => x.UserState) : users.OrderBy(x => x.UserState),
                    _ => users.OrderBy(x => x.UserName) // Default sorting
                };
            }

            var totalRecords = users.Count();
            users = users.Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize).Take(criteriaModel.PageSize);
            return new PagedResult<AdminProfileDto>(_mapper.MapList<ApplicationUser, AdminProfileDto>(users), totalRecords);
        }

        public async Task<IEnumerable<AdminProfileDto>> GetAllAdminsAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            users = users.Where(u => u.UserState != UserStateType.Deleted).ToList();
            return _mapper.MapList<ApplicationUser, AdminProfileDto>(users);
        }

        public async Task<AdminRegistrationDto> FindAdminDtoByIdAsync(string userId)
        {
            var entity = await _userManager.FindByIdAsync(userId);

            if (entity.UserState == UserStateType.Deleted)
                throw new ArgumentNullException(UserResources.UserNotFound);

            return _mapper.MapModel<ApplicationUser, AdminRegistrationDto>(entity);
        }

        public async Task<AdminProfileDto> GetAdminProfileAsync(string userId)
        {
            // Validate input (fail fast)
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            // Get userProfile data 
            var userProfile = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentNullException(UserResources.UserNotFound);

            if (userProfile.UserState == UserStateType.Deleted)
                throw new ArgumentNullException(UserResources.UserNotFound);

            // Map to DTO
            return _mapper.MapModel<ApplicationUser, AdminProfileDto>(userProfile)
                ?? throw new NotFoundException(UserResources.UserNotFound, _logger);
        }

        public async Task<ResponseModel<AdminProfileDto>> UpdateAdminProfileAsync(string userId, AdminProfileUpdateDto ProfileUpdateDto, Guid updatorId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.UserState == UserStateType.Deleted)
            {
                return new ResponseModel<AdminProfileDto>
                {
                    Success = false,
                    Message = "User not found.",
                    Errors = new List<string> { UserResources.UserNotFound }
                };
            }

            // Update user properties
            user.FirstName = ProfileUpdateDto.FirstName;
            user.LastName = ProfileUpdateDto.LastName;
            user.UpdatedBy = updatorId;
            user.UpdatedDateUtc = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ResponseModel<AdminProfileDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.ProfileUpdated,
                    Data = _mapper.MapModel<ApplicationUser, AdminProfileDto>(user),
                };
            }
            // Collect errors from IdentityResult
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ResponseModel<AdminProfileDto> { Success = false, Message = string.Join(", ", errors) };
        }

        private async Task<string> SaveImageSync(string image)
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
                var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/ProfileImages");

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
