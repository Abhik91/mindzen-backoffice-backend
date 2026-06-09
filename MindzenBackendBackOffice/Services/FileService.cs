using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;
using MindzenBackendBackOffice.Modules.FileProcessing;

namespace MindzenBackendBackOffice.Services
{
    public class FileService(IWebHostEnvironment env) : IFile
    {
        private readonly FileModule _fileModule = new(env);

        public async Task<ApiResponse> GetPractitionerProfilePic(string filePath)
        {
            return await _fileModule.GetPractitionerProfilePic(filePath);
        }

        public async Task<ApiResponse> UploadFile(IFormFile formFile, string fileName, string directoryPath)
        {
            return await _fileModule.UploadFile(formFile, fileName, directoryPath);
        }
    }
}
