using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attributes
{
    public class ConditionalRequiredImageAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;
        private readonly object _targetValue;
        private readonly string _imagePathProperty;
        private readonly int _maxFileSizeMB;
        private readonly int _maxFiles;

        public ConditionalRequiredImageAttribute(string dependentProperty, object targetValue, string imagePathProperty, int maxFileSizeMB = 5, int maxFiles = 1)
        {
            _dependentProperty = dependentProperty;
            _targetValue = targetValue;
            _imagePathProperty = imagePathProperty;
            _maxFileSizeMB = maxFileSizeMB;
            _maxFiles = maxFiles;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the dependent property value
            var dependentProperty = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (dependentProperty == null)
                return new ValidationResult($"Unknown property: {_dependentProperty}");

            var dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance);

            // Skip validation entirely if condition isn't met
            if (!Equals(dependentValue, _targetValue))
                return ValidationResult.Success;

            // Condition is met - proceed with validation
            if (value == null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidationResult(
                    ErrorMessage ?? ValidationResources.FieldRequired,
                    new[] { validationContext.MemberName });
            }

            // Validate image only when required
            return ValidateImageUpload(value);
        }

        private ValidationResult ValidateImageUpload(object value)
        {
            // For a single base64 image
            if (value is string singleBase64)
            {
                return ValidateBase64Image(singleBase64);
            }
            // For multiple base64 images
            else if (value is IEnumerable<string> base64Images)
            {
                var imageList = base64Images?.Where(img => !string.IsNullOrEmpty(img)).ToList();
                if (imageList == null || !imageList.Any())
                {
                    return new ValidationResult(ValidationResources.FieldRequired);
                }
                return ValidateBase64Images(imageList);
            }

            return new ValidationResult("Invalid image format provided");
        }

        private ValidationResult ValidateBase64Image(string base64Image)
        {
            // Calculate approx size: base64 string length * 0.75 is rough byte size
            string base64Data = base64Image;
            if (base64Image.Contains(","))
            {
                base64Data = base64Image.Split(',')[1];
            }

            long approximateSizeInBytes = (long)(base64Data.Length * 0.75);
            if (approximateSizeInBytes > _maxFileSizeMB * 1024 * 1024)
            {
                return new ValidationResult(string.Format(ValidationResources.ImageSizeLimitExceeded, _maxFileSizeMB));
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateBase64Images(IEnumerable<string> base64Images)
        {
            int imageCount = base64Images.Count();
            if (imageCount > _maxFiles)
            {
                return new ValidationResult(string.Format(ValidationResources.MaxImageUploadLimit, _maxFiles));
            }

            foreach (var base64Image in base64Images)
            {
                var result = ValidateBase64Image(base64Image);
                if (result != ValidationResult.Success)
                {
                    return result;
                }
            }

            return ValidationResult.Success;
        }
    }
}