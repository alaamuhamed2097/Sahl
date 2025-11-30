using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Fulfillment
{
    public class TbFulfillmentMethod : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; } = true;

        public int BuyBoxPriorityBoost { get; set; } = 0;

        public bool RequiresWarehouse { get; set; }

        public int DisplayOrder { get; set; }

        public ICollection<TbFulfillmentFee> FulfillmentFees { get; set; } = new HashSet<TbFulfillmentFee>();
    }
}
