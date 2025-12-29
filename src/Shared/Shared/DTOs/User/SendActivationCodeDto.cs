using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User
{
    public class SendActivationCodeDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Identifire { get; set; } = null!;
    }
}
