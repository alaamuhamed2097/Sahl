using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User.Admin
{
    public class AdminRegistrationDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessageResourceName = "NameFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessageResourceName = "NameFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "EmailLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "EmailFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 5, ErrorMessageResourceName = "UserNameLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        [Compare(nameof(Password), ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; } = "";
    }
}
