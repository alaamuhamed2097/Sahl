using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Merchandising.CouponCode
{
    /// <summary>
    /// DTO for updating an existing coupon code
    /// </summary>
    public class UpdateCouponCodeDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "Arabic title cannot exceed 200 characters")]
        public string? TitleAr { get; set; }

        [StringLength(200, ErrorMessage = "English title cannot exceed 200 characters")]
        public string? TitleEn { get; set; }

        [StringLength(500, ErrorMessage = "Arabic description cannot exceed 500 characters")]
        public string? DescriptionAr { get; set; }

        [StringLength(500, ErrorMessage = "English description cannot exceed 500 characters")]
        public string? DescriptionEn { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1")]
        public int? UsageLimit { get; set; }

        public bool? IsActive { get; set; }
    }

}
