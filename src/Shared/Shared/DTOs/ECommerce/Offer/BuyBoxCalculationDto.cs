using Shared.DTOs.Base;
using Shared.GeneralModels.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Offer
{
    public class BuyBoxCalculationDto : BaseDto
    {
        public Guid ItemId { get; set; }

        /// <summary>
        /// CRITICAL: BuyBox is calculated per combination, not just item
        /// </summary>
        public Guid ItemCombinationId { get; set; }

        /// <summary>
        /// The winning offer for this specific combination
        /// </summary>
        public Guid WinningOfferId { get; set; }

        /// <summary>
        /// Backwards-compatible individual score components (0-100 scaled using decimal(5,2))
        /// These fields are kept for existing configuration & indexes.
        /// </summary>
        public decimal PriceScore { get; set; }

        public decimal SellerRatingScore { get; set; }

        public decimal ShippingSpeedScore { get; set; }

        public decimal FBMUsageScore { get; set; }

        public decimal StockLevelScore { get; set; }

        public decimal ReturnRateScore { get; set; }

        /// <summary>
        /// Backwards-compatible total score (0-100)
        /// </summary>
        public decimal TotalScore { get; set; }

        /// <summary>
        /// New unified score with higher precision
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// When this calculation was performed
        /// </summary>
        public DateTime CalculatedAt { get; set; }

        /// <summary>
        /// How long this BuyBox is valid (cache duration)
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        public string? CalculationDetails { get; set; }

        /// <summary>
        /// Breakdown of score components for transparency (JSON)
        /// </summary>
        public string? ScoreBreakdown { get; set; }
    }
}