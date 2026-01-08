using Common.Enumerations.FieldType;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.WithdrawalMethods
{
    public class TbField : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        public FieldType FieldType { get; set; }

        public Guid WithdrawalMethodId { get; set; }

        public TbWithdrawalMethod WithdrawalMethod { get; set; }

        public ICollection<TbWithdrawalMethodField> WithdrawalMethodField { get; set; }

        public TbField()
        {
            WithdrawalMethodField = new List<TbWithdrawalMethodField>();
        }

    }
}

