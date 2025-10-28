using Common.Enumerations.User;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User.Admin
{
    public class AdminProfileDto
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\p{L}+$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\p{L}+$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 5, ErrorMessageResourceName = "UserNameLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public UserStateType UserState { get; set; }

    }
}
