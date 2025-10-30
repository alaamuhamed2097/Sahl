using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attributes
{
    public class RequiredImageOnCreateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var objectType = validationContext.ObjectType;
            var instance = validationContext.ObjectInstance;

            // Id
            var idProperty = objectType.GetProperty("Id");
            var idValue = idProperty?.GetValue(instance);

            // LogoBase64
            var logoProperty = objectType.GetProperty("Base64Image");
            var logoValue = logoProperty?.GetValue(instance) as string;

            bool isNew = idValue == null || (idValue is Guid guid && guid == Guid.Empty);
            bool isLogoMissing = string.IsNullOrWhiteSpace(logoValue);

            if (isNew && isLogoMissing)
            {
                return new ValidationResult(ValidationResources.ImageRequired, new[] { "Base64Image" });
            }

            return ValidationResult.Success;
        }
    }
}
