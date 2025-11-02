using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Resources;
using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.ECommerce.Order
{
    public class RefundResponseDto
    {
        public Guid RefundId { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal RefundAmount { get; set; }

        public RefundStatus CurrentState { get; set; } = RefundStatus.Pending;
        
        public string? AdminComments { get; set; }
    }
}
