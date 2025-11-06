using Resources;
using Shared.Attributes;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce
{
    public class ShippingCompanyDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessageResourceName = "NameFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Name { get; set; } = null!;

        // Contact Information
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [PhoneNumber(nameof(PhoneCode), ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneCode { get; set; } = null!;

        public string FullPhoneNumber => PhoneCode + PhoneNumber;

        // Required only on Create (custom validation)
        [RequiredImageOnCreate]
        public string? Base64Image { get; set; } // Base64 from Blazor input

        public string? LogoImagePath { get; set; }
    }
}
