using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferPriceHistoryDto :BaseDto
    {
        public Guid OfferCombinationPricingId { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public string? ChangeNote { get; set; }
    }
}