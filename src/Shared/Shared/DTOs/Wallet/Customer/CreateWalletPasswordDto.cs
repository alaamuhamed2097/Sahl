using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Wallet.Customer
{
    public class CreateWalletPasswordDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessageResourceName = "MinLengthError", ErrorMessageResourceType = typeof(ValidationResources))]
        public string NewPassword { get; set; } = null!;
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "NewPassAndConfirmPassNotMatch", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
