using DocumentApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class UserService(DocumentDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IUserService
    {
        public string? AuthorizeUser(string login, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == login);
            if (user is not null)
            {
                PasswordHasher<IdentityUser> hasher = new();
                if (hasher.VerifyHashedPassword(user, user.PasswordHash!, password) != PasswordVerificationResult.Failed)
                {
                    var getUserRoles = userManager.GetRolesAsync(user);
                    var roles = getUserRoles.Result;

                    var claims = new List<Claim>()
                    {
                        new("Id", Guid.NewGuid().ToString()),
                        new(JwtRegisteredClaimNames.Sub, login),
                        new(JwtRegisteredClaimNames.Email, password),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    // Add roles to JWT token:
                    claims.AddRange(roles.Select(x =>
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, x)));

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
                    return tokenToString;
                }
            }
            return null;
        }

        public bool RegisterUser(string login, string password)
        {
            IdentityUser newUser = new()
            {
                UserName = login
            };
            var creationTask = userManager.CreateAsync(newUser, password);
            var creation = creationTask.Result;
            return creation.Succeeded;
        }

        public bool AddRole(string roleName)
        {            
            var creationTask = roleManager.CreateAsync(new IdentityRole(roleName));
            var creation = creationTask.Result;
            return creation.Succeeded;
        }

        public bool RemoveRole(string roleName)
        {
            var target = context.Roles.SingleOrDefault(x => x.Name == roleName);

            if (target is null)
                return false;

            var creationTask = roleManager.DeleteAsync(target);
            var creation = creationTask.Result;
            return creation.Succeeded;
        }

        public bool AssignUserToRole(string userName, string roleName)
        {
            var targetUser = userManager.Users.SingleOrDefault(x => x.UserName == userName);
            var targetRole = roleManager.Roles.SingleOrDefault(x => x.Name == roleName);

            if (targetRole is null || targetUser is null)
                return false;

            var assignTask = userManager.AddToRoleAsync(targetUser, roleName);
            var result = assignTask.Result;
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userName, string roleName)
        {
            var result = false;
            var targetUser = context.Users.SingleOrDefault(x => x.UserName == userName);
            if (targetUser is not null)
            {
                var checkIfInRole = userManager.IsInRoleAsync(targetUser, roleName);
                result = checkIfInRole.Result;
            }
            return result;
        }
    }
}
