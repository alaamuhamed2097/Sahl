using Domains.Entities.Base;
using Domains.Entities.Order;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class TbPaymentGatewayMethod : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; }

        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; }

        [Required]
        public int Type { get; set; }

        public virtual ICollection<TbOrder> Orders { get; set; } = new HashSet<TbOrder>();
    }
}

