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

        public void RegisterUser(string login, string password)
        {
            IdentityUser newUser = new()
            {
                UserName = login
            };
            userManager.CreateAsync(newUser, password);

        }
    }
}
