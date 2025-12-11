using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Offer
{
    public class ShippingDetailDto : BaseDto
    {
        public Guid OfferId { get; set; }

        public Guid CityId { get; set; }

        public decimal ShippingCost { get; set; }

        public int ShippingMethod { get; set; }

        public int MinimumEstimatedDays { get; set; }

        public int MaximumEstimatedDays { get; set; }

        public bool IsCODSupported { get; set; }

        public int FulfillmentType { get; set; }
    }
}