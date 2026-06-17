using MindzenBackend.Modules.AuthenticationDetails;
using MindzenBackendBackOffice.Interfaces;
using MindzenBackendBackOffice.Models;

namespace MindzenBackend.Services
{
    public class AuthenticationService(IConfiguration configuration) : IAuthentication
    {
        private readonly AuthenticationDetailsModule _authenticationDetailsModule = new(configuration);


        #region Generate Access Tokens
        public async Task<ApiResponse> GenerateAccessTokens(string email)
        {
            return await _authenticationDetailsModule.GenerateAccessTokens(email);
        }
        #endregion

        #region Generate Refresh Tokens
        public async Task<ApiResponse> GenerateRefreshTokens(string email, string refreshToken)
        {
            return await _authenticationDetailsModule.GenerateRefreshToken(email, refreshToken);
        }
        #endregion


        #region Get Valid Refresh Tokens
        public async Task<bool> IsValidRefreshToken(string email, string refreshToken)
        {
            return await _authenticationDetailsModule.IsValidRefreshToken(email, refreshToken);
        }
        #endregion


        public async Task<ApiResponse> LoginWithPassword(string email, string password)
        {
            return await _authenticationDetailsModule.LoginWithPassword(email, password);
        }

        public async Task<ApiResponse> Logout(string email, string refreshToken)
        {
            return await _authenticationDetailsModule.Logout(email, refreshToken);
        }
    }
}
