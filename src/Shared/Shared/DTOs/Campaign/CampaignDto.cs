using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Campaign
{
    /// <summary>
    /// Campaign DTO for read operations
    /// </summary>
    public class CampaignDto
    {
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Arabic name is required")]
		[StringLength(200, ErrorMessage = "Arabic name cannot exceed 200 characters")]
		public string NameAr { get; set; } = string.Empty;

		[Required(ErrorMessage = "English name is required")]
		[StringLength(200, ErrorMessage = "English name cannot exceed 200 characters")]
		public string NameEn { get; set; } = string.Empty;

		[Required(ErrorMessage = "Start date is required")]
		public DateTime StartDate { get; set; }

		[Required(ErrorMessage = "End date is required")]
		public DateTime EndDate { get; set; }

		public bool IsActive { get; set; }

		// Flash Sale properties
		public bool IsFlashSale { get; set; }

		public DateTime? FlashSaleEndTime { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Max quantity per user must be greater than zero")]
		public int? MaxQuantityPerUser { get; set; }

		// Badge
		[StringLength(100, ErrorMessage = "Badge Arabic text cannot exceed 100 characters")]
		public string? BadgeTextAr { get; set; }

		[StringLength(100, ErrorMessage = "Badge English text cannot exceed 100 characters")]
		public string? BadgeTextEn { get; set; }

		[StringLength(20, ErrorMessage = "Badge color cannot exceed 20 characters")]
		public string? BadgeColor { get; set; }

		// Counts (Read-only usually)
		[Range(0, int.MaxValue)]
		public int TotalItems { get; set; }

		[Range(0, int.MaxValue)]
		public int TotalSold { get; set; }

		public DateTime CreatedDateUtc { get; set; }
	}
}