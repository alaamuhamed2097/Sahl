using Domains.Entities.Base;
using Domains.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities
{
    public class TbShippingCompany : BaseEntity
    {
        public string LogoImagePath { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        
        public virtual ICollection<TbOrder> Orders { get; set; } 
    }
}
