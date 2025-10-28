using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.IMapper;
using Common.Enumerations.User;
using DAL.Contracts.UnitOfWork;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resources;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels.ResultModels;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace BL.GeneralService.UserManagement
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IBaseMapper _mapper;

        public UserRegistrationService(UserManager<ApplicationUser> userManager,
            IFileUploadService fileUploadService,
            IUserAuthenticationService userAuthenticationService,
            IImageProcessingService imageProcessingService,
            IBaseMapper mapper)
        {
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _userAuthenticationService = userAuthenticationService;
            _imageProcessingService = imageProcessingService;
            _mapper = mapper;
        }

        public async Task<OperationResult> RegisterAdminAsync(AdminRegistrationDto userDto, Guid CreatorId)
        {
            // Check if the email is already registered
            var existingUser = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = string.Format(UserResources.Email_Duplicate, userDto.Email),
                    Errors = new List<string> { string.Format(UserResources.Email_Duplicate, userDto.Email) }
                };
            }

            existingUser = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userDto.UserName);
            if (existingUser != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = string.Format(UserResources.UserName_Duplicate, userDto.UserName),
                    Errors = new List<string> { string.Format(UserResources.UserName_Duplicate, userDto.UserName) }
                };
            }

            // Map user DTO to ApplicationUser
            var applicationUser = _mapper.MapModel<AdminRegistrationDto, ApplicationUser>(userDto);
            applicationUser.Id = Guid.NewGuid().ToString();
            applicationUser.PhoneNumber = "0000000000";
            applicationUser.ProfileImagePath = "uploads/Images/default.png";
            applicationUser.PhoneCode = "+000";
            applicationUser.CreatedBy = CreatorId;
            applicationUser.CreatedDateUtc = DateTime.UtcNow;
            applicationUser.UserState = UserStateType.Inactive;

            // Generate and save verification code via UserActivationService
            //var codeSent = await _userActivationService.SendActivationCodeAsync(user.PhoneNumber);
            //if (!codeSent)
            //{
            //    return new BaseResult
            //    {
            //        Success = false,
            //        Message = UserResources.VerificationCodeError
            //    };
            //}

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
                // Create the user

                if (result.Succeeded)
                {
                    // Add user to the specified role
                    var addedToRole = (await _userManager.AddToRoleAsync(applicationUser, "Admin")).Succeeded;

                    if (!addedToRole)
                    {
                        throw new InvalidOperationException("Failed to add user to the specified role.");
                    }

                    return new OperationResult
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.RegistrationSuccessful
                    };
                }

                // Map identity errors to user-friendly messages
                var friendlyErrors = result.Errors
                    .Select(e => GetUserFriendlyErrorMessage(e.Code))
                    .ToList();

                return new OperationResult
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.RegistrationFailed,
                    Errors = friendlyErrors
                };
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        //public async Task<SignInResult> RegisterCustomerAsync(
        //    CustomerRegistrationDto userDto, string clientType)
        //{
        //    try
        //    {
        //        // Check if the customer is already registered by Email or UserName
        //        var existingUser = await _userManager.Users
        //            .FirstOrDefaultAsync(u => u.Email == userDto.Email || u.UserName == userDto.UserName);

        //        if (existingUser != null)
        //        {
        //            return new SignInResult
        //            {
        //                Success = false,
        //                Message = (existingUser.Email == userDto.Email)
        //                              ? string.Format(UserResources.Email_Duplicate, userDto.Email)
        //                              : string.Format(UserResources.UserName_Duplicate, userDto.UserName),
        //                Errors = (existingUser.Email == userDto.Email)
        //                              ? new List<string> { string.Format(UserResources.Email_Duplicate, userDto.Email) }
        //                              : new List<string> { string.Format(UserResources.UserName_Duplicate, userDto.UserName) }
        //            };
        //        }

        //        // Map Customer Registration Dto to ApplicationUser
        //        var customer = _mapper.MapModel<CustomerRegistrationDto, TbCustomer>(userDto);

        //        var applicationUser = customer.ApplicationUser;
        //        applicationUser.Id = Guid.NewGuid().ToString();
        //        applicationUser.ProfileImagePath = "uploads/Images/ProfileImages/Customers/default.png";
        //        applicationUser.CreatedBy = Guid.Empty;
        //        applicationUser.CreatedDateUtc = DateTime.UtcNow;
        //        applicationUser.UserState = UserStateType.Inactive;

        //        await _marketerUnitOfWork.BeginTransactionAsync();

        //        // Create the user
        //        var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
        //        if (!result.Succeeded)
        //        {
        //            var friendlyErrors = result.Errors
        //                .Select(e => GetUserFriendlyErrorMessage(e.Code))
        //                .ToList();

        //            return new SignInResult
        //            {
        //                Success = false,
        //                Message = NotifiAndAlertsResources.RegistrationFailed,
        //                Errors = friendlyErrors
        //            };
        //        }

        //        // Add user to Customer role
        //        var addedToRole = (await _userManager.AddToRoleAsync(applicationUser, "Customer")).Succeeded;
        //        if (!addedToRole)
        //        {
        //            throw new InvalidOperationException("Failed to add user to the specified role.");
        //        }

        //        // Create The marketer
        //        customer.ApplicationUserId = applicationUser.Id;

        //        var saveCustomerResult = await _marketerUnitOfWork.TableRepository<TbCustomer>().CreateAsync(customer, new Guid());

        //        await _marketerUnitOfWork.CommitAsync();

        //        var signInResult = await _userAuthenticationService.EmailOrUserNameSignInAsync(userDto.Email, userDto.Password, clientType);

        //        return signInResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        _marketerUnitOfWork.Rollback();
        //        return new SignInResult
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Errors = new List<string> { ex.InnerException?.Message ?? "" }
        //        };
        //    }
        //}

        //public async Task<OperationResult> RegisterMarketerAsync(string recruitmentLinkCode, MarketerRegistrationDto userDto)
        //{
        //    try
        //    {
        //        // Validate the recruitment link
        //        var recruitmentLinkValidation = await IsValidLinkForMarketerRegistration(recruitmentLinkCode);
        //        if (!recruitmentLinkValidation.Success)
        //            return recruitmentLinkValidation;

        //        // Check if the marketer is already registered
        //        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email || u.UserName == userDto.UserName);
        //        if (existingUser != null)
        //        {
        //            return new OperationResult
        //            {
        //                Success = false,
        //                Message = string.Format(UserResources.Email_Duplicate, userDto.Email),
        //                Errors = (existingUser.Email == userDto.Email)
        //                              ? new List<string> { string.Format(UserResources.Email_Duplicate, userDto.Email) }
        //                              : new List<string> { string.Format(UserResources.UserName_Duplicate, userDto.UserName) }
        //            };
        //        }

        //        // Map Marketer Registration Dto to TbMarketer
        //        var marketer = _mapper.MapModel<MarketerRegistrationDto, TbMarketer>(userDto);

        //        // ApplicationUser
        //        var applicationUser = marketer.ApplicationUser;
        //        applicationUser.Id = Guid.NewGuid().ToString();
        //        applicationUser.ProfileImagePath = "uploads/Images/ProfileImages/Marketers/default.png";
        //        applicationUser.CreatedBy = Guid.Empty;
        //        applicationUser.CreatedDateUtc = DateTime.UtcNow;
        //        applicationUser.UserState = UserStateType.Inactive;

        //        await _marketerUnitOfWork.BeginTransactionAsync();

        //        // Create the user
        //        var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
        //        if (!result.Succeeded)
        //        {
        //            // Map identity errors to user-friendly messages
        //            var friendlyErrors = result.Errors
        //                .Select(e => GetUserFriendlyErrorMessage(e.Code))
        //                .ToList();

        //            return new OperationResult
        //            {
        //                Success = false,
        //                Message = NotifiAndAlertsResources.RegistrationFailed,
        //                Errors = friendlyErrors
        //            };
        //        }

        //        // Add user to the specified role
        //        var addedToRole = (await _userManager.AddToRoleAsync(applicationUser, "Marketer")).Succeeded;
        //        if (!addedToRole)
        //        {
        //            throw new InvalidOperationException("Failed to add user to the specified role.");
        //        }
        //        var recruitmentLink = await _marketerUnitOfWork.TableRepository<TbRecruitment>().FindAsync(c => c.Code == recruitmentLinkCode);

        //        // Create The marketer
        //        marketer.Address = "";
        //        marketer.PathCode = (await _marketerUnitOfWork.TableRepository<TbRecruitment>().GetAsync(l => l.Code == recruitmentLinkCode)).Select(r => r.PathCode).First();
        //        marketer.JoinDate = DateTime.UtcNow;
        //        marketer.TotalPVs = 0;
        //        marketer.OldPVs = 0;
        //        marketer.ApplicationUserId = applicationUser.Id;
        //        marketer.RecruitmentId = recruitmentLink.Id;
        //        marketer.IsProfileCompleted = true;
        //        marketer.NationalIdImagePath1 = await SaveImage(userDto.NationalIdImageFiles[0]);
        //        marketer.NationalIdImagePath2 = (userDto.NationalIdImageFiles.Count > 1 && userDto.NationalIdImageFiles[1] != null && userDto.NationalIdImageFiles[1].Length > 0) ? await SaveImage(userDto.NationalIdImageFiles[1]) : null;

        //        var saveMarketerResult = await _marketerUnitOfWork.TableRepository<TbMarketer>().CreateAsync(marketer, new Guid());

        //        var parentMarketer = await _marketerUnitOfWork.TableRepository<TbMarketer>()
        //            .FindAsync(m => m.PathCode == marketer.PathCode.Remove(marketer.PathCode.Length - 2));

        //        var sponsorMarketer = await _marketerUnitOfWork.TableRepository<TbMarketer>()
        //            .FindAsync(m => m.Id == recruitmentLink.SponsorId);

        //        if (parentMarketer == null || sponsorMarketer == null)
        //        {
        //            throw new ValidationException(UserResources.UserNotFound);
        //        }

        //        //// send notifications
        //        //if (sponsorMarketer.ApplicationUserId != parentMarketer.ApplicationUserId)
        //        //{
        //        //    var sent = await _notificationService.SendNotificationAsync
        //        //   (new NotificationRequest()
        //        //   {
        //        //       Channel = NotificationChannel.SignalR,
        //        //       Type = NotificationType.NewIndirectMarketerJoined,
        //        //       Recipient = sponsorMarketer.ApplicationUserId
        //        //   });
        //        //}

        //        //var response = await _notificationService.SendNotificationAsync
        //        //(new NotificationRequest()
        //        //{
        //        //    Channel = NotificationChannel.SignalR,
        //        //    Type = NotificationType.NewDirectMarketerRegistered,
        //        //    Recipient = parentMarketer.ApplicationUserId.ToString(),
        //        //});
        //        await _marketerUnitOfWork.CommitAsync();

        //        return new OperationResult
        //        {
        //            Success = true,
        //            Message = NotifiAndAlertsResources.RegistrationSuccessful
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _marketerUnitOfWork.Rollback();
        //        throw new Exception(ex.Message, ex.InnerException);
        //    }
        //}

        //public async Task<OperationResult> IsValidLinkForMarketerRegistration(string recruitmentLinkCode)
        //{
        //    // 1- code is exist in db
        //    var codeInDb = await _marketerUnitOfWork.TableRepository<TbRecruitment>().FindAsync(c => c.Code == recruitmentLinkCode);
        //    if (codeInDb == null)
        //        return new OperationResult() { Success = false, Message = ValidationResources.CodeNotFound };

        //    // 2- the code without marketer
        //    var isWithMarketer = await _marketerUnitOfWork.TableRepository<TbMarketer>().IsExistsAsync("RecruitmentId", codeInDb.Id);
        //    if (isWithMarketer)
        //        return new OperationResult() { Success = false, Message = ValidationResources.RecruitmentLinkLimit };

        //    return new OperationResult() { Success = true };
        //}

        //public async Task<ServiceResult<bool>> GetProfileCompletionStatusAsync(string userId)
        //{
        //    try
        //    {
        //        // Find the marketer by user ID
        //        var marketer = await _marketerUnitOfWork.TableRepository<TbMarketer>()
        //            .GetAsync(m => m.ApplicationUserId == userId);

        //        if (marketer == null || !marketer.Any())
        //        {
        //            return new ServiceResult<bool>
        //            {
        //                Success = false,
        //                Message = UserResources.UserNotFound,
        //                Errors = new List<string> { UserResources.UserNotFound }
        //            };
        //        }

        //        var marketerEntity = marketer.First();

        //        return new ServiceResult<bool>
        //        {
        //            Success = true,
        //            Data = marketerEntity.IsProfileCompleted,
        //            Message = marketerEntity.IsProfileCompleted 
        //                ? "Profile is completed." 
        //                : "Profile is not completed."
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResult<bool>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Errors = new List<string> { ex.InnerException?.Message ?? ex.Message }
        //        };
        //    }
        //}

        //public async Task<ServiceResult<bool>> CompleteMarketerProfileAsync(string userId, MarketerProfileCompletionDto profileDto)
        //{
        //    try
        //    {
        //        // Find the marketer by user ID
        //        var marketerEntity = (await _marketerUnitOfWork.TableRepository<TbMarketer>()
        //            .GetAsync(m => m.ApplicationUserId == userId))
        //            .FirstOrDefault();

        //        var applicationUser = await _userManager.FindByIdAsync(userId);

        //        if (marketerEntity == null || applicationUser == null)
        //        {
        //            return new ServiceResult<bool>
        //            {
        //                Success = false,
        //                Data = false,
        //                Message = UserResources.UserNotFound,
        //                Errors = new List<string> { UserResources.UserNotFound }
        //            };
        //        }

        //        // Check if profile is already completed
        //        if (marketerEntity.IsProfileCompleted)
        //        {
        //            return new ServiceResult<bool>
        //            {
        //                Success = false,
        //                Data = marketerEntity.IsProfileCompleted,
        //                Message = "Profile is already completed.",
        //                Errors = new List<string> { "Profile is already completed." }
        //            };
        //        }

        //        await _marketerUnitOfWork.BeginTransactionAsync();

        //        // Update ApplicationUser information
                
        //        // Verify old password before allowing changes
        //        var passwordCheck = await _userManager.CheckPasswordAsync(applicationUser, profileDto.OldPassword);
        //        if (!passwordCheck)
        //        {
        //            return new ServiceResult<bool>
        //            {
        //                Success = false,
        //                Message = UserResources.Invalid_UsernamePassword_Couple,
        //                Errors = new List<string> { UserResources.Invalid_UsernamePassword_Couple }
        //            };
        //        }

        //        applicationUser.FirstName = profileDto.FirstName;
        //        applicationUser.LastName = profileDto.LastName;
        //        applicationUser.PhoneNumber = profileDto.PhoneNumber;
        //        applicationUser.PhoneCode = profileDto.PhoneCode;

        //        var updateUserResult = await _userManager.UpdateAsync(applicationUser);
        //        if (!updateUserResult.Succeeded)
        //        {
        //            var friendlyErrors = updateUserResult.Errors
        //                .Select(e => GetUserFriendlyErrorMessage(e.Code))
        //                .ToList();

        //            return new ServiceResult<bool>
        //            {
        //                Success = false,
        //                Message = NotifiAndAlertsResources.SaveFailed,
        //                Errors = friendlyErrors
        //            };
        //        }

        //        // Change password
        //        var passwordChangeResult = await _userManager.ChangePasswordAsync(applicationUser, profileDto.OldPassword, profileDto.NewPassword);
        //        if (!passwordChangeResult.Succeeded)
        //        {
        //            var friendlyErrors = passwordChangeResult.Errors
        //                .Select(e => GetUserFriendlyErrorMessage(e.Code))
        //                .ToList();

        //            return new ServiceResult<bool>
        //            {
        //                Success = false,
        //                Message = NotifiAndAlertsResources.SaveFailed,
        //                Errors = friendlyErrors
        //            };
        //        }

        //        // Update Marketer information
        //        marketerEntity.Address = profileDto.Address;
        //        marketerEntity.ZipCode = profileDto.ZipCode;
        //        marketerEntity.NationalIdNumber = profileDto.NationalIdNumber;
        //        marketerEntity.CityId = profileDto.CityId;
        //        marketerEntity.TeamId = profileDto.TeamId;
        //        marketerEntity.NationalIdImagePath1 = await SaveImage(profileDto.NationalIdImageFiles[0]);
        //        marketerEntity.NationalIdImagePath2 = (profileDto.NationalIdImageFiles.Count > 1 && 
        //            profileDto.NationalIdImageFiles[1] != null && 
        //            profileDto.NationalIdImageFiles[1].Length > 0) 
        //            ? await SaveImage(profileDto.NationalIdImageFiles[1]) 
        //            : null;
        //        marketerEntity.IsProfileCompleted = true;
        //        marketerEntity.IsProfileCompleted = true;

        //        await _marketerUnitOfWork.TableRepository<TbMarketer>().UpdateAsync(marketerEntity, Guid.Empty);
        //        await _marketerUnitOfWork.CommitAsync();

        //        return new ServiceResult<bool>
        //        {
        //            Success = true,
        //            Message = NotifiAndAlertsResources.SavedSuccessfully,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _marketerUnitOfWork.Rollback();
        //        return new ServiceResult<bool>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Errors = new List<string> { ex.InnerException?.Message ?? ex.Message }
        //        };
        //    }
        //}

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
}