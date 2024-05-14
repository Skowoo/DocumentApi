using DocumentApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class IdentityController(IConfiguration configuration, IUserService identityService) : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateToken(IdentityUser user)
        {
            if (user is not null && identityService.AuthorizeUser(user.UserName!, user.PasswordHash!))
            {
                var issuer = configuration.GetValue<string>("Jwt:Issuer");
                var audience = configuration.GetValue<string>("Jwt:Audience");
                var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Key")!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(8),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);                
                var tokenToString = tokenHandler.WriteToken(token);
                return Ok(tokenToString);
            }
            return Unauthorized();
        }
    }
}
