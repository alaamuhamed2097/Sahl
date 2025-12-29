using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domains.Entities.HomeSlider
{
	public class TbHomePageSlider : BaseEntity
	{
		[MaxLength(200)]
		public string? TitleAr { get; set; }

		[MaxLength(200)]
		public string? TitleEn { get; set; }
		[Required]
		public string ImageUrl { get; set; } = null!;
		[Required]
		public int DisplayOrder { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
