using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Customer
{
	public class CustomerDto : BaseDto
	{
		public string? UserId { get; set; }  
		public string? Notes { get; set; }   
	}
}
