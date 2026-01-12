using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Setting
{
    /// <summary>
    /// Represents global settings or feature flags for the development environment or application configuration.
    /// </summary>
    public class TbDevelopmentSettings  : BaseEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether the system operates in Multi-Vendor mode.
        /// If true, multiple vendors can sell products. If false, it operates as a single-store system.
        /// </summary>
        [Required]
        public bool IsMultiVendorSystem { get; set; } = true; // Default value
    }
}
