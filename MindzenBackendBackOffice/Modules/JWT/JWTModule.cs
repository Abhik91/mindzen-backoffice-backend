using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MindzenDatabaseLibrary.DataAccess.Authentication;

namespace MindzenBackend.Modules.JWT
{

    public class JWTModule(IConfiguration config)
    {
        private readonly IConfiguration _config = config;
        private readonly AuthenticationDataAccess authenticationDataAccess = new();

        public string GenerateAccessToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiryMinutes"]!)), // 15 mins expiry
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        // public async Task<RefreshToken?> GetValidRefreshToken(string email, string refreshToken)
        // {
        //     // return await _context.RefreshTokens
        //     //     .Where(rt => rt.Email == email && rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
        //     //     .FirstOrDefaultAsync();

        //     //Fetch from the database
        // }

        // public async Task InvalidateRefreshToken(string email, string refreshToken)
        // {
        //     var token = await GetValidRefreshToken(email, refreshToken);
        //     if (token != null)
        //     {
        //         // token.IsRevoked = true;
        //         // await _context.SaveChangesAsync();
        //         //Update in the database
        //     }
        // }
    }

}
