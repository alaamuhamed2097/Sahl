using Domains.Entities.Inventory;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Warehouse
{
    public class TbWarehouse : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string TitleEn { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(4)]
        public string? PhoneCode { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<TbMovitemsdetail>? MovitemsDetails { get; set; }
    }
}
