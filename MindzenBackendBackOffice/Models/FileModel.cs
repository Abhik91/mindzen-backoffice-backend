using System.ComponentModel.DataAnnotations;

namespace MindzenBackendBackOffice.Models
{
    public class CreatedFileContent
    {
        public Stream? Stream { get; set; }
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
    }

    public class UploadRequest
    {
        [Required]
        public string? FileName { get; set; }

        [Required]
        public string? DirectoryPath { get; set; }

        [Required]
        public IFormFile? UploadFile { get; set; }
    }
}
