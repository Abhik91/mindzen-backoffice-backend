using Microsoft.AspNetCore.Http;
using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Interfaces
{
    public interface IFile
    {
        Task<ApiResponse> GetPractitionerProfilePic(string filePath);
        Task<ApiResponse> UploadFile(IFormFile formFile, string fileName, string directoryPath);
    }
}
