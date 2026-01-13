using Common.Enumerations.IdentificationType;
using Common.Enumerations.VendorType;
using Resources;
using Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Vendor
{
    public class VendorRegistrationRequestDto
    {
        // User Information
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(50, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(50, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "EmailLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "EmailFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        [Compare(nameof(Password), ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(4, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneCode { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(20, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [PhoneNumber(nameof(PhoneCode), ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneNumber { get; set; } = null!;

        // Vendor Personal Information
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public IdentificationType IdentificationType { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(20, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(8)]
        public string IdentificationNumber { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [RequiredImage(nameof(IdentificationImageFront), 5, 1)]
        public string IdentificationImageFront { get; set; } = null!;

        public string? IdentificationImageBack { get; set; }

        // Business Information
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public VendorType VendorType { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string StoreName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(500, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Address { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public Guid CityId { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(9, MinimumLength = 4, ErrorMessageResourceName = "ZipCodeLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^[0-9\-]+$", ErrorMessageResourceName = "InvalidZipCode", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PostalCode { get; set; } = null!;

        public bool IsRealEstateRegistered { get; set; }
    }
}
