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
    }
}
