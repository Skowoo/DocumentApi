using DocumentApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DocumentApi.Infrastructure.Data;
using DocumentApi.Infrastructure.Identity.Auxiliary;

namespace DocumentApi.Infrastructure.Identity.Services
{
    public class UserService(
        DocumentDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
        : IUserService
    {
        public async Task<(IdentityResult Result, string? Token)> AuthorizeUser(string login, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == login);
            if (user is not null)
            {
                PasswordHasher<IdentityUser> hasher = new();
                if (hasher.VerifyHashedPassword(user, user.PasswordHash!, password) != PasswordVerificationResult.Failed)
                    return (IdentityResult.Success, await JwtTokenGenerator.GenerateJwtString(user, userManager, configuration));
            }
            return (IdentityResult.Failed(new IdentityError() { Description = user is null ? "User not found!" : "Wrong password!" }), null);
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
                return IdentityResult.Failed(new IdentityError() { Code = "Custom", Description = "Role not found!" });

            return await roleManager.DeleteAsync(target);
        }

        public async Task<IdentityResult> AssignUserToRole(string userName, string roleName)
        {
            var targetUser = userManager.Users.SingleOrDefault(x => x.UserName == userName);
            var targetRole = roleManager.Roles.SingleOrDefault(x => x.Name == roleName);

            if (targetUser is null)
                return IdentityResult.Failed(new IdentityError() { Code = "Custom", Description = "User not found!" });

            if (targetRole is null)
                return IdentityResult.Failed(new IdentityError() { Code = "Custom", Description = "Role not found!" });

            return await userManager.AddToRoleAsync(targetUser, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRole(string userName, string roleName)
        {
            var targetUser = userManager.Users.SingleOrDefault(x => x.UserName == userName);
            var targetRole = roleManager.Roles.SingleOrDefault(x => x.Name == roleName);

            if (targetUser is null)
                return IdentityResult.Failed(new IdentityError() { Code = "Custom", Description = "User not found!" });

            if (targetRole is null)
                return IdentityResult.Failed(new IdentityError() { Code = "Custom", Description = "Role not found!" });

            return await userManager.RemoveFromRoleAsync(targetUser, roleName);
        }

        public async Task<List<IdentityUser>> GetAllUsers() => await userManager.Users.ToListAsync();

        public async Task<IdentityUser?> GetUserById(string id) => await userManager.FindByIdAsync(id);

        public async Task<List<IdentityRole>> GetAllRoles() => await roleManager.Roles.ToListAsync();
    }
}
