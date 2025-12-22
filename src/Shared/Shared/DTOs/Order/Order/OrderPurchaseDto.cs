using Resources;
using Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Order.Order
{
    public class OrderPurchaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 5, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [NotEmptyGuid(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public Guid PaymentGatewayMethodId { get; set; }

        public string? CouponCode { get; set; }

        public string? DirectSaleLinkCode { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(1, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public IEnumerable<OrderItemDto> Items { get; set; }

        [JsonIgnore]
        public decimal TaxAmount { get; set; }

        [JsonIgnore]
        public decimal ShippingAmount { get; set; }
    }
}
