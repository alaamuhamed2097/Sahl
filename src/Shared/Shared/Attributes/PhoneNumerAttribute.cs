using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private readonly string _phoneCodePropertyName;

        public PhoneNumberAttribute(string phoneCodePropertyName)
        {
            _phoneCodePropertyName = phoneCodePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ValidationResult.Success;

            // Get phone code property
            var phoneCodeProp = validationContext.ObjectType.GetProperty(_phoneCodePropertyName);
            if (phoneCodeProp == null)
                return new ValidationResult($"Property '{_phoneCodePropertyName}' not found.", new[] { validationContext.MemberName });

            var phoneCodeValue = phoneCodeProp.GetValue(validationContext.ObjectInstance) as string;
            if (string.IsNullOrWhiteSpace(phoneCodeValue))
                return new ValidationResult(ValidationResources.InvalidPhoneNumber, new[] { validationContext.MemberName });

            try
            {
                if (ValidatePhoneNumber(phoneNumber, phoneCodeValue))
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ValidationResources.InvalidPhoneNumber, new[] { validationContext.MemberName });
            }
            catch (Exception ex)
            {
                return new ValidationResult(ValidationResources.InvalidPhoneNumber, new[] { validationContext.MemberName });
            }
        }

        private bool ValidatePhoneNumber(string phoneNumber, string phoneCode)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(phoneCode))
            {
                // Remove leading zero and extract digits only
                var cleanedPhone = RemoveLeadingZero(phoneNumber);
                var digitsOnly = new string(cleanedPhone.Where(char.IsDigit).ToArray());
                var isPhoneValid = ValidatePhoneLength(digitsOnly, phoneCode);

                // Update the model with cleaned phone number
                return isPhoneValid;
            }
            else
            {
                return false;
            }
        }

        private bool ValidatePhoneLength(string digitsOnly, string countryCode)
        {
            if (string.IsNullOrEmpty(digitsOnly)) return false;

            return countryCode switch
            {
                // North America
                "+1" => digitsOnly.Length == 10, // US / Canada

                // Africa
                "+20" => digitsOnly.Length == 10, // Egypt
                "+212" => digitsOnly.Length == 9, // Morocco
                "+213" => digitsOnly.Length == 9, // Algeria
                "+216" => digitsOnly.Length == 8, // Tunisia
                "+218" => digitsOnly.Length == 9, // Libya
                "+221" => digitsOnly.Length == 9, // Senegal
                "+222" => digitsOnly.Length == 8, // Mauritania
                "+225" => digitsOnly.Length == 8, // Ivory Coast
                "+234" => digitsOnly.Length == 10, // Nigeria
                "+249" => digitsOnly.Length == 9, // Sudan
                "+251" => digitsOnly.Length == 9, // Ethiopia
                "+254" => digitsOnly.Length == 9, // Kenya
                "+255" => digitsOnly.Length == 9, // Tanzania
                "+256" => digitsOnly.Length == 9, // Uganda
                "+260" => digitsOnly.Length == 9, // Zambia
                "+263" => digitsOnly.Length == 9, // Zimbabwe
                "+27" => digitsOnly.Length == 9, // South Africa

                // Middle East
                "+961" => digitsOnly.Length == 8, // Lebanon
                "+962" => digitsOnly.Length == 9, // Jordan
                "+963" => digitsOnly.Length == 9, // Syria
                "+964" => digitsOnly.Length == 10, // Iraq
                "+965" => digitsOnly.Length == 8, // Kuwait
                "+966" => digitsOnly.Length == 9, // Saudi Arabia
                "+967" => digitsOnly.Length == 9, // Yemen
                "+968" => digitsOnly.Length == 8, // Oman
                "+970" => digitsOnly.Length == 9, // Palestine
                "+971" => digitsOnly.Length == 9, // UAE
                "+972" => digitsOnly.Length == 9, // Israel
                "+973" => digitsOnly.Length == 8, // Bahrain
                "+974" => digitsOnly.Length == 8, // Qatar
                "+975" => digitsOnly.Length == 8, // Bhutan
                "+976" => digitsOnly.Length == 8, // Mongolia

                // Europe
                "+30" => digitsOnly.Length == 10, // Greece
                "+31" => digitsOnly.Length == 9,  // Netherlands
                "+32" => digitsOnly.Length == 9,  // Belgium
                "+33" => digitsOnly.Length == 9,  // France
                "+34" => digitsOnly.Length == 9,  // Spain
                "+36" => digitsOnly.Length == 9,  // Hungary
                "+39" => digitsOnly.Length >= 9 && digitsOnly.Length <= 10, // Italy
                "+40" => digitsOnly.Length == 9,  // Romania
                "+41" => digitsOnly.Length == 9,  // Switzerland
                "+43" => digitsOnly.Length == 10, // Austria
                "+44" => digitsOnly.Length >= 10 && digitsOnly.Length <= 11, // UK
                "+45" => digitsOnly.Length == 8,  // Denmark
                "+46" => digitsOnly.Length == 9,  // Sweden
                "+47" => digitsOnly.Length == 8,  // Norway
                "+48" => digitsOnly.Length == 9,  // Poland
                "+49" => digitsOnly.Length >= 10 && digitsOnly.Length <= 12, // Germany
                "+351" => digitsOnly.Length == 9, // Portugal
                "+352" => digitsOnly.Length == 9, // Luxembourg
                "+353" => digitsOnly.Length == 9, // Ireland
                "+354" => digitsOnly.Length == 7, // Iceland
                "+355" => digitsOnly.Length == 9, // Albania
                "+356" => digitsOnly.Length == 8, // Malta
                "+357" => digitsOnly.Length == 8, // Cyprus
                "+358" => digitsOnly.Length == 10, // Finland
                "+359" => digitsOnly.Length == 9, // Bulgaria
                "+380" => digitsOnly.Length == 9, // Ukraine
                "+381" => digitsOnly.Length == 9, // Serbia
                "+385" => digitsOnly.Length == 9, // Croatia
                "+386" => digitsOnly.Length == 8, // Slovenia
                "+387" => digitsOnly.Length == 8, // Bosnia & Herzegovina
                "+389" => digitsOnly.Length == 8, // North Macedonia

                // Asia
                "+60" => digitsOnly.Length >= 8 && digitsOnly.Length <= 10, // Malaysia
                "+61" => digitsOnly.Length == 9, // Australia
                "+62" => digitsOnly.Length >= 9 && digitsOnly.Length <= 11, // Indonesia
                "+63" => digitsOnly.Length == 10, // Philippines
                "+64" => digitsOnly.Length == 9, // New Zealand
                "+65" => digitsOnly.Length == 8, // Singapore
                "+66" => digitsOnly.Length == 9, // Thailand
                "+81" => digitsOnly.Length == 10, // Japan
                "+82" => digitsOnly.Length == 10, // South Korea
                "+84" => digitsOnly.Length == 9,  // Vietnam
                "+86" => digitsOnly.Length == 11, // China
                "+90" => digitsOnly.Length == 10, // Turkey
                "+91" => digitsOnly.Length == 10, // India
                "+92" => digitsOnly.Length == 10, // Pakistan
                "+94" => digitsOnly.Length == 9,  // Sri Lanka
                "+95" => digitsOnly.Length == 9,  // Myanmar
                "+98" => digitsOnly.Length == 10, // Iran

                // South America
                "+51" => digitsOnly.Length == 9,  // Peru
                "+52" => digitsOnly.Length == 10, // Mexico
                "+54" => digitsOnly.Length == 10, // Argentina
                "+55" => digitsOnly.Length == 11, // Brazil
                "+56" => digitsOnly.Length == 9,  // Chile
                "+57" => digitsOnly.Length == 10, // Colombia
                "+58" => digitsOnly.Length == 10, // Venezuela
                "+591" => digitsOnly.Length == 8, // Bolivia
                "+593" => digitsOnly.Length == 9, // Ecuador
                "+595" => digitsOnly.Length == 9, // Paraguay
                "+598" => digitsOnly.Length == 9, // Uruguay

                // Generic fallback
                _ => digitsOnly.Length >= 7 && digitsOnly.Length <= 15
            };
        }

        private string RemoveLeadingZero(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            // Trim whitespace first
            phoneNumber = phoneNumber.Trim();

            // If the number starts with 0, remove it
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }

            return phoneNumber;
        }
    }
}
