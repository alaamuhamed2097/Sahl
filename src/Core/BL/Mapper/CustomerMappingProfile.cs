using Domains.Entities.Setting;
using Domins.Entities.Customer;
using Shared.DTOs.Customer;
using Shared.DTOs.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
