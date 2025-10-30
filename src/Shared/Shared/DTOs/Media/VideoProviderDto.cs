using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Media
{
    public class VideoProviderDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "TitleArRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = null!;

        [Required(ErrorMessageResourceName = "TitleEnRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = null!;
    }
}
