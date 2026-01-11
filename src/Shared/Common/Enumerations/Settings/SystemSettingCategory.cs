using System.ComponentModel;

namespace Common.Enumerations.Settings
{
    /// <summary>
    /// System setting categories enumeration
    /// تعداد فئات إعدادات النظام
    /// </summary>
    public enum SystemSettingCategory
    {
        [Description("Tax")]
        Tax = 1, //<<<

        [Description("Shipping")]
        Shipping = 2,

        [Description("Payment")]
        Payment = 3,

        [Description("Order")]
        Order = 4,

        [Description("Business")]
        Business = 5,

        [Description("Notification")]
        Notification = 6,

        [Description("Security")]
        Security = 7,

        [Description("Refund Allowed Days")]
		RefundAllowedDays = 8

        
	}
}
