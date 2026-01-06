using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Wallet.Customer
{
    public class ChangeWalletPasswordDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessageResourceName = "MinLengthError", ErrorMessageResourceType = typeof(ValidationResources))]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "NewPassAndConfirmPassNotMatch", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
