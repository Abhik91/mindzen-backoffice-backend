using System;

namespace MindzenDatabaseLibrary.DataModels
{
    public class AuthenticationModel
    {

    }

    public class RefreshTokenResponse
    {
        public string? Username { get; set; }
        public string? RefreshAuthenticationToken { get; set; }
        public DateTime RefreshAuthTokenExpiry { get; set; }
        public bool IsRefreshTokenRevoked { get; set; }

    }
}
