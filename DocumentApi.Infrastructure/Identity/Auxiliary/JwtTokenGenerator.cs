using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentApi.Infrastructure.Identity.Auxiliary
{
    internal static class JwtTokenGenerator
    {
        public static async Task<string> GenerateJwtString(IdentityUser user, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var getUserRoles = await userManager.GetRolesAsync(user);
            var roles = getUserRoles;

            var claims = new List<Claim>()
            {
                new("Id", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add roles to JWT token:
            claims.AddRange(roles.Select(x =>
                new Claim(ClaimsIdentity.DefaultRoleClaimType, x)));

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToString = tokenHandler.WriteToken(token);
            return tokenToString;
        }
    }
}
