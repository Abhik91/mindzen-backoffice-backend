using Microsoft.AspNetCore.Mvc;
using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController(IFile file) : ControllerBase
    {
        [HttpGet("unsigned-download")]
        public async Task<IActionResult> UnsignedDownloadFile([FromQuery] string filePath)
        {
            try
            {
                ApiResponse apiResponse = await file.GetPractitionerProfilePic(filePath);

                if (!apiResponse.Success)
                    return NotFound("File not found.");

                CreatedFileContent fileContent = (CreatedFileContent)apiResponse.Data!;

                return File(fileContent.Stream!, fileContent.ContentType!, fileContent.FileName!);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

         [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadRequest uploadRequest)
        {
            if (uploadRequest.UploadFile == null || uploadRequest.UploadFile.Length == 0)
                return BadRequest("No file uploaded.");

            var response = await file.UploadFile(uploadRequest.UploadFile, uploadRequest.FileName, uploadRequest.DirectoryPath);
            if (!response.Success)
                return StatusCode(500, "Internal server error: File upload failed.");

            // string fileUrl = $"{Request.Scheme}://{Request.Host}/api/File/files/{relativeFilePath}";
            return Ok(response);
        }
    }
}
