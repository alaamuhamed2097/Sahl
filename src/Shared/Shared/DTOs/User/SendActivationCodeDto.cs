using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User
{
    public class SendActivationCodeDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\+\d{1,4}$", ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneCode { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [Phone(ErrorMessageResourceName = "InvalidFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneNumber { get; set; } = null!;
    }
}
