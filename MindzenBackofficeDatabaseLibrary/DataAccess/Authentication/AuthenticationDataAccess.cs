using MindzenBackofficeDatabaseLibrary.Helper;
using MindzenBackofficeDatabaseLibrary.Internal;
using MindzenDatabaseLibrary.DataModels;

namespace MindzenDatabaseLibrary.DataAccess.Authentication
{
    public class AuthenticationDataAccess
    {
        private readonly SqlDataAccess _sqlDataAccess;

        public AuthenticationDataAccess()
        {
            _sqlDataAccess = new SqlDataAccess();
        }

        

        #region Update Access tokens details
        public async Task<int> UpdateAccessTokensDetails(string Email, string AuthenticationToken, string RefreshAuthenticationToken, bool IsRefreshTokenRevoked = false)
        {
            try
            {
                var parameter = new
                {
                    // Add parameters here
                    Email = Email,
                    AuthenticationToken = AuthenticationToken,
                    RefreshAuthenticationToken = RefreshAuthenticationToken,
                    RefreshAuthTokenExpiry = DateTime.UtcNow.AddDays(7),
                    IsRefreshTokenRevoked = IsRefreshTokenRevoked
                };

                var result = await _sqlDataAccess.SaveData<dynamic>("dbo.UpdateAccessTokensDetails", parameter, Global.MindzenConnectionString!);
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

       

        #region Revoke refresh token
        public async Task<int> RevokeRefreshToken(string email, string refreshToken)
        {
            try
            {
                var parameter = new
                {
                    // Add parameters here
                    Email = email,
                    RefreshAuthenticationToken = refreshToken,
                };

                var result = await _sqlDataAccess.SaveData<dynamic>("dbo.RevokeRefreshToken", parameter, Global.MindzenConnectionString!);
                return result;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Get Refresh Token
         public async Task<List<RefreshTokenResponse>> GetRefreshToken(string Email, string RefreshToken)
        {
            try
            {
                var parameter = new
                {
                    // Add parameters here
                    Email = Email,
                    RefreshAuthenticationToken = RefreshToken
                };

                var RefreshTokenResponse = await _sqlDataAccess.LoadData<RefreshTokenResponse, dynamic>("dbo.GetRefreshToken", parameter, Global.MindzenConnectionString!);
                return RefreshTokenResponse;
            }
            catch
            {
                throw;
            }

        }
        #endregion

        

        

    }
}
