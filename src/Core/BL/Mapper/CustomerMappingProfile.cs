using Domains.Entities.Setting;
using Shared.DTOs.Customer;
using Shared.DTOs.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Entities.ECommerceSystem.Customer;

namespace BL.Mapper
{
	public partial class MappingProfile
	{
		private void ConfigureCustomerMappings()
		{
			CreateMap<TbCustomer, CustomerDto>().ReverseMap();
		}
	}
	
}
