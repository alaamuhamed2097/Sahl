using Domains.Entities.Location;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Shipping
{
    public class TbShippingDetail : BaseEntity
    {
        [Required]
        public Guid OfferId { get; set; }

        [Required]
        public Guid CityId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        public int ShippingMethod { get; set; }

        public int MinimumEstimatedDays { get; set; }

        public int MaximumEstimatedDays { get; set; }

        public bool IsCODSupported { get; set; }

        public int FulfillmentType { get; set; }

        [ForeignKey("OfferId")]
        public virtual TbOffer Offer { get; set; }

        [ForeignKey("CityId")]
        public virtual TbCity City { get; set; }
    }
}
