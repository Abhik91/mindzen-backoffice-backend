using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;
using MindzenBackendBackOffice.Modules.FileProcessing;

namespace MindzenBackendBackOffice.Services
{
    public class FileService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IFile
    {
        private readonly FileModule _fileModule = new(configuration, httpClientFactory);

        public async Task<ApiResponse> GetPractitionerProfilePic(string filePath)
        {
            return await _fileModule.GetPractitionerProfilePic(filePath);
        }
    }
}
