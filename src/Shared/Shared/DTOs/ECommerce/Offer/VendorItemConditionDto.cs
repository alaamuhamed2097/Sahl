using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Offer
{
    public class VendorItemConditionDto : BaseDto
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }
        public string Name()=> ResourceManager.CurrentLanguage == Language.Arabic ? NameAr : NameEn;

        public bool IsNew { get; set; }
    }
}