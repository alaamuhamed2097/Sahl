using System;
using Common.Filters;
using Shared.GeneralModels;

namespace Shared.DTOs.Merchandising.PromoCode
{
    /// <summary>
    /// Request model for admin listing of vendor promo code participation requests
    /// </summary>
    public class AdminVendorPromoCodeParticipationListRequestDto
    {
        public Guid? PromoCodeId { get; set; }
        public BaseSearchCriteriaModel Criteria { get; set; } = new();
    }
}

