using Microsoft.AspNetCore.Http;

namespace BL.Contracts.GeneralService.CMS;

public interface IFileUploadService
{
    Task<byte[]> GetFileBytesAsync(IFormFile file);
    Task<byte[]> GetFileBytesAsync(string base64String);
    Task<string> UploadFileAsync(IFormFile file);
    Task<string> UploadFileAsync(byte[] fileBytes, string folderName);
    bool IsValidFile(IFormFile file);
    bool IsValidFile(string base64File, string fileName);
    (bool isValid, string errorMessage) ValidateFile(IFormFile file, int MaxFileSizeBytes = 5 * 1024 * 1024);
    (bool isValid, string errorMessage) ValidateFile(string base64String, int MaxFileSizeBytes = 5 * 1024 * 1024);
    Task DeleteFileAsync(string filePath);
}
