using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;
using MindzenBackendBackOffice.Modules.FileProcessing;

namespace MindzenBackendBackOffice.Services
{
    public class FileService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IFile
    {
        private readonly FileModule _fileModule = new(
            httpClientFactory.CreateClient(),
            configuration["MainBackend:BaseUrl"] ?? throw new InvalidOperationException("MainBackend:BaseUrl is not configured in appsettings.json")
        );

        public async Task<ApiResponse> GetPractitionerProfilePic(string filePath)
            => await _fileModule.GetPractitionerProfilePic(filePath);

        public async Task<ApiResponse> UploadFile(IFormFile formFile, string fileName, string directoryPath)
            => await _fileModule.UploadFile(formFile, fileName, directoryPath);
    }
}
