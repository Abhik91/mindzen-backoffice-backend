using MindzenBackend.Models.AuthenticationModel;
using MindzenBackend.Modules.JWT;
using MindzenBackend.Modules.Shared;
using MindzenBackendBackOffice.Models;
using MindzenDatabaseLibrary.DataAccess.Authentication;
using MindzenDatabaseLibrary.DataAccess.User;
using MindzenDatabaseLibrary.DataModels;

namespace MindzenBackend.Modules.AuthenticationDetails
{
    public class AuthenticationDetailsModule(IConfiguration configuration)
    {
        private readonly AuthenticationDataAccess authenticationDataAccess = new();
        private readonly JWTModule jWTModule = new(configuration);
        private readonly UserDataAccess userDataAccess = new();

        

        public async Task<ApiResponse> GenerateAccessTokens(string email)
        {
            ApiResponse apiResponse = new();
            AccessTokenResponse accessTokenResponse = new();
            string accessToken = jWTModule.GenerateAccessToken(email);
            string refreshToken = jWTModule.GenerateRefreshToken();

            var response = await authenticationDataAccess.UpdateAccessTokensDetails(email, accessToken, refreshToken);

            if (response > 0)
            {
                // Fetch user details
                var userDetailsList = await userDataAccess.GetUserDetails(UserId: null, Email: email);
                if (userDetailsList.Count > 0)
                {
                    var userDetails = userDetailsList[0];
                    accessTokenResponse.UserID = userDetails.UserID;
                    accessTokenResponse.Username = userDetails.Username;
                    accessTokenResponse.Name = userDetails.Name;
                    accessTokenResponse.IsPractitioner = userDetails.IsPractitioner;
                    accessTokenResponse.AuthenticationToken = accessToken;
                    accessTokenResponse.RefreshAuthenticationToken = refreshToken;
                    apiResponse.Success = true;
                    apiResponse.Message = "Token generated";
                    apiResponse.Data = accessTokenResponse;
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse> GenerateRefreshToken(string email, string refreshToken)
        {
            //Invalidate old refresh token
            await InvalidateRefreshToken(email, refreshToken);
            return await GenerateAccessTokens(email);
        }

        public async Task<bool> IsValidRefreshToken(string email, string refreshToken)
        {
            bool isRefreshTokenValid = false;
            var getRefreshToken = await authenticationDataAccess.GetRefreshToken(email, refreshToken);

            // Check if refresh token is valid or not
            if (getRefreshToken.Count == 1)
            {
                if (getRefreshToken[0].Username == email && getRefreshToken[0].RefreshAuthenticationToken == refreshToken && !getRefreshToken[0].IsRefreshTokenRevoked && getRefreshToken[0].RefreshAuthTokenExpiry > DateTime.UtcNow)
                {
                    isRefreshTokenValid = true;
                }
            }

            return isRefreshTokenValid;
        }

        public async Task InvalidateRefreshToken(string email, string refreshToken)
        {
            var isValidToken = await IsValidRefreshToken(email, refreshToken);
            if (isValidToken)
            {
                await authenticationDataAccess.RevokeRefreshToken(email, refreshToken);
            }
        }

        public async Task<ApiResponse> Logout(string email, string refreshToken)
        {
            ApiResponse apiResponse = new();

            var isValid = await IsValidRefreshToken(email, refreshToken);
            if (!isValid)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Invalid or already expired session.";
                return apiResponse;
            }

            await authenticationDataAccess.RevokeRefreshToken(email, refreshToken);
            apiResponse.Success = true;
            apiResponse.Message = "Logged out successfully.";
            return apiResponse;
        }

        public async Task<ApiResponse> LoginWithPassword(string email, string password)
        {
            ApiResponse apiResponse = new();

            try
            {
                // 1. Check if email is in the admin allowlist
                var allowedEmails = configuration.GetSection("AdminSettings:AllowedEmails").Get<List<string>>() ?? [];
                if (!allowedEmails.Contains(email, StringComparer.OrdinalIgnoreCase))
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "Access denied.";
                    return apiResponse;
                }

                // 2. Get user details with password
                var userDetailsResponse = await userDataAccess.GetUserDetails(
                    UserId: null,
                    Email: email,
                    IncludePassword: true
                );

                if (userDetailsResponse.Count == 0)
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "Invalid email or password.";
                    return apiResponse;
                }

                var userDetails = userDetailsResponse[0];

                // Check if password exists (for OTP-only users)
                if (string.IsNullOrEmpty(userDetails.Password))
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "This account uses email verification. Please use the OTP login option.";
                    return apiResponse;
                }

                // 3. Verify password
                bool isPasswordValid = PasswordUtility.VerifyPassword(
                    password,
                    email,
                    userDetails.Password
                );

                if (!isPasswordValid)
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "Invalid email or password.";
                    return apiResponse;
                }

                // 4. Generate access tokens
                apiResponse = await GenerateAccessTokens(email);
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = "Error during login.";
                apiResponse.Data = new { error = ex.Message };
                return apiResponse;
            }
        }

    }
}
