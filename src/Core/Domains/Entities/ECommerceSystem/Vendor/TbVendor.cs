using Common.Enumerations.VendorType;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem.Vendor
{
    public class TbVendor : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Discription { get; set; }
		
        public string? VendorCode { get; set; }

        // Contact Information
        public string? PostalCode { get; set; }

        public string? Address { get; set; }


        // Contact Information
        public string? CommercialRegister { get; set; }
        public bool VATRegistered { get; set; }
        public string? TaxNumber { get; set; }
        //
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal?AverageRating { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
		public virtual ICollection<TbItemReview> ItemReviews { get; set; } = new List<TbItemReview>();
		public virtual ICollection<TbVendorReview> VendorReviews { get; set; } = new List<TbVendorReview>();
        public virtual ICollection<TbRefund> Refunds { get; set; } = new List<TbRefund>();
    }
}
