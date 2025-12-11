using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferConditionDto : BaseDto
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public bool IsNew { get; set; }
    }
}