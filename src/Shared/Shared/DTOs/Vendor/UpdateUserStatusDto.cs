using Common.Enumerations.User;

namespace Shared.DTOs.Vendor
{
    public class UpdateUserStatusDto
    {
        public Guid VendorId { get; set; }
        public UserStateType Status { get; set; }
    }
}
