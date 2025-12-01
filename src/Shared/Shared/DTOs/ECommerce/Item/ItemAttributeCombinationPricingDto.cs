//using Resources;
//using Shared.DTOs.Base;
//using System.ComponentModel.DataAnnotations;

//namespace Shared.DTOs.ECommerce.Item
//{
//    public class ItemAttributeCombinationPricingDto : BaseDto
//    {
//        public Guid ItemId { get; set; }

//        public string AttributeIds { get; set; } = null!;

//        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
//        [Range(0.01, double.MaxValue, ErrorMessageResourceName = "PriceMustBeGreaterThanZero", ErrorMessageResourceType = typeof(ValidationResources))]
//        public decimal Price { get; set; }

//        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
//        [Range(0.01, double.MaxValue, ErrorMessageResourceName = "PriceMustBeGreaterThanZero", ErrorMessageResourceType = typeof(ValidationResources))]
//        public decimal SalesPrice { get; set; }

//        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
//        [Range(0, int.MaxValue, ErrorMessageResourceName = "QuantityCannotBeNegative", ErrorMessageResourceType = typeof(ValidationResources))]
//        public int Quantity { get; set; }

//        /// <summary>
//        /// Indicates if this is the default pricing combination for the item.
//        /// Only one combination per item can be marked as default.
//        /// </summary>
//        public bool IsDefault { get; set; } = false;
//    }
//}
