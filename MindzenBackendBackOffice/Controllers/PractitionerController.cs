using Microsoft.AspNetCore.Mvc;
using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PractitionerController(IPractitioner practitioner) : ControllerBase
    {
        [HttpPost("updateFeaturedOrder")]
        public async Task<IActionResult> UpdateFeaturedPractitionerOrder([FromBody] UpdateFeaturedOrderRequest request)
        {
            try
            {
                ApiResponse response = await practitioner.UpdateFeaturedPractitionerOrder(request.Practitioners!);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("updateFeaturedStatus")]
        public async Task<IActionResult> UpdatePractitionerFeaturedStatus([FromBody] UpdateFeaturedStatusRequest request)
        {
            try
            {
                ApiResponse response = await practitioner.UpdatePractitionerFeaturedStatus(request.UserID!, request.IsFeatured);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getPractitionerFeatureDetails")]
        public async Task<IActionResult> GetPractitionerFeatureDetails()
        {
            try
            {
                ApiResponse response = await practitioner.GetPractitionerFeatureDetails();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getPractitionerProfileDetails")]
        public async Task<IActionResult> GetPractionerProfileDetails(string UserId)
        {
            try
            {
                ApiResponse response = await practitioner.GetPractionerProfileDetails(UserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
