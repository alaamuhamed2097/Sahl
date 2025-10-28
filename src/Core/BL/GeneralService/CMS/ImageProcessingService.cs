using BL.Contracts.GeneralService.CMS;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

public class ImageProcessingService : IImageProcessingService
{
    /// <summary>
    /// Resizes an image without converting its format.
    /// </summary>
    /// <param name="imageBytes">The image as a byte array.</param>
    /// <param name="width">The target width.</param>
    /// <param name="height">The target height.</param>
    /// <returns>The resized image as a byte array.</returns>
    public byte[] ResizeImage(byte[] imageBytes, int width, int height)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var image = Image.Load(inputStream);

        // Resize the image
        image.Mutate(x => x.Resize(width, height));

        // Save the image in its original format
        using var outputStream = new MemoryStream();
        image.Save(outputStream, image.Metadata.DecodedImageFormat); // Preserve the original format
        return outputStream.ToArray();
    }

    public byte[] ResizeImagePreserveAspectRatio(byte[] imageBytes, int maxWidth = 1200, int maxHeight = 1200)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var image = Image.Load(inputStream);

        // Original dimensions
        var originalWidth = image.Width;
        var originalHeight = image.Height;

        // Calculate the scaling ratios
        double ratioX = (double)maxWidth / originalWidth;
        double ratioY = (double)maxHeight / originalHeight;

        // Use the smaller ratio to preserve aspect ratio
        double ratio = Math.Min(ratioX, ratioY);

        // Compute new size
        int newWidth = (int)(originalWidth * ratio);
        int newHeight = (int)(originalHeight * ratio);

        // Resize while keeping proportions
        image.Mutate(x => x.Resize(newWidth, newHeight));

        // Save the image in its original format
        using var outputStream = new MemoryStream();
        image.Save(outputStream, image.Metadata.DecodedImageFormat);
        return outputStream.ToArray();
    }

    /// <summary>
    /// Converts an image to WebP format.
    /// </summary>
    /// <param name="imageBytes">The image as a byte array.</param>
    /// <param name="quality">The quality of the output image (1-100).</param>
    /// <returns>The converted image as a byte array.</returns>
    public byte[] ConvertToWebP(byte[] imageBytes, int quality = 100)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var image = Image.Load(inputStream);

        // Configure the WebP encoder
        var webpEncoder = new WebpEncoder
        {
            Quality = quality, // Use the provided quality parameter
        };

        // Save the image in WebP format
        using var outputStream = new MemoryStream();
        image.SaveAsWebp(outputStream, webpEncoder);
        return outputStream.ToArray();
    }

    /// <summary>
    /// Resizes an image and converts it to WebP format.
    /// </summary>
    /// <param name="imageBytes">The image as a byte array.</param>
    /// <param name="width">The target width.</param>
    /// <param name="height">The target height.</param>
    /// <param name="quality">The quality of the output image (1-100).</param>
    /// <returns>The resized and converted image as a byte array.</returns>
    public byte[] ResizeAndConvertToWebP(byte[] imageBytes, int width, int height, int quality = 100)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var image = Image.Load(inputStream);

        // Resize the image
        image.Mutate(x => x.Resize(width, height));

        // Configure the WebP encoder
        var webpEncoder = new WebpEncoder
        {
            Quality = quality
        };

        // Save the image in WebP format
        using var outputStream = new MemoryStream();
        image.SaveAsWebp(outputStream, webpEncoder);
        return outputStream.ToArray();
    }
}