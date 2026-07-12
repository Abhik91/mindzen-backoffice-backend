using System;

namespace MindzenBackend.Models.AuthenticationModel
{


    public class RefreshTokenRequest
    {
        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
    }


    public class AccessTokenResponse
    {
        public string? UserID { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public bool IsPractitioner { get; set; }
        public string? AuthenticationToken { get; set; }
        public string? RefreshAuthenticationToken { get; set; }
    }

    public class LoginWithPasswordRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }


}