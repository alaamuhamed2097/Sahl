using System.ComponentModel;

namespace Common.Enumerations.Settings
{
    /// <summary>
    /// System setting keys enumeration
    /// تعداد مفاتيح إعدادات النظام
    /// </summary>
    public enum SystemSettingKey
    {
        // Tax Settings
        [Description("Order Tax Percentage")]
        OrderTaxPercentage = 1, //<<<

        [Description("Tax Included In Price")]
        TaxIncludedInPrice = 2,

        // Shipping Settings
        [Description("Shipping Amount")]
        ShippingAmount = 10,

        [Description("Free Shipping Threshold")]
        FreeShippingThreshold = 11,

        [Description("Shipping Per Kg")]
        ShippingPerKg = 12,

        [Description("Estimated Delivery Days")]
        EstimatedDeliveryDays = 13,

        // Order Settings
        [Description("Order Extra Cost")]
        OrderExtraCost = 20,

        [Description("Minimum Order Amount")]
        MinimumOrderAmount = 21,

        [Description("Maximum Order Amount")]
        MaximumOrderAmount = 22,

        [Description("Order Cancellation Period Hours")]
        OrderCancellationPeriodHours = 23,

        // Payment Settings
        [Description("Payment Gateway Enabled")]
        PaymentGatewayEnabled = 30,

        [Description("Cash On Delivery Enabled")]
        CashOnDeliveryEnabled = 31,

        [Description("Stripe Public Key")]
        StripePublicKey = 32,

        [Description("Stripe Secret Key")]
        StripeSecretKey = 33,


        //--------34

        // Business Settings
        [Description("Business Hours Start")]
        BusinessHoursStart = 40,

        [Description("Business Hours End")]
        BusinessHoursEnd = 41,

        [Description("Maintenance Mode")]
        MaintenanceMode = 42,

        [Description("Allow Guest Checkout")]
        AllowGuestCheckout = 43,

        // Notification Settings
        [Description("Email Notifications Enabled")]
        EmailNotificationsEnabled = 50,

        [Description("SMS Notifications Enabled")]
        SmsNotificationsEnabled = 51,

        [Description("Order Confirmation Email")]
        OrderConfirmationEmail = 52,

        // Security Settings
        [Description("Max Login Attempts")]
        MaxLoginAttempts = 60,

        [Description("Session Timeout Minutes")]
        SessionTimeoutMinutes = 61,

        [Description("Password Min Length")]
        PasswordMinLength = 62,

		// Refund Settings
		[Description("Refund Allowed Days")]
		RefundAllowedDays = 70,

		// DateTime Settings
		[Description("System Timezone ID")]
		SystemTimeZoneId = 80
	}
}
