using System.ComponentModel;
using System.Reflection;

namespace Common.Enumerations.Settings
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get description from enum
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field == null)
                return value.ToString();

            DescriptionAttribute attribute =
                field.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// Get enum value from string
        /// </summary>
        public static T GetEnumFromString<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Get category from setting key
        /// </summary>
        public static SystemSettingCategory GetCategory(this SystemSettingKey key)
        {
            return key switch
            {
                SystemSettingKey.OrderTaxPercentage or
                SystemSettingKey.TaxIncludedInPrice
                    => SystemSettingCategory.Tax,

                SystemSettingKey.ShippingAmount or
                SystemSettingKey.FreeShippingThreshold or
                SystemSettingKey.ShippingPerKg or
                SystemSettingKey.EstimatedDeliveryDays
                    => SystemSettingCategory.Shipping,

                SystemSettingKey.OrderExtraCost or
                SystemSettingKey.MinimumOrderAmount or
                SystemSettingKey.MaximumOrderAmount or
                SystemSettingKey.OrderCancellationPeriodHours
                    => SystemSettingCategory.Order,

                SystemSettingKey.PaymentGatewayEnabled or
                SystemSettingKey.CashOnDeliveryEnabled or
                SystemSettingKey.StripePublicKey or
                SystemSettingKey.StripeSecretKey
                    => SystemSettingCategory.Payment,

                SystemSettingKey.BusinessHoursStart or
                SystemSettingKey.BusinessHoursEnd or
                SystemSettingKey.MaintenanceMode or
                SystemSettingKey.AllowGuestCheckout
                    => SystemSettingCategory.Business,

                SystemSettingKey.EmailNotificationsEnabled or
                SystemSettingKey.SmsNotificationsEnabled or
                SystemSettingKey.OrderConfirmationEmail
                    => SystemSettingCategory.Notification,

                SystemSettingKey.MaxLoginAttempts or
                SystemSettingKey.SessionTimeoutMinutes or
                SystemSettingKey.PasswordMinLength
                    => SystemSettingCategory.Security,

                _ => SystemSettingCategory.Business
            };
        }
    }
}
