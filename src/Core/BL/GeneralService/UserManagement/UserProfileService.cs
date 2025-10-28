using BL.Contracts.GeneralService.UserManagement;

namespace BL.GeneralService.UserManagement
{
    public class UserProfileService : IUserProfileService
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IUnitOfWork _userProfileUnitOfWork;
        //private readonly IRepository<VwUserProfile> _userProfileService;
        //private readonly IFileUploadService _fileUploadService;
        //private readonly IImageProcessingService _imageProcessingService;
        //private readonly IBaseMapper _mapper;
        //private readonly ILogger _logger;

        //public UserProfileService(
        //    UserManager<ApplicationUser> userManager,
        //    IBaseMapper mapper,
        //    IUnitOfWork userProfileUnitOfWork,
        //    IRepository<VwUserProfile> userProfileService,
        //    IFileUploadService fileUploadService,
        //    IImageProcessingService imageProcessingService,
        //    ILogger logger)
        //{
        //    _userManager = userManager;
        //    _userProfileUnitOfWork = userProfileUnitOfWork;
        //    _mapper = mapper;
        //    _userProfileService = userProfileService;
        //    _fileUploadService = fileUploadService;
        //    _imageProcessingService = imageProcessingService;
        //    _logger = logger;
        //}

        //public async Task<bool> DeleteAccount(Guid id, Guid updatorId)
        //{
        //    if (!_userManager.Users.Any(u => u.Id == updatorId.ToString()))
        //        throw new Exception("updator not found");

        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null)
        //        throw new Exception("User not found");
        //    if (user.UserState == UserStateType.Deleted)
        //        throw new Exception("The account was already deleted.");

        //    // Deleted algorithm 
        //    var guid = Guid.NewGuid();
        //    user.UserState = UserStateType.Deleted;
        //    user.UserName = user.UserName + "-" + guid;
        //    user.Email = user.Email + "-" + guid;

        //    user.UpdatedBy = updatorId;
        //    user.UpdatedDateUtc = DateTime.UtcNow;

        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        //    return true;
        //}

        //public async Task<UserStateType> GetUserStateAsync(Guid Id)
        //{
        //    var user = await _userManager.FindByIdAsync(Id.ToString());
        //    if (user == null)
        //        throw new Exception("User not found");

        //    return user.UserState;
        //}

        //public async Task<PaginatedDataModel<AdminProfileDto>> GetAdminsPage(BaseSearchCriteriaModel criteriaModel)
        //{
        //    if (criteriaModel == null)
        //        throw new ArgumentNullException(nameof(criteriaModel));

        //    if (criteriaModel.PageNumber < 1)
        //        throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

        //    if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
        //        throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");

        //    IEnumerable<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("Admin");
        //    users = users.Where(u => u.UserState != UserStateType.Deleted);

        //    // Apply search term if provided
        //    if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
        //    {
        //        string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
        //        users = users.Where(u => (u.UserName.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm)) ||
        //                                 ((u.FirstName + ' ' + u.LastName) != null && (u.FirstName + ' ' + u.LastName).ToLower().Contains(searchTerm)));
        //    }

        //    var totalRecords = users.Count();
        //    users = users.Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize).Take(criteriaModel.PageSize);
        //    return new PaginatedDataModel<AdminProfileDto>(_mapper.MapList<ApplicationUser, AdminProfileDto>(users), totalRecords);
        //}

        //public async Task<IEnumerable<AdminProfileDto>> GetAllAdminsAsync()
        //{
        //    var users = await _userManager.GetUsersInRoleAsync("Admin");
        //    users = users.Where(u => u.UserState != UserStateType.Deleted).ToList();
        //    return _mapper.MapList<ApplicationUser, AdminProfileDto>(users);
        //}

        //public async Task<AdminRegistrationDto> FindAdminDtoByIdAsync(string userId)
        //{
        //    var entity = await _userManager.FindByIdAsync(userId);

        //    if (entity.UserState == UserStateType.Deleted)
        //        throw new ArgumentNullException(UserResources.UserNotFound);

        //    return _mapper.MapModel<ApplicationUser, AdminRegistrationDto>(entity);
        //}

        //public async Task<AdminProfileDto> GetAdminProfileAsync(string userId)
        //{
        //    // Validate input (fail fast)
        //    if (string.IsNullOrWhiteSpace(userId))
        //        throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

        //    // Get userProfile data 
        //    var userProfile = await _userManager.FindByIdAsync(userId)
        //        ?? throw new ArgumentNullException(UserResources.UserNotFound);

        //    if (userProfile.UserState == UserStateType.Deleted)
        //        throw new ArgumentNullException(UserResources.UserNotFound);

        //    // Map to DTO
        //    return _mapper.MapModel<ApplicationUser, AdminProfileDto>(userProfile)
        //        ?? throw new NotFoundException(UserResources.UserNotFound, _logger);
        //}

        //public async Task<ResponseModel<AdminProfileDto>> UpdateAdminProfileAsync(string userId, AdminProfileUpdateDto ProfileUpdateDto, Guid updatorId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null || user.UserState == UserStateType.Deleted)
        //    {
        //        return new ResponseModel<AdminProfileDto>
        //        {
        //            Success = false,
        //            Message = "User not found.",
        //            Errors = new List<string> { UserResources.UserNotFound }
        //        };
        //    }

        //    // Update user properties
        //    user.FirstName = ProfileUpdateDto.FirstName;
        //    user.LastName = ProfileUpdateDto.LastName;
        //    user.UpdatedBy = updatorId;
        //    user.UpdatedDateUtc = DateTime.UtcNow;

        //    var result = await _userManager.UpdateAsync(user);
        //    if (result.Succeeded)
        //    {
        //        return new ResponseModel<AdminProfileDto>
        //        {
        //            Success = true,
        //            Message = NotifiAndAlertsResources.ProfileUpdated,
        //            Data = _mapper.MapModel<ApplicationUser, AdminProfileDto>(user),
        //        };
        //    }
        //    // Collect errors from IdentityResult
        //    var errors = result.Errors.Select(e => e.Description).ToList();
        //    return new ResponseModel<AdminProfileDto> { Success = false, Message = string.Join(", ", errors) };
        //}


        //public async Task<PaginatedDataModel<MarketerProfileDto>> GetMarketersPage(BaseSearchCriteriaModel criteriaModel)
        //{
        //    if (criteriaModel == null)
        //        throw new ArgumentNullException(nameof(criteriaModel));

        //    if (criteriaModel.PageNumber < 1)
        //        throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

        //    if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
        //        throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");

        //    IEnumerable<ApplicationUser> usersWithRole = await _userManager.GetUsersInRoleAsync("Marketer");
        //    usersWithRole = usersWithRole.Where(u => u.UserState != UserStateType.Deleted);
        //    var userIds = usersWithRole.Select(u => u.Id).ToList();

        //    IEnumerable<ApplicationUser> users = _userProfileUnitOfWork.Repository<ApplicationUser>().Get(u => userIds.Contains(u.Id), includeProperties: "Marketer");

        //    // Apply search term if provided
        //    if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
        //    {
        //        string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
        //        users = users.Where(u => (u.UserName.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm) ||
        //                                  u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm)));
        //    }

        //    var totalRecords = users.Count();
        //    users = users.Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize).Take(criteriaModel.PageSize);
        //    return new PaginatedDataModel<MarketerProfileDto>(_mapper.MapList<ApplicationUser, MarketerProfileDto>(users), totalRecords);
        //}

        //public async Task<IEnumerable<MarketerProfileDto>> GetAllMarketersAsync()
        //{
        //    IEnumerable<ApplicationUser> usersWithRole = await _userManager.GetUsersInRoleAsync("Marketer");
        //    usersWithRole = usersWithRole.Where(u => u.UserState != UserStateType.Deleted).ToList();
        //    var userIds = usersWithRole.Select(u => u.Id).ToList();

        //    IEnumerable<ApplicationUser> users = _userProfileUnitOfWork.Repository<ApplicationUser>().Get(u => userIds.Contains(u.Id), includeProperties: "Marketer");
        //    return _mapper.MapList<ApplicationUser, MarketerProfileDto>(users);
        //}

        //public async Task<MarketerRegistrationDto> FindMarketerDtoByIdAsync(string userId)
        //{
        //    var entity = _userProfileUnitOfWork.Repository<ApplicationUser>().Get(u => u.Id == userId && u.UserState != UserStateType.Deleted, includeProperties: "Marketer").FirstOrDefault();
        //    return _mapper.MapModel<ApplicationUser, MarketerRegistrationDto>(entity);
        //}

        //public async Task<SponsorDto> GetSponsorAsync(string recruitmentLinkCode)
        //{
        //    var marketer = _userProfileUnitOfWork.TableRepository<TbMarketer>().Get(m => m.SponsoredRecruitments.Any(r => r.Code == recruitmentLinkCode), includeProperties: "ApplicationUser,Recruitment").FirstOrDefault();
        //    if (marketer == null)
        //        return null;

        //    return new SponsorDto
        //    {
        //        Id = marketer.Id,
        //        FirstName = marketer.ApplicationUser.FirstName,
        //        LastName = marketer.ApplicationUser.LastName,
        //        ProfileImagePath = marketer.ApplicationUser.ProfileImagePath,
        //        TeamId = marketer.TeamId
        //    };
        //}

        //public SponsorDto GetSponsorByUserIdAsync(string UserId)
        //{
        //    var marketer = _userProfileUnitOfWork.TableRepository<TbMarketer>().
        //        Get(m => m.ApplicationUserId == UserId, includeProperties: "Recruitment.Sponsor.ApplicationUser").FirstOrDefault();

        //    var sponsor = marketer?.Recruitment.Sponsor;
        //    if (sponsor == null)
        //        return null;

        //    return new SponsorDto
        //    {
        //        Id = sponsor.Id,
        //        FirstName = sponsor.ApplicationUser.FirstName,
        //        LastName = sponsor.ApplicationUser.LastName,
        //        ProfileImagePath = sponsor.ApplicationUser.ProfileImagePath,
        //        Email = sponsor.ApplicationUser.Email,
        //        PhoneNumber = sponsor.ApplicationUser.PhoneCode + sponsor.ApplicationUser.PhoneNumber,
        //    };
        //}

        //public async Task<MarketerProfileDto> GetMarketerProfileAsync(string userId)
        //{
        //    // Validate input (fail fast)
        //    if (string.IsNullOrEmpty(userId))
        //        throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

        //    // Get marketerProfile data 
        //    var marketerProfile = _userProfileUnitOfWork.Repository<VwMarketerProfile>().Get(m => m.ApplicationUserId == userId).FirstOrDefault()
        //        ?? throw new ArgumentNullException(NotifiAndAlertsResources.AccessDenied);

        //    // Map to DTO
        //    return _mapper.MapModel<VwMarketerProfile, MarketerProfileDto>(marketerProfile)
        //        ?? throw new NotFoundException(UserResources.UserNotFound, _logger);
        //}

        //public async Task<ResponseModel<MarketerProfileDto>> UpdateMarketerProfileAsync(string userId, MarketerProfileUpdateDto updateProfileDto)
        //{
        //    try
        //    {
        //        // Check if the marketer is already registered
        //        var existingMarketer = _userProfileUnitOfWork.TableRepository<TbMarketer>().Get(u => u.ApplicationUserId == userId).FirstOrDefault();
        //        if (existingMarketer == null)
        //        {
        //            return new ResponseModel<MarketerProfileDto>
        //            {
        //                Success = false,
        //                Message = UserResources.UserNotFound,
        //                Errors = new List<string> { UserResources.UserNotFound }
        //            };
        //        }

        //        // Update profile image in application user
        //        var applicationUser = await _userManager.FindByIdAsync(existingMarketer.ApplicationUserId);

        //        if (!string.IsNullOrEmpty(updateProfileDto.ProfileImage))
        //        {
        //            // Remove old
        //            // Add new
        //            applicationUser.ProfileImagePath = await SaveImageSync(updateProfileDto.ProfileImage);
        //        }
        //        else
        //        {
        //            if (updateProfileDto.IsProfileImageDeleted)
        //            {
        //                // remove old
        //                // set default
        //                applicationUser.ProfileImagePath = "uploads/Images/ProfileImages/Marketers/default.png";
        //            }
        //        }

        //        var updateUserResault = await _userManager.UpdateAsync(applicationUser);
        //        if (!updateUserResault.Succeeded)
        //        {
        //            return new ResponseModel<MarketerProfileDto>
        //            {
        //                Success = false,
        //                Message = NotifiAndAlertsResources.UpdateImageFailed,
        //                Errors = updateUserResault.Errors.Select(e => e.Description).ToList()
        //            };
        //        }

        //        // Update marketer
        //        if (updateProfileDto?.Address != null) existingMarketer.Address = updateProfileDto.Address;
        //        if (updateProfileDto?.AlternativePhoneCode != null) existingMarketer.AlternativePhoneCode = updateProfileDto.AlternativePhoneCode;
        //        if (updateProfileDto?.AlternativePhoneNumber != null) existingMarketer.AlternativePhoneNumber = updateProfileDto.AlternativePhoneNumber;

        //        var updateMarketerResault = _userProfileUnitOfWork.TableRepository<TbMarketer>().Update(existingMarketer, Guid.Empty, out Guid _);
        //        if (!updateMarketerResault)
        //        {
        //            return new ResponseModel<MarketerProfileDto>
        //            {
        //                Success = false,
        //                Message = NotifiAndAlertsResources.MarketerUpdateFailed,
        //                Errors = new List<string> { NotifiAndAlertsResources.MarketerUpdateFailed }
        //            };
        //        }

        //        var newProfile = GetMarketerProfileAsync(userId);
        //        return new ResponseModel<MarketerProfileDto>
        //        {
        //            Success = true,
        //            Message = NotifiAndAlertsResources.ProfileUpdated,
        //            Data = newProfile.Result
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception and rethrow it
        //        _logger.Error(ex, ValidationResources.ErrorProcessingImage);
        //        throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
        //    }
        //}

        //private async Task<string> SaveImageSync(string image)
        //{
        //    // Check if the file is null or empty
        //    if (image == null || image.Length == 0)
        //    {
        //        throw new ValidationException(ValidationResources.ImageRequired);
        //    }

        //    // Validate the file
        //    var imageValidation = _fileUploadService.ValidateFile(image);
        //    if (!imageValidation.isValid)
        //    {
        //        throw new ValidationException(imageValidation.errorMessage);
        //    }

        //    try
        //    {
        //        // Convert the file to byte array
        //        var imageBytes = await _fileUploadService.GetFileBytesAsync(image);

        //        // Resize the image
        //        var resizedImage = _imageProcessingService.ResizeImagePreserveAspectRatio(imageBytes, 500, 500);

        //        // Convert the resized image to WebP format
        //        var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

        //        // Upload the WebP image to the specified location
        //        var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images/ProfileImages");

        //        // Return the path of the uploaded image
        //        return imagePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception and rethrow it
        //        _logger.Error(ex, ValidationResources.ErrorProcessingImage);
        //        throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
        //    }
        //}

    }
}
