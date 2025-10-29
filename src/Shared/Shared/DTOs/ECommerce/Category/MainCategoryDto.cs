using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Category
{
    public class MainCategoryDto : BaseDto
    {
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;

        public string Icon { get; set; } = null!;
        public bool IsMainCategory { get; set; }
        public bool PriceRequired { get; set; }
    }
}
