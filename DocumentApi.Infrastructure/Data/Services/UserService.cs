using DocumentApi.Application.Interfaces;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class UserService(DocumentDbContext context) : IUserService
    {
        public bool AuthorizeUser(string login, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.UserName == login);
            if (user is not null)
            {
                return true;
            }

            return true;
        }
    }
}
