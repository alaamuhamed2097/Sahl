using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User
{
    public class ForgetPasswordRequestDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Identifier { get; set; } = null!;
    }
}
