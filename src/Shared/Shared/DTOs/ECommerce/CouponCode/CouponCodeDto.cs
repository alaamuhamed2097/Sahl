using Common.Enumerations;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.CouponCode
{
    public class CouponCodeDto : BaseDto
    {
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldLength100")]
        public string TitleAR { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldLength100")]
        public string TitleEN { get; set; } = null!;

        public string Title => ResourceManager.CurrentLanguage == Language.English ? TitleEN : TitleAR;

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldLength50")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        public DateTime StartDate
        {
            get => StartDateUTC.ToLocalTime();
            set => StartDateUTC = value.ToUniversalTime();
        }

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        public DateTime EndDate
        {
            get => EndDateUTC.ToLocalTime();
            set => EndDateUTC = value.ToUniversalTime();
        }

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        public CouponCodeType CouponCodeType { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRange")]
        public decimal Value { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRange")]
        public int? UsageLimit { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRequired")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "FieldRange")]
        public int UsageCount { get; set; } = 0;

        public bool IsActive { get; set; }

        [JsonIgnore]
        public DateTime StartDateUTC { get; set; }

        [JsonIgnore]
        public DateTime EndDateUTC { get; set; }
    }
}
