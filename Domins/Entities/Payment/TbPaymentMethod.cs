using Domains.Entities.Base;
using Domains.Entities.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.Payment
{
    public class TbPaymentMethod : BaseEntity
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
        
        public TbPaymentMethod()
        {
            Fields = new List<TbField>();       
        }
    }
}
