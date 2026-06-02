using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Interfaces
{
    public interface IFile
    {
        Task<ApiResponse> GetPractitionerProfilePic(string filePath);
    }
}
