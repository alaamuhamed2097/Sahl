using System;
using System.Collections.Generic;

namespace Shared.DTOs.Campaign
{
    // Campaign DTOs
    public class CampaignCreateDto
    {
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int CampaignType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinimumDiscountPercentage { get; set; }
        public decimal? BudgetLimit { get; set; }
        public string BannerImagePath { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CampaignUpdateDto : CampaignCreateDto
    {
        public Guid Id { get; set; }
    }

    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int CampaignType { get; set; }
        public string CampaignTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinimumDiscountPercentage { get; set; }
        public decimal? BudgetLimit { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalProducts { get; set; }
        public int TotalVendors { get; set; }
        public string BannerImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    // Flash Sale DTOs
    public class FlashSaleCreateDto
    {
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInHours { get; set; }
        public decimal MinimumDiscountPercentage { get; set; }
        public string BannerImagePath { get; set; }
        public bool ShowCountdownTimer { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }

    public class FlashSaleDto
    {
        public Guid Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInHours { get; set; }
        public decimal MinimumDiscountPercentage { get; set; }
        public string BannerImagePath { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalSales { get; set; }
        public bool ShowCountdownTimer { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    // Campaign Product DTOs
    public class CampaignProductCreateDto
    {
        public Guid CampaignId { get; set; }
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal CampaignPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? StockQuantity { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CampaignProductDto
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public string CampaignTitle { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal CampaignPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? StockQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedByUserName { get; set; }
    }

    // Search/Filter DTOs
    public class CampaignSearchRequest
    {
        public int? CampaignType { get; set; }
        public int? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class CampaignStatisticsDto
    {
        public int TotalCampaigns { get; set; }
        public int ActiveCampaigns { get; set; }
        public int UpcomingCampaigns { get; set; }
        public int ExpiredCampaigns { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProductsSold { get; set; }
        public decimal AverageDiscountPercentage { get; set; }
    }
}
