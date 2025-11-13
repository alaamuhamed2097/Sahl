using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Currency
{
    public class TbCurrency : BaseEntity
    {
        [Required]
        [StringLength(3)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        public decimal ExchangeRate { get; set; } = 1m;

        public bool IsBaseCurrency { get; set; }
        public bool IsActive { get; set; } = true;
        public string CountryCode { get; set; } = string.Empty;
    }
}