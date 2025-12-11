using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferStatusHistoryDto : BaseDto
    {
        public Guid OfferId { get; set; }

        public decimal OldStatus { get; set; }

        public decimal NewStatus { get; set; }

        public string? ChangeNote { get; set; }
    }
}