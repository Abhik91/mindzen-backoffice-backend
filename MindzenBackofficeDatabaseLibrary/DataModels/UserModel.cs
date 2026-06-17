using System;

namespace MindzenDatabaseLibrary.DataModels
{
    public class UserModel { }

    public class UserDetails
    {
        public string? UserID { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public bool IsPractitioner { get; set; }
        public string? AuthenticationToken { get; set; }
        public string? RefreshAuthenticationToken { get; set; }
        public string? Password { get; set; }
    }

}


