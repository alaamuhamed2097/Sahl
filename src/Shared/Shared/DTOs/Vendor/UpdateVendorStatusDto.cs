using Common.Enumerations.VendorStatus;

namespace Shared.DTOs.Vendor
{
    public class UpdateVendorStatusDto
    {
        public Guid VendorId { get; set; }
        public VendorStatus Status { get; set; }
    }
}
