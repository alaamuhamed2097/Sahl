using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Setting
{
    /// <summary>
    /// DTO for general application settings
    /// </summary>
    public class GeneralSettingsDto : BaseSeoDto
    {
        public Guid Id { get; set; }

        // Contact Information
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [Phone(ErrorMessageResourceName = "InvalidPhone", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(25, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(500, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Address { get; set; } = string.Empty;

        // Social Media Links
        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? FacebookUrl { get; set; }

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? InstagramUrl { get; set; }

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? TwitterUrl { get; set; }

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? LinkedInUrl { get; set; }

        [Phone(ErrorMessageResourceName = "InvalidPhone", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(25, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? WhatsAppNumber { get; set; }
    }
}