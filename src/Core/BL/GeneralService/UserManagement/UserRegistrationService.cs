using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.IMapper;
using BL.Utils;
using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Customer;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Vendor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resources;
using Shared.DTOs.User;
using Shared.DTOs.User.Admin;
using Shared.DTOs.User.Customer;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.ResultModels;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace BL.GeneralService.UserManagement;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileUploadService _fileUploadService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IBaseMapper _mapper;
    private readonly Serilog.ILogger _logger;
    private readonly IVendorManagementRepository _vendorRepository;
    private readonly ICustomerRepository _customerRepository;

	public UserRegistrationService(UserManager<ApplicationUser> userManager,
		IFileUploadService fileUploadService,
		IUserAuthenticationService userAuthenticationService,
		IImageProcessingService imageProcessingService,
		IBaseMapper mapper,
		Serilog.ILogger logger,
		IVendorManagementRepository vendorRepository,
		ICustomerRepository customerRepository)
	{
		_userManager = userManager;
		_fileUploadService = fileUploadService;
		_userAuthenticationService = userAuthenticationService;
		_imageProcessingService = imageProcessingService;
		_mapper = mapper;
		_logger = logger;
		_vendorRepository = vendorRepository;
		_customerRepository = customerRepository;
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

        try
        {
            var result = await _userManager.CreateAsync(applicationUser, userDto.Password);

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

    public async Task<ServiceResult<CustomerRegistrationResponseDto>> RegisterCustomerAsync(
        CustomerRegistrationDto userDto, string clientType)
    {
        try
        {
            // Validate phone uniqueness
            var normalizedPhone = PhoneNormalizationHelper.CreateNormalizedPhone(
                userDto.PhoneCode, userDto.PhoneNumber);

            var existingUserByPhone = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedPhone == normalizedPhone);

            if (existingUserByPhone != null)
            {
                return new ServiceResult<CustomerRegistrationResponseDto>
                {
                    Success = false,
                    Message = "This phone number is already registered",
                    Errors = new List<string> { "Phone number already in use" }
                };
            }

            // Check email uniqueness only if email is provided
            if (!string.IsNullOrWhiteSpace(userDto.Email))
            {
                var existingUserByEmail = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == userDto.Email);

                if (existingUserByEmail != null)
                {
                    return new ServiceResult<CustomerRegistrationResponseDto>
                    {
                        Success = false,
                        Message = string.Format(UserResources.Email_Duplicate, userDto.Email),
                        Errors = new List<string> { string.Format(UserResources.Email_Duplicate, userDto.Email) }
                    };
                }
            }

            // Auto-generate username if not provided
            var username = userDto.PhoneNumber;
            if (string.IsNullOrWhiteSpace(username))
            {
                username = await UsernameGenerationHelper.GenerateUniqueUsernameAsync(
                    userDto.FirstName, _userManager);
            }
            else
            {
                // Check if provided username already exists
                var existingUserByUsername = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.UserName == username);

                if (existingUserByUsername != null)
                {
                    return new ServiceResult<CustomerRegistrationResponseDto>
                    {
                        Success = false,
                        Message = string.Format(UserResources.UserName_Duplicate, username),
                        Errors = new List<string> { string.Format(UserResources.UserName_Duplicate, username) }
                    };
                }
            }

            // Create ApplicationUser
            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = userDto.Email,
                UserName = username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                PhoneCode = userDto.PhoneCode,
                NormalizedPhone = normalizedPhone,
                ProfileImagePath = "uploads/Images/ProfileImages/Customers/default.png",
                CreatedBy = Guid.Empty,
                CreatedDateUtc = DateTime.UtcNow,
                UserState = UserStateType.Active,
                EmailConfirmed = !string.IsNullOrWhiteSpace(userDto.Email)
            };

            // Create the user with password
            var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
            if (!result.Succeeded)
            {
                var friendlyErrors = result.Errors
                    .Select(e => GetUserFriendlyErrorMessage(e.Code))
                    .ToList();

                return new ServiceResult<CustomerRegistrationResponseDto>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.RegistrationFailed,
                    Errors = friendlyErrors
                };
            }

            // Add user to Customer role
            var addedToRole = (await _userManager.AddToRoleAsync(applicationUser, "Customer")).Succeeded;
            if (!addedToRole)
            {
                _logger.Error("Failed to add user {UserId} to Customer role", applicationUser.Id);
                throw new InvalidOperationException("Failed to add user to the specified role.");
            }
			var customer = new TbCustomer
			{
				Id = Guid.NewGuid(),
				UserId = applicationUser.Id,
				
			};

			    await _customerRepository.CreateAsync(customer);
			
			// Sign in the user automatically using email or username
			var signInIdentifier = !string.IsNullOrWhiteSpace(userDto.Email)
                ? userDto.Email
                : username;

            var signInResult = await _userAuthenticationService.EmailOrPhoneNumberSignInAsync(
                signInIdentifier, userDto.Password, clientType);

            if (!signInResult.Success)
            {
                return new ServiceResult<CustomerRegistrationResponseDto>
                {
                    Success = false,
                    Message = "Registration successful but automatic login failed"
                };
            }

            return new ServiceResult<CustomerRegistrationResponseDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.RegistrationSuccessful,
                Data = new CustomerRegistrationResponseDto
                {
                    UserId = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Email = applicationUser.Email,
                    UserName = applicationUser.UserName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    PhoneCode = applicationUser.PhoneCode,
                    ProfileImagePath = applicationUser.ProfileImagePath,
                    Token = signInResult.Token,
                    RefreshToken = signInResult.RefreshToken,
                    RegisteredDate = DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error registering customer");
            return new ServiceResult<CustomerRegistrationResponseDto>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrong,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ServiceResult<VendorRegistrationResponseDto>> RegisterVendorAsync(
        VendorRegistrationRequestDto request, string clientType)
    {
        try
        {
            // Validate phone uniqueness
            var normalizedPhone = PhoneNormalizationHelper.CreateNormalizedPhone(
                request.PhoneCode, request.PhoneNumber);

            var existingUserByPhone = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedPhone == normalizedPhone);

            if (existingUserByPhone != null)
            {
                return new ServiceResult<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = "This phone number is already registered",
                    Errors = new List<string> { "Phone number already in use" }
                };
            }

            // Check email uniqueness if email is provided
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var existingUserByEmail = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUserByEmail != null)
                {
                    return new ServiceResult<VendorRegistrationResponseDto>
                    {
                        Success = false,
                        Message = string.Format(UserResources.Email_Duplicate, request.Email),
                        Errors = new List<string> { string.Format(UserResources.Email_Duplicate, request.Email) }
                    };
                }
            }

            // Check BirthDate is before Last Now 
            if (request.BirthDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return new ServiceResult<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = string.Format(UserResources.InValidDate, request.BirthDate),
                    Errors = new List<string> { string.Format(UserResources.InValidDate, request.BirthDate) }
                };
            }

            // Auto-generate username from phone number
            var username = request.Email;
            var existingUserByUsername = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (existingUserByUsername != null)
            {
                return new ServiceResult<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = string.Format(UserResources.UserName_Duplicate, username),
                    Errors = new List<string> { string.Format(UserResources.UserName_Duplicate, username) }
                };
            }

            // Create ApplicationUser
            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                PhoneCode = request.PhoneCode,
                NormalizedPhone = normalizedPhone,
                ProfileImagePath = "uploads/Images/ProfileImages/Vendors/default.png",
                CreatedBy = Guid.Empty,
                CreatedDateUtc = DateTime.UtcNow,
                UserState = UserStateType.Active,
                EmailConfirmed = !string.IsNullOrWhiteSpace(request.Email),
                PhoneNumberConfirmed = true
            };

            // Create Vendor Profile using AutoMapper
            var vendor = _mapper.MapModel<VendorRegistrationRequestDto, TbVendor>(request);

            // Handle Image Uploads
            vendor.IdentificationImageFrontPath = await SaveImage(request.IdentificationImageFront);
            vendor.IdentificationImageBackPath = await SaveImage(request.IdentificationImageBack);

            vendor.Id = Guid.NewGuid();
            vendor.Status = VendorStatus.Pending;
            vendor.CreatedDateUtc = DateTime.UtcNow;
            // UserId and CreatedBy will be properly linked in the repository method

            // Call Repository Method to execute in transaction
            var registrationResult = await _vendorRepository.RegisterVendorWithUserAsync(applicationUser, request.Password, vendor);

            if (!registrationResult.Success)
            {
                var friendlyErrors = registrationResult.Errors
                   .Select(e => GetUserFriendlyErrorMessage(e))
                   .ToList();

                return new ServiceResult<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.RegistrationFailed,
                    Errors = friendlyErrors
                };
            }

            // Sign in the user automatically
            var signInIdentifier = !string.IsNullOrWhiteSpace(request.Email)
                ? request.Email
                : username;

            var signInResult = await _userAuthenticationService.VendorSignInAsync(
                new EmailLoginDto
                {
                    Email = signInIdentifier,
                    Password = request.Password
                }, clientType);

            if (!signInResult.Success)
            {
                return new ServiceResult<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = "Registration successful but automatic login failed"
                };
            }

            return new ServiceResult<VendorRegistrationResponseDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.RegistrationSuccessful,
                Data = new VendorRegistrationResponseDto
                {
                    UserId = applicationUser.Id,
                    VendorId = vendor.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Email = applicationUser.Email,
                    UserName = applicationUser.UserName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    PhoneCode = applicationUser.PhoneCode,
                    ProfileImagePath = applicationUser.ProfileImagePath,
                    Token = signInResult.Token,
                    RefreshToken = signInResult.RefreshToken,
                    RegisteredDate = DateTime.UtcNow,
                    StoreName = request.StoreName
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error registering vendor");
            return new ServiceResult<VendorRegistrationResponseDto>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrong,
                Errors = new List<string> { ex.Message }
            };
        }
    }
	public async Task<ServiceResult<CustomerUpdateByAdminDto>> UpdateCustomerByAdminAsync(
	CustomerUpdateByAdminDto updateDto, Guid adminId)
	{
		try
		{
			// Get the customer user
			var user = await _userManager.FindByIdAsync(updateDto.UserId);
			if (user == null)
			{
				return new ServiceResult<CustomerUpdateByAdminDto>
				{
					Success = false,
					Message = UserResources.UserNotFound,
					Errors = new List<string> { "Customer not found" }
				};
			}

			// Verify user is actually a customer
			var isCustomer = await _userManager.IsInRoleAsync(user, "Customer");
			if (!isCustomer)
			{
				return new ServiceResult<CustomerUpdateByAdminDto>
				{
					Success = false,
					Message = "User is not a customer",
					Errors = new List<string> { "Invalid user role" }
				};
			}

			// Update names if provided
			if (!string.IsNullOrWhiteSpace(updateDto.FirstName))
			{
				user.FirstName = updateDto.FirstName;
			}

			if (!string.IsNullOrWhiteSpace(updateDto.LastName))
			{
				user.LastName = updateDto.LastName;
			}

			// Update email if provided
			if (!string.IsNullOrWhiteSpace(updateDto.Email) && updateDto.Email != user.Email)
			{
				var existingUserByEmail = await _userManager.Users
					.FirstOrDefaultAsync(u => u.Email == updateDto.Email && u.Id != user.Id);

				if (existingUserByEmail != null)
				{
					return new ServiceResult<CustomerUpdateByAdminDto>
					{
						Success = false,
						Message = string.Format(UserResources.Email_Duplicate, updateDto.Email),
						Errors = new List<string> { string.Format(UserResources.Email_Duplicate, updateDto.Email) }
					};
				}

				user.Email = updateDto.Email;
				user.UserName = updateDto.Email; // Update username to match email
			}

			// Track who modified the user
			user.UpdatedBy = adminId;
			user.UpdatedDateUtc = DateTime.UtcNow;

			// Update user in database
			var updateResult = await _userManager.UpdateAsync(user);
			if (!updateResult.Succeeded)
			{
				var friendlyErrors = updateResult.Errors
					.Select(e => GetUserFriendlyErrorMessage(e.Code))
					.ToList();

				return new ServiceResult<CustomerUpdateByAdminDto>
				{
					Success = false,
					Message = NotifiAndAlertsResources.SaveFailed,
					Errors = friendlyErrors
				};
			}

			// Update password if provided
			if (!string.IsNullOrWhiteSpace(updateDto.NewPassword))
			{
				// Remove old password
				await _userManager.RemovePasswordAsync(user);

				// Add new password
				var passwordResult = await _userManager.AddPasswordAsync(user, updateDto.NewPassword);
				if (!passwordResult.Succeeded)
				{
					var friendlyErrors = passwordResult.Errors
						.Select(e => GetUserFriendlyErrorMessage(e.Code))
						.ToList();

					return new ServiceResult<CustomerUpdateByAdminDto>
					{
						Success = false,
						Message = "User updated but password change failed",
						Errors = friendlyErrors
					};
				}
			}

			_logger.Information("Customer {UserId} updated by admin {AdminId}", user.Id, adminId);

			return new ServiceResult<CustomerUpdateByAdminDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.SavedSuccessfully,
				Data = new CustomerUpdateByAdminDto
				{
					UserId = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
				}
			};
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error updating customer by admin");
			return new ServiceResult<CustomerUpdateByAdminDto>
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