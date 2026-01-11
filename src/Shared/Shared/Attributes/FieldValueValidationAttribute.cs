using Common.Enumerations.FieldType;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Shared.Attributes
{
    public class FieldValueValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(ValidationResources.FieldRequired);

            var fieldValue = value.ToString();
            if (string.IsNullOrWhiteSpace(fieldValue))
                return new ValidationResult(ValidationResources.FieldRequired);

            // Get the FieldType property from the same object
            var fieldTypeProperty = validationContext.ObjectType.GetProperty("FieldType");
            if (fieldTypeProperty == null)
                return new ValidationResult("FieldType property not found");

            var fieldType = (FieldType)fieldTypeProperty.GetValue(validationContext.ObjectInstance)!;

            return ValidateFieldValue(fieldValue, fieldType, validationContext);
        }

        private ValidationResult? ValidateFieldValue(string value, FieldType fieldType, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value))
                return new ValidationResult(ValidationResources.FieldRequired);

            return fieldType switch
            {
                FieldType.Text => ValidateText(value),
                FieldType.IntegerNumber => ValidateInteger(value),
                FieldType.DecimalNumber => ValidateDecimal(value),
                FieldType.Date => ValidateDate(value),
                FieldType.CheckBox => ValidateBoolean(value),
                FieldType.List => ValidateText(value), // List values are typically text
                FieldType.MultiSelectList => ValidateText(value), // MultiSelectList values are typically text
                _ => ValidationResult.Success
            };
        }

        private ValidationResult? ValidateText(string value)
        {
            // Text validation - just check if it's not too long
            if (value.Length > 1000) // Adjust max length as needed
            {
                return new ValidationResult(ValidationResources.OutOfMaxLength);
            }
            return ValidationResult.Success;
        }

        private ValidationResult? ValidateInteger(string value)
        {
            if (!int.TryParse(value, out int intValue))
            {
                return new ValidationResult(ValidationResources.InvalidFormat);
            }

            if (intValue < 0)
            {
                return new ValidationResult(ValidationResources.PositiveRange);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateDecimal(string value)
        {
            if (!decimal.TryParse(value, out decimal decimalValue))
            {
                return new ValidationResult(ValidationResources.InvalidFormat);
            }

            if (decimalValue < 0)
            {
                return new ValidationResult(ValidationResources.PositiveRange);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateDate(string value)
        {
            if (!DateTime.TryParse(value, out DateTime dateValue))
            {
                return new ValidationResult(ValidationResources.DateTimeFormat);
            }

            // Check if date is not in the future (adjust as needed)
            if (dateValue > DateTime.Now.AddYears(100))
            {
                return new ValidationResult(ValidationResources.DateTimeFormat);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateDateTime(string value)
        {
            if (!DateTime.TryParse(value, out DateTime dateTimeValue))
            {
                return new ValidationResult(ValidationResources.DateTimeFormat);
            }

            // Check if datetime is not too far in the future (adjust as needed)
            if (dateTimeValue > DateTime.Now.AddYears(100))
            {
                return new ValidationResult(ValidationResources.DateTimeFormat);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateBoolean(string value)
        {
            if (!bool.TryParse(value, out _))
            {
                return new ValidationResult(ValidationResources.InvalidFormat);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateEmail(string value)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(value))
            {
                return new ValidationResult(ValidationResources.EmailFormat);
            }

            if (value.Length > 100)
            {
                return new ValidationResult(ValidationResources.OutOfMaxLength);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidatePhoneNumber(string value)
        {
            // Remove all non-digit characters for validation
            var digitsOnly = new string(value.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length < 7 || digitsOnly.Length > 15)
            {
                return new ValidationResult(ValidationResources.InvalidPhoneNumber);
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidatePassword(string value)
        {
            if (value.Length < 6)
            {
                return new ValidationResult(UserResources.PasswordRange);
            }

            if (value.Length > 50)
            {
                return new ValidationResult(ValidationResources.OutOfMaxLength);
            }

            return ValidationResult.Success;
        }
    }
}
