using System.Text.RegularExpressions;

namespace BL.Utils
{
    /// <summary>
    /// Helper class for phone number normalization and validation
    /// </summary>
    public static class PhoneNormalizationHelper
    {
        /// <summary>
        /// Normalizes a phone number by removing non-numeric characters
        /// </summary>
        public static string NormalizePhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber ?? string.Empty;

            // Remove all non-digit characters except +
            return Regex.Replace(phoneNumber, @"[^\d\+]", "");
        }

        /// <summary>
        /// Creates a full normalized phone identifier combining code and number
        /// </summary>
        public static string CreateNormalizedPhone(string phoneCode, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneCode) || string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            var normalizedCode = NormalizePhone(phoneCode);
            var normalizedNumber = NormalizePhone(phoneNumber);

            return $"{normalizedCode}{normalizedNumber}";
        }

        /// <summary>
        /// Validates phone number format
        /// </summary>
        public static bool IsValidPhoneFormat(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var normalized = NormalizePhone(phoneNumber);
            return normalized.Length >= 6 && normalized.Length <= 15 && Regex.IsMatch(normalized, @"^\d+$");
        }

        /// <summary>
        /// Validates phone code format
        /// </summary>
        public static bool IsValidPhoneCode(string phoneCode)
        {
            if (string.IsNullOrWhiteSpace(phoneCode))
                return false;

            var normalized = NormalizePhone(phoneCode);
            return normalized.Length >= 1 && normalized.Length <= 5 && Regex.IsMatch(normalized, @"^\+?\d+$");
        }
    }
}
