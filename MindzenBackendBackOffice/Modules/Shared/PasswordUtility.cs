using System;
using MindzenDatabaseLibrary.Common;

namespace MindzenBackend.Modules.Shared
{
    public static class PasswordUtility
    {
        public static string GeneratePassword(string Password, string EmailAddress)
        {
            string salt = new(EmailAddress.ToCharArray().Reverse().ToArray());
            return Utility.GetSHA256Hash(Password, salt);
        }

        public static bool VerifyPassword(string Password, string EmailAddress, string CorrectPassword)
        {
            string salt = new(EmailAddress.ToCharArray().Reverse().ToArray());
            string generatedPassword = Utility.GetSHA256Hash(Password, salt);

            if (generatedPassword == CorrectPassword)
                return true;

            return false;

        }
    }
}
