using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Warehouse
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }


        [StringLength(500, ErrorMessage = "AddressMaxLength")]
        public string? Address { get; set; }
		public bool IsDefaultPlatformWarehouse { get; set; }
        public Guid? VendorId { get; set; }

		[EmailAddress(ErrorMessage = "InvalidEmail")]
		[StringLength(100)]
		public string? Email { get; set; } 

		public bool IsActive { get; set; } = true;

    }
}
