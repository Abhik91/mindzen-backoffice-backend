using Microsoft.AspNetCore.StaticFiles;
using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Modules.FileProcessing
{
    public class FileModule(IWebHostEnvironment env)
    {
        private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

        public Task<ApiResponse> GetPractitionerProfilePic(string filePath)
        {
            ApiResponse apiResponse = new();
            try
            {
                var fullPath = Path.Combine(env.ContentRootPath, filePath);

                if (!File.Exists(fullPath))
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "File not found";
                    return Task.FromResult(apiResponse);
                }

                if (!_contentTypeProvider.TryGetContentType(fullPath, out var contentType))
                    contentType = "application/octet-stream";

                apiResponse.Success = true;
                apiResponse.Message = "File found and ready to download";
                apiResponse.Data = new CreatedFileContent
                {
                    Stream = File.OpenRead(fullPath),
                    ContentType = contentType,
                    FileName = Path.GetFileName(fullPath)
                };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error fetching file";
                apiResponse.Data = new { errorMessage = ex.Message };
            }
            return Task.FromResult(apiResponse);
        }

        public async Task<ApiResponse> UploadFile(IFormFile formFile, string fileName, string directoryPath)
        {
            ApiResponse apiResponse = new();
            try
            {
                var fullDirectoryPath = Path.Combine(env.ContentRootPath, directoryPath);

                if (!Directory.Exists(fullDirectoryPath))
                    Directory.CreateDirectory(fullDirectoryPath);

                var fileNameWithExtension = fileName + Path.GetExtension(formFile.FileName);
                var fullFilePath = Path.Combine(fullDirectoryPath, fileNameWithExtension);

                using var stream = new FileStream(fullFilePath, FileMode.Create);
                await formFile.CopyToAsync(stream);

                apiResponse.Success = true;
                apiResponse.Message = "File uploaded successfully";
                apiResponse.Data = new { relativeFilePath = Path.Combine(directoryPath, fileNameWithExtension).Replace('\\', '/') };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "File failed to upload";
                apiResponse.Data = new { errorMessage = ex.Message, errorStackTrace = ex.StackTrace };
            }
            return apiResponse;
        }
    }
}
