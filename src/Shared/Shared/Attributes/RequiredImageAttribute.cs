using Resources;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class RequiredImageAttribute : ValidationAttribute
{
    private readonly string _imagePathProperty;
    private readonly int _maxFileSizeMB;
    private readonly int _maxFiles; // Maximum number of files allowed

    public RequiredImageAttribute(string imagePathProperty, int maxFileSizeMB = 5, int maxFiles = 1)
    {
        _imagePathProperty = imagePathProperty;
        _maxFileSizeMB = maxFileSizeMB;
        _maxFiles = maxFiles;

        ErrorMessage = ValidationResources.FieldRequired + " : " +
                      string.Format(ValidationResources.ImageSizeLimitExceeded, maxFileSizeMB);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Get the value of the ImagePath property (which stores paths of existing images)
        PropertyInfo imagePathPropertyInfo = validationContext.ObjectType.GetProperty(_imagePathProperty);
        if (imagePathPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property: {_imagePathProperty}");
        }

        var imagePathValue = imagePathPropertyInfo.GetValue(validationContext.ObjectInstance) as string;
        bool hasExistingImages = !string.IsNullOrEmpty(imagePathValue);

        // If there's already an image path, no need to validate the upload
        if (hasExistingImages)
        {
            return ValidationResult.Success;
        }

        // If value is null, it's allowed (image is optional)
        if (value == null)
        {
            return ValidationResult.Success;
        }

        // Otherwise, validate the new uploaded image(s) if they exist
        // For a single base64 image
        if (value is string singleBase64)
        {
            if (string.IsNullOrEmpty(singleBase64))
            {
                return ValidationResult.Success; // Empty string is allowed (treated as null)
            }
            return ValidateBase64Image(singleBase64);
        }
        // For multiple base64 images
        else if (value is IEnumerable<string> base64Images)
        {
            var imageList = base64Images?.Where(img => !string.IsNullOrEmpty(img)).ToList();
            if (imageList == null || !imageList.Any())
            {
                return ValidationResult.Success; // Empty collection is allowed (treated as null)
            }
            return ValidateBase64Images(imageList);
        }

        // If we get here, the value is not null but also not a recognized image type
        return new ValidationResult("Invalid image format provided");
    }

    private ValidationResult ValidateBase64Image(string base64Image)
    {
        // Calculate approx size: base64 string length * 0.75 is rough byte size
        // Remove data URI prefix if present
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