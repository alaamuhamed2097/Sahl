using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.WithdrawalMethods
{
    public class TbUserWithdrawalMethod : BaseEntity
    {
        [ForeignKey("WithdrawalMethodId")]
        public TbWithdrawalMethod WithdrawalMethod { get; set; }
        public Guid WithdrawalMethodId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ICollection<TbWithdrawalMethodField> WithdrawalMethodFields { get; set; } = new HashSet<TbWithdrawalMethodField>();
        //public virtual ICollection<TbWithdrawalRequest> WithdrawalRequests { get; set; } = new HashSet<TbWithdrawalRequest>();
    }
}
