namespace Shared.DTOs.Setting
{
	public class SystemSettingsViewModelDto
	{
		// Tax Settings
		public bool TaxIncludedInPrice { get; set; }
		public decimal OrderTaxPercentage { get; set; }
		public int RefundAllowedDays { get; set; }

		//// Shipping Settings
		//public decimal ShippingAmount { get; set; }
		//public decimal FreeShippingThreshold { get; set; }
		//public decimal ShippingPerKg { get; set; }
		//public int EstimatedDeliveryDays { get; set; }

		//// Order Settings
		//public decimal OrderExtraCost { get; set; }
		//public decimal MinimumOrderAmount { get; set; }
		//public decimal MaximumOrderAmount { get; set; }
		//public int OrderCancellationPeriodHours { get; set; }

		//// Payment Settings
		//public bool PaymentGatewayEnabled { get; set; }
		//public bool CashOnDeliveryEnabled { get; set; }

		//// Business Settings
		//public bool MaintenanceMode { get; set; }
		//public bool AllowGuestCheckout { get; set; }

		//// Notification Settings
		//public bool EmailNotificationsEnabled { get; set; }
		//public bool SmsNotificationsEnabled { get; set; }

		//// Security Settings
		//public int MaxLoginAttempts { get; set; }
		//public int SessionTimeoutMinutes { get; set; }
		//public int PasswordMinLength { get; set; }
	}
}