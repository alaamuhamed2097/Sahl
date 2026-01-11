using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.DTOs.HomeSlider
{
	public class HomePageSliderDto : BaseDto
	{
		[StringLength(200, MinimumLength = 0, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
		public string? TitleAr { get; set; }

		[StringLength(200, MinimumLength = 0, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
		public string? TitleEn { get; set; }

		[JsonIgnore]
		public string? Title => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[RequiredImage(nameof(ImageUrl), 5, 1)]
		public string ImageUrl { get; set; } = null!;

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public int DisplayOrder { get; set; }

		//[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]

		
		//[Display(Name = "Start Date")]
		//public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

		//[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		
		//[Display(Name = "End Date")]
		//public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddDays(30);
	}
}
