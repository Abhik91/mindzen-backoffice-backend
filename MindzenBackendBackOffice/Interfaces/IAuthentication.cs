using System;
using MindzenBackendBackOffice.Models;

namespace MindzenBackendBackOffice.Interfaces
{
    public interface IAuthentication
    {
        public Task<ApiResponse> GenerateAccessTokens(string email);
        public Task<bool> IsValidRefreshToken(string email, string refreshToken);
        public Task<ApiResponse> GenerateRefreshTokens(string email, string refreshToken);
        public Task<ApiResponse> LoginWithPassword(string email, string password);
        public Task<ApiResponse> Logout(string email, string refreshToken);
    }
}
