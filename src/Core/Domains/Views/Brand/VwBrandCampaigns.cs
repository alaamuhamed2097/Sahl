namespace Domains.Views.Brand
{
	public class VwBrandCampaigns
	{
		// Brand
		public Guid BrandId { get; set; }
		public string BrandNameAr { get; set; } = string.Empty;
		public string BrandNameEn { get; set; } = string.Empty;

		// Campaign
		public Guid CampaignId { get; set; }
		public string CampaignTitleAr { get; set; } = string.Empty;
		public string CampaignTitleEn { get; set; } = string.Empty;
		public DateTime CampaignStartDate { get; set; }
		public DateTime CampaignEndDate { get; set; }
		public bool CampaignIsActive { get; set; }

		public int CampaignType { get; set; }
		public bool IsFeatured { get; set; }
		public int DisplayOrder { get; set; }

		// Campaign Items
		public int TotalCampaignItems { get; set; }
		public int TotalSoldInCampaign { get; set; }
		public decimal AverageCampaignPrice { get; set; }

		// Regular Prices
		public decimal AverageRegularPrice { get; set; }
		public decimal AverageDiscountPercentage { get; set; }

		// Campaign Status
		public string CampaignStatus { get; set; } = string.Empty;
	}
}
