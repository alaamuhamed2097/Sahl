using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Setting
{
    /// <summary>
    /// General application settings (single row table)
    /// </summary>
    public class TbGeneralSettings : BaseSeo
    {
        // Contact Information
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(25)]
        public string Phone { get; set; } = string.Empty; // Format: +201234567890

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        // Social Media Links
        [Url]
        [StringLength(200)]
        public string? FacebookUrl { get; set; }

        [Url]
        [StringLength(200)]
        public string? InstagramUrl { get; set; }

        [Url]
        [StringLength(200)]
        public string? TwitterUrl { get; set; }

        [Url]
        [StringLength(200)]
        public string? LinkedInUrl { get; set; }

        [Phone]
        [StringLength(25)]
        public string? WhatsAppNumber { get; set; } // Format: +201234567890
    }
}
