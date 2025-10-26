using Domains.Entities.Base;
using Domains.Entities.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.Item
{
    public class TbField : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        public int FieldType { get; set; }

        public Guid PaymentMethodId { get; set; }

        public TbPaymentMethod PaymentMethod { get; set; }

        public ICollection<TbPaymentMethodField> PaymentMethodField { get; set; }

        public TbField()
        {
            PaymentMethodField = new List<TbPaymentMethodField>();
        }

    }
}

