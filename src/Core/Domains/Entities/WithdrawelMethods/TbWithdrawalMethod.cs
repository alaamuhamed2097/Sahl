using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.WithdrawalMethods
{
    public class TbWithdrawalMethod : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        [Required]
        [MaxLength(400)]
        public string ImagePath { get; set; } = null!;

        public virtual ICollection<TbField> Fields { get; set; } = null!;
        public virtual ICollection<TbUserWithdrawalMethod> UserWithdrawalMethods { get; set; } = null!;

        public TbWithdrawalMethod()
        {
            Fields = new List<TbField>();
            UserWithdrawalMethods = new List<TbUserWithdrawalMethod>();
        }
    }
}
