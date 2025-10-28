namespace BL.Contracts.GeneralService.CMS
{
    public interface IImageProcessingService
    {
        byte[] ResizeImage(byte[] imageBytes, int width, int height);
        public byte[] ResizeImagePreserveAspectRatio(byte[] imageBytes, int maxWidth = 1200, int maxHeight = 1200);
        byte[] ConvertToWebP(byte[] imageBytes, int quality = 100);
        byte[] ResizeAndConvertToWebP(byte[] imageBytes, int width, int height, int quality = 100);
    }
}
