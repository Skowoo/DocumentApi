using DocumentApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class UserService(
        DocumentDbContext context, 
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IConfiguration configuration) 
        : IUserService
    {
        public async Task<string?> AuthorizeUser(string login, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == login);
            if (user is not null)
            {
                PasswordHasher<IdentityUser> hasher = new();
                if (hasher.VerifyHashedPassword(user, user.PasswordHash!, password) != PasswordVerificationResult.Failed)
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

        public async Task<(IdentityResult Result, string? UserId)> RegisterUser(string login, string password)
        {
            IdentityUser newUser = new()
            {
                UserName = login
            };
            var creationTask = await userManager.CreateAsync(newUser, password);
            return creationTask.Succeeded ? (creationTask, newUser.Id) : (creationTask, null);
        }

        public async Task<IdentityResult> AddRole(string roleName) => await roleManager.CreateAsync(new IdentityRole(roleName));

        public async Task<IdentityResult> RemoveRole(string roleName)
        {
            var target = context.Roles.SingleOrDefault(x => x.Name == roleName);

            if (target is null)
                return IdentityResult.Failed(new IdentityError() { Description = "Role not found!" });

            return await roleManager.DeleteAsync(target);
        }

        public async Task<IdentityResult> AssignUserToRole(string userName, string roleName)
        {
            var targetUser = userManager.Users.SingleOrDefault(x => x.UserName == userName);
            var targetRole = roleManager.Roles.SingleOrDefault(x => x.Name == roleName);

            if (targetUser is null)
                return IdentityResult.Failed(new IdentityError() { Description = "User not found!"});

            if (targetRole is null)
                return IdentityResult.Failed(new IdentityError() { Description = "Role not found!" });

            return await userManager.AddToRoleAsync(targetUser, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRole(string userName, string roleName)
        {
            var targetUser = userManager.Users.SingleOrDefault(x => x.UserName == userName);
            var targetRole = roleManager.Roles.SingleOrDefault(x => x.Name == roleName);

            if (targetUser is null)
                return IdentityResult.Failed(new IdentityError() { Description = "User not found!" });

            if (targetRole is null)
                return IdentityResult.Failed(new IdentityError() { Description = "Role not found!" });

            return await userManager.RemoveFromRoleAsync(targetUser, roleName);
        }

        public async Task<List<IdentityUser>> GetAllUsers() => await userManager.Users.ToListAsync();

        public async Task<IdentityUser?> GetUserById(string id) => await userManager.FindByIdAsync(id);

        public async Task<List<IdentityRole>> GetAllRoles() => await roleManager.Roles.ToListAsync();
    }
}
