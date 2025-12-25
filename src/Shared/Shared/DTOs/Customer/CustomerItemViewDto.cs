using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Customer
{
    public class CustomerItemViewDto : BaseDto
	{
		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public Guid ItemCombinationId { get; set; }
		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public Guid CustomerId { get; set; }
		public DateTime ViewedAt { get; set; }
    }
}
