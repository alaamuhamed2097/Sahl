using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Setting;
using Shared.DTOs.Setting;
using Shared.DTOs.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapper
{
	public partial class MappingProfile
	{
		private void ConfigureVendorMappings()
		{
			CreateMap<TbVendor, VendorDto>().ReverseMap();
		}
	}
	
}
