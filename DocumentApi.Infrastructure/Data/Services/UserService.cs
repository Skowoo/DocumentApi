using DocumentApi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class UserService(DocumentDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IUserService
    {
        public (bool, IList<string>?) AuthorizeUser(string login, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == login);
            if (user is not null)
            {
                PasswordHasher<IdentityUser> hasher = new();
                if (hasher.VerifyHashedPassword(user, user.PasswordHash!, password) != PasswordVerificationResult.Failed)
                {
                    var getUserRoles = userManager.GetRolesAsync(user);
                    var roles = getUserRoles.Result;                    
                    return (true, roles);
                }
            }
            return (false, null);
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
