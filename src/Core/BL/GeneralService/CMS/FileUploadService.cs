using BL.Contracts.GeneralService.CMS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BL.GeneralService.CMS
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> GetFileBytesAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<byte[]> GetFileBytesAsync(string base64String)
        {
            // Simulate async operation, if needed
            return await Task.Run(() => Convert.FromBase64String(base64String));
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        public async Task<string> UploadFileAsync(byte[] fileBytes, string folderName)
        {
            // Create the uploads folder if it doesn't exist
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a new GUID and append the original file extension
            var uniqueFileName = $"{Guid.NewGuid()}.webp";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Write the file bytes to the specified path
            await File.WriteAllBytesAsync(filePath, fileBytes);

            var finalPath = Path.Combine("uploads", folderName, uniqueFileName);
            return finalPath.Replace('\\','/');
        }

        public bool IsValidFile(IFormFile file)
        {
            return ValidateFile(file).isValid;
        }

        public bool IsValidFile(string base64File, string fileName)
        {
            // Implementation for base64 string validation
            var validation = ValidateFile(base64File);
            return validation.isValid;
        }

        public (bool isValid, string errorMessage) ValidateFile(IFormFile file, int maxFileSizeBytes = 5 * 1024 * 1024)
        {
            if (file == null)
                return (false, "File is null.");

            if (!file.ContentType.StartsWith("image/"))
                return (false, "Invalid file type. Only images are allowed.");

            if (file.Length > maxFileSizeBytes)
                return (false, $"File is too large. Maximum size allowed is {maxFileSizeBytes / (1024 * 1024)}MB.");

            return (true, string.Empty);
        }

        public (bool isValid, string errorMessage) ValidateFile(string base64String, int maxFileSizeBytes = 5 * 1024 * 1024)
        {
            if (string.IsNullOrEmpty(base64String))
                return (false, "File is null or empty.");

            try
            {
                // Calculate the approximate size of the decoded base64 string
                // Base64 encodes 3 bytes in 4 chars (with some padding)
                int decodedSize = base64String.Length * 3 / 4;
                if (decodedSize > maxFileSizeBytes)
                    return (false, $"File is too large. Maximum size allowed is {maxFileSizeBytes / (1024 * 1024)}MB.");

                // Validate if it's a valid base64 string
                byte[] decodedBytes = Convert.FromBase64String(base64String);

                // Additional check with the actual decoded size
                if (decodedBytes.Length > maxFileSizeBytes)
                    return (false, $"File is too large. Maximum size allowed is {maxFileSizeBytes / (1024 * 1024)}MB.");

                return (true, string.Empty);
            }
            catch (FormatException)
            {
                return (false, "File is not a valid base64 string.");
            }
        }
    }
}