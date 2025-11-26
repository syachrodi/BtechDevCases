using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TakeHomeAssignment.General.AccountAPI.Model;
using TakeHomeAssignment.General.AccountAPI.Model.General;

namespace TakeHomeAssignment.General.AccountAPI.Utils
{
    public static class TokenUtil
    {
        public static string GenerateJwtToken(User user, ApplicationSetting app)
        {
            var jwtKey = Environment.GetEnvironmentVariable("THA_JWT_SIGNING_KEY");
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            var key = new SymmetricSecurityKey(keyBytes);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tentukan expire dari appsetting
            DateTime expires = app.JWTValidScale?.ToLower() switch
            {
                "minutes" => DateTime.UtcNow.AddMinutes(int.Parse(app.JWTValidMinutes)),
                "hours" => DateTime.UtcNow.AddHours(int.Parse(app.JWTValidHours)),
                "days" => DateTime.UtcNow.AddDays(int.Parse(app.JWTValidDays)),
                _ => DateTime.UtcNow.AddMinutes(int.Parse(app.JWTValidMinutes))
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                expires: expires,
                signingCredentials: creds,
                claims: new[]
                {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.Id.ToString())
                }
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
