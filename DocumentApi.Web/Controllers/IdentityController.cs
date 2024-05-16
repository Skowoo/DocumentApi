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
            var authResult = identityService.AuthorizeUser(user.Login!, user.Password!);

            if (user is not null && authResult.Item1 && authResult.Item2 is not null)
            {
                var claims = new List<Claim>()
                {
                    new("Id", Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Sub, user.Login!),
                    new(JwtRegisteredClaimNames.Email, user.Password!),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in authResult.Item2)
                    claims.Add(new Claim("Role", role));

                var issuer = configuration.GetValue<string>("Jwt:Issuer");
                var audience = configuration.GetValue<string>("Jwt:Audience");
                var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Key")!);
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
                return Ok(tokenToString);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult RegisterUser(AppUser user) => identityService.RegisterUser(user.Login!, user.Password!) ? Ok() : BadRequest("Creation failed!");

        [HttpPost]
        public IActionResult AddRole(string roleName) => identityService.AddRole(roleName) ? Ok(roleName) : BadRequest();

        [HttpPost]
        public IActionResult RemoveRole(string roleName) => identityService.RemoveRole(roleName) ? Ok(roleName) : BadRequest();

        [HttpPost]
        public IActionResult AddUserToRole(string userName, string roleName) => identityService.AssignUserToRole(userName, roleName) ? Ok(roleName) : BadRequest();

        [HttpPost]
        public IActionResult RemoveUserFromRole(string userName, string roleName) => identityService.RemoveUserFromRole(userName, roleName) ? Ok(roleName) : BadRequest();
    }
}
