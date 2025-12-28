using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User
{
    public class ResetPasswordWithCodeDto
    {
        /// <summary>
        /// The mobile number associated with the user account.
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Identifier { get; set; } = null!;

        /// <summary>
        /// The verification code sent to the user’s email address.
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\d{6}$", ErrorMessageResourceName = "ResetPassword_VerificationCode", ErrorMessageResourceType = typeof(ValidationResources))]
        public string VerificationCode { get; set; } = null!;

        /// <summary>
        /// The new password to set for the user account.
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        [Compare(nameof(NewPassword), ErrorMessageResourceName = "PasswordConfirmation_Mismatch", ErrorMessageResourceType = typeof(UserResources))]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; } = "";
    }
}
