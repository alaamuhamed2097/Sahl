using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemCombinationDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public bool IsDefault { get; set; } = false;
        public decimal? BasePrice { get; set; }
        //public virtual TbCombinationAttribute CombinationAttributes { get; set; }
    }
}