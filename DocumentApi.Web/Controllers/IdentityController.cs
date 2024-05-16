using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class IdentityController(IConfiguration configuration, IUserService identityService) : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateToken(AppUser user)
        {
            if (user is not null && identityService.AuthorizeUser(user.Login!, user.Password!))
            {
                var issuer = configuration.GetValue<string>("Jwt:Issuer");
                var audience = configuration.GetValue<string>("Jwt:Audience");
                var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Key")!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Login!),
                        new Claim(JwtRegisteredClaimNames.Email, user.Password!),
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

        [HttpPost]
        public IActionResult RegisterUser(AppUser user) => identityService.RegisterUser(user.Login!, user.Password!) ? Ok() : BadRequest("Creation failed!");
    }
}
