using DocumentApi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class UserService(DocumentDbContext context, UserManager<IdentityUser> userManager) : IUserService
    {
        public bool AuthorizeUser(string login, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == login);
            if (user is not null)
            {
                PasswordHasher<IdentityUser> hasher = new();
                if (hasher.VerifyHashedPassword(user, user.PasswordHash!, password) != PasswordVerificationResult.Failed)
                    return true;
            }
            return false;
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
    }
}
