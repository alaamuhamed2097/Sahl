using Common.Enumerations.VendorType;

namespace Domains.Entities.ECommerceSystem.Vendor
{
    public class TbVendor : BaseEntity
    {
        public string? UserId { get; set; }
        public VendorType VendorType { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? VendorCode { get; set; }

        //
        public string? PostalCode { get; set; }

        public string? Address { get; set; }


        //
        public string? CommercialRegister { get; set; }
        public bool VATRegistered { get; set; }
        public string? TaxNumber { get; set; }
        //
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public byte? Rating { get; set; }
    }
}
