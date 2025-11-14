using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Order
{
    public class OrderDetailsDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public string ItemNameAr { get; set; }
        public string ItemNameEn { get; set; }
        [JsonIgnore]
        public string ItemName => ResourceManager.CurrentLanguage == Language.Arabic ? ItemNameAr : ItemNameEn;
        public string ItemImage { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public int UnitPVs { get; set; }
        public decimal SubTotal { get; set; }
        public Guid OrderId { get; set; }
    }
}
