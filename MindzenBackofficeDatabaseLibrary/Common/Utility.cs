using System;
using System.Security.Cryptography;
using System.Text;

namespace MindzenDatabaseLibrary.Common
{
    public class Utility
    {
        public static string GetSHA256Hash(string Text, string Salt)
        {
            string finalStringToBeHashed = "";
            try
            {
                if (String.IsNullOrEmpty(Salt))
                {
                    finalStringToBeHashed = Text;
                }
                else
                {
                    finalStringToBeHashed = String.Concat(Text, Salt);
                }
                byte[] byteArray = Encoding.Unicode.GetBytes(finalStringToBeHashed);
                // ComputeHash - returns byte array  
                byte[] hashed = SHA256.HashData(byteArray);
                return Convert.ToHexString(hashed).ToLower();
            }
            catch
            {
                throw;
            }

        }
        public static string GetMD5Hash(string Text, string Salt = "")
        {
            string finalStringToBeHashed = "";
            try
            {
                if (String.IsNullOrEmpty(Salt))
                {
                    finalStringToBeHashed = Text;
                }
                else
                {
                    finalStringToBeHashed = String.Concat(Text, Salt);
                }

                byte[] byteArray = Encoding.Unicode.GetBytes(finalStringToBeHashed);
                // ComputeHash - returns byte array  
                byte[] hashed = MD5.HashData(byteArray);
                return Convert.ToHexString(hashed).ToLower();
            }
            catch
            {
                throw;
            }

        }

    }
}
