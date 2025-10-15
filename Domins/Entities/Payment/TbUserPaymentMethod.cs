using Domains.Entities.Base;
using Domains.Entities.Wallet;
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
    public class TbUserPaymentMethod : BaseEntity
    {
        [ForeignKey("PaymentMethodId")]
        public TbPaymentMethod PaymentMethod { get; set; }
        public Guid PaymentMethodId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }      
        public string UserId { get; set; }

        public ICollection<TbPaymentMethodField> PaymentMethodFields { get; set; } = new HashSet<TbPaymentMethodField>();
        public virtual ICollection<TbWithdrawalRequest> WithdrawalRequests { get; set; } = new HashSet<TbWithdrawalRequest>();
    }
}
