using Resources;
using Shared.Contracts;
using Shared.DTOs.Base;
using Shared.DTOs.Currency;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Shared.DTOs.Setting
{
    public class SettingDto : BaseSeoDto, ICurrencyConvertible
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(4, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneCode { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(500, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Address { get; set; } = string.Empty;

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? FacebookUrl { get; set; }

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? InstagramUrl { get; set; }

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? TwitterUrl { get; set; }

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? LinkedInUrl { get; set; }


        [StringLength(20, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? WhatsAppNumber { get; set; }

        [StringLength(4, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? WhatsAppCode { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal Level1Percentage { get; set; } = 0;

        [Range(0, 100, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal Level2Percentage { get; set; } = 0;


        public string? MainBannerPath { get; set; }

        public string? Base64Image { get; set; }

        public bool IsBannerDeleted { get; set; } = false;

        [Range(0, double.MaxValue)]
        public decimal WithdrawalLimit { get; set; } = 1000m;

        [Range(0, double.MaxValue)]
        public decimal WithdrawalFeePersentage { get; set; } = 1m;

        [Range(0, double.MaxValue, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal ShippingAmount { get; set; } = 0m;

        [Range(0, 100, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal OrderTaxPercentage { get; set; } = 14;

        [Range(0, double.MaxValue, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal OrderExtraCost { get; set; } = 0m;

        // Currency information (populated after conversion)
        public string? CurrencyCode { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? FormattedShippingAmount { get; set; }
        public string? FormattedWithdrawalLimit { get; set; }
        public string? FormattedOrderExtraCost { get; set; }

        public string GetId() => Id.ToString();

        public decimal GetPrice() => ShippingAmount;

        public async Task ApplyCurrencyConversionAsync(CurrencyConversionDto conversion, string toCurrency)
        {
            var rate = conversion.ExchangeRate;

            // Convert monetary values
            ShippingAmount *= rate;
            WithdrawalLimit *= rate;
            OrderExtraCost *= rate;

            // Format
            FormattedShippingAmount = conversion.Culture != null ? ShippingAmount.ToString("C", conversion.Culture) : conversion.FormattedConvertedAmount;
            FormattedWithdrawalLimit = conversion.Culture != null ? WithdrawalLimit.ToString("C", conversion.Culture) : null;
            FormattedOrderExtraCost = conversion.Culture != null ? OrderExtraCost.ToString("C", conversion.Culture) : null;
            await Task.CompletedTask;
        }

        public void SetCurrencyInfo(string currencyCode, CultureInfo culture)
        {
            CurrencyCode = currencyCode;
            CurrencySymbol = culture.NumberFormat.CurrencySymbol;
        }
    }

}