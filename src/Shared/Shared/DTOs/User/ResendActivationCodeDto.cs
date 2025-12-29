using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User
{
    public class ResendActivationCodeDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(15, MinimumLength = 10, ErrorMessageResourceName = "MobileLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Identifire { get; set; } = null!;
    }
}
