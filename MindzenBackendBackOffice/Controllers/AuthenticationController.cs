using Microsoft.AspNetCore.Mvc;
using MindzenBackend.Models.AuthenticationModel;
using MindzenBackendBackOffice.Interfaces;

namespace MindzenBackendBackOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthentication authentication) : ControllerBase
    {
        private readonly IAuthentication _authentication = authentication;




        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (string.IsNullOrEmpty(refreshTokenRequest.Email) || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
                return BadRequest("Email and refresh token are required.");

            var isTokenValid = await _authentication.IsValidRefreshToken(refreshTokenRequest.Email, refreshTokenRequest.RefreshToken);

            if (!isTokenValid)
                return Unauthorized("Invalid or expired refresh token.");

            var newAccessTokens = await _authentication.GenerateRefreshTokens(refreshTokenRequest.Email, refreshTokenRequest.RefreshToken);

            // string newAccessToken = _jwtService.GenerateAccessToken(request.Email);
            // string newRefreshToken = _jwtService.GenerateRefreshToken();

            // await _jwtService.InvalidateRefreshToken(request.Email, request.RefreshToken);
            // await _jwtService.StoreRefreshToken(request.Email, newRefreshToken);

            return Ok(newAccessTokens);
        }

        [HttpPost("loginWithPassword")]
        public async Task<IActionResult> LoginWithPassword(LoginWithPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and password are required.");

            var apiResponse = await _authentication.LoginWithPassword(request.Email, request.Password);

            if (!apiResponse.Success)
                return BadRequest(apiResponse);

            return Ok(apiResponse);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Email and refresh token are required.");

            var apiResponse = await _authentication.Logout(request.Email, request.RefreshToken);

            if (!apiResponse.Success)
                return BadRequest(apiResponse);

            return Ok(apiResponse);
        }

    }
}
