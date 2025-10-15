using Domains.Entities.Base;
using Domains.Entities.Item;
using Domains.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.Payment
{
    public class TbPaymentMethodField : BaseEntity
    {
        public Guid FieldId { get; set; }
        public Guid UserPaymentMethodId { get; set; }

        [ForeignKey("FieldId")]
        public TbField Field { get; set; }

        [ForeignKey("UserPaymentMethodId")]
        public TbUserPaymentMethod UserPaymentMethod { get; set; }

        public string? Value { get; set; }       

    }

}
