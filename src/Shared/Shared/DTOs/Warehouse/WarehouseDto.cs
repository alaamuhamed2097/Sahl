using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Warehouse
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "TitleArRequired")]
        [StringLength(200, ErrorMessage = "TitleArMaxLength")]
        public string TitleAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "TitleEnRequired")]
        [StringLength(200, ErrorMessage = "TitleEnMaxLength")]
        public string TitleEn { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "AddressMaxLength")]
        public string? Address { get; set; }

        [StringLength(20, ErrorMessage = "PhoneNumberMaxLength")]
        public string? PhoneNumber { get; set; }

        [StringLength(4, ErrorMessage = "PhoneCodeMaxLength")]
        public string? PhoneCode { get; set; }

        public bool IsActive { get; set; } = true;

        public string Title => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") 
            ? TitleAr 
            : TitleEn;
    }
}
