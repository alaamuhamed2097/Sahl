using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.WithdrawalMethods
{
    public class TbWithdrawalMethodField : BaseEntity
    {
        public Guid FieldId { get; set; }
        public Guid UserWithdrawalMethodId { get; set; }

        [ForeignKey("FieldId")]
        public TbField Field { get; set; }

        [ForeignKey("UserWithdrawalMethodId")]
        public TbUserWithdrawalMethod UserWithdrawalMethod { get; set; }

        public string? Value { get; set; }

    }

}
