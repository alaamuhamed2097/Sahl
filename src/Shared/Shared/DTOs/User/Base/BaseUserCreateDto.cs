using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User.Base
{
    public class BaseUserCreateDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "Invalid_Email", ErrorMessageResourceType = typeof(UserResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 5, ErrorMessageResourceName = "UserNameLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordRange", ErrorMessageResourceType = typeof(UserResources))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessageResourceName = "WeakPassword", ErrorMessageResourceType = typeof(UserResources))]
        public string Password { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\p{L}+$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\p{L}+$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string LastName { get; set; } = null!;
    }
}
